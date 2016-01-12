using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    public class SentencaController : BaseController
    {
        private JuriRepository _JuriRepository;
        private ProcessoRepository _ProcessoRepository;
        private SentencaRepository _SentencaRepository;

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
        public SentencaRepository SentencaRepository
        {
            get
            {
                if (_SentencaRepository == null)
                    _SentencaRepository = new SentencaRepository();
                return _SentencaRepository;
            }
            set { _SentencaRepository = value; }
        }


        // Sentenca/List (Lista da sentença por processo)
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult ListByIdProcess(int id, String message)
        {
            ViewData["message"] = message;
            return View(SentencaRepository.GetAllByIdProcess(id));
        }

        //
        // GET: /Vítima/Details/5

        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Details(int id)
        {
            return View(SentencaRepository.GetOne(id));
        }

        //
        // GET: /Vítima/Create
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Create()
        {
            LoadFormProcesso();

            return View(new sentenca());
        }

        //
        // POST: /Vítima/Create
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Create(sentenca sentenca)
        {
            LoadFormProcesso();

            try
            {
                if (validate(sentenca))
                    return View(sentenca);
                SentencaRepository.Create(sentenca);

                int idProcesso = sentenca.fk_id_processo;
                processo processo = ProcessoRepository.GetOne(idProcesso);
                processo.juri.juri_ativo = false;
                ProcessoRepository.Edit(processo);

                return RedirectToAction("ListByIdProcess", new { id = sentenca.fk_id_processo, message = "Dados cadastrados com sucesso!" });
            }
            catch
            {
                return View(sentenca);
            }
        }

        //
        // GET: /Vítima/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Edit(int id)
        {
            LoadFormProcesso();

            return View(SentencaRepository.GetOne(id));
        }

        //
        // POST: /Vítima/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Edit(int id, sentenca sentenca)
        {
            LoadFormProcesso();

            try
            {
                if (validate(sentenca))
                    return View(sentenca);
                SentencaRepository.Edit(sentenca);
                return RedirectToAction("ListByIdProcess", new { id = sentenca.fk_id_processo, message = "Dados editados com sucesso!" });
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
            return View(SentencaRepository.GetOne(id));
        }

        //
        // POST: /Vítima/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Delete(int id, sentenca sentenca)
        {
            try
            {
                sentenca = SentencaRepository.GetOne(id);
                SentencaRepository.Delete(sentenca);

                return RedirectToAction("ListByIdProcess", new { id = sentenca.fk_id_processo, message = "Dados excluídos com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        public bool validate(sentenca entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.texto_sentenca))
            {
                ModelState.AddModelError("texto_sentenca", "Campo obrigatório");
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
