using System.Collections.Generic;
using System.ServiceModel;
using Irsa.PDM.Security.Service.Dtos;

namespace Irsa.PDM.Security.Service
{
        
    [ServiceContract(Namespace = "http://framework.irsa.com.ar/WebServices/Security/"), XmlSerializerFormat]
    public interface ISecurityService
    {
        [OperationContract(Action = "http://framework.irsa.com.ar/WebServices/Security/UserLogonByName", ReplyAction = "http://framework.irsa.com.ar/WebServices/Security/UserLogonByName")] 
        UserLogonByNameResult UserLogonByName(string ntUserName, int idApplication);

        [OperationContract(Action = "http://framework.irsa.com.ar/WebServices/Security/GroupsListPerUser", ReplyAction = "http://framework.irsa.com.ar/WebServices/Security/GroupsListPerUser")]
        List<Group> GroupsListPerUser(UserLogonByNameResult user, int idApplication);

        [OperationContract(Action = "http://framework.irsa.com.ar/WebServices/Security/PermissionListPerGroup", ReplyAction = "http://framework.irsa.com.ar/WebServices/Security/PermissionListPerGroup")]
        List<Permission> PermissionListPerGroup(Group grupo);
    }
}
