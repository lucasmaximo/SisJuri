using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    public class QuesAtenuanteController : BaseController
    {
        private JuriRepository _JuriRepository;
        private ProcessoRepository _ProcessoRepository;
        private QuesAtenuanteRepository _QuesAtenuanteRepository;

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
        public QuesAtenuanteRepository QuesAtenuanteRepository
        {
            get
            {
                if (_QuesAtenuanteRepository == null)
                    _QuesAtenuanteRepository = new QuesAtenuanteRepository();
                return _QuesAtenuanteRepository;
            }
            set { _QuesAtenuanteRepository = value; }
        }


        // QuesAtenuante/ListByIdProcess (Lista de quesitos atenuantes por processo)
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult ListByIdProcess(int id, String message)
        {
            ViewData["message"] = message;
            return View(QuesAtenuanteRepository.GetAllByIdProcess(id));
        }

        //
        // GET: /QuesAtenuante/Details/5
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Details(int id)
        {
            return View(QuesAtenuanteRepository.GetOne(id));
        }

        //
        // GET: /QuesAtenuante/Create
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Create()
        {
            LoadFormProcesso();

            return View(new ques_atenuante());
        }

        //
        // POST: /QuesAtenuante/Create
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Create(ques_atenuante ques_atenuante)
        {
            LoadFormProcesso();

            try
            {
                if (validate(ques_atenuante))
                    return View(ques_atenuante);
                QuesAtenuanteRepository.Create(ques_atenuante);

                return RedirectToAction("ListByIdProcess", new { id = ques_atenuante.fk_id_processo, message = "Dados cadastrados com sucesso!" });
            }
            catch
            {
                return View(ques_atenuante);
            }
        }

        //
        // GET: /QuesAtenuante/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Edit(int id)
        {
            LoadFormProcesso();

            return View(QuesAtenuanteRepository.GetOne(id));
        }

        //
        // POST: /QuesAtenuante/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Edit(int id, ques_atenuante ques_atenuante)
        {
            LoadFormProcesso();

            try
            {
                if (validate(ques_atenuante))
                    return View(ques_atenuante);
                QuesAtenuanteRepository.Edit(ques_atenuante);
                return RedirectToAction("ListByIdProcess", new { id = ques_atenuante.fk_id_processo, message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /QuesAtenuante/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Delete(int id)
        {
            return View(QuesAtenuanteRepository.GetOne(id));
        }

        //
        // POST: /QuesAtenuante/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Delete(int id, ques_atenuante ques_atenuante)
        {
            try
            {
                ques_atenuante = QuesAtenuanteRepository.GetOne(id);
                QuesAtenuanteRepository.Delete(ques_atenuante);

                return RedirectToAction("ListByIdProcess", new { id = ques_atenuante.fk_id_processo, message = "Dados excluídos com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        public bool validate(ques_atenuante entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.quesito_atenuante))
            {
                ModelState.AddModelError("quesito_atenuante", "Campo obrigatório");
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
