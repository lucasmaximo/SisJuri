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
    public class ParticipantesReuniaoController : BaseController
    {
        private ReuniaoRepository _ReuniaoRepository;
        private InscricaoRepository _InscricaoRepository;
        private ParticipantesReuniaoRepository _ParticipantesReuniaoRepository;

        public ReuniaoRepository ReuniaoRepository
        {
            get {
                if (_ReuniaoRepository == null)
                    _ReuniaoRepository = new ReuniaoRepository();
                return _ReuniaoRepository; }
            set { _ReuniaoRepository = value; }
        }

        public InscricaoRepository InscricaoRepository
        {
            get {
                if (_InscricaoRepository == null)
                    _InscricaoRepository = new InscricaoRepository();
                return _InscricaoRepository; }
            set { _InscricaoRepository = value; }
        }

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

        public ActionResult ListParticipantesByReuniao(int id, String message)
        {
            ViewData["message"] = message;
            return View(ParticipantesReuniaoRepository.GetParticipantesByReuniao(id));
        } 

        //
        // GET: /ParticipantesReuniao/Details/5
        public ActionResult Details(int id)
        {
            return View(ParticipantesReuniaoRepository.GetOne(id));
        }

        //
        // GET: /ParticipantesReuniao/Edit/5
        public ActionResult Edit(int id)
        {
            return View(ParticipantesReuniaoRepository.GetOne(id));
        }

        //
        // POST: /ParticipantesReuniao/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, participantes_reuniao participantes_reuniao)
        {
            try
            {
                ParticipantesReuniaoRepository.Edit(participantes_reuniao);
                return RedirectToAction("ListParticipantesByReuniao", new {  id = participantes_reuniao.pfk_id_reuniao, message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }
    }
}
