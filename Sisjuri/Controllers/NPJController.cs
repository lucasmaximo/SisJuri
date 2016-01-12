using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    [Authorize(Roles = "Administrador, Organizador")]
    public class NPJController : BaseController
    {
        FaculdadeRepository _FaculdadeRepository;
        NPJRepository _NPJRepository;

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


        //
        // GET: /NPJ/
        public ActionResult Index()
        {
            return View(new npj());
        }

        public ActionResult List(npj entity, String message, String messageError)
        {
            ViewData["message"] = message;
            ViewData["messageError"] = messageError;
            if (string.IsNullOrEmpty(entity.cnpj_npj) && (string.IsNullOrEmpty(entity.nome_npj)))
            {
                return View(NPJRepository.GetAll());
            }
            else
            {
                return View(NPJRepository.GetAllByCriteria(entity.nome_npj, entity.cnpj_npj));
            }
        }

        //
        // GET: /NPJ/Details/5
        public ActionResult Details(int id)
        {
            return View(NPJRepository.GetOne(id));
        }

        //
        // GET: /NPJ/Create
        public ActionResult Create()
        {
            LoadFormFaculdade(); 
            return View(new npj());
        }

        //
        // POST: /NPJ/Create
        [HttpPost]
        public ActionResult Create(npj npj)
        {
            LoadFormFaculdade(); 

            try
            {
                if (validate(npj))
                    return View(npj);

                NPJRepository.Create(npj);

                return RedirectToAction("List", new { message = "Dados cadastrados com sucesso!" });
            }
            catch
            {
                return View(npj);
            }
        }

        //
        // GET: /NPJ/Edit/5
        public ActionResult Edit(int id)
        {
            LoadFormFaculdade(); 

            return View(NPJRepository.GetOne(id));
        }

        //
        // POST: /NPJ/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, npj npj)
        {
            LoadFormFaculdade(); 

            try
            {
                if (validate(npj))
                    return View(npj);

                NPJRepository.Edit(npj);
                return RedirectToAction("List", new { message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /NPJ/Delete/5
        public ActionResult Delete(int id)
        {
            return View(NPJRepository.GetOne(id));
        }

        //
        // POST: /NPJ/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, npj npj)
        {
            try
            {
                npj = NPJRepository.GetOne(id);

                if (npj.juri.Count > 0)
                    return RedirectToAction("List", new { messageError = "Esse NPJ possui registros vinculados a seu cadastro. Não é possível excluí-lo." });

                NPJRepository.Delete(npj);

                return RedirectToAction("List", new { message = "Dados excluídos com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        public bool validate(npj entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.nome_npj))
            {
                ModelState.AddModelError("nome_npj", "Campo obrigatório");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.cnpj_npj))
            {
                ModelState.AddModelError("cnpj_npj", "Campo obrigatório");
                retorno = true;
            }
            if (!ValidaCNPJ(entity.cnpj_npj))
            {
                ModelState.AddModelError("cnpj_npj", "CNPJ inválido");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.endereco_npj))
            {
                ModelState.AddModelError("endereco_npj", "Campo obrigatório");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.telefone_npj))
            {
                ModelState.AddModelError("telefone_npj", "Campo obrigatório");
                retorno = true;
            }

            return retorno;
        }

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

        public void LoadFormFaculdade()
        {
            IEnumerable<faculdade> lst = FaculdadeRepository.GetAll();
            int c = lst.Count();
            ViewData["lstFaculdade"] = lst;
        }
    }
}
