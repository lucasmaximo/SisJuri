using Sisjuri.Helpers;
using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    public class DocumentoController : BaseController
    {
        JuriRepository _JuriRepository;
        DocumentoRepository _DocumentoRepository;

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
        public DocumentoRepository DocumentoRepository
        {
            get {
                if (_DocumentoRepository == null)
                    _DocumentoRepository = new DocumentoRepository();
                return _DocumentoRepository; }
            set { _DocumentoRepository = value; }
        }

        //
        // GET: /Documento/
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Index()
        {
            LoadFormJuri();
            return View(new documento());
        }

        // Documento/List
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult List(documento entity, String message)
        {
            ViewData["message"] = message;
            if (string.IsNullOrEmpty(entity.nome_documento) && (entity.fk_id_juri.Equals(0)))
            {
                return View(DocumentoRepository.GetAll());
            }
            else
            {
                return View(DocumentoRepository.GetAllByCriteria(entity.nome_documento, entity.fk_id_juri));
            }
        }

        //
        // GET: /Documento/Create
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Create()
        {
            LoadFormJuri();

            return View(new documento());
        }

        //
        // POST: /Documento/Create
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Create(documento documento)
        {
            LoadFormJuri();

            try
            {
                if (validate(documento))
                    return View(documento);

                if (UploadFile(ref documento))
                {
                    DocumentoRepository.Create(documento);
                    return RedirectToAction("List", new { message = "Dados cadastrados com sucesso!" });
                }
                else
                {
                    throw new Exception("Ocorreu um erro ao criar o registro");
                }
            }
            catch
            {
                if (!FileServerHelper.DeleteFile(documento.path))
                {
                    throw new Exception("Não foi possível excluir o arquivo");
                }
                else
                {
                    return View(documento);
                }
            }
        }

        //
        // GET: /Documento/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Delete(int id)
        {
            return View(DocumentoRepository.GetOne(id));
        }

        //
        // POST: /Documento/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Delete(int id, documento documento)
        {
            try
            {
                documento = DocumentoRepository.GetOne(id);

                if (ExistDependences(documento))
                    return View(documento);

                if (!FileServerHelper.DeleteFile(documento.path))
                {
                    throw new Exception("Não foi possível salvar o arquivo");
                }
                else
                {
                    DocumentoRepository.Delete(documento);
                }

                return RedirectToAction("List", new { message = "Dados excluídos com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public FileStreamResult Download(int id)
        {
            documento anexos = DocumentoRepository.GetOne(id);
            string contentType = FileServerHelper.GetContentType(anexos.path.Substring(anexos.path.LastIndexOf('.')));

            return File(new FileStream(anexos.path, FileMode.Open), contentType);            
        }

        private bool UploadFile(ref documento documento)
        {
            if (Request.Files != null && Request.Files[0].ContentLength > 0)
            {
                string path = String.Empty;

                if (!FileServerHelper.UploadFile(Request.Files[0].InputStream,
                                                Request.Files[0].FileName, ref path))
                {
                    throw new Exception("Não foi possível salvar o arquivo");
                }
                else
                {
                    documento.path = path;
                    return true;
                }
            }
            else
            {
                throw new Exception("O arquivo informado está vazio");
            }
        }

        public bool validate(documento entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.nome_documento))
            {
                ModelState.AddModelError("nome_documento", "Campo obrigatório");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.path))
            {
                ModelState.AddModelError("path", "Campo obrigatório");
                retorno = true;
            }

            return retorno;
        }

        public bool ExistDependences(documento entity)
        {
            bool retorno = false;

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
