using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    public class QuesAbsolvicaoController : BaseController
    {
        private JuriRepository _JuriRepository;
        private ProcessoRepository _ProcessoRepository;
        private QuesAbsolvicaoRepository _QuesAbsolvicaoRepository;

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
        public QuesAbsolvicaoRepository QuesAbsolvicaoRepository
        {
            get {
                if (_QuesAbsolvicaoRepository == null)
                    _QuesAbsolvicaoRepository = new QuesAbsolvicaoRepository();
                return _QuesAbsolvicaoRepository; }
            set { _QuesAbsolvicaoRepository = value; }
        }


        // GET: /QuesAbsolvicao/ListByIdProcess (Lista de quesitos da absolvição por processo)
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult ListByIdProcess(int id, String message)
        {
            ViewData["message"] = message;
            return View(QuesAbsolvicaoRepository.GetAllByIdProcess(id));
        }

        //
        // GET: /QuesAbsolvicao/Details/5

        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Details(int id)
        {
            return View(QuesAbsolvicaoRepository.GetOne(id));
        }

        //
        // GET: /QuesAbsolvicao/Create
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Create()
        {
            LoadFormProcesso();

            return View(new ques_absolvicao());
        }

        //
        // POST: /QuesAbsolvicao/Create
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Create(ques_absolvicao ques_absolvicao)
        {
            LoadFormProcesso();

            try
            {
                if (validate(ques_absolvicao))
                    return View(ques_absolvicao);
                QuesAbsolvicaoRepository.Create(ques_absolvicao);

                return RedirectToAction("ListByIdProcess", new { id = ques_absolvicao.fk_id_processo, message = "Dados cadastrados com sucesso!" });
            }
            catch
            {
                return View(ques_absolvicao);
            }
        }

        //
        // GET: /QuesAbsolvicao/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Edit(int id)
        {
            LoadFormProcesso();

            return View(QuesAbsolvicaoRepository.GetOne(id));
        }

        //
        // POST: /QuesAbsolvicao/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Edit(int id, ques_absolvicao ques_absolvicao)
        {
            LoadFormProcesso();

            try
            {
                if (validate(ques_absolvicao))
                    return View(ques_absolvicao);
                QuesAbsolvicaoRepository.Edit(ques_absolvicao);
                return RedirectToAction("ListByIdProcess", new { id = ques_absolvicao.fk_id_processo, message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /QuesAbsolvicao/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Delete(int id)
        {
            return View(QuesAbsolvicaoRepository.GetOne(id));
        }

        //
        // POST: /QuesAbsolvicao/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Delete(int id, ques_absolvicao ques_absolvicao)
        {
            try
            {
                ques_absolvicao = QuesAbsolvicaoRepository.GetOne(id);
                QuesAbsolvicaoRepository.Delete(ques_absolvicao);

                return RedirectToAction("ListByIdProcess", new { id = ques_absolvicao.fk_id_processo, message = "Dados excluídos com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        public bool validate(ques_absolvicao entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.quesito_absolvicao))
            {
                ModelState.AddModelError("ques_absolvicao", "Campo obrigatório");
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
