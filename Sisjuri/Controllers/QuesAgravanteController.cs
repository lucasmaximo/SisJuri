using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    public class QuesAgravanteController : BaseController
    {
        private JuriRepository _JuriRepository;
        private ProcessoRepository _ProcessoRepository;
        private QuesAgravanteRepository _QuesAgravanteRepository;

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
        public QuesAgravanteRepository QuesAgravanteRepository
        {
            get
            {
                if (_QuesAgravanteRepository == null)
                    _QuesAgravanteRepository = new QuesAgravanteRepository();
                return _QuesAgravanteRepository;
            }
            set { _QuesAgravanteRepository = value; }
        }


        // QuesAgravante/ListByIdProcess (Lista de quesitos agravantes por processo)
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult ListByIdProcess(int id, String message)
        {
            ViewData["message"] = message;
            return View(QuesAgravanteRepository.GetAllByIdProcess(id));
        }

        //
        // GET: /QuesAgravante/Details/5
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Details(int id)
        {
            return View(QuesAgravanteRepository.GetOne(id));
        }

        //
        // GET: /QuesAgravante/Create
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Create()
        {
            LoadFormProcesso();

            return View(new ques_agravante());
        }

        //
        // POST: /QuesAgravante/Create
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Create(ques_agravante ques_agravante)
        {
            LoadFormProcesso();

            try
            {
                if (validate(ques_agravante))
                    return View(ques_agravante);
                QuesAgravanteRepository.Create(ques_agravante);

                return RedirectToAction("ListByIdProcess", new { id = ques_agravante.fk_id_processo, message = "Dados cadastrados com sucesso!" });
            }
            catch
            {
                return View(ques_agravante);
            }
        }

        //
        // GET: /QuesAgravante/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Edit(int id)
        {
            LoadFormProcesso();

            return View(QuesAgravanteRepository.GetOne(id));
        }

        //
        // POST: /QuesAgravante/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Edit(int id, ques_agravante ques_agravante)
        {
            LoadFormProcesso();

            try
            {
                if (validate(ques_agravante))
                    return View(ques_agravante);
                QuesAgravanteRepository.Edit(ques_agravante);
                return RedirectToAction("ListByIdProcess", new { id = ques_agravante.fk_id_processo, message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /QuesAgravante/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Delete(int id)
        {
            return View(QuesAgravanteRepository.GetOne(id));
        }

        //
        // POST: /QuesAgravante/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Delete(int id, ques_agravante ques_agravante)
        {
            try
            {
                ques_agravante = QuesAgravanteRepository.GetOne(id);
                QuesAgravanteRepository.Delete(ques_agravante);

                return RedirectToAction("ListByIdProcess", new { id = ques_agravante.fk_id_processo, message = "Dados excluídos com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        public bool validate(ques_agravante entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.quesito_agravante))
            {
                ModelState.AddModelError("quesito_agravante", "Campo obrigatório");
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
