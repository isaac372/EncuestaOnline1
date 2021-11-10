using EncuestaOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EncuestaOnline.Controllers
{
    public class SurveysController : Controller
    {
        EncuestaEntities _encuestaContext = new EncuestaEntities();
        // GET: Surveys
        public ActionResult Index()
        {
            return View(_encuestaContext.Surveys.ToList());
        }

        [HttpGet]
        public ActionResult Create(SurveyCreate model)
        {
            int maxQuestions = 15;
            int maxOptions = 10;

            bool isInvalid = model == null
                || model.NumberOfQuestions <= 0
                || model.NumberOfOptions <= 1
                || model.NumberOfQuestions > maxQuestions
                || model.NumberOfOptions > maxOptions;

            if (isInvalid) return RedirectToAction("Index");

            ViewBag.Numbers = model;
            return View();
        }
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateData(Surveys model)
        {
            try
            {

                if (Session["User"] != null)
                {
                    var usuario = (Users)Session["User"];
                    var idUser = usuario.id;
                    model.UserId = idUser.ToString();
                }

                _encuestaContext.Surveys.Add(model);
                _encuestaContext.SaveChanges();

                if (Session["question"] != null)
                {
                    var sesionExist = (List<Questions>)Session["question"];
                    foreach (var item in sesionExist)
                    {
                        item.SurveyId = model.Id;
                        _encuestaContext.Questions.Add(item);
                        _encuestaContext.SaveChanges();

                        foreach (var item2 in item.Options)
                        {
                            item2.QuestionId = item.Id;
                            _encuestaContext.Options.Add(item2);
                            _encuestaContext.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception e)
            {

                var mensaje = e.Message;
            }
            Session.Remove("question");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ActualizaSession(int numlinea, string title)
        {
            var questions = new List<Questions>();
            var question = new Questions();

            if (Session["question"] != null)
            {
                var sesionExist = (List<Questions>)Session["question"];
                question.Title = title;
                question.auxnum = numlinea;
                sesionExist.Add(question);
                Session["question"] = sesionExist;
            }
            else
            {
                question.Title = title;
                question.auxnum = numlinea;
                questions.Add(question);

                Session["question"] = questions;
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActualizaSessionOptions(int idpregunta, string title)
        {

            if (Session["question"] != null)
            {
                var sesionExist = (List<Questions>)Session["question"];

                bool iterador = true;

                for (int i = 0; i < sesionExist.Count; i++)
                {
                    if (sesionExist[i].auxnum == idpregunta && iterador)
                    {
                        var Options = new Options();
                        Options.Title = title;
                        sesionExist[i].Options.Add(Options);
                        Session["question"] = sesionExist;
                        iterador = false;
                    }
                }

            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Answer(int id)
        {
            if (id <= 0) return RedirectToAction("Index");

            var survey = _encuestaContext.Surveys.Find(id);

            if (survey == null) RedirectToAction("Index");

            var surveyModel = new Surveys()
            {
                Id = survey.Id,
                Title = survey.Title,
                CreatedAt = survey.CreatedAt,
                UpdatedAt = survey.UpdatedAt,
                Questions = survey.Questions,
                FilledSurveys = survey.FilledSurveys
            };

            var filledSurveyModel = new FilledSurveys()
            {
                Surveys = survey
            };

            var tupleModel = new Tuple<Surveys, FilledSurveys>(surveyModel, filledSurveyModel);

            return View(tupleModel);
        }

        public ActionResult Edit(int id)
        {
            Surveys encuesta = _encuestaContext.Surveys.Find(id);
            return View(encuesta);
        }
        [HttpPost]
        public ActionResult Edit(Surveys encuesta)
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            Surveys encuesta = _encuestaContext.Surveys.Find(id);
            return View(encuesta);
        }
        [HttpPost]
        public ActionResult Details(Surveys encuesta)
        {
            return View();
        }

        public ActionResult Delete(int id)
        {
            Surveys encuesta = _encuestaContext.Surveys.Find(id);
            return View(encuesta);
        }
        [HttpPost]
        public ActionResult Delete(Surveys encuesta)
        {
            return View();
        }

    }
}