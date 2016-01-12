using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    public class QuesTentativaController : BaseController
    {
        private JuriRepository _JuriRepository;
        private ProcessoRepository _ProcessoRepository;
        private QuesTentativaRepository _QuesTentativaRepository;

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
        public QuesTentativaRepository QuesTentativaRepository
        {
            get
            {
                if (_QuesTentativaRepository == null)
                    _QuesTentativaRepository = new QuesTentativaRepository();
                return _QuesTentativaRepository;
            }
            set { _QuesTentativaRepository = value; }
        }


        // QuesTentativa/List (Lista de quesitos da tentativa por processo)
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult ListByIdProcess(int id, String message)
        {
            ViewData["message"] = message;
            return View(QuesTentativaRepository.GetAllByIdProcess(id));
        }

        //
        // GET: /QuesTentativa/Details/5

        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Details(int id)
        {
            return View(QuesTentativaRepository.GetOne(id));
        }

        //
        // GET: /QuesTentativa/Create
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Create()
        {
            LoadFormProcesso();

            return View(new ques_tentativa());
        }

        //
        // POST: /QuesTentativa/Create
        [Authorize(Roles = "Administrador, Organizador")]        
        [HttpPost]
        public ActionResult Create(ques_tentativa ques_tentativa)
        {
            LoadFormProcesso();

            try
            {
                if (validate(ques_tentativa))
                    return View(ques_tentativa);
                QuesTentativaRepository.Create(ques_tentativa);

                return RedirectToAction("ListByIdProcess", new { id = ques_tentativa.fk_id_processo, message = "Dados cadastrados com sucesso!" });
            }
            catch
            {
                return View(ques_tentativa);
            }
        }

        //
        // GET: /QuesTentativa/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Edit(int id)
        {
            LoadFormProcesso();

            return View(QuesTentativaRepository.GetOne(id));
        }

        //
        // POST: /QuesTentativa/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Edit(int id, ques_tentativa ques_tentativa)
        {
            LoadFormProcesso();

            try
            {
                if (validate(ques_tentativa))
                    return View(ques_tentativa);
                QuesTentativaRepository.Edit(ques_tentativa);
                return RedirectToAction("ListByIdProcess", new { id = ques_tentativa.fk_id_processo, message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /QuesTentativa/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Delete(int id)
        {
            return View(QuesTentativaRepository.GetOne(id));
        }

        //
        // POST: /QuesMaterialidade/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Delete(int id, ques_tentativa ques_tentativa)
        {
            try
            {
                ques_tentativa = QuesTentativaRepository.GetOne(id);
                QuesTentativaRepository.Delete(ques_tentativa);

                return RedirectToAction("ListByIdProcess", new { id = ques_tentativa.fk_id_processo, message = "Dados excluídos com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        public bool validate(ques_tentativa entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.quesito_tentativa))
            {
                ModelState.AddModelError("quesito_tentativa", "Campo obrigatório");
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
