using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using System.IO;


namespace Sisjuri.Controllers
{
    public class InscricaoController : BaseController
    {
        private InscricaoRepository _InscricaoRepository;
        private FuncaoRepository _FuncaoRepository;
        private UsuarioRepository _UsuarioRepository;
        private JuriRepository _JuriRepository;
        private CursoRepository _CursoRepository;
        private PresencaRepository _PresencaRepository;
        private VSorteioFuncoesRepository _VSorteioFuncoesRepository;

        public InscricaoRepository InscricaoRepository
        {
            get {
                if (_InscricaoRepository == null)
                    _InscricaoRepository = new InscricaoRepository();
                return _InscricaoRepository; }
            set { _InscricaoRepository = value; }
        }
        public FuncaoRepository FuncaoRepository
        {
            get {
                if (_FuncaoRepository == null)
                    _FuncaoRepository = new FuncaoRepository(); 
                return _FuncaoRepository;
            }
            set { _FuncaoRepository = value; }
        }
        public UsuarioRepository UsuarioRepository
        {
            get {
                if (_UsuarioRepository == null)
                    _UsuarioRepository = new UsuarioRepository();
                return _UsuarioRepository; }
            set { _UsuarioRepository = value; }
        }
        public JuriRepository JuriRepository
        {
            get {
                if (_JuriRepository == null)
                    _JuriRepository = new JuriRepository();
                return _JuriRepository; }
            set { _JuriRepository = value; }
        }
        public CursoRepository CursoRepository
        {
            get {
                if (_CursoRepository == null)
                    _CursoRepository = new CursoRepository();
                return _CursoRepository; }
            set { _CursoRepository = value; }
        }
        public PresencaRepository PresencaRepository
        {
            get {
                if (_PresencaRepository == null)
                    _PresencaRepository = new PresencaRepository();
                return _PresencaRepository; }
            set { _PresencaRepository = value; }
        }
        public VSorteioFuncoesRepository VSorteioFuncoesRepository
        {
            get {
                if (_VSorteioFuncoesRepository == null)
                    _VSorteioFuncoesRepository = new VSorteioFuncoesRepository();
                return _VSorteioFuncoesRepository; }
            set { _VSorteioFuncoesRepository = value; }
        }

        //
        // GET: /Inscricao/
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Index()
        {
            LoadFormJuri();
            LoadFormFuncao();

            inscricao inscricao = new inscricao();
            inscricao.funcao = new funcao();
            inscricao.usuario = new usuario();

            return View(new inscricao());
        }

        // Inscricao/List (Resultado da pesquisa)
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult List(inscricao entity, String message, String messageError)
        {
            ViewData["message"] = message;
            ViewData["messageError"] = messageError;
            if (string.IsNullOrEmpty(entity.usuario.nome_completo) && (entity.fk_id_juri == 0) &&
                string.IsNullOrEmpty(entity.status_inscricao) && (entity.fk_id_funcao == 0) &&
                string.IsNullOrEmpty(entity.usuario.num_matric_aluno))
            {
                return View(InscricaoRepository.GetAll());
            }
            else
            {
                return View(InscricaoRepository.GetAllByCriteria(entity.usuario.nome_completo ?? "", entity.fk_id_juri,
                    entity.fk_id_funcao, entity.usuario.num_matric_aluno ?? "", entity.status_inscricao ?? ""));
            }
        }

        // Inscricao/ListByIdJuri (Lista de inscrições de um determinado júri)
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult ListByIdJuri(int id, String message, String messageError)
        {
            ViewData["message"] = message;
            ViewData["messageError"] = messageError;
            return View(InscricaoRepository.GetAllByIdJuri(id));
        }

        // Inscricao/ListBySortJuriAndExport (Lista de sorteados para as funções do júri)
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult ListBySortJuriAndExport(int idJuri, String message, String messageError)
        {
            ViewData["message"] = message;
            ViewData["messageError"] = messageError;
            return View(VSorteioFuncoesRepository.GetAllBySortJuriAndExport(idJuri));
        }

        //Inscricao/InscricoesSorteadas (PDF com as inscrições que foram sorteadas)
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult InscricoesSorteadas(int idJuri)
        {
            String format = "PDF";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Report"), "vsorteiofuncoesreport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ListBySortAndJuriExport");
            }
            List<vsorteiofuncoes> cm = VSorteioFuncoesRepository.GetAllBySortJuriAndExport(idJuri);

            ReportDataSource rd = new ReportDataSource("vsorteiofuncoesreport", cm);
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

        //Inscricao/Sortear (Sortear funções para o júri)
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Sortear(int id, inscricao inscricao, String messageError)
        {
            ViewData["messageError"] = messageError;
            List<inscricao> PromotorSort = new List<inscricao>();
            List<inscricao> AdvDefesaSort = new List<inscricao>();
            List<inscricao> EscrivaoSort = new List<inscricao>();
            List<inscricao> JuradoSort = new List<inscricao>();
            List<inscricao> OficJusticaSort = new List<inscricao>();
            List<inscricao> Assistente = new List<inscricao>();
            List<inscricao> SegurancaSort = new List<inscricao>();
            List<inscricao> ReuSort = new List<inscricao>();
            List<inscricao> JuizSort = new List<inscricao>();

            /*  OS CAMPOS ABAIXO ESTÃO COMENTADOS PORQUE DÁ ERRO NO SORTEIO SE NÃO TIVER UMA QUANTIDADE MÍNIMA DE INSCRIÇÕES FEITAS
                EM DETERMINADA FUNÇÃO MAIOR QUE O ÚLTIMO NÚMERO DE CADA LINHA DE CÓDIGO. DESCOMENTAR O CÓDIGO ABAIXO QUANDO TIVER
                MUITAS INSCRIÇÕES E FOR EXECUTAR O SORTEIO...  */


            // Verificando se há a quantidade mínima de inscrições para fazer o sorteio
            if (InscricaoRepository.GetPromotorByAuthorization(id).Count < 3 || 
                InscricaoRepository.GetAdvogadoDefesaByAuthorization(id).Count < 3 ||
                InscricaoRepository.GetEscrivaoByAuthorization(id).Count < 1 ||
                InscricaoRepository.GetJuradoByAuthorization(id).Count < 25 ||
                InscricaoRepository.GetOficialJusticaoByAuthorization(id).Count < 4 ||
                InscricaoRepository.GetSegurancaByAuthorization(id).Count < 4 ||
                InscricaoRepository.GetReuByAuthorization(id).Count < 1 ||
                InscricaoRepository.GetJuizByAuthorization(id).Count < 1 )
                return RedirectToAction("List", "Juri", new { messageError = "Não é possível realizar o sorteio porque a quantidade de inscrições autorizadas é insuficiente para preencher todas as funções obrigatórias." });

            // Realizando o sorteio
            PromotorSort = Sorteio(InscricaoRepository.GetPromotorByAuthorization(id), 3);
            AdvDefesaSort = Sorteio(InscricaoRepository.GetAdvogadoDefesaByAuthorization(id), 3);
            EscrivaoSort = Sorteio(InscricaoRepository.GetEscrivaoByAuthorization(id), 1);
            JuradoSort = Sorteio(InscricaoRepository.GetJuradoByAuthorization(id), 25);
            OficJusticaSort = Sorteio(InscricaoRepository.GetOficialJusticaoByAuthorization(id), 4);
            SegurancaSort = Sorteio(InscricaoRepository.GetSegurancaByAuthorization(id), 4);
            ReuSort = Sorteio(InscricaoRepository.GetReuByAuthorization(id), 1);
            JuizSort = Sorteio(InscricaoRepository.GetJuizByAuthorization(id), 1);
            Assistente = InscricaoRepository.GetAssistenteByAuthorization(id);


            // Cria uma lista de sorteados do júri simulado
            List<inscricao> sorteados = new List<inscricao>();
            foreach (var sort in PromotorSort)
                sorteados.Add(sort);
            foreach (var sort in AdvDefesaSort)
                sorteados.Add(sort);
            foreach (var sort in EscrivaoSort)
                sorteados.Add(sort);
            foreach (var sort in JuradoSort)
                sorteados.Add(sort);
            foreach (var sort in OficJusticaSort)
                sorteados.Add(sort);
            foreach (var sort in SegurancaSort)
                sorteados.Add(sort);
            foreach (var sort in ReuSort)
                sorteados.Add(sort);
            foreach (var sort in JuizSort)
                sorteados.Add(sort);
            foreach (var sort in Assistente)
                sorteados.Add(sort);

            //Atribui true a juri_sorteado para mostrar que o júri já foi sorteado e que suas inscrições estão encerradas
            juri juri = JuriRepository.GetOne(id);
            juri.juri_sorteado = true;
            JuriRepository.Edit(juri);

            //Cria a lista de presença
            List<inscricao> lst = InscricaoRepository.GetAllByAutorizados(juri.id_juri);

            foreach (var item in lst)
            {
                presenca temp = new presenca();
                temp.pfk_id_juri = juri.id_juri;
                temp.pfk_id_inscricao = item.id_inscricao;

                PresencaRepository.Create(temp);
            }


            return View(sorteados);
        }        

        // Função de realização de sorteio de funções
        public List<inscricao> Sorteio(List<inscricao> lista, int qtdFuncao)
        {
            Random randNum = new Random();
            List<int> nSorteados = new List<int>();
            List<inscricao> sorteados = new List<inscricao>();
            while (nSorteados.Count < qtdFuncao)
            {
                var n = randNum.Next(0, (lista.Count - 1));
                if (!nSorteados.Contains(n))
                    nSorteados.Add(n);
            }

            foreach (var insc in lista)
            {
                foreach (var n in nSorteados) {
                    if (lista.IndexOf(insc) == n)
                    {
                       sorteados.Add(insc);
                       insc.inscricao_sorteada = true;
                       InscricaoRepository.Edit(insc);
                    }                        
                }

                if (insc.inscricao_sorteada == false)
                {
                    insc.fk_id_funcao = 6;
                    InscricaoRepository.Edit(insc);
                }      
            }

            return sorteados;
        }

        //
        // GET: /Inscricao/Details/5
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Details(int id)
        {
            return View(InscricaoRepository.GetOne(id));
        }

        //
        // GET: /Inscricao/Create
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        public ActionResult Create()
        {
            LoadFormJuriToInscricao();
            LoadFormFuncao();
            LoadFormCurso();

            return View(new inscricao());
        }

        //
        // POST: /Inscricao/Create
        [Authorize(Roles = "Administrador, Organizador, Professor, Aluno")]
        [HttpPost]
        public ActionResult Create(inscricao inscricao)
        {
            LoadFormJuriToInscricao();
            LoadFormFuncao();
            LoadFormCurso();

            var user = UsuarioRepository.GetOne(((usuario)Session["Usuario"]).id_usuario);

            inscricao.status_inscricao = "Pendente";
            if (user.perfil.id_perfil != 4)
                inscricao.fk_id_funcao = 6;
            user.inscricao.Add(inscricao);

            try
            {
                if (validate(inscricao))
                    return View(inscricao);
                UsuarioRepository.Edit(user);

                return RedirectToAction("List", "Juri", new { id = inscricao.fk_id_juri, message = "Inscrição realizada com sucesso!" });
            }
            catch
            {
                return View(inscricao);
            }
        }

        //
        // GET: /Inscricao/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Edit(int id)
        {
            LoadFormFuncao();

            return View(InscricaoRepository.GetOne(id));
        }

        //
        // POST: /Inscricao/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Edit(int id, inscricao inscricao)
        {
            LoadFormFuncao();
            
            try
            {
                if (validate(inscricao))
                    return View(inscricao);
                InscricaoRepository.Edit(inscricao);
                return RedirectToAction("ListByIdJuri", new { id = inscricao.fk_id_juri, message = "Inscrição editada com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Inscricao/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Delete(int id)
        {
            return View(InscricaoRepository.GetOne(id));
        }

        //
        // POST: /Inscricao/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Delete(int id, inscricao inscricao)
        {
            try
            {
                inscricao = InscricaoRepository.GetOne(id);
                if (inscricao.presenca.Count > 0)
                    return RedirectToAction("ListByIdJuri", new { id = inscricao.fk_id_juri, messageError = "Essa inscrição possui registros vinculados a seu cadastro. Não é possível excluí-la." });
                
                InscricaoRepository.Delete(inscricao);

                return RedirectToAction("ListByIdJuri", new { id = inscricao.fk_id_juri, message = "Inscrição excluída com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        public bool validate(inscricao entity)
        {
            bool retorno = false;

            if (entity.fk_id_juri == 0)
            {
                ModelState.AddModelError("fk_id_juri", "Campo obrigatório");
                retorno = true;
            }

            return retorno;
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

        public void LoadFormJuriToInscricao()
        {
            IEnumerable<juri> lst = JuriRepository.GetAllByNotSorted();
            int c = lst.Count();
            ViewData["lstJuriToInscricao"] = lst;
        }

        public void LoadFormCurso()
        {
            IEnumerable<curso> lst = CursoRepository.GetAll();
            int c = lst.Count();
            ViewData["lstCurso"] = lst;
        }

    }
}
