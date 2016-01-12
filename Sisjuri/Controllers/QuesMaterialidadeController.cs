using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    public class QuesMaterialidadeController : BaseController
    {
        private JuriRepository _JuriRepository;
        private ProcessoRepository _ProcessoRepository;
        private QuesMaterialidadeRepository _QuesMaterialidadeRepository;

        public JuriRepository JuriRepository
        {
            get {
                if (_JuriRepository == null)
                    _JuriRepository = new JuriRepository();
                return _JuriRepository; }
            set { _JuriRepository = value; }
        }
        public ProcessoRepository ProcessoRepository
        {
            get {
                if (_ProcessoRepository == null)
                    _ProcessoRepository = new ProcessoRepository();
                return _ProcessoRepository; }
            set { _ProcessoRepository = value; }
        }
        public QuesMaterialidadeRepository QuesMaterialidadeRepository
        {
            get
            {
                if (_QuesMaterialidadeRepository == null)
                    _QuesMaterialidadeRepository = new QuesMaterialidadeRepository();
                return _QuesMaterialidadeRepository;
            }
            set { _QuesMaterialidadeRepository = value; }
        }

        // QuesMaterialidade/ListByIdProcess (Lista de quesitos da materialidade por processo)
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult ListByIdProcess(int id, String message)
        {
            ViewData["message"] = message;
            return View(QuesMaterialidadeRepository.GetAllByIdProcess(id));
        }

        //
        // GET: /QuesMaterialidade/Details/5

        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Details(int id)
        {
            return View(QuesMaterialidadeRepository.GetOne(id));
        }

        //
        // GET: /QuesMaterialidade/Create
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Create()
        {
            LoadFormProcesso();

            return View(new ques_materialidade());
        }

        //
        // POST: /QuesMaterialidade/Create
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Create(ques_materialidade ques_materialidade)
        {
            LoadFormProcesso();

            try
            {
                if (validate(ques_materialidade))
                    return View(ques_materialidade);
                QuesMaterialidadeRepository.Create(ques_materialidade);

                return RedirectToAction("ListByIdProcess", new { id = ques_materialidade.fk_id_processo, message = "Dados cadastrados com sucesso!" });
            }
            catch
            {
                return View(ques_materialidade);
            }
        }

        //
        // GET: /QuesMaterialidade/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Edit(int id)
        {
            LoadFormProcesso();

            return View(QuesMaterialidadeRepository.GetOne(id));
        }

        //
        // POST: /QuesMaterialidade/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Edit(int id, ques_materialidade ques_materialidade)
        {
            LoadFormProcesso();

            try
            {
                if (validate(ques_materialidade))
                    return View(ques_materialidade);
                QuesMaterialidadeRepository.Edit(ques_materialidade);
                return RedirectToAction("ListByIdProcess", new { id = ques_materialidade.fk_id_processo, message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /QuesMaterialidade/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Delete(int id)
        {
            return View(QuesMaterialidadeRepository.GetOne(id));
        }

        //
        // POST: /QuesMaterialidade/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Delete(int id, ques_materialidade ques_materialidade)
        {
            try
            {
                ques_materialidade = QuesMaterialidadeRepository.GetOne(id);
                QuesMaterialidadeRepository.Delete(ques_materialidade);

                return RedirectToAction("ListByIdProcess", new { id = ques_materialidade.fk_id_processo, message = "Dados excluídos com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        public bool validate(ques_materialidade entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.quesito_materialidade))
            {
                ModelState.AddModelError("quesito_materialidade", "Campo obrigatório");
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
