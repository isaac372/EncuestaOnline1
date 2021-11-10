using EncuestaOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EncuestaOnline.Controllers
{
    public class AccesoController : Controller
    {
        EncuestaEntities _encuestaContext = new EncuestaEntities();
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Users users)
        {
            try
            {
                if (!String.IsNullOrEmpty(users.name) && !String.IsNullOrEmpty(users.password))
                {
                    var desencript = encrypt.GetSHA256(users.password);
                    Users BuscarUsuario =
                   _encuestaContext.Users.Where(user => user.name == users.name && user.password == desencript).FirstOrDefault();

                    if (BuscarUsuario == null)
                    {
                        ViewBag.Error = "Usuario o Contraseña Invalida";
                        return View();
                    }
                    Session["User"] = BuscarUsuario;
                }
                else
                {

                }
                return RedirectToAction("Index", "Surveys");
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Users users)
        {
            try
            {
                if (!String.IsNullOrEmpty(users.name) && !String.IsNullOrEmpty(users.password))
                {
                    var encriptPassword = encrypt.GetSHA256(users.password).ToString();
                    users.password = encriptPassword;
                    _encuestaContext.Users.Add(users);
                    _encuestaContext.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {

                }
            }
            catch (Exception e)
            {
                var Mensaje = e.Message;
            }
            return View();
        }
    }
}