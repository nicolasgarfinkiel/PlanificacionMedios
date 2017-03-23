namespace Irsa.PDM.Dtos.Common
{
    public class FilterBase
    {
        public string MultiColumnSearchText { get; set; }
        public int IdGrupoEmpresa { get; set; }
        public int EmpresaId { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
    }     
}
