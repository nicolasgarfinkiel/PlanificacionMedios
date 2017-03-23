using System.Runtime.Serialization;

namespace Irsa.PDM.Security.Service.Dtos
{
    [DataContract(Name = "UserLogonByNameResult", Namespace = "")]
    public class UserLogonByNameResult
    {
        [DataMember(IsRequired = true)]
        public int Id { get; set; }

        [DataMember(IsRequired = true)]
        public bool Online { get; set; }

        [DataMember(IsRequired = true)]
        public string Nombre { get; set; }

        [DataMember(IsRequired = true)]
        public string Apellido { get; set; }

        [DataMember(IsRequired = true)]
        public string Telefono { get; set; }

        [DataMember(IsRequired = true)]
        public string Email { get; set; }

        [DataMember(IsRequired = true)]
        public string UserName { get; set; }

        [DataMember(IsRequired = true)]
        public int IdLanguage { get; set; }

    }
}
