using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using AutoMapper;
using Irsa.PDM.Dtos;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Entities;
using OfficeOpenXml;
using ServiceStack.Common.Extensions;
using ServiceStack.ServiceClient.Web;
using ServiceStack.Text;

namespace Irsa.PDM.Admin
{
    public class CampaniasAdmin : BaseAdmin<int, Entities.Campania, Dtos.Campania, FilterBase>
    {        
        private const string ImportUser = "Import process";
        private const string GetPautas = "/client?method=get-list&action=pautas_a_aprobar";
        private const string PostPautasAction = "/client?method=create&action=pautas_aprobadas";
        private const string SuccessMessage = "Se actualizaron satisfactoriamente las pautas aprobadas.";

        #region Base
      
        public override Entities.Campania ToEntity(Dtos.Campania dto)
        {
            return null;
        }

        public override void Validate(Dtos.Campania dto)
        {                      
        }

        public override IQueryable GetQuery(FilterBase filter)
        {
            var result = PdmContext.Campanias.OrderBy(e => e.Nombre).AsQueryable();

            if (filter.MultiColumnSearchText != null)
            {
                var multiColumnSearchText = filter.MultiColumnSearchText.ToLower();
                result = result.Where(e => e.Nombre != null && e.Nombre.ToLower().Contains(multiColumnSearchText)).AsQueryable();
            }

            return result;
        }

        #endregion      

        #region Sync

        public void SyncCampanias()
        {
            var client = new JsonServiceClient(FcMediosTarifarioUrl);
            var pautas = client.Get<IList<PautaFcMedios>>(GetPautas).ToList();
            var campanias = pautas.Select(e => e.campania).Distinct().ToList();

            campanias.ForEach(c =>
            {                                
                #region Campanias

                var campania = PdmContext.Campanias.SingleOrDefault(cc => string.Equals(cc.Nombre, c));

                if (campania == null)
                {
                    campania = new Entities.Campania
                    {
                        CreateDate = DateTime.Now,
                        CreatedBy = ImportUser,
                        Enabled = true,
                        Estado = EstadoCampania.Pendiente,
                        Nombre = c,
                        Pautas = new List<Entities.Pauta>()
                    };

                    PdmContext.Campanias.Add(campania);
                }
                                
                if (campania.Estado != EstadoCampania.Pendiente) return;

                #endregion

                #region Pautas

                var pautasWs = pautas.Where(e => string.Equals(e.campania, c)).Select(e => e.nro_pauta).Distinct().ToList();

                pautasWs.ForEach(pcodigo =>
                {
                    var pauta = campania.Pautas.SingleOrDefault(ee => string.Equals(ee.Codigo, pcodigo));

                    if (pauta == null)
                    {
                        pauta = new Entities.Pauta
                        {
                            CreateDate = DateTime.Now,
                            CreatedBy = ImportUser,
                            Enabled = true,
                            Estado = EstadoPauta.Pendiente,
                            Campania = campania,
                            Codigo = pcodigo,
                            Items = new List<Entities.PautaItem>()
                        };

                        campania.Pautas.Add(pauta);                        
                    }

                    if (pauta.Estado != EstadoPauta.Pendiente && pauta.Estado != EstadoPauta.ConflictoTarifas) return;

                    var items = pautas.Where(e => string.Equals(e.nro_pauta, pcodigo)).ToList();

                    items.ForEach(itemWs =>
                    {
                        var item = pauta.Items.SingleOrDefault(e => string.Equals(e.CodigoPrograma, itemWs.cod_programa));

                        if (item == null)
                        {
                            item = new Entities.PautaItem
                            {
                                CreateDate = DateTime.Now,
                                CreatedBy = ImportUser,
                                Enabled = true,
                                Pauta = pauta,
                                CodigoAviso = itemWs.cod_aviso,
                                CodigoPrograma = itemWs.cod_programa                                                     
                            };

                            pauta.Items.Add(item);
                        }

                        item.Tarifa = PdmContext.Tarifas.FirstOrDefault(e =>
                            e.CodigoPrograma == itemWs.cod_programa &&
                            e.Tarifario.Estado == EstadoTarifario.Editable);

                        item.CostoUnitario = itemWs.costo_unitario;
                        item.Descuento1 = itemWs.descuento_1;
                        item.Descuento2 = itemWs.descuento_2;
                        item.Descuento3 = itemWs.descuento_3;
                        item.Descuento4 = itemWs.descuento_4;
                        item.Descuento5 = itemWs.descuento_5;
                        item.Tema = itemWs.des_tema;
                        item.Proveedor = itemWs.des_proveedor;
                        item.DuracionTema = itemWs.duracion_tema;
                        item.Espacio = itemWs.espacio;
                        item.FechaAviso = itemWs.fecha_aviso;
                    });

                    pauta.Estado = pauta.Items.Any(e => e.Tarifa == null) ? EstadoPauta.ConflictoTarifas : pauta.Estado;
                });


                campania.Estado = campania.Pautas.Any(e => e.Estado == EstadoPauta.ConflictoTarifas) ? EstadoCampania.ConflictoTarifas : campania.Estado;

                #endregion              
            });

            PdmContext.SaveChanges();
        }

        #endregion

        public PagedListResponse<Dtos.PautaItem> GetItemsByFilter(Dtos.Filters.FilterPautaItems filter)
        {
            var query = PdmContext
                      .PautasItem.Include(p => p.Tarifa)
                      .Where(t => t.Pauta.Id == filter.PautaId)
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
            var campania = PdmContext.Campanias.Single(e => e.Id == id);
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
                      
            #region Sync
       
            var list = campania.Pautas.Select(pauta =>  
                string.Format("{{\"nro_pauta\":{0},\"aprobada\":{1},\"usuario\":\"{2}\",\"motivo\":\"{3}\"}}",
                pauta.Codigo, estado == EstadoCampania.Aprobada ? 1 : 0, UsuarioLogged, motivo))
                .ToList();

            var json = string.Join(",", list);
            json = string.Format("[{0}]", json);

            var result = string.Format("{0}{1}", FcMediosTarifarioUrl, PostPautasAction).PostJsonToUrl(json);

            if (!string.Equals(result, SuccessMessage))
            {
                throw new Exception(string.Format("Error en la sincronización con FC Medios: {0}", result));
            }

            #endregion

            PdmContext.SaveChanges();            
        }

        public ExcelPackage GetExcelVisualDePauta(int campaniaId)
        {
            var campania = PdmContext.Campanias.Single(e => e.Id == campaniaId);

            var template = new FileInfo(String.Format(@"{0}\Reports\Rpt_VisualDePauta.xlsx", AppDomain.CurrentDomain.BaseDirectory));
            var pck = new ExcelPackage(template, true);
            var ws = pck.Workbook.Worksheets[1];
            var row = 2;



            return pck;
        }

        public ExcelPackage GetExcelPauta(int pautaId)
        {
            var pauta = PdmContext.Pautas.Single(e => e.Id == pautaId);

            var template = new FileInfo(String.Format(@"{0}\Reports\Rpt_Pauta.xlsx", AppDomain.CurrentDomain.BaseDirectory));
            var pck = new ExcelPackage(template, true);
            var ws = pck.Workbook.Worksheets[1];
            var row = 2;



            return pck;
        }
    }
}
