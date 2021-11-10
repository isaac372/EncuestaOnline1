using EncuestaOnline.Controllers;
using EncuestaOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EncuestaOnline.Filters
{
    public class VerificaSession : ActionFilterAttribute
    {
        private Users _usuario;
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                base.OnActionExecuted(filterContext);

                _usuario = (Users)HttpContext.Current.Session["User"];

                if (_usuario == null)
                {
                    if (filterContext.Controller is AccesoController == false)
                    {
                        filterContext.HttpContext.Response.Redirect("/Acceso/Index");
                    }
                }
            }
            catch (Exception)
            {
                filterContext.Result = new RedirectResult("~/Acceso/Index");
            }
        }
    }
}