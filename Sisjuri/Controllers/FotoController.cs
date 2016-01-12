using Sisjuri.Helpers;
using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
    public class FotoController : BaseController
    {
        JuriRepository _JuriRepository;
        FotoRepository _FotoRepository;

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

        public FotoRepository FotoRepository
        {
            get {
                if (_FotoRepository == null)
                    _FotoRepository = new FotoRepository();
                return _FotoRepository; }
            set { _FotoRepository = value; }
        }

        //Pesquisar fotos
        // GET: /Documento/
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Index()
        {
            LoadFormJuri();
            return View(new foto());
        }

        //Álbum de fotos
        // Foto/List
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult List(foto entity, String message, int idJuri = 0)
        {
            ViewData["message"] = message;
            ViewData["Juri"] = entity.juri;
            ViewData["idFoto"] = entity.id_foto;

            if (idJuri == 0)
            {
                idJuri = entity.fk_id_juri;
                ViewData["idJuri"] = entity.fk_id_juri;
            }
            else
            {
                ViewData["idJuri"] = idJuri;
            }

            var imagesModel = new ImageGallery();

            if (Directory.Exists(Server.MapPath("~/Documentos/Fotos/" + idJuri)))
            {
                var imageFiles = Directory.GetFiles(Server.MapPath("~/Documentos/Fotos/" + idJuri));
                foreach (var item in imageFiles)
                {
                    if (item.IndexOf("Thumbs.db") < 0)
                    imagesModel.ImageList.Add(Path.GetFileName(item));
                }
            }

            return View(imagesModel);
        }

        //
        // GET: /Foto/Create
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult UploadImage(int idJuri)
        {
            Session["idJuri"] = idJuri;
            return View();
        }

        //
        // POST: /Foto/Create
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult UploadImage(foto foto)
        {
            if (Request.Files.Count != 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Documentos/Fotos/" + Session["idJuri"])))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Documentos/Fotos/" + Session["idJuri"]));
                }
                if (validate(foto)){
                    return View(foto);
                }

                HttpFileCollectionBase hfc = Request.Files;
                List<foto> fotos = new List<foto>();

                for (int i = 0; i < hfc.Count; i++)
                {
                    HttpPostedFileBase file = hfc[i];
                    int fileSize = file.ContentLength;
                    string fileName = file.FileName;

                    file.SaveAs(Server.MapPath("~/Documentos/Fotos/" + Session["idJuri"] + "/" + fileName));

                    ImageGallery imageGallery = new ImageGallery();
                    imageGallery.ID = Guid.NewGuid();
                    imageGallery.Name = fileName;
                    imageGallery.ImagePath = "~/Documentos/Fotos/" + Session["idJuri"] + "/" + fileName;

                    foto salvafoto = new foto();
                    salvafoto.fk_id_juri = int.Parse(Session["idJuri"].ToString());
                    salvafoto.path = imageGallery.ImagePath;
                    fotos.Add(salvafoto);
                }

                FotoRepository.Create(fotos);
                return RedirectToAction("List", new { message = "Fotos publicadas com sucesso!", idJuri = int.Parse(Session["idJuri"].ToString()) });
            }

            return View(foto);
        }

        //
        // GET: /Foto/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Delete(string path)
        {
            return View(FotoRepository.GetOne(path));
        }

        //
        // POST: /Foto/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Delete(string path, foto foto)
        {
            try
            {
                foto = FotoRepository.GetFotoById(path);

                if (!FileServerHelper.DeleteFile(Server.MapPath(foto.path)))
                {
                    throw new Exception("Não foi possível salvar o arquivo");
                }
                else
                {
                    FotoRepository.Delete(foto);
                }

                return RedirectToAction("List", new { message = "Dados excluídos com sucesso!" });
            }
            catch
            {
                return View();
            }
        }


        public void LoadFormJuri()
        {
            IEnumerable<juri> lst = JuriRepository.GetAll();
            int c = lst.Count();
            ViewData["lstJuri"] = lst;
        }

        public bool validate(foto entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.path))
            {
                ModelState.AddModelError("path", "Escolha uma ou mais fotos para publicar.");
                retorno = true;
            }

            return retorno;
        }


    }
}
