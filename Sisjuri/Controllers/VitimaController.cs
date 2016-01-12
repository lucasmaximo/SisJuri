using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    public class VitimaController : BaseController
    {
        private JuriRepository _JuriRepository;
        private ProcessoRepository _ProcessoRepository;
        private VitimaRepository _VitimaRepository;

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
        public VitimaRepository VitimaRepository
        {
            get
            {
                if (_VitimaRepository == null)
                    _VitimaRepository = new VitimaRepository();
                return _VitimaRepository;
            }
            set { _VitimaRepository = value; }
        }


        // Vitima/ListByIdProcess (Lista de vítimas por processo)
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult ListByIdProcess(int id, String message)
        {
            ViewData["message"] = message;
            return View(VitimaRepository.GetAllByIdProcess(id));
        }

        //
        // GET: /Vítima/Details/5

        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Details(int id)
        {
            return View(VitimaRepository.GetOne(id));
        }

        //
        // GET: /Vítima/Create
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Create()
        {
            LoadFormProcesso();

            return View(new vitima());
        }

        //
        // POST: /Vítima/Create
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Create(vitima vitima)
        {
            LoadFormProcesso();

            try
            {
                if (validate(vitima))
                    return View(vitima);
                VitimaRepository.Create(vitima);

                return RedirectToAction("ListByIdProcess", new { id = vitima.fk_id_processo, message = "Dados cadastrados com sucesso!" });
            }
            catch
            {
                return View(vitima);
            }
        }

        //
        // GET: /Vítima/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Edit(int id)
        {
            LoadFormProcesso();

            return View(VitimaRepository.GetOne(id));
        }

        //
        // POST: /Vítima/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Edit(int id, vitima vitima)
        {
            LoadFormProcesso();

            try
            {
                if (validate(vitima))
                    return View(vitima);
                VitimaRepository.Edit(vitima);
                return RedirectToAction("ListByIdProcess", new { id = vitima.fk_id_processo, message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Vítima/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Delete(int id)
        {
            return View(VitimaRepository.GetOne(id));
        }

        //
        // POST: /Vítima/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Delete(int id, vitima vitima)
        {
            try
            {
                vitima = VitimaRepository.GetOne(id);
                VitimaRepository.Delete(vitima);

                return RedirectToAction("ListByIdProcess", new { id = vitima.fk_id_processo, message = "Dados excluídos com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        public bool validate(vitima entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.nome_vitima))
            {
                ModelState.AddModelError("nome_vitima", "Campo obrigatório");
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
