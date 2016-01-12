using Sisjuri.Helpers;
using Sisjuri.Models;
using Sisjuri.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sisjuri.Controllers
{

    public class UsuarioController : BaseController
    {
        private UsuarioRepository _UsuarioRepository;
        private PerfilRepository _PerfilRepository;
        private FaculdadeRepository _FaculdadeRepository;
        private CursoRepository _CursoRepository;

        public UsuarioRepository UsuarioRepository
        {
            get {
                if (_UsuarioRepository == null)
                    _UsuarioRepository = new UsuarioRepository();
                return _UsuarioRepository; }
            set { _UsuarioRepository = value; }
        }
        public PerfilRepository PerfilRepository
        {
            get
            {
                if (_PerfilRepository == null)
                    _PerfilRepository = new PerfilRepository();
                return _PerfilRepository;
            }
            set { _PerfilRepository = value; }
        }
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
        // GET: /Usuario/
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Index()
        {
            LoadFormCurso();
            return View(new usuario());
        }

        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult List(usuario entity, String message, String messageError)
        {
            ViewData["message"] = message;
            ViewData["messageError"] = messageError;
            if (string.IsNullOrEmpty(entity.nome_completo) && (string.IsNullOrEmpty(entity.cpf_usuario) &&
                (entity.fk_id_curso == 0)))
            {
                return View(UsuarioRepository.GetAll());
            }
            else
            {
                return View(UsuarioRepository.GetAllByCriteria(entity.nome_completo ?? "", entity.cpf_usuario ?? "", 
                    entity.fk_id_curso ?? 0));
            }
        }

        //
        // GET: /Usuario/Details/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Details(int id)
        {
            return View(UsuarioRepository.GetOne(id));
        }

        //
        // GET: /Usuario/Create
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Create()
        {
            LoadFormPerfil();
            LoadFormFaculdade();

            return View(new usuario());
        }

        //
        // POST: /Usuario/Create
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Create(usuario usuario)
        {
            LoadFormPerfil();
            LoadFormFaculdade(); 

            try
            {
                usuario.fk_id_faculdade = 1; //Faculdade CNEC Unaí

                if (validateusuario(usuario))
                    return View(usuario);

                usuario.senha_usuario = SecurityHelper.EncryptData(usuario.senha_usuario);
                UsuarioRepository.Create(usuario);

                return RedirectToAction("List", new { message = "Dados cadastrados com sucesso!" });
            }
            catch
            {
                return View(usuario);
            }
        }

        //
        // GET: /Usuario/CreateAluno
        public ActionResult CreateAluno()
        {
            LoadFormCurso();

            return PartialView(new usuario());
        }

        //
        // POST: /Usuario/CreateAluno
        [HttpPost]
        public ActionResult CreateAluno(usuario usuario)
        {
            LoadFormCurso();

            try
            {
                usuario.fk_id_perfil = 4; //Aluno
                usuario.fk_id_faculdade = 1; //Faculdade CNEC Unaí

                if (validatealuno(usuario))
                    return PartialView(usuario);

                usuario.senha_usuario = SecurityHelper.EncryptData(usuario.senha_usuario);
                UsuarioRepository.Create(usuario);

                return RedirectToAction("Login","Account", new { message = "Usuário criado com sucesso!" });
            }
            catch
            {
                return PartialView(usuario);
            }
        }

        //
        // GET: /Usuario/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Edit(int id)
        {
            LoadFormPerfil();
            LoadFormFaculdade(); 

            return View(UsuarioRepository.GetOne(id));
        }

        //
        // POST: /Usuario/Edit/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Edit(int id, usuario usuario)
        {
            LoadFormPerfil();
            LoadFormFaculdade(); 

            try
            {

                usuario.fk_id_faculdade = 1; //Faculdade CNEC Unaí

                if (validateusuario(usuario))
                    return View(usuario);

                usuario.senha_usuario = SecurityHelper.EncryptData(usuario.senha_usuario);
                UsuarioRepository.Edit(usuario);
                return RedirectToAction("List", new { message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Usuario/EditAluno
        [Authorize(Roles = "Aluno")]
        public ActionResult EditAluno(usuario usuario)
        {
            LoadFormCurso();

            usuario = (((Sisjuri.Models.usuario)Session["usuario"]));
            return View(UsuarioRepository.GetOne(usuario.id_usuario));
        }

        //
        // POST: /Usuario/EditAluno
        [Authorize(Roles = "Aluno")]
        [HttpPost]
        public ActionResult EditAluno(int id, usuario usuario)
        {
            LoadFormCurso();

            try
            {
                if (validatealuno(usuario))
                    return View(usuario);

                usuario.fk_id_faculdade = 1; // Faculdade CNEC Unaí
                usuario.fk_id_perfil = 4; // Aluno

                usuario.senha_usuario = SecurityHelper.EncryptData(usuario.senha_usuario);
                UsuarioRepository.Edit(usuario);
                return RedirectToAction("Index", "Home", new { message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Usuario/EditProfessor
        [Authorize(Roles = "Professor")]
        public ActionResult EditProfessor(usuario usuario)
        {
            usuario = (((Sisjuri.Models.usuario)Session["usuario"]));
            return View(UsuarioRepository.GetOne(usuario.id_usuario));
        }

        //
        // POST: /Usuario/EditProfessor
        [Authorize(Roles = "Professor")]
        [HttpPost]
        public ActionResult EditProfessor(int id, usuario usuario)
        {
            try
            {
                if (validateusuario(usuario))
                    return View(usuario);

                usuario.fk_id_faculdade = 1; // Faculdade CNEC Unaí
                usuario.fk_id_perfil = 3; // Professor

                List<usuario> cpfdosusuarios = UsuarioRepository.GetAll();

                usuario.senha_usuario = SecurityHelper.EncryptData(usuario.senha_usuario);
                UsuarioRepository.Edit(usuario);
                return RedirectToAction("Index", "Home", new { message = "Dados editados com sucesso!" });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Usuario/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        public ActionResult Delete(int id)
        {
            return View(UsuarioRepository.GetOne(id));
        }

        //
        // POST: /Usuario/Delete/5
        [Authorize(Roles = "Administrador, Organizador")]
        [HttpPost]
        public ActionResult Delete(int id, usuario usuario)
        {
            try
            {
                if(usuario != null)
                    usuario = UsuarioRepository.GetOne(id);
                if (usuario.inscricao.Count > 0)
                    return RedirectToAction("List", new { messageError = "Esse usuário possui registros vinculados a seu cadastro. Não é possível excluí-lo." });
                
                UsuarioRepository.Delete(usuario);
                return RedirectToAction("List", new { message = "Dados excluídos com sucesso!" });
            }
            catch
            {
                return View();
            }
        }


        public bool validatealuno (usuario entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.nome_completo))
            {
                ModelState.AddModelError("nome_completo", "Campo obrigatório");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.cpf_usuario))
            {
                ModelState.AddModelError("cpf_usuario", "Campo obrigatório");
                retorno = true;
            }
            if (!ValidaCPF(entity.cpf_usuario))
            {
                ModelState.AddModelError("cpf_usuario", "CPF inválido");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.senha_usuario))
            {
                ModelState.AddModelError("senha_usuario", "Campo obrigatório");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.email_usuario))
            {
                ModelState.AddModelError("email_usuario", "Campo obrigatório");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.telefone_usuario))
            {
                ModelState.AddModelError("telefone_usuario", "Campo obrigatório");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.num_matric_aluno))
            {
                ModelState.AddModelError("num_matric_aluno", "Campo obrigatório");
                retorno = true;
            }
            if (entity.periodo_aluno == null)
            {
                ModelState.AddModelError("periodo_aluno", "Campo obrigatório");
                retorno = true;
            }

            return retorno;
        }

        public bool validateusuario (usuario entity)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(entity.nome_completo))
            {
                ModelState.AddModelError("nome_completo", "Campo obrigatório");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.cpf_usuario))
            {
                ModelState.AddModelError("cpf_usuario", "Campo obrigatório");
                retorno = true;
            }
            if (!ValidaCPF(entity.cpf_usuario))
            {
                ModelState.AddModelError("cpf_usuario", "CPF inválido");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.senha_usuario))
            {
                ModelState.AddModelError("senha_usuario", "Campo obrigatório");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.email_usuario))
            {
                ModelState.AddModelError("email_usuario", "Campo obrigatório");
                retorno = true;
            }
            if (string.IsNullOrEmpty(entity.telefone_usuario))
            {
                ModelState.AddModelError("telefone_usuario", "Campo obrigatório");
                retorno = true;
            }

            return retorno;
        }

        //Método que valida o CPF
        public bool ValidaCPF(string vrCPF)
        {
            string valor = vrCPF.Replace(".", "");
            valor = valor.Replace("-", "");
            
            if (valor.Length != 11)
                return false;
 
            bool igual = true;
            for (int i = 1; i < 11 && igual; i++)
                if (valor[i] != valor[0])
                    igual = false;
 
            if (igual || valor == "12345678909")
                return false;
 
            int[] numeros = new int[11];
            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(
                valor[i].ToString());
 
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];
           
            int resultado = soma % 11;
            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;
 
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];
 
            resultado = soma % 11;
 
            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
 
            }
            else
                if (numeros[10] != 11 - resultado)
                    return false;
            return true;
 
        }

        public void LoadFormPerfil()
        {
            IEnumerable<perfil> lst = PerfilRepository.GetAll();
            int c = lst.Count();
            ViewData["lstPerfil"] = lst;
        }

        public void LoadFormFaculdade()
        {
            IEnumerable<faculdade> lst = FaculdadeRepository.GetAll();
            int c = lst.Count();
            ViewData["lstFaculdade"] = lst;
        }

        public void LoadFormCurso()
        {
            IEnumerable<curso> lst = CursoRepository.GetAll();
            int c = lst.Count();

            curso temp = new curso();
            temp.id_curso = 0;
            temp.nome_curso = "";

            List<curso> lstTemp = new List<curso>();
            lstTemp.Add(temp);

            foreach (var item in lst)
            {
                lstTemp.Add(item);
            }

            ViewData["lstCurso"] = lstTemp.AsEnumerable<curso>();
        }


    }
}
