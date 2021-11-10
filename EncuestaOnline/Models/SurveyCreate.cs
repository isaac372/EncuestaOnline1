using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncuestaOnline.Models
{
    public class SurveyCreate
    {
        public int NumberOfQuestions { get; set; }

        public int NumberOfOptions { get; set; }
    }
}