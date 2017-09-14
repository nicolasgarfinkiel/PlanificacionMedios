using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using AutoMapper;
using Irsa.PDM.Dtos;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Dtos.Filters;
using Irsa.PDM.Entities;
using Irsa.PDM.Repositories;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ServiceStack.Common.Extensions;
using ServiceStack.ServiceClient.Web;
using ServiceStack.Text;
using Pauta = Irsa.PDM.Entities.Pauta;
using System.Drawing;

namespace Irsa.PDM.Admin
{
    public class CampaniasAdmin : BaseAdmin<int, Entities.Campania, Dtos.Campania, FilterCampanias>
    {
        private const string GetPautas = "/client?method=get-list&action=pautas_a_aprobar";
        private const string PostPautasAction = "/client?method=create&action=pautas_aprobadas";
        private const string SuccessMessage = "Se actualizaron satisfactoriamente las pautas aprobadas.";
        private readonly LogAdmin LogAdmin;

        public CampaniasAdmin()
        {
            LogAdmin = new LogAdmin();
        }

        #region Base

        public override Entities.Campania ToEntity(Dtos.Campania dto)
        {
            return null;
        }

        public override void Validate(Dtos.Campania dto)
        {
        }

        public override IQueryable GetQuery(FilterCampanias filter)
        {
            var result = PdmContext.Campanias.OrderBy(e => e.Nombre).AsQueryable();

            if (filter.FechaDesde.HasValue)
            {
                result = result.Where(e => e.CreateDate >= filter.FechaDesde.Value).AsQueryable();
            }

            if (filter.FechaHasta.HasValue)
            {
                var fh = filter.FechaHasta.Value.AddDays(1).AddMilliseconds(-1);
                result = result.Where(e => e.CreateDate <= fh).AsQueryable();
            }

            if (!string.IsNullOrEmpty(filter.Estado))
            {
                var estado = (EstadoCampania)Enum.Parse(typeof(EstadoCampania), filter.Estado);
                result = result.Where(e => e.Estado == estado).AsQueryable();
            }

            if (!string.IsNullOrEmpty(filter.MultiColumnSearchText))
            {
                var nombre = filter.MultiColumnSearchText.ToLower();
                result = result.Where(e => e.Nombre.ToLower().StartsWith(nombre)).AsQueryable();
            }

            return result;
        }

        #endregion

        #region Sync

        public void SyncCampanias()
        {            
            LogSyncCampaniasInit();

            try
            {               
                var pautas = FCMediosclient.Get<IList<PautaFcMedios>>(GetPautas).ToList(); //GetPautasMock();  
                SyncTablasBasicas(pautas);

                var actualMedios = PdmContext.Medios.ToList();
                var actualPlazas = PdmContext.Plazas.ToList();
                var actualVehiculos = PdmContext.Vehiculos.ToList();

                var campanias = pautas.Select(e => new { e.cod_campania, e.des_campania }).Distinct().ToList();

                LogSyncCampaniasDetail(pautas);

                campanias.ForEach(c =>
                {
                    #region Campanias

                    var campania = PdmContext.Campanias.FirstOrDefault(cc => cc.Codigo == c.cod_campania);
                    var pautasWs = pautas.Where(e => string.Equals(e.cod_campania, c.cod_campania)).Select(e => e.nro_pauta).Distinct().ToList();

                    if (campania == null)
                    {
                        campania = new Entities.Campania
                        {
                            CreateDate = DateTime.Now,
                            CreatedBy = App.ImportUser,
                            Enabled = true,
                            Estado = EstadoCampania.Pendiente,
                            Nombre = c.des_campania,
                            Codigo = c.cod_campania,
                            Pautas = new List<Entities.Pauta>()
                        };

                        PdmContext.Campanias.Add(campania);
                    }
                    else if (campania.Estado == EstadoCampania.Cerrada)
                    {
                        SyncEstadoPautas(pautasWs.Select(p => new Entities.Pauta { Codigo = p }).ToList(), 0, Resource.RechazoCampaniaCerrada);
                        LogSyncCampaniasRechazoCampaniaCerrada(campania, pautasWs);
                        return;
                    }

                    #endregion

                    #region Pautas

                    pautasWs.ForEach(pcodigo =>
                    {
                        var pauta = campania.Pautas.SingleOrDefault(ee => string.Equals(ee.Codigo, pcodigo));

                        if (pauta == null)
                        {
                            pauta = new Entities.Pauta
                            {
                                CreateDate = DateTime.Now,
                                CreatedBy = App.ImportUser,
                                Enabled = true,
                                Estado = EstadoPauta.Pendiente,
                                Campania = campania,
                                Codigo = pcodigo,
                                Items = new List<Entities.PautaItem>()
                            };

                            campania.Pautas.Add(pauta);
                        }

                        var items = pautas.Where(e => string.Equals(e.nro_pauta, pcodigo)).ToList();

                        items.ForEach(itemWs =>
                        {
                            var item = pauta.Items.FirstOrDefault(e => string.Equals(e.CodigoPrograma, itemWs.cod_programa));

                            if (item == null)
                            {
                                item = new Entities.PautaItem
                                {
                                    CreateDate = DateTime.Now,
                                    CreatedBy = App.ImportUser,
                                    Enabled = true,
                                    Pauta = pauta,
                                    CodigoAviso = itemWs.cod_aviso,
                                    CodigoPrograma = itemWs.cod_programa
                                };

                                pauta.Items.Add(item);
                            }

                            item.Tarifa = PdmContext.Tarifas.FirstOrDefault(e =>
                                e.CodigoPrograma == itemWs.cod_programa &&
                                e.Tarifario.Estado == EstadoTarifario.Aprobado);

                            item.DiferenciaEnMontoTarifas = item.Tarifa != null && item.Tarifa.Importe != itemWs.costo_unitario;
                            item.CostoUnitario = itemWs.costo_unitario;
                            item.Descuento1 = itemWs.descuento_1;
                            item.Descuento2 = itemWs.descuento_2;
                            item.Descuento3 = itemWs.descuento_3;
                            item.Descuento4 = itemWs.descuento_4;
                            item.Descuento5 = itemWs.descuento_5;
                            item.Tema = itemWs.des_tema;
                            item.Proveedor = itemWs.des_proveedor;
                            item.DuracionTema = itemWs.duracion_tema;
                            item.Producto = itemWs.des_producto;
                            item.Espacio = itemWs.espacio;
                            item.FechaAviso = itemWs.fecha_aviso;
                        });

                        pauta.Estado = pauta.Items.Any(e => e.Tarifa == null)
                            ? EstadoPauta.ProgramasNoTarifados : pauta.Items.Any(e => e.DiferenciaEnMontoTarifas)
                            ? EstadoPauta.DiferenciaEnMontoTarifas : pauta.Estado;
                    });

                    #endregion

                    #region Inconsistencias

                    campania.Estado = campania.Pautas.Any(e =>
                                e.Estado == EstadoPauta.ProgramasNoTarifados ||
                                e.Estado == EstadoPauta.DiferenciaEnMontoTarifas)
                            ? EstadoCampania.InconsistenciasEnPautas : campania.Estado;

                    if (campania.Estado == EstadoCampania.InconsistenciasEnPautas)
                    {
                        campania.Pautas.ForEach(p =>
                        {
                            var sinTarifa = p.Items.Where(i => i.Tarifa == null).Select(i => i.CodigoPrograma).ToList();
                            var diferenteMonto = p.Items.Where(i => i.DiferenciaEnMontoTarifas).Select(i => i.CodigoPrograma).ToList();

                            if (!sinTarifa.Any() && !diferenteMonto.Any()) return;

                            var motivo = sinTarifa.Any()
                                ? string.Format(Resource.ProgramasNoTarifados, string.Join(",", sinTarifa))
                                : string.Format(Resource.DiferenciaEnMontoTarifas, string.Join(",", diferenteMonto));

                            LogSyncCampaniasRechazoInconsistencias(p, motivo);
                            //   SyncEstadoPautas(new List<Pauta> {p}, 0, motivo);

                            sinTarifa.ForEach(st =>
                            {
                                var tfc = pautas.First(t => t.cod_programa == st && string.Equals(t.nro_pauta, p.Codigo));
                                var medio = actualMedios.Single(e => e.Codigo == tfc.cod_medio);
                                var plaza = actualPlazas.Single(e => e.Codigo == tfc.cod_plaza);
                                var vehiculo = actualVehiculos.Single(e => e.Codigo == tfc.cod_vehiculo);
                                var tarifario = PdmContext.Tarifarios.SingleOrDefault(tarif =>
                                            tarif.Estado == EstadoTarifario.Aprobado &&
                                            tarif.Vehiculo.Codigo == vehiculo.Codigo);

                                if (tarifario == null)
                                {
                                    return;
                                }

                                var tarifa = new Entities.Tarifa
                                {
                                    CodigoPrograma = tfc.cod_programa,
                                    CreateDate = DateTime.Now,
                                    CreatedBy = App.ImportUser,
                                    Descripcion = tfc.espacio,
                                    Enabled = true,
                                    HoraDesde = tfc.hora_inicio,
                                    HoraHasta = tfc.hora_fin,
                                    Importe = tfc.costo_unitario,
                                    Plaza = plaza,
                                    Tarifario = tarifario,
                                    Vehiculo = vehiculo,
                                    Medio = medio,
                                    Nueva = true
                                };

                                tarifario.Tarifas.Add(tarifa);
                            });
                        });
                    }

                    #endregion
                });
              
                PdmContext.SaveChanges();               
            }
            catch (Exception ex)
            {
                LogSyncCampaniasError(ex);
                LogSyncCampaniasEnd();
                throw;
            }

            LogSyncCampaniasEnd();
        }

        public void SyncTablasBasicas(IList<PautaFcMedios> pautas)
        {           
            #region Base

            var actualMedios = PdmContext.Medios.ToList();
            var actualPlazas = PdmContext.Plazas.ToList();
            var actualVehiculos = PdmContext.Vehiculos.ToList();

            var serviceMedios = pautas.Select(e => new { Codigo = e.cod_medio, Descripcion = e.des_medio }).Distinct().ToList();
            var servicePlazas = pautas.Select(e => new { Codigo = e.cod_plaza, Descripcion = e.des_plaza }).Distinct().ToList();
            var serviceVehiculos = pautas.Select(e => new { Codigo = e.cod_vehiculo, Descripcion = e.des_vehiculo }).Distinct().ToList();

            #region Medios

            serviceMedios.ForEach(t =>
            {
                var medio = actualMedios.FirstOrDefault(e => e.Codigo == t.Codigo);

                if (medio == null)
                {
                    medio = new Entities.Medio
                    {
                        Codigo = t.Codigo,
                        Descripcion = t.Descripcion,
                        Nombre = t.Descripcion,
                        CreateDate = DateTime.Now,
                        CreatedBy = App.ImportUser,
                        Enabled = true
                    };

                    PdmContext.Medios.Add(medio);
                }
                else
                {
                    medio.Descripcion = t.Descripcion;
                    medio.UpdateDate = DateTime.Now;
                    medio.UpdatedBy = App.ImportUser;
                }
            });

            #endregion

            #region Plazas

            servicePlazas.ForEach(t =>
            {
                var plaza = actualPlazas.FirstOrDefault(e => e.Codigo == t.Codigo);

                if (plaza == null)
                {
                    plaza = new Entities.Plaza
                    {
                        Codigo = t.Codigo,
                        Descripcion = t.Descripcion,
                        CreateDate = DateTime.Now,
                        CreatedBy = App.ImportUser,
                        Enabled = true
                    };

                    PdmContext.Plazas.Add(plaza);
                }
                else
                {
                    plaza.Descripcion = t.Descripcion;
                    plaza.UpdateDate = DateTime.Now;
                    plaza.UpdatedBy = App.ImportUser;
                }
            });

            #endregion

            #region Vehiculos

            serviceVehiculos.ForEach(t =>
            {
                var vehiculo = actualVehiculos.FirstOrDefault(e => e.Codigo == t.Codigo);

                if (vehiculo == null)
                {
                    vehiculo = new Entities.Vehiculo
                    {
                        Codigo = t.Codigo,
                        Descripcion = t.Descripcion,
                        CreateDate = DateTime.Now,
                        CreatedBy = App.ImportUser,
                        Enabled = true,
                        Nombre = t.Descripcion
                    };

                    PdmContext.Vehiculos.Add(vehiculo);
                }
                else
                {
                    vehiculo.Nombre = t.Descripcion;
                    vehiculo.Descripcion = t.Descripcion;
                    vehiculo.UpdateDate = DateTime.Now;
                    vehiculo.UpdatedBy = App.ImportUser;
                }
            });

            #endregion

            #endregion

            PdmContext.Configuration.AutoDetectChangesEnabled = false;
            PdmContext.SaveChanges();
            PdmContext.Configuration.AutoDetectChangesEnabled = true;
            PdmContext = new PDMContext();
        }

        private IList<PautaFcMedios> GetPautasMock()
        {
            return new List<PautaFcMedios>
            {
                new PautaFcMedios
                {
                  cod_aviso  = 10.ToString(),
                  cod_campania = 4760,
                  cod_plaza = "GBA",
                  cod_medio = "T",
                  cod_programa = 100,
                  cod_proveedor = 10,
                  cod_vehiculo = 10055,
                  costo_unitario = 100,
                  des_campania = "Demo 1",
                  des_proveedor = "prov",
                  des_tema = "Tesma",
                  duracion_tema = 10,
                  espacio = "l a v",
                  hora_fin = 23,
                  hora_inicio = 0,
                  fecha_aviso = DateTime.Now,
                  nro_pauta = "120"                  
                },
                 new PautaFcMedios
                {
                  cod_aviso  = 10.ToString(),
                  cod_campania = 4761,
                  cod_plaza = "GBA",
                  cod_medio = "T",
                  cod_programa = 41474,
                  cod_proveedor = 10,
                  cod_vehiculo = 964,
                  costo_unitario = 100,
                  des_campania = "Demo 2",
                  des_proveedor = "prov",
                  des_tema = "Tesma",
                  duracion_tema = 10,
                  espacio = "l a v",
                  hora_fin = 23,
                  hora_inicio = 0,
                  fecha_aviso = DateTime.Now,
                  nro_pauta = "121"                  
                },
            };
        }

        #endregion

        public IList<string> GetEstadosCampania()
        {
            return Enum.GetNames(typeof(EstadoCampania)).OrderBy(t => t).ToList();
        }

        public PagedListResponse<Dtos.PautaDetail> GetPautasByFilter(Dtos.Filters.FilterPautaItems filter)
        {
            var query = PdmContext
                      .Pautas.Include(e => e.Campania)
                      .Where(e => e.Estado == EstadoPauta.Aprobada || e.Estado == EstadoPauta.Cerrada || e.Estado == EstadoPauta.Pendiente)
                      .OrderBy(t => t.Campania.Nombre)
                      .AsQueryable();

            if (filter.CampaniaCodigo > 0)
            {
                query = query.Where(e => e.Campania.Codigo == filter.CampaniaCodigo).AsQueryable();
            }

            return new PagedListResponse<Dtos.PautaDetail>
            {
                Count = query.Count(),
                Data = Mapper.Map<IList<Entities.Pauta>, IList<Dtos.PautaDetail>>(query.Skip(filter.PageSize * (filter.CurrentPage - 1)).Take(filter.PageSize).ToList())
            };
        }

        public PagedListResponse<Dtos.PautaItem> GetItemsByFilter(Dtos.Filters.FilterPautaItems filter)
        {
            var pautaId = filter.PautaId.HasValue ? filter.PautaId.Value : PdmContext.Pautas.Single(e => string.Equals(e.Codigo, filter.PautaCodigo)).Id;

            var query = PdmContext
                      .PautasItem.Include(p => p.Tarifa)
                      .Where(t => t.Pauta.Id == pautaId && t.Pauta.Campania.Codigo == filter.CampaniaCodigo)
                      .OrderBy(t => t.CodigoPrograma)
                      .AsQueryable();

            return new PagedListResponse<Dtos.PautaItem>
            {
                Count = query.Count(),
                Data = Mapper.Map<IList<Entities.PautaItem>, IList<Dtos.PautaItem>>(query.Skip(filter.PageSize * (filter.CurrentPage - 1)).Take(filter.PageSize).ToList())
            };
        }

        public void ChangeEstadoCampania(int id, string est, string motivo)
        {
            var campania = default(Entities.Campania);

            try
            {
                campania = PdmContext.Campanias.Single(e => e.Id == id);
                var estado = (EstadoCampania)Enum.Parse(typeof(EstadoCampania), est);

                campania.UpdateDate = DateTime.Now;
                campania.UpdatedBy = UsuarioLogged;
                campania.Estado = estado;

                campania.Pautas.ForEach(p =>
                {
                    p.UpdateDate = DateTime.Now;
                    p.UpdatedBy = UsuarioLogged;
                    p.Estado = estado == EstadoCampania.Aprobada ? EstadoPauta.Aprobada : EstadoPauta.Rechazada;
                });

                SyncEstadoPautas(campania.Pautas, (estado == EstadoCampania.Aprobada ? 1 : 0), motivo);
                PdmContext.SaveChanges();

                LogChangeEstadoCampaniaInfo(campania, est, motivo);
            }
            catch (Exception ex)
            {
                LogChangeEstadoCampaniaError(campania, est, motivo, ex);
                throw;
            }
        }

        public string ChangeEstadoPauta(int pautaId, string est, string motivo)
        {
            var pauta = PdmContext.Pautas.Single(e => e.Id == pautaId);

            try
            {
                var estado = (EstadoPauta)Enum.Parse(typeof(EstadoPauta), est);

                pauta.Estado = estado;
                pauta.UpdateDate = DateTime.Now;
                pauta.UpdatedBy = UsuarioLogged;

                if (pauta.Campania.Pautas.All(e => e.Estado == estado))
                {
                    pauta.Campania.UpdateDate = DateTime.Now;
                    pauta.Campania.UpdatedBy = UsuarioLogged;
                    pauta.Campania.Estado = estado == EstadoPauta.Aprobada ? EstadoCampania.Aprobada : EstadoCampania.Rechazada;
                }

                SyncEstadoPautas(new List<Pauta> { pauta }, (estado == EstadoPauta.Aprobada ? 1 : 0), motivo);
                PdmContext.SaveChanges();
                LogChangeEstadoPautaInfo(pauta, est, motivo);
            }
            catch (Exception ex)
            {
                LogChangeEstadoPautaError(pauta, est, motivo, ex);
                throw;
            }

            return pauta.Campania.Estado.ToString();
        }

        private void SyncEstadoPautas(IList<Entities.Pauta> pautas, int estado, string motivo)
        {
            var list = pautas.Select(pauta =>
               string.Format("{{\"nro_pauta\":{0},\"aprobada\":{1},\"usuario\":\"{2}\",\"motivo\":\"{3}\"}}",
               pauta.Codigo, estado, UsuarioLogged, motivo))
               .ToList();

            var json = string.Join(",", list);
            json = string.Format("[{0}]", json);

            var result = string.Format("{0}{1}", FcMediosTarifarioUrl, PostPautasAction).PostJsonToUrl(json);

            if (!string.Equals(result, SuccessMessage))
            {
                throw new Exception(string.Format("Error en la sincronización con FC Medios: {0}", result));
            }
        }

        public ExcelPackage GetExcelVisualDePauta(int pautaId)
        {
            var pauta = PdmContext.Pautas.Single(e => e.Id == pautaId);

            var template = new FileInfo(String.Format(@"{0}\Reports\Rpt_VisualDePauta.xlsx", AppDomain.CurrentDomain.BaseDirectory));
            var pck = new ExcelPackage(template, true);
            var ws = pck.Workbook.Worksheets[1];
            var row = 2;



            return pck;
        }

        public ExcelPackage GetExcelReporteDePauta(int pautaId)
        {
            var pauta = PdmContext.Pautas.Single(e => e.Id == pautaId);

            var template = new FileInfo(String.Format(@"{0}\Reports\Rpt_Pauta.xlsx", AppDomain.CurrentDomain.BaseDirectory));
            var pck = new ExcelPackage(template, true);
            var ws = pck.Workbook.Worksheets[1];
            var fechaDesde = pauta.Items.Select(e => e.FechaAviso).Min().Value;
            var fechaHasta = pauta.Items.Select(e => e.FechaAviso).Max().Value;

            #region Header

            ws.Cells[5, 4].Value = DateTime.Now.ToString("dd/MM/yyyy");
            ws.Cells[7, 2].Value = pauta.Codigo;
            ws.Cells[7, 4].Value = pauta.Campania.Nombre;
            ws.Cells[8, 4].Value = string.Format("{0} a {1}", fechaDesde.ToString("dd/MM/yyyy"), fechaHasta.ToString("dd/MM/yyyy"));
            ws.Cells[10, 2].Value = pauta.Items[0].Producto;

            #endregion

            #region Columns

            var esDow = new Dictionary<DayOfWeek, string>()
            {
                {DayOfWeek.Monday, "Lun"},
                {DayOfWeek.Tuesday, "Mar"},
                {DayOfWeek.Wednesday, "Mie"},
                {DayOfWeek.Thursday, "Jue"},
                {DayOfWeek.Friday, "Vie"},
                {DayOfWeek.Saturday, "Sab"},
                {DayOfWeek.Sunday, "Dom"}                
            };

            var col = 8;
            var days = (fechaHasta - fechaDesde).Days;           

            for (var i = 0; i <= days; i++)
            {
                col++;
                var day = fechaDesde.AddDays(i).Day;
                var dow = fechaDesde.AddDays(i).DayOfWeek;

                ws.Cells[12, col].Value = esDow[dow];
                ws.Cells[13, col].Value = day;

                ws.Cells[13, col].ApplyCellBorder();                
                ws.Cells[12, col].ApplyCellBK();
                ws.Cells[13, col].ApplyCellBK();                                

                if (dow == DayOfWeek.Sunday)
                {
                    ws.Cells[12, col].Style.Font.Color.SetColor(Color.DarkRed);
                    ws.Cells[13, col].Style.Font.Color.SetColor(Color.DarkRed);
                }
            }

            col++;
            ws.Column(col).Width = 12;
            ws.Cells[13, col].Value = "Tot.Unit.";
            ws.Cells[13, col].ApplyCellBorder();
            ws.Cells[12, col].ApplyCellBK();
            ws.Cells[13, col].ApplyCellBK();     

            col++;
            ws.Cells[13, col].Value = "Total";
            ws.Cells[13, col].ApplyCellBorder();
            ws.Cells[12, col].ApplyCellBK();
            ws.Cells[13, col].ApplyCellBK();

            col++;
            ws.Cells[12, col].Value = "Descuentos";
            ws.Cells[12, col, 12, col + 3].Merge = true;
            ws.Cells[13, col].Value = "1";
            ws.Cells[13, col].ApplyCellBorder();
            ws.Cells[12, col].ApplyCellBK();
            ws.Cells[13, col].ApplyCellBK();

            col++;                        
            ws.Cells[13, col].Value = "2";
            ws.Cells[13, col].ApplyCellBorder();
            ws.Cells[12, col].ApplyCellBK();
            ws.Cells[13, col].ApplyCellBK();

            col++;
            ws.Cells[13, col].Value = "3";
            ws.Cells[13, col].ApplyCellBorder();
            ws.Cells[12, col].ApplyCellBK();
            ws.Cells[13, col].ApplyCellBK();

            col++;
            ws.Cells[13, col].Value = "4";
            ws.Cells[13, col].ApplyCellBorder();
            ws.Cells[12, col].ApplyCellBK();
            ws.Cells[13, col].ApplyCellBK();

            col++;
            ws.Column(col).Width = 14;
            ws.Cells[13, col].Value = "Inversión Neta";
            ws.Cells[13, col].ApplyCellBorder();
            ws.Cells[12, col].ApplyCellBK();
            ws.Cells[13, col].ApplyCellBK();

            col++;
            ws.Cells[13, col].Value = "RTG 1";
            ws.Cells[13, col].ApplyCellBorder();
            ws.Cells[12, col].ApplyCellBK();
            ws.Cells[13, col].ApplyCellBK();

            col++;
            ws.Cells[13, col].Value = "RTG 2";
            ws.Cells[13, col].ApplyCellBorder();
            ws.Cells[12, col].ApplyCellBK();
            ws.Cells[13, col].ApplyCellBK();

            col++;
            ws.Cells[13, col].Value = "RTG 3";
            ws.Cells[13, col].ApplyCellBorder();
            ws.Cells[12, col].ApplyCellBK();
            ws.Cells[13, col].ApplyCellBK();

            col++;
            ws.Cells[13, col].Value = "CPR 1";
            ws.Cells[13, col].ApplyCellBorder();
            ws.Cells[12, col].ApplyCellBK();
            ws.Cells[13, col].ApplyCellBK();

            col++;
            ws.Cells[13, col].Value = "CPR 2";
            ws.Cells[13, col].ApplyCellBorder();
            ws.Cells[12, col].ApplyCellBK();
            ws.Cells[13, col].ApplyCellBK();

            col++;
            ws.Cells[13, col].Value = "CPR 3";
            ws.Cells[13, col].ApplyCellBorder();
            ws.Cells[12, col].ApplyCellBK();
            ws.Cells[13, col].ApplyCellBK();


            #endregion

            #region Data

            var row = 13;

            var group = pauta.Items.GroupBy(e => e.Tarifa, e => e)
                       .Select(e => new { Tarifa = e.Key, Items = e.ToList() })
                       .OrderBy(e => e.Tarifa.Vehiculo.Nombre).ToList();

            group.ForEach(e =>
            {
                row++;
                ws.Cells[row, 1].Value = e.Tarifa.Vehiculo.Nombre;
                ws.Cells[row, 4].Value = e.Tarifa.HoraDesde;
                ws.Cells[row, 5].Value = e.Tarifa.HoraHasta;
                ws.Cells[row, 6].Value = e.Tarifa.Importe;
                ws.Cells[row, 7].Value = e.Items[0].Tema;
                ws.Cells[row, 8].Value = e.Items[0].DuracionTema;

                e.Items.ForEach(i =>
                {
                    var col2 = (i.FechaAviso.Value - fechaDesde).Days + 9;
                    ws.Cells[row, col2].Value = 1;
                });

                ws.Cells[row, (8 + days + 2)].Value = e.Items.Count;
                ws.Cells[row, (8 + days + 3)].Value = e.Items.Count * e.Items[0].DuracionTema;
            });

            #endregion

            return pck;
        }

        #region Log

        private void LogSyncCampaniasRechazoInconsistencias(Pauta pauta, string motivo)
        {
            var log = new Dtos.Log
            {
                Accion = "CampaniasAdmin.SyncCampanias",
                App = "Irsa.PDM.WindowsService",
                CreateDate = DateTime.Now,
                Modulo = "Campanias",
                Tipo = App.Warning,
                UsuarioAccion = UsuarioLogged,
                Descripcion = string.Format("Pauta Codigo: {0}. {1}.", pauta.Codigo, motivo)
            };

            LogAdmin.Create(log);
        }

        private void LogSyncCampaniasRechazoCampaniaCerrada(Entities.Campania campania, List<string> pautasWs)
        {
            var log = new Dtos.Log
            {
                Accion = "CampaniasAdmin.SyncCampanias",
                App = "Irsa.PDM.WindowsService",
                CreateDate = DateTime.Now,
                Modulo = "Campanias",
                Tipo = App.Warning,
                UsuarioAccion = UsuarioLogged,
                Descripcion = string.Format("La campaña ID: {0} se encuentra cerrada. Los siguientes códigos de pauta fueron rechazados: ", string.Join(", ", pautasWs))
            };

            LogAdmin.Create(log);
        }

        private void LogSyncCampaniasDetail(IList<PautaFcMedios> pautas)
        {
            var log = new Dtos.Log
            {
                Accion = "CampaniasAdmin.SyncCampanias",
                App = "Irsa.PDM.WindowsService",
                CreateDate = DateTime.Now,
                Modulo = "Campanias",
                Tipo = App.Info,
                UsuarioAccion = UsuarioLogged,
                Descripcion = string.Format("DETALLE de pautas a ingresar: {0}", JsonConvert.SerializeObject(pautas))
            };

            LogAdmin.Create(log);
        }

        private void LogSyncCampaniasInit()
        {
            var log = new Dtos.Log
            {
                Accion = "CampaniasAdmin.SyncCampanias",
                App = "Irsa.PDM.WindowsService",
                CreateDate = DateTime.Now,
                Modulo = "Campanias",
                Tipo = App.Info,
                UsuarioAccion = UsuarioLogged,
                Descripcion = "INICIO de sincronización de campañas."
            };

            LogAdmin.Create(log);
        }

        private void LogSyncCampaniasEnd()
        {
            var log = new Dtos.Log
            {
                Accion = "CampaniasAdmin.SyncCampanias",
                App = "Irsa.PDM.WindowsService",
                CreateDate = DateTime.Now,
                Modulo = "Campanias",
                Tipo = App.Info,
                UsuarioAccion = UsuarioLogged,
                Descripcion = "FIN de sincronización de campañas."
            };

            LogAdmin.Create(log);
        }

        private void LogSyncCampaniasError(Exception ex)
        {
            var log = new Dtos.Log
            {
                Accion = "CampaniasAdmin.SyncCampanias",
                App = "Irsa.PDM.WindowsService",
                CreateDate = DateTime.Now,
                Modulo = "Campanias",
                Tipo = App.Error,
                UsuarioAccion = UsuarioLogged,
                Descripcion = "Error",
                StackTrace = GetExceptionDetail(ex)
            };

            LogAdmin.Create(log);
        }

        private void LogChangeEstadoCampaniaInfo(Entities.Campania campania, string est, string motivo)
        {
            var log = new Dtos.Log
            {
                Accion = "CampaniasAdmin.ChangeEstadoCampania",
                App = "Irsa.PDM.WindowsService",
                CreateDate = DateTime.Now,
                Modulo = "Campanias",
                Tipo = App.Info,
                UsuarioAccion = UsuarioLogged,
                Descripcion = string.Format("Se modificó el estado de la campaña ID {0} a {1}. Motivo {2}", campania.Id, est, motivo ?? "Aprobada")
            };

            LogAdmin.Create(log);
        }

        private void LogChangeEstadoPautaInfo(Pauta pauta, string est, string motivo)
        {
            var log = new Dtos.Log
            {
                Accion = "CampaniasAdmin.ChangeEstadoPauta",
                App = "Irsa.PDM.WindowsService",
                CreateDate = DateTime.Now,
                Modulo = "Campanias",
                Tipo = App.Info,
                UsuarioAccion = UsuarioLogged,
                Descripcion = string.Format("Se modificó el estado de la pauta ID {0} a {1}. Motivo {2}", pauta.Id, est, motivo)
            };

            LogAdmin.Create(log);
        }

        private void LogChangeEstadoCampaniaError(Entities.Campania campania, string est, string motivo, Exception ex)
        {
            var log = new Dtos.Log
            {
                Accion = "CampaniasAdmin.ChangeEstadoCampania",
                App = "Irsa.PDM.WindowsService",
                CreateDate = DateTime.Now,
                Modulo = "Campanias",
                Tipo = App.Info,
                UsuarioAccion = UsuarioLogged,
                Descripcion = string.Format("Se produjo un error al intentar modificar el estado de la campaña ID {0} a {1}. Motivo {2}", campania.Id, est, motivo),
                StackTrace = GetExceptionDetail(ex)

            };

            LogAdmin.Create(log);
        }

        private void LogChangeEstadoPautaError(Pauta pauta, string est, string motivo, Exception ex)
        {
            var log = new Dtos.Log
            {
                Accion = "CampaniasAdmin.ChangeEstadoPauta",
                App = "Irsa.PDM.WindowsService",
                CreateDate = DateTime.Now,
                Modulo = "Campanias",
                Tipo = App.Info,
                UsuarioAccion = UsuarioLogged,
                Descripcion = string.Format("Se produjo un error al intentar modificar el estado de la pauta ID {0} a {1}. Motivo {2}", pauta.Id, est, motivo),
                StackTrace = GetExceptionDetail(ex)
            };

            LogAdmin.Create(log);
        }

        #endregion

    }


    public static class ExcelHelper
    {
        public static void ApplyCellBorder(this ExcelRange range)
        {           
            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;            
        }

        public static void ApplyCellBK(this ExcelRange range)
        {
            var bkColor = System.Drawing.ColorTranslator.FromHtml("#D9E1F2");
            
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(bkColor);
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(bkColor);
        }
    }
}
