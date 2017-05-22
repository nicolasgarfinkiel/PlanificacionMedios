using System;
using System.Collections.Generic;
using System.Linq;
using Irsa.PDM.Dtos;
using Irsa.PDM.Dtos.Common;
using Irsa.PDM.Entities;
using ServiceStack.ServiceClient.Web;

namespace Irsa.PDM.Admin
{
    public class CampaniasAdmin : BaseAdmin<int, Entities.Campania, Dtos.Campania, FilterBase>
    {        
        private const string ImportUser = "Import process";
        private const string GetPautas = "/client?method=get-list&action=pautas_a_aprobar";

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

                    if (pauta.Estado != EstadoPauta.Pendiente) return;

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


                });

                #endregion              
            });

            PdmContext.SaveChanges();
        }

        #endregion
    }
}
