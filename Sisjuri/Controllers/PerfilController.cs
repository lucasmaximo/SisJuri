using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    public class PerfilController : BaseController
    {
        private PerfilRepository _PerfilRepository;

        public PerfilRepository PerfilRepository
        {
            get {
                if (_PerfilRepository == null)
                    _PerfilRepository = new PerfilRepository();
                return _PerfilRepository; }
            set { _PerfilRepository = value; }
        }


        //
        // GET: /Perfil/
        public ActionResult Index()
        {
            return View(new perfil());
        }

        // /Perfil/List
        public ActionResult List(perfil entity, String message)
        {
            ViewData["message"] = message;
            if (string.IsNullOrEmpty(entity.nome_perfil))
            {
                return View(PerfilRepository.GetAll());
            }
            else
            {
                return View(PerfilRepository.GetAllByCriteria(entity.nome_perfil));
            }
        }

        //
        // GET: /Perfil/Details/5
        public ActionResult Details(int id)
        {
            return View(PerfilRepository.GetOne(id));
        }

        //
        // GET: /Perfil/Create
        public ActionResult Create()
        {
            return View(new perfil());
        }

        //
        // POST: /Perfil/Create
        [HttpPost]
        public ActionResult Create(perfil perfil)
        {
            try
            {
                if (validate(perfil))
                    return View(perfil);
                PerfilRepository.Create(perfil);

                return RedirectToAction("List", new { message = "Dados cadastrados com sucesso!" });
            }
            catch
            {
                return View(perfil);
            }
        }

        //
        // GET: /Perfil/Edit/5
        public ActionResult Edit(int id)
        {
            return View(PerfilRepository.GetOne(id));
        }

        //
        // POST: /Perfil/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, perfil perfil)
        {
            try
            {
                if (validate(perfil))
                    return View(perfil);
                PerfilRepository.Edit(perfil);
                return RedirectToAction("List", new { message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Perfil/Delete/5
        public ActionResult Delete(int id)
        {
            return View(PerfilRepository.GetOne(id));
        }

        //
        // POST: /Perfil/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, perfil perfil)
        {
            try
            {
                perfil = PerfilRepository.GetOne(id);
                PerfilRepository.Delete(perfil);

                return RedirectToAction("List", new { message = "Dados excluídos com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        public bool validate(perfil entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.nome_perfil))
            {
                ModelState.AddModelError("nome_perfil", "Campo obrigatório");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.descricao_perfil))
            {
                ModelState.AddModelError("descricao_perfil", "Campo obrigatório");
                retorno = true;
            }

            return retorno;
        }

    }
}
