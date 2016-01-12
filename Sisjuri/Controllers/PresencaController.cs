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
    public class PresencaController : BaseController
    {
        private PresencaRepository _PresencaRepository;
        private FuncaoRepository _FuncaoRepository;
        private JuriRepository _JuriRepository;
        private VListadePresencaRepository _VListadePresencaRepository;

        public PresencaRepository PresencaRepository
        {
            get {
                if (_PresencaRepository == null)
                    _PresencaRepository = new PresencaRepository();
                return _PresencaRepository;
            }
            set { _PresencaRepository = value; }
        }
        public FuncaoRepository FuncaoRepository
        {
            get
            {
                if (_FuncaoRepository == null)
                    _FuncaoRepository = new FuncaoRepository();
                return _FuncaoRepository;
            }
            set { _FuncaoRepository = value; }
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
        public VListadePresencaRepository VListadePresencaRepository
        {
            get
            {
                if (_VListadePresencaRepository == null)
                    _VListadePresencaRepository = new VListadePresencaRepository();
                return _VListadePresencaRepository;
            }
            set { VListadePresencaRepository = value; }
        }

        //
        // GET: /Presenca/
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Index()
        {
            LoadFormJuri();
            LoadFormFuncao();
            return View(new presenca());
        }

        // Presenca/List (Lista resultado da pesquisa)
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult List(presenca entity, String message)
        {
            ViewData["message"] = message;
            if (string.IsNullOrEmpty(entity.inscricao.usuario.num_matric_aluno) &&
                string.IsNullOrEmpty(entity.inscricao.usuario.nome_completo) && (entity.inscricao.funcao.id_funcao == 0) 
                && (entity.pfk_id_juri == 0))           
            {
                return View(PresencaRepository.GetAll());
            }
            else
            {
                return View(PresencaRepository.GetAllByCriteria(entity.inscricao.usuario.nome_completo ?? "", 
                    entity.inscricao.funcao.id_funcao, entity.pfk_id_juri));
            }
        }

        // Presenca/ListAllByAutorizados/ (Lista de presença)
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult ListAllByAutorizados(int id, int idJuri, String message)
        {
            ViewData["message"] = message;
            return View(PresencaRepository.GetAllByAutorizados(id));
        }

        // Presenca/ListadePresenca (Exportar lista de presença)
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult ListadePresenca(int idJuri)
        {
            String format = "PDF";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Report"), "vlistadepresencareport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }

            List<vlistadepresenca> cm = VListadePresencaRepository.GetAllByInscritos(idJuri);

            ReportDataSource rd = new ReportDataSource("vlistadepresencareport", cm);
            lr.DataSources.Add(rd);
            string reportType = format;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

                "<DeviceInfo>" +
                "<OutputFormat>" + format + "</OutputFormat>" +
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

        //
        // GET: /Presenca/Details/5
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Details(int id)
        {
            return View(PresencaRepository.GetOne(id));
        }

        //
        // GET: /Presenca/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Edit(int id)
        {
            LoadFormJuri();
            LoadFormFuncao();

            return View(PresencaRepository.GetOne(id));
        }

        //
        // POST: /Presenca/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Edit(int id, int idJuri, presenca presenca)
        {
            LoadFormJuri();
            LoadFormFuncao();
            
            try
            {
                PresencaRepository.Edit(presenca);
                return RedirectToAction("ListAllByAutorizados", new { id = presenca.pfk_id_juri, idJuri = presenca.pfk_id_juri, 
                    message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        public void LoadFormFuncao()
        {
            IEnumerable<funcao> lst = FuncaoRepository.GetAll();
            int c = lst.Count();

            funcao temp = new funcao();
            temp.id_funcao = 0;
            temp.nome_funcao = "";

            List<funcao> lstTemp = new List<funcao>();
            lstTemp.Add(temp);

            foreach (var item in lst)
            {
                lstTemp.Add(item);
            }

            ViewData["lstFuncao"] = lstTemp.AsEnumerable<funcao>();
        }

        public void LoadFormJuri()
        {
            IEnumerable<juri> lst = JuriRepository.GetAll();
            int c = lst.Count();
            ViewData["lstJuri"] = lst;
        }
    }
}
