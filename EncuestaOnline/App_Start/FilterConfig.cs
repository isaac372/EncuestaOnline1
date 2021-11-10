﻿using System.Web;
using System.Web.Mvc;

namespace EncuestaOnline
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Filters.VerificaSession());
        }
    }

}
