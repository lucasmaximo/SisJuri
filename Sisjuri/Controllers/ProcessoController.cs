using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    public class ProcessoController : BaseController
    {

        private JuriRepository _JuriRepository;
        private ProcessoRepository _ProcessoRepository;

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

        //
        // GET: /Processo/
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Index()
        {
            LoadFormJuri();
            return View(new processo());
        }
        
        // Processo/List
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult List(processo entity, String message, String messageError)
        {
            ViewData["message"] = message;
            ViewData["messageError"] = messageError;
            if (string.IsNullOrEmpty(entity.num_processo) && (entity.fk_id_juri.Equals(0)))
            {
                return View(ProcessoRepository.GetAll());
            }
            else
            {
                return View(ProcessoRepository.GetAllByCriteria(entity.num_processo, entity.fk_id_juri));
            }
        }


        //
        // GET: /Processo/Details/5
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Details(int id)
        {
            return View(ProcessoRepository.GetOne(id));
        }

        //
        // GET: /Processo/Create
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Create()
        {
            LoadFormJuri();

            return View(new processo());
        }

        //
        // POST: /Processo/Create
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Create(processo processo)
        {
            LoadFormJuri();

            try
            {
                if (validate(processo))
                    return View(processo);

                ProcessoRepository.Create(processo);
                return RedirectToAction("List", new { message = "Dados cadastrados com sucesso!" });
            }
            catch
            {
                return View(processo);
            }
        }

        //
        // GET: /Processo/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Edit(int id)
        {
            LoadFormJuri();

            return View(ProcessoRepository.GetOne(id));
        }

        //
        // POST: /Processo/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Edit(int id, processo processo)
        {
            LoadFormJuri();

            try
            {
                if (validate(processo))
                    return View(processo);
                ProcessoRepository.Edit(processo);
                return RedirectToAction("List", new { message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Processo/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Delete(int id)
        {
            return View(ProcessoRepository.GetOne(id));
        }

        //
        // POST: /Processo/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Delete(int id, processo processo)
        {
            try
            {
                processo = ProcessoRepository.GetOne(id);
                if (processo.ques_materialidade.Count > 0 || processo.ques_autoria.Count > 0 || processo.ques_absolvicao.Count > 0 ||
                    processo.ques_atenuante.Count > 0 || processo.ques_agravante.Count > 0 || processo.ques_tentativa.Count > 0 ||
                    processo.vitima.Count > 0 || processo.reu.Count > 0 || processo.sentenca.Count > 0)
                    return RedirectToAction("List", new { messageError = "Esse processo possui registros vinculados a seu cadastro. Não é possível excluí-lo." });

                ProcessoRepository.Delete(processo);

                return RedirectToAction("List", new { message = "Dados excluídos com sucesso!" });
            }
            catch
            {
                return View();
            }
        }



        public bool validate(processo entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.num_processo))
            {
                ModelState.AddModelError("num_processo", "Campo obrigatório");
                retorno = true;
            }

            return retorno;
        }


        public void LoadFormJuri()
        {
            IEnumerable<juri> lst = JuriRepository.GetAll();
            int c = lst.Count();
            ViewData["lstJuri"] = lst;
        }

    }
}
