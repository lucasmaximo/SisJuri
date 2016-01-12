using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Sisjuri.Models;
using Sisjuri.Repository;
using System.Web.Security;
using Sisjuri.Helpers;
using System.Web.Configuration;
using System.Configuration;
using System.Net.Mail;

namespace Sisjuri.Controllers
{

    public class AccountController : BaseController
    {
        private UsuarioRepository _UsuarioRepository;

        public UsuarioRepository UsuarioRepository
        {
            get {
                if (_UsuarioRepository == null)
                    _UsuarioRepository = new UsuarioRepository();
                return _UsuarioRepository; }
            set { _UsuarioRepository = value; }
        }

        //
        // GET: /Account/Login
        public ActionResult Login(string returnUrl, string message)
        {
            ViewData["message"] = message;
            ViewBag.ReturnUrl = returnUrl;
            return PartialView();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                usuario user = null;
                {
                    if (ValidateLogin(model))
                    {
                        return PartialView(model);
                    }

                    if (ValidateAutenticacao(model, out user))
                    {
                        return PartialView(model);
                    }

                    HttpCookiesSection cookieSection = (HttpCookiesSection)ConfigurationManager.GetSection("system.web/httpCookies");
                    AuthenticationSection authenticationSection = (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");

                    FormsAuthenticationTicket authTicket =
                        new FormsAuthenticationTicket(
                        1, user.perfil.nome_perfil, DateTime.Now, DateTime.Now.AddMinutes(authenticationSection.Forms.Timeout.TotalMinutes),
                        false, string.Empty);
                    
                    String encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                    FormsAuthentication.Authenticate(user.perfil.nome_perfil, null);

                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                    if (cookieSection.RequireSSL || authenticationSection.Forms.RequireSSL)
                    {
                        authCookie.Secure = true;
                    }

                    HttpContext.Response.Cookies.Add(authCookie);

                    FormsAuthentication.SetAuthCookie(user.perfil.nome_perfil, true);

                    Session["usuario"] = user;

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception)
            {
                return PartialView(model); 

            }
        }

        //Método de gerenciamento de sessão
        private void SetAuthenticationCookie(string employeeID, List<string> roles)
        {
            HttpCookiesSection cookieSection = (HttpCookiesSection)ConfigurationManager.GetSection("system.web/httpCookies");
            AuthenticationSection authenticationSection = (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");

            FormsAuthenticationTicket authTicket =
                new FormsAuthenticationTicket(
                1, employeeID.ToString(), DateTime.Now, DateTime.Now.AddMinutes(authenticationSection.Forms.Timeout.TotalMinutes),
                false, string.Empty);

            Session["usuario"] = string.Join("|", roles.ToArray());

            String encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            FormsAuthentication.Authenticate(employeeID, null);

            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            if (cookieSection.RequireSSL || authenticationSection.Forms.RequireSSL)
            {
                authCookie.Secure = true;
            }

            HttpContext.Response.Cookies.Add(authCookie);

            FormsAuthentication.SetAuthCookie(employeeID, true);
        }

        //Método de envio de e-mail de recuperação de senha
        public void EnviarEmail(usuario user)
        {

            MailMessage mail = new MailMessage();
            mail.To.Add(user.email_usuario);

            string remetenteEmail = "sisjuri.sisjuri@gmail.com"; //O e-mail do remetente
            mail.From = new MailAddress(remetenteEmail, "SisJuri - Recuperar senha", System.Text.Encoding.UTF8);

            mail.Subject = "SisJuri - Recuperar Senha";
            mail.Body = "Para acessar o SisJuri entre com o CPF " + user.cpf_usuario + " e senha " + SecurityHelper.DecryptData(user.senha_usuario) + ".";

            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High; //Prioridade do E-Mail



            SmtpClient client = new SmtpClient();  //Adicionando as credenciais do seu e-mail e senha:

            client.Credentials = new System.Net.NetworkCredential(remetenteEmail, "sisjurifcu");
            //ACESSE O LINK ABAIXO E CLIQUE EM ATIVAR.
            //https://www.google.com/settings/security/lesssecureapps
            client.Port = 587; // Esta porta é a utilizada pelo Gmail para envio

            client.Host = "smtp.gmail.com"; //Definindo o provedor que irá disparar o e-mail

            client.EnableSsl = true; //Gmail trabalha com Server Secured Layer

            try
            {

                client.Send(mail);

            }
            catch (Exception)
            {

            }
        }

        //
        // GET: /Account/RecuperarSenha
        public ActionResult RecuperarSenha()
        {
            return PartialView(new usuario());
        }

        //
        // POST: /Account/RecuperarSenha
        [HttpPost]
        public ActionResult RecuperarSenha(usuario usuario)
        {
            try
            {
                usuario user = UsuarioRepository.GetOneByCPF(usuario.cpf_usuario);
                if (user == null)
                {
                    ModelState.AddModelError("", "Dados inválidos.");
                    return PartialView(usuario);
                }
                else
                {
                    EnviarEmail(user);
                    ViewData["message"] = "E-mail enviado com sucesso.";
                }

                return RedirectToAction("Login", new { message = ViewData["message"] });
            }
            catch
            {

                return PartialView(usuario);
            }
        }

        //Validações de autenticação (Dados corretos?)
        public bool ValidateAutenticacao(LoginViewModel Login, out usuario user)
        {
            bool retorno = false;

            user = UsuarioRepository.GetOne(Login.UserCpf, SecurityHelper.EncryptData(Login.Password));

            if (user == null)
            {
                ModelState.AddModelError("", "Dados inválidos. Verifique os dados informados e tente novamente.");
                retorno = true;
            }

            return retorno;
        }

        //Validações de autenticação (Dados preenchidos?)
        public bool ValidateLogin(LoginViewModel Login)
        {
            bool retorno = false;
            if (string.IsNullOrEmpty(Login.UserCpf))
            {
                ModelState.AddModelError("UserCpf", "Campo obrigatório.");
                retorno = true;
            }
            if (string.IsNullOrEmpty(Login.Password))
            {
                ModelState.AddModelError("Password", "Campo obrigatório.");
                retorno = true;
            }

            return retorno;
        }

        //LogOff - Encerrar sessão
        [HttpPost]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session["usuario"] = null;

            return RedirectToAction("Login", "Account");
        }


    }
}