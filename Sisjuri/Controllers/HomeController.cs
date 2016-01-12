using System;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    public class HomeController : BaseController
    {

        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Index(String message)
        {
            ViewData["message"] = message;
            return View();
        }
    }
}