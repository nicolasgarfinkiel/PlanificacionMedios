using System;
using System.Collections.Generic;
using System.Linq;
using Irsa.PDM.Dtos;

namespace Irsa.PDM.Security
{
    public class IrsaRoleProvider : System.Web.Security.RoleProvider
    {
        #region Properties
        
        #endregion 

        public IrsaRoleProvider()
        {
        
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();      
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();          
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            bool result;

            throw new NotImplementedException();       

            return result;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            var result = new string[]{};

            throw new NotImplementedException();      

            return result;
        }

        public override string[] GetAllRoles()
        {
            return null;
        }

        public override string[] GetRolesForUser(string username)
        {
            if (PDMSession.Current.Usuario == null || PDMSession.Current.Usuario.Roles == null ) return new List<string>().ToArray();

            return PDMSession.Current.Usuario.Roles.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            var result = new string[]{};

            throw new NotImplementedException();        

            return result;
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return PDMSession.Current.Usuario != null &&                  
                   PDMSession.Current.Usuario.Roles != null &&
                   PDMSession.Current.Usuario.Roles.Any(r => string.Equals(r, roleName));
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();    
        }

        public override bool RoleExists(string roleName)
        {
            bool result;

            throw new NotImplementedException();     

            return result;
        }
    }
}