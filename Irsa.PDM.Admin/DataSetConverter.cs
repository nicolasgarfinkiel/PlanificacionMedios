using System.Data;

namespace Irsa.PDM.Admin
{
    //public static class DataSetConverter
    //{
    //    public static DataSet GetDataSet(RemitoParaguay remito)
    //    {
    //        var result = new DataSet();
    //        var table = result.Tables.Add("RemitoCrescaDS");

    //        table.Columns.Add(new DataColumn("idsolicitud"));
    //        table.Columns.Add(new DataColumn("Descripcion"));
    //        table.Columns.Add(new DataColumn("Cee"));
    //        table.Columns.Add(new DataColumn("TranspRazonSocial"));
    //        table.Columns.Add(new DataColumn("TransportistaCUIT"));
    //        table.Columns.Add(new DataColumn("FechaCreacion"));
    //        table.Columns.Add(new DataColumn("FechaVencimiento"));
    //        table.Columns.Add(new DataColumn("NumeroRemision"));
    //        table.Columns.Add(new DataColumn("FechaDeEmision"));
    //        table.Columns.Add(new DataColumn("RazonSocial"));
    //        table.Columns.Add(new DataColumn("CUIT"));
    //        table.Columns.Add(new DataColumn("Direccion"));
    //        table.Columns.Add(new DataColumn("MotivoTraslado"));
    //        table.Columns.Add(new DataColumn("CteDeVta"));
    //        table.Columns.Add(new DataColumn("EPDireccion"));
    //        table.Columns.Add(new DataColumn("LocPartida"));
    //        table.Columns.Add(new DataColumn("ProvPartida"));
    //        table.Columns.Add(new DataColumn("EDDireccion"));
    //        table.Columns.Add(new DataColumn("LocLlegada"));
    //        table.Columns.Add(new DataColumn("ProvLlegada"));
    //        table.Columns.Add(new DataColumn("KmRecorridos"));
    //        table.Columns.Add(new DataColumn("PatenteCamion"));
    //        table.Columns.Add(new DataColumn("PatenteAcoplado"));
    //        table.Columns.Add(new DataColumn("ChoferRazonSocial"));
    //        table.Columns.Add(new DataColumn("ChoferCUIT"));
    //        table.Columns.Add(new DataColumn("ChoferDomicilio"));
    //        table.Columns.Add(new DataColumn("MarcaVehiculo"));
    //        table.Columns.Add(new DataColumn("Cantidad"));
    //        table.Columns.Add(new DataColumn("KG"));
    //        table.Columns.Add(new DataColumn("DescripcionDetallada"));
    //        table.Columns.Add(new DataColumn("HabilitacionNum"));

    //        var row = table.NewRow();

    //        row["idsolicitud"] = remito.Id.ToString();
    //        row["Descripcion"] = remito.Descripcion;
    //        row["Cee"] = remito.Cee;
    //        row["FechaCreacion"] = remito.FechaCreacion.Value.ToShortDateString();
    //        row["FechaVencimiento"] = remito.FechaVencimiento.HasValue ? remito.FechaVencimiento.Value.ToShortDateString() : string.Empty;
    //        row["NumeroRemision"] = remito.NumeroRemision;
    //        row["FechaDeEmision"] = remito.FechaDeEmision.HasValue ? remito.FechaDeEmision.Value.ToShortDateString() : string.Empty;
    //        row["RazonSocial"] = remito.RazonSocial;
    //        row["CUIT"] = remito.Cuit;
    //        row["Direccion"] = remito.Direccion;
    //        row["MotivoTraslado"] = remito.MotivoTraslado;
    //        row["CteDeVta"] = remito.CteDeVta;
    //        row["EPDireccion"] = remito.EPDireccion;
    //        row["LocPartida"] = remito.LocPartida;
    //        row["ProvPartida"] = remito.ProvPartida;
    //        row["EDDireccion"] = remito.EDDireccion;
    //        row["LocLlegada"] = remito.LocLlegada;
    //        row["ProvLlegada"] = remito.ProvLlegada;
    //        row["KmRecorridos"] = remito.KmRecorridos;
    //        row["PatenteCamion"] = remito.PatenteCamion;
    //        row["PatenteAcoplado"] = remito.PatenteAcoplado;
    //        row["ChoferRazonSocial"] = remito.ChoferRazonSocial;
    //        row["ChoferCUIT"] = remito.ChoferCuit;
    //        row["ChoferDomicilio"] = remito.ChoferDomicilio;
    //        row["MarcaVehiculo"] = remito.MarcaVehiculo;
    //        row["Cantidad"] = remito.Cantidad;
    //        row["KG"] = remito.Kg;
    //        row["DescripcionDetallada"] = remito.DescripcionDetallada;
    //        row["TranspRazonSocial"] = remito.TranspRazonSocial;
    //        row["TransportistaCUIT"] = remito.TransportistaCuit;
    //        row["HabilitacionNum"] = remito.HabilitacionNum;

    //        table.Rows.Add(row);            

    //        return result;
    //    }
    //}
}