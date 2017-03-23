using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using Irsa.PDM.Infrastructure;

namespace Irsa.PDM.MainWebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                FormsAuthentication.SignOut();

            if (Membership.ValidateUser(WindowsIdentity.GetCurrent().Name, null))
            {
                FormsAuthentication.SetAuthCookie(WindowsIdentity.GetCurrent().Name, false);
                return RedirectToLocal(returnUrl);
            }

            return RedirectToAction("NoTienePermisos");
        }

        [AllowAnonymous]
        public ActionResult NotienePermisos()
        {
            return View();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult ConnectionStatus()
        {
            return this.JsonNet(new { Status = true });
        }

        [AllowAnonymous]
        public ActionResult SessionStatus()
        {
            return this.JsonNet(new { Status = Request.IsAuthenticated });
        }

    }
}
