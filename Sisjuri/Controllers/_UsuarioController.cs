using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sisjuri.Models;
using Sisjuri.Repository;

namespace Sisjuri.Controllers
{
    public class _UsuarioController : BaseController
    {
        DataModel DataModel = new DataModel();

        private UsuarioRepository _UsuarioRepository;

        public UsuarioRepository UsuarioRepository
        {
            get {
                if (_UsuarioRepository == null)
                    _UsuarioRepository = new UsuarioRepository();
                return _UsuarioRepository; }
            set { _UsuarioRepository = value; }
        }

        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult All()
        {
            List<usuario> model = DataModel.usuario.ToList();
            return PartialView("_Usuario", model);
        }
        public PartialViewResult Top3()
        {
            List<usuario> model = DataModel.usuario.OrderByDescending(x => x.cpf_usuario).Take(3).ToList();
            return PartialView("_Usuario", model);
        }
        public PartialViewResult Bottom3()
        {
            List<usuario> model = DataModel.usuario.OrderBy(x => x.cpf_usuario).Take(3).ToList();
            return PartialView("_Usuario", model);
        }
	}
}