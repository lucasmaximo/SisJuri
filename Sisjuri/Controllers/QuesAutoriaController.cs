using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    public class QuesAutoriaController : BaseController
    {
        JuriRepository _JuriRepository;
        ProcessoRepository _ProcessoRepository;
        QuesAutoriaRepository _QuesAutoriaRepository;

        public JuriRepository JuriRepository
        {
            get {
                if (_JuriRepository == null)
                    _JuriRepository = new JuriRepository();
                return _JuriRepository;
            }
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
        public QuesAutoriaRepository QuesAutoriaRepository
        {
            get {
                if (_QuesAutoriaRepository == null)
                    _QuesAutoriaRepository = new QuesAutoriaRepository();
                return _QuesAutoriaRepository; }
            set { _QuesAutoriaRepository = value; }
        }

        // QuesAutoria/ListByIdProcess (Lista de quesitos da autoria por processo)
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult ListByIdProcess(int id, String message)
        {
            ViewData["message"] = message;
            return View(QuesAutoriaRepository.GetAllByIdProcess(id));
        }

        //
        // GET: /QuesAutoria/Details/5

        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Details(int id)
        {
            return View(QuesAutoriaRepository.GetOne(id));
        }

        //
        // GET: /QuesAutoria/Create
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Create()
        {
            LoadFormProcesso();

            return View(new ques_autoria());
        }

        //
        // POST: /QuesAutoria/Create
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Create(ques_autoria ques_autoria)
        {
            LoadFormProcesso();

            try
            {
                if (validate(ques_autoria))
                    return View(ques_autoria);
                QuesAutoriaRepository.Create(ques_autoria);

                return RedirectToAction("ListByIdProcess", new { id = ques_autoria.fk_id_processo, message = "Dados cadastrados com sucesso!" });
            }
            catch
            {
                return View(ques_autoria);
            }
        }

        //
        // GET: /QuesAutoria/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Edit(int id)
        {
            LoadFormProcesso();

            return View(QuesAutoriaRepository.GetOne(id));
        }

        //
        // POST: /QuesAutoria/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Edit(int id, ques_autoria ques_autoria)
        {
            LoadFormProcesso();

            try
            {
                if (validate(ques_autoria))
                    return View(ques_autoria);
                QuesAutoriaRepository.Edit(ques_autoria);
                return RedirectToAction("ListByIdProcess", new { id = ques_autoria.fk_id_processo, message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /QuesAutoria/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Delete(int id)
        {
            return View(QuesAutoriaRepository.GetOne(id));
        }

        //
        // POST: /QuesAutoria/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Delete(int id, ques_autoria ques_autoria)
        {
            try
            {
                ques_autoria = QuesAutoriaRepository.GetOne(id);
                QuesAutoriaRepository.Delete(ques_autoria);

                return RedirectToAction("ListByIdProcess", new { id = ques_autoria.fk_id_processo, message = "Dados excluídos com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        public bool validate(ques_autoria entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.quesito_autoria))
            {
                ModelState.AddModelError("quesito_autoria", "Campo obrigatório");
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
