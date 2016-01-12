using Microsoft.Reporting.WebForms;
using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{
    public class GerarCertificadoController : BaseController
    {
        private PresencaRepository _PresencaRepository;
        private JuriRepository _JuriRepository;
        private VGerarCertificadoRepository _VGerarCertificadoRepository;

        public PresencaRepository PresencaRepository
        {
            get
            {
                if (_PresencaRepository == null)
                    _PresencaRepository = new PresencaRepository();
                return _PresencaRepository;
            }
            set { _PresencaRepository = value; }
        }
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
        public VGerarCertificadoRepository VGerarCertificadoRepository
        {
            get {
                if (_VGerarCertificadoRepository == null)
                    _VGerarCertificadoRepository = new VGerarCertificadoRepository();
                return _VGerarCertificadoRepository; }
            set { _VGerarCertificadoRepository = value; }
        }


        //
        // GET: /GerarCertificado/
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Index()
        {
            LoadFormJuri();

            return View(new presenca());
        }

        //
        // POST: /GerarCertificado
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        [HttpPost]
        public ActionResult Index(presenca presenca)
        {
            LoadFormJuri();

            var idUser = ((Sisjuri.Models.usuario)Session["usuario"]).id_usuario;
            var presencaJuri = PresencaRepository.GetOneByJuriAndPresenca(presenca.pfk_id_juri, idUser );

            if (validate(presencaJuri))
            {
                return View(presenca);
            }
            else
            {
                AtribuirHoras(presencaJuri);

                presenca = presencaJuri;
                PresencaRepository.Edit(presenca);

                return RedirectToAction("GerarCertificado", new { idJuri = presenca.pfk_id_juri, idUsuario = idUser });
            }
        }

        public void AtribuirHoras(presenca presenca)
        {
            var usuario = presenca.inscricao.usuario;
            var inscricao = presenca.inscricao;
            var juri = presenca.juri;
            presenca.horas_certif = calcularHoras(presenca).ToString();      
        }

        //Calcular horas do certificado
        public TimeSpan calcularHoras(presenca presenca)
        {
            var usuario = presenca.inscricao.usuario;
            var inscricao = presenca.inscricao;
            var juri = presenca.juri;

            TimeSpan qtdeHoras = TimeSpan.Zero;
            TimeSpan ref2hrs = new TimeSpan(2, 0, 0);
            TimeSpan ref3hrs = new TimeSpan(3, 0, 0);
            TimeSpan ref4hrs = new TimeSpan(4, 0, 0);
            TimeSpan ref5hrs = new TimeSpan(5, 0, 0);

            if (inscricao.fk_id_funcao == 1 || inscricao.fk_id_funcao == 2 || inscricao.fk_id_funcao == 9)
            {
                qtdeHoras = new TimeSpan(15, 0, 0);
            }
            if (inscricao.fk_id_funcao == 3 || inscricao.fk_id_funcao == 4 || inscricao.fk_id_funcao == 5 
                || inscricao.fk_id_funcao == 7 || inscricao.fk_id_funcao == 8)
            {
                qtdeHoras = new TimeSpan(5, 0, 0);
            }
            if (inscricao.fk_id_funcao == 6 && presenca.pres_saida_total == true && presenca.pres_saida_parcial == false)
            {
                qtdeHoras = new TimeSpan(5, 0, 0);
            }
            if (inscricao.fk_id_funcao == 6 && presenca.pres_saida_total == false && presenca.pres_saida_parcial == true)
            {
                var calcHoras = presenca.hora_saida_parcial - juri.data_hora_juri.TimeOfDay;
                
                if (calcHoras < ref2hrs)
                {
                    qtdeHoras = ref2hrs;
                }
                if (calcHoras > ref2hrs && calcHoras < ref3hrs)
                {
                    qtdeHoras = ref3hrs;
                }
                if (calcHoras > ref3hrs && calcHoras < ref5hrs)
                {
                    qtdeHoras = ref4hrs;
                }
            }

            return qtdeHoras;
        }

        //Gerar Certificado, Relatório
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult GerarCertificado(int idJuri, int idUsuario)
        {
            String format = "PDF";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Report"), "vgerarcertificadoreport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            List<vgerarcertificado> cm = VGerarCertificadoRepository.GetOneByJuriAndPresenca(idJuri, idUsuario);

            ReportDataSource rd = new ReportDataSource("vgerarcertificadoreport", cm);           
            lr.DataSources.Add(rd);
            string reportType = format;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

                "<DeviceInfo>" +
                "<OutputFormat>" + format + "</OutputFormat>" +
                "<PageWidth>8.5in</PageWidth>" +
                "<PageHeight>11in</PageHeight>" +
                "<MarginTop>0.5in</MarginTop>" +
                "<MarginLeft>0.5in</MarginLeft>" +
                "<MarginRight>0.5in</MarginRight>" +
                "<MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
            reportType,
            deviceInfo,
            out mimeType,
            out encoding,
            out fileNameExtension,
            out streams,
            out warnings);

            return File(renderedBytes, mimeType);
        }


        public bool validate(presenca entity)
        {
            bool retorno = false;
            if (entity != null)
            {
                if (entity.pres_saida_parcial == false && entity.pres_saida_total == false)
                {
                    ModelState.AddModelError("", "Você não participou deste júri simulado");
                    retorno = true;
                }
            }
            else
            {
                ModelState.AddModelError("", "Você não participou deste júri simulado");
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


