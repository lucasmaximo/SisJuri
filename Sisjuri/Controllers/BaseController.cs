using Sisjuri.Models;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Sisjuri.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (HttpContext.User != null && Request.IsAuthenticated)
            {
                if (Request.IsAuthenticated)
                {
                    string cookieName = FormsAuthentication.FormsCookieName;
                    HttpCookie authCookie = HttpContext.Request.Cookies[cookieName];

                    if (authCookie == null)
                        return;

                    if (Session["usuario"] == null)
                    {
                        FormsAuthentication.SignOut();
                        return;
                    }

                    usuario user = ((usuario)Session["usuario"]);
                    string[] roles = new string[1];
                    roles[0] = user.perfil.nome_perfil;

                    FormsIdentity formsIdentity = (FormsIdentity)(HttpContext.User.Identity);
                    HttpContext.User = new System.Security.Principal.GenericPrincipal(formsIdentity, roles);
                }
                else
                {
                    FormsAuthentication.SignOut();
                }
            }
        }
    }
}
