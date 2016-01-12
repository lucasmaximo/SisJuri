using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    public class FaculdadeController : BaseController
    {
        FaculdadeRepository _FaculdadeRepository;

        public FaculdadeRepository FaculdadeRepository
        {
            get {
                if (_FaculdadeRepository == null)
                    _FaculdadeRepository = new FaculdadeRepository();
                return _FaculdadeRepository; }
            set { _FaculdadeRepository = value; }
        }


        //
        // GET: /Faculdade/
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Index()
        {
            return View(new faculdade());
        }

        [Authorize(Roles = "Administrador, Organizador")]

        // Faculdade/List
        public ActionResult List(faculdade entity, String message, String messageError)
        {
            ViewData["message"] = message;
            ViewData["messageError"] = messageError;
            if (string.IsNullOrEmpty(entity.cnpj_faculdade) && (string.IsNullOrEmpty(entity.nome_faculdade)))
            {
                return View(FaculdadeRepository.GetAll());
            }
            else
            {
                return View(FaculdadeRepository.GetAllByCriteria(entity.nome_faculdade, entity.cnpj_faculdade));
            }
        }

        //
        // GET: /Faculdade/Details/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Details(int id)
        {
            return View(FaculdadeRepository.GetOne(id));
        }

        //
        // GET: /Faculdade/Create
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            return View(new faculdade());
        }

        //
        // POST: /Faculdade/Create
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult Create(faculdade faculdade)
        {
            try
            {
                if (validate(faculdade))
                    return View(faculdade);

                FaculdadeRepository.Create(faculdade);

                return RedirectToAction("List", new { message = "Dados cadastrados com sucesso!" });
            }
            catch
            {
                return View(faculdade);
            }
        }

        //
        // GET: /Faculdade/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Edit(int id)
        {
            return View(FaculdadeRepository.GetOne(id));
        }

        //
        // POST: /Faculdade/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Edit(int id, faculdade faculdade)
        {
            try
            {
                if (validate(faculdade))
                    return View(faculdade);

                FaculdadeRepository.Edit(faculdade);
                return RedirectToAction("List", new { message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Faculdade/Delete/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int id)
        {
            return View(FaculdadeRepository.GetOne(id));
        }

        //
        // POST: /Faculdade/Delete/5
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult Delete(int id, faculdade faculdade)
        {
            try
            {
                faculdade = FaculdadeRepository.GetOne(id);
                if (faculdade.npj.Count > 0 || faculdade.usuario.Count > 0)
                    return RedirectToAction("List", new { messageError = "Essa faculdade possui registros vinculados a seu cadastro. Não é possível excluí-la." });
                
                FaculdadeRepository.Delete(faculdade);

                return RedirectToAction("List", new { message = "Dados excluídos com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        public bool validate(faculdade entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.nome_faculdade))
            {
                ModelState.AddModelError("nome_faculdade", "Campo obrigatório");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.cnpj_faculdade))
            {
                ModelState.AddModelError("cnpj_faculdade", "Campo obrigatório");
                retorno = true;
            }
            if (!ValidaCNPJ(entity.cnpj_faculdade))
            {
                ModelState.AddModelError("cnpj_faculdade", "CNPJ inválido");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.endereco_faculdade))
            {
                ModelState.AddModelError("endereco_faculdade", "Campo obrigatório");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.telefone_faculdade))
            {
                ModelState.AddModelError("telefone_faculdade", "Campo obrigatório");
                retorno = true;
            }

            return retorno;
        }

        //Método que valida o CNPJ 
        public bool ValidaCNPJ(string vrCNPJ)
        {
 
            string CNPJ = vrCNPJ.Replace(".", "");
            CNPJ = CNPJ.Replace("/", "");
            CNPJ = CNPJ.Replace("-", "");
 
            int[] digitos, soma, resultado;
            int nrDig;
            string ftmt;
            bool[] CNPJOk;
 
            ftmt = "6543298765432";
            digitos = new int[14];
            soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;
            resultado = new int[2];
            resultado[0] = 0;
            resultado[1] = 0;
            CNPJOk = new bool[2];
            CNPJOk[0] = false;
            CNPJOk[1] = false;
 
            try
            {
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(
                     CNPJ.Substring(nrDig, 1));
                    if (nrDig <= 11)
                        soma[0] += (digitos[nrDig] *
                        int.Parse(ftmt.Substring(
                          nrDig + 1, 1)));
                    if (nrDig <= 12)
                        soma[1] += (digitos[nrDig] *
                        int.Parse(ftmt.Substring(
                          nrDig, 1)));
                  }
 
                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = (soma[nrDig] % 11);
                    if ((resultado[nrDig] == 0) || (resultado[nrDig] == 1))
                        CNPJOk[nrDig] = (
                        digitos[12 + nrDig] == 0);
 
                    else
                        CNPJOk[nrDig] = (
                        digitos[12 + nrDig] == (
                        11 - resultado[nrDig]));
 
                }
 
                return (CNPJOk[0] && CNPJOk[1]);
 
            }
            catch
            {
                return false;
            }
 
        }
    }
}
