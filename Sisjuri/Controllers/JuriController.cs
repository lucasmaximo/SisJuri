using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    public class JuriController : BaseController
    {
        private FaculdadeRepository _FaculdadeRepository;
        private NPJRepository _NPJRepository;
        private JuriRepository _JuriRepository;

        public FaculdadeRepository FaculdadeRepository
        {
            get {
                if (_FaculdadeRepository == null)
                    _FaculdadeRepository = new FaculdadeRepository();
                return _FaculdadeRepository; }
            set { _FaculdadeRepository = value; }
        }

        public NPJRepository NPJRepository
        {
            get {
                if (_NPJRepository == null)
                    _NPJRepository = new NPJRepository(); 
                return _NPJRepository; }
            set { _NPJRepository = value; }
        }

        public JuriRepository JuriRepository
        {
            get {
                if (_JuriRepository == null)
                    _JuriRepository = new JuriRepository();
                return _JuriRepository; }
            set { _JuriRepository = value; }
        }

        //
        // GET: /Juri/
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Index()
        {
            return View(new juri());
        }

        // Juri/List
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult List(juri entity, String message, String messageError)
        {
            ViewData["message"] = message;
            ViewData["messageError"] = messageError;
            if (string.IsNullOrEmpty(entity.nome_juri))
            {
                return View(JuriRepository.GetAll());
            }
            else
            {
                return View(JuriRepository.GetAllByCriteria(entity.nome_juri));
            }
        }

        //
        // GET: /Juri/Details/5
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Details(int id)
        {
            return View(JuriRepository.GetOne(id));
        }

        //
        // GET: /Juri/Create
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Create()
        {
            LoadFormNPJ();

            return View(new juri());
        }

        //
        // POST: /Juri/Create
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Create(juri juri, usuario usuario)
        {
            LoadFormNPJ();

            try
            {
                if (validate(juri))
                    return View(juri);

                juri.juri_ativo = true;
                JuriRepository.Create(juri);

                return RedirectToAction("List", new { message = "Dados cadastrados com sucesso!" });
            }
            catch
            {
                return View(juri);
            }
        }

        //
        // GET: /Juri/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Edit(int id)
        {
            LoadFormNPJ();

            return View(JuriRepository.GetOne(id));
        }

        //
        // POST: /Juri/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Edit(int id, juri juri)
        {
            LoadFormNPJ();

            try
            {
                if (validate(juri))
                    return View(juri);

                JuriRepository.Edit(juri);
                return RedirectToAction("List", new { message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Juri/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Delete(int id)
        {
            return View(JuriRepository.GetOne(id));
        }

        //
        // POST: /Juri/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Delete(int id, juri juri)
        {
            try
            {
                juri = JuriRepository.GetOne(id);
                if (juri.processo.Count > 0 || juri.documento.Count > 0 || juri.foto.Count > 0 || juri.inscricao.Count > 0 || juri.presenca.Count > 0)
                    return RedirectToAction("List", new { messageError = "Esse júri simulado possui registros vinculados a seu cadastro. Não é possível excluí-lo." });

                JuriRepository.Delete(juri);

                return RedirectToAction("List", new { message = "Dados excluídos com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        public bool validate(juri entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.nome_juri))
            {
                ModelState.AddModelError("nome_juri", "Campo obrigatório");
                retorno = true;
            }
            if (entity.data_hora_juri == DateTime.MinValue)
            {
                ModelState.AddModelError("data_hora_juri", "Campo obrigatório");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.local_juri))
            {
                ModelState.AddModelError("local_juri", "Campo obrigatório");
                retorno = true;
            }

            return retorno;
        }

        public void LoadFormNPJ()
        {
            IEnumerable<npj> lst = NPJRepository.GetAll();
            int c = lst.Count();
            ViewData["lstNPJ"] = lst;
        }
    }
}
