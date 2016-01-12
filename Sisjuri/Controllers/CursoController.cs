using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    [Authorize(Roles = "Administrador, Organizador")]
    public class CursoController : BaseController
    {
        private FaculdadeRepository _FaculdadeRepository;
        private CursoRepository _CursoRepository;

        public FaculdadeRepository FaculdadeRepository
        {
            get
            {
                if (_FaculdadeRepository == null)
                    _FaculdadeRepository = new FaculdadeRepository();
                return _FaculdadeRepository;
            }
            set { _FaculdadeRepository = value; }
        }

        public CursoRepository CursoRepository
        {
            get {
                if (_CursoRepository == null)
                    _CursoRepository = new CursoRepository();
                return _CursoRepository; }
            set { _CursoRepository = value; }
        }

        //
        // GET: /Curso/
        public ActionResult Index()
        {
            return View(new curso());
        }

        public ActionResult List(curso entity, String message, String messageError)
        {
            ViewData["message"] = message;
            ViewData["messageError"] = messageError;
            if (string.IsNullOrEmpty(entity.nome_curso))
            {
                return View(CursoRepository.GetAll());
            }
            else
            {
                return View(CursoRepository.GetAllByCriteria(entity.nome_curso));
            }
        }

        //
        // GET: /Curso/Details/5
        public ActionResult Details(int id)
        {
            return View(CursoRepository.GetOne(id));
        }

        //
        // GET: /Curso/Create
        public ActionResult Create()
        {
            LoadFormFaculdade(); 

            return View(new curso());
        }

        //
        // POST: /Curso/Create
        [HttpPost]
        public ActionResult Create(curso curso)
        {
            LoadFormFaculdade(); 

            try
            {
                if (validate(curso))
                    return View(curso);

                CursoRepository.Create(curso);

                return RedirectToAction("List", new { message = "Dados cadastrados com sucesso!" });
            }
            catch
            {
                return View(curso);
            }
        }

        //
        // GET: /Curso/Edit/5
        public ActionResult Edit(int id)
        {
            LoadFormFaculdade(); 

            return View(CursoRepository.GetOne(id));
        }

        //
        // POST: /Curso/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, curso curso)
        {
            LoadFormFaculdade(); 

            try
            {
                if (validate(curso))
                    return View(curso);

                CursoRepository.Edit(curso);
                return RedirectToAction("List", new { message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Curso/Delete/5
        public ActionResult Delete(int id)
        {
            return View(CursoRepository.GetOne(id));
        }

        //
        // POST: /Curso/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, curso curso)
        {
            try
            {
                curso = CursoRepository.GetOne(id);
                if (curso.usuario != null)
                    return RedirectToAction("List", new { messageError = "Esse curso possui registros vinculados a seu cadastro. Não é possível excluí-lo." });

                CursoRepository.Delete(curso);

                return RedirectToAction("List", new { message = "Dados excluídos com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        public bool validate(curso entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.nome_curso))
            {
                ModelState.AddModelError("nome_curso", "Campo obrigatório");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.descricao_curso))
            {
                ModelState.AddModelError("descricao_curso", "Campo obrigatório");
                retorno = true;
            }

            return retorno;
        }

        public void LoadFormFaculdade()
        {
            IEnumerable<faculdade> lst = FaculdadeRepository.GetAll();
            int c = lst.Count();
            ViewData["lstFaculdade"] = lst;
        }
    }
}
