using System.Runtime.Serialization;

namespace Irsa.PDM.Security.Service.Dtos
{

    [DataContract(Name = "Group", Namespace = "")]
    public class Group
    {
        [DataMember(IsRequired = true)]
        public int Id { get; set; }

        [DataMember(IsRequired = true)]        
        public int IdApplication { get; set; }

        [DataMember(IsRequired = true)]        
        public string Name { get; set; }

        [DataMember(IsRequired = true)]        
        public string Description { get; set; }
    }  
}
