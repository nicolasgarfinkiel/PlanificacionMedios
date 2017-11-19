using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using System.Web.Security;
using Irsa.PDM.Admin;
using Irsa.PDM.Dtos;
using Irsa.PDM.Infrastructure.Services;
using Irsa.PDM.Security.Service;

namespace Irsa.PDM.Security
{
    public class IrsaMembershipProvider : MembershipProvider
    {
        #region Properties

        private const int AppId = 1;

        #endregion

        public IrsaMembershipProvider()
        {

        }

        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            bool result = false;

            try
            {

            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password,
            string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email,
            string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey,
            out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { return true; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            var result = 0;

            throw new NotImplementedException();

            return result;
        }

        public override string GetPassword(string username, string answer)
        {
            var result = String.Empty;

            throw new NotImplementedException();

            return result;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();

        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return 3; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 10; }
        }

        public override int PasswordAttemptWindow
        {
            get { return 30; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return ".{10,}"; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        //public ResetPasswordProveedorResponse ResetPasswordProveedor(ResetPasswordProveedorRequest request)
        //{
        //    return _admin.ResetPasswordProveedor(request);
        //}

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            if (PDMSession.Current.Usuario == null)
            {
                username = @"IRSACORP\Sposzalski"; //TODO: comentar esta linea en prod
                Init(username);
            }

            return PDMSession.Current.Usuario != null &&  PDMSession.Current.Usuario.Roles.Any();
        }

        private void Init(string username)
        {            
            PDMSession.Current.Usuario = new Usuario
            {
                Nombre = username,
                Roles = GetRoles(username) 
            };            
        }

        private IList<string> GetRoles(string username)
        {
            var client = new WebServiceClient<ISecurityService>(ConfigurationManager.AppSettings["SecurityServiceUrl"],
               (binding, httpTransport, address, factory) =>
               {
                   var credentialBehaviour = factory.Endpoint.Behaviors.Find<ClientCredentials>();
                   credentialBehaviour.UserName.UserName = ConfigurationManager.AppSettings["SecurityServiceUser"];
                   credentialBehaviour.UserName.Password = ConfigurationManager.AppSettings["SecurityServicePassword"];
                   httpTransport.AuthenticationScheme = AuthenticationSchemes.Ntlm;
               });

            var roles = new List<string>();
            var usuario = client.Channel.UserLogonByName(username, AppId);

            if (usuario == null) return roles;

            client.Channel.GroupsListPerUser(usuario, AppId).ToList().ForEach(g =>
            {
                roles.AddRange(client.Channel.PermissionListPerGroup(g).Select(r => r.Description).ToList());
            });

            return roles;
        }       
    }
}