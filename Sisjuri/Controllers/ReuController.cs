using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    public class ReuController : BaseController
    {
        private JuriRepository _JuriRepository;
        private ProcessoRepository _ProcessoRepository;
        private ReuRepository _ReuRepository;

        public JuriRepository JuriRepository
        {
            get
            {
                if (_JuriRepository == null)
                    _JuriRepository = new JuriRepository();
                return _JuriRepository;
            }
            set { _JuriRepository = value; }
        }
        public ProcessoRepository ProcessoRepository
        {
            get
            {
                if (_ProcessoRepository == null)
                    _ProcessoRepository = new ProcessoRepository();
                return _ProcessoRepository;
            }
            set { _ProcessoRepository = value; }
        }
        public ReuRepository ReuRepository
        {
            get
            {
                if (_ReuRepository == null)
                    _ReuRepository = new ReuRepository();
                return _ReuRepository;
            }
            set { _ReuRepository = value; }
        }


        // Reu/ListByIdProcess (Lista de réus por processo)
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult ListByIdProcess(int id, String message)
        {
            ViewData["message"] = message;
            return View(ReuRepository.GetAllByIdProcess(id));
        }

        //
        // GET: /Reu/Details/5

        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Details(int id)
        {
            return View(ReuRepository.GetOne(id));
        }

        //
        // GET: /Reu/Create
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Create()
        {
            LoadFormProcesso();

            return View(new reu());
        }

        //
        // POST: /Reu/Create
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Create(reu reu)
        {
            LoadFormProcesso();

            try
            {
                if (validate(reu))
                    return View(reu);
                ReuRepository.Create(reu);

                return RedirectToAction("ListByIdProcess", new { id = reu.fk_id_processo, message = "Dados cadastrados com sucesso!" });
            }
            catch
            {
                return View(reu);
            }
        }

        //
        // GET: /Reu/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Edit(int id)
        {
            LoadFormProcesso();

            return View(ReuRepository.GetOne(id));
        }

        //
        // POST: /Reu/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Edit(int id, reu reu)
        {
            LoadFormProcesso();

            try
            {
                if (validate(reu))
                    return View(reu);
                ReuRepository.Edit(reu);
                return RedirectToAction("ListByIdProcess", new { id = reu.fk_id_processo, message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Reu/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Delete(int id)
        {
            return View(ReuRepository.GetOne(id));
        }

        //
        // POST: /Reu/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Delete(int id, reu reu)
        {
            try
            {
                reu = ReuRepository.GetOne(id);
                ReuRepository.Delete(reu);

                return RedirectToAction("ListByIdProcess", new { id = reu.fk_id_processo, message = "Dados excluídos com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        public bool validate(reu entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.nome_reu))
            {
                ModelState.AddModelError("nome_reu", "Campo obrigatório");
                retorno = true;
            }

            return retorno;
        }

        public void LoadFormProcesso()
        {
            IEnumerable<processo> lst = ProcessoRepository.GetAll();
            int c = lst.Count();
            ViewData["lstProcesso"] = lst;
        }

    }
}
