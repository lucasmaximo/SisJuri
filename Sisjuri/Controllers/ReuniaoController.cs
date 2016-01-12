using Microsoft.Reporting.WebForms;
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
    public class ReuniaoController : BaseController
    {
        JuriRepository _JuriRepository;
        ReuniaoRepository _ReuniaoRepository;
        InscricaoRepository _InscricaoRepository;
        ParticipantesReuniaoRepository _ParticipantesReuniaoRepository;

        public ParticipantesReuniaoRepository ParticipantesReuniaoRepository
        {
            get 
            {
                if (_ParticipantesReuniaoRepository == null)
                    _ParticipantesReuniaoRepository = new ParticipantesReuniaoRepository();
                return _ParticipantesReuniaoRepository; 
            }
            set { _ParticipantesReuniaoRepository = value; }
        }
        public InscricaoRepository InscricaoRepository
        {
            get 
            {
                if (_InscricaoRepository == null)
                    _InscricaoRepository = new InscricaoRepository();
                return _InscricaoRepository; 
            }
            set { _InscricaoRepository = value; }
        }
        public JuriRepository JuriRepository
        {
            get {
                if (_JuriRepository == null)
                    _JuriRepository = new JuriRepository();
                return _JuriRepository; }
            set { _JuriRepository = value; }
        }

        public ReuniaoRepository ReuniaoRepository
        {
            get {
                if (_ReuniaoRepository == null)
                    _ReuniaoRepository = new ReuniaoRepository();
                return _ReuniaoRepository; }
            set { _ReuniaoRepository = value; }
        }

        //
        // GET: /Reuniao/
        [Authorize(Roles = "Administrador, Organizador, Professor")]
        public ActionResult Index()
        {
            LoadFormJuri();
            return View(new reuniao());
        }

        //Reuniao/List 
        [Authorize(Roles = "Administrador, Organizador, Professor")]
        public ActionResult List(reuniao entity, String message)
        {
            ViewData["message"] = message;
            if (string.IsNullOrEmpty(entity.nome_reuniao) && (entity.fk_id_juri == 0))
            {
                return View(ReuniaoRepository.GetAll());
            }
            else
            {
                return View(ReuniaoRepository.GetAllByCriteria(entity.nome_reuniao, entity.fk_id_juri));
            }
        }

        //
        // GET: /Reuniao/Details/5
        [Authorize(Roles = "Administrador, Organizador, Professor")]
        public ActionResult Details(int id)
        {
            return View(ReuniaoRepository.GetOne(id));
        }

        //
        // GET: /Reuniao/Create
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Create()
        {
            LoadFormJuriCompleto();
            return View(new reuniao());
        }

        //
        // POST: /Reuniao/Create
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Create(reuniao reuniao)
        {
            LoadFormJuriCompleto();

            try
            {
                if (validate(reuniao))
                    return View(reuniao);
                ReuniaoRepository.Create(reuniao);

                List<inscricao> lst = InscricaoRepository.GetAllBySortAndJuri(reuniao.fk_id_juri);

                foreach (var item in lst)
                {
                    participantes_reuniao temp = new participantes_reuniao();
                    temp.pfk_id_reuniao = reuniao.id_reuniao;
                    temp.pfk_id_inscricao = item.id_inscricao;

                    ParticipantesReuniaoRepository.Create(temp);
                }

                return RedirectToAction("List", new { message = "Dados cadastrados com sucesso!" });
            }
            catch
            {
                return View(reuniao);
            }
        }

        //
        // GET: /Reuniao/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Edit(int id)
        {
            LoadFormJuriCompleto();

            return View(ReuniaoRepository.GetOne(id));
        }

        //
        // POST: /Reuniao/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Edit(int id, reuniao reuniao)
        {
            LoadFormJuriCompleto();

            try
            {
                if (validate(reuniao))
                    return View(reuniao);
                ReuniaoRepository.Edit(reuniao);
                return RedirectToAction("List", new { message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Reuniao/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Delete(int id)
        {
            return View(ReuniaoRepository.GetOne(id));
        }

        //
        // POST: /Reuniao/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Delete(int id, reuniao reuniao)
        {
            try
            {
                List<participantes_reuniao> lst = ParticipantesReuniaoRepository.GetAllByIdReuniao(id);

                foreach (var item in lst)
                {
                    participantes_reuniao temp = ParticipantesReuniaoRepository.GetOne(item.id_participante_reuniao);
                    ParticipantesReuniaoRepository.Delete(temp);
                }

                reuniao = ReuniaoRepository.GetOne(id);
                ReuniaoRepository.Delete(reuniao);

                return RedirectToAction("List", new { message = "Dados excluídos com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        public bool validate(reuniao entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.nome_reuniao))
            {
                ModelState.AddModelError("nome_reuniao", "Campo obrigatório");
                retorno = true;
            }
            if (entity.data_hora_reuniao == DateTime.MinValue)
            {
                ModelState.AddModelError("data_hora_reuniao", "Campo obrigatório");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.local_reuniao))
            {
                ModelState.AddModelError("local_reuniao", "Campo obrigatório");
                retorno = true;
            }

            List<inscricao> lst = InscricaoRepository.GetAllBySortAndJuri(entity.fk_id_juri);

            if (lst.Count == 0)
            {
                ModelState.AddModelError("", "Não é possivel cadastrar a reunião porque não foram sorteados as funções");
                retorno = true;
            }
            return retorno;
        }

        public void LoadFormJuri()
        {
            IEnumerable<juri> lst = JuriRepository.GetAll();
            int c = lst.Count();

            juri temp = new juri();
            temp.id_juri = 0;
            temp.nome_juri = "";

            List<juri> lstTemp = new List<juri>();
            lstTemp.Add(temp);

            foreach (var item in lst)
            {
                lstTemp.Add(item);
            }

            ViewData["lstJuri"] = lstTemp.AsEnumerable<juri>();
        }

        public void LoadFormJuriCompleto()
        {
            IEnumerable<juri> lst = JuriRepository.GetAll();
            int c = lst.Count();
            ViewData["lstJuriCompleto"] = lst;
        }
    }
}
