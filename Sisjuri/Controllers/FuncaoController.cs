using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    public class FuncaoController : BaseController
    {
        private FuncaoRepository _FuncaoRepository;

        public FuncaoRepository FuncaoRepository
        {
            get {
                if (_FuncaoRepository == null)
                    _FuncaoRepository = new FuncaoRepository();
                return _FuncaoRepository; }
            set { _FuncaoRepository = value; }
        }

        //
        // GET: /Função/
        public ActionResult Index()
        {
            return View(new funcao());
        }

        // Função/List
        public ActionResult List(funcao entity, String message)
        {
            ViewData["message"] = message;
            if (string.IsNullOrEmpty(entity.nome_funcao))
            {
                return View(FuncaoRepository.GetAll());
            }
            else
            {
                return View(FuncaoRepository.GetAllByCriteria(entity.nome_funcao));
            }
        }

        //
        // GET: /Função/Details/5
        public ActionResult Details(int id)
        {
            return View(FuncaoRepository.GetOne(id));
        }

        //
        // GET: /Função/Create
        public ActionResult Create()
        {
            return View(new funcao());
        }

        //
        // POST: /Função/Create
        [HttpPost]
        public ActionResult Create(funcao funcao)
        {
            try
            {

                if (validate(funcao))
                    return View(funcao);
                FuncaoRepository.Create(funcao);

                return RedirectToAction("List", new { message = "Dados criados com sucesso!" });
            }
            catch
            {
                return View(funcao);
            }
        }

        //
        // GET: /Função/Edit/5
        public ActionResult Edit(int id)
        {
            return View(FuncaoRepository.GetOne(id));
        }

        //
        // POST: /Função/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, funcao funcao)
        {
            try
            {
                if (validate(funcao))
                    return View(funcao);
                FuncaoRepository.Edit(funcao);
                return RedirectToAction("List", new { message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Função/Delete/5
        public ActionResult Delete(int id)
        {
            return View(FuncaoRepository.GetOne(id));
        }

        //
        // POST: /Função/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, funcao funcao)
        {
            try
            {
                funcao = FuncaoRepository.GetOne(id);
                FuncaoRepository.Delete(funcao);

                return RedirectToAction("List", new { message = "Dados apagados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        public bool validate(funcao entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.nome_funcao))
            {
                ModelState.AddModelError("nome_funcao", "Campo obrigatório");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.descricao_funcao))
            {
                ModelState.AddModelError("descricao_funcao", "Campo obrigatório");
                retorno = true;
            }

            return retorno;
        }
    }
}
