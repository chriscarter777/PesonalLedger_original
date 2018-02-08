using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PersonalLedger.DomainModels;
using PersonalLedger.ViewModels;
using PersonalLedger.Repositories;
using System.Web.Security.AntiXss;
using System.Diagnostics;
using NLog;
using System.Web.UI;

namespace PersonalLedger.Controllers
{
    public class PayeesController : Controller
    {
        #region Fields
        private readonly IDatabaseRepository dbRepo;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private List<Payee> payees;
        #endregion

        public PayeesController(IDatabaseRepository idbr)
        {
            if (idbr == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                dbRepo = idbr;
            }
        }  //ctor

        #region Views
        [HttpGet]
        public ActionResult Payees()
        {
            try
            {
                PayeesViewModel model = new PayeesViewModel();
                model.Payees = dbRepo.Payees();
                return View("Payees", model);
            }
            catch (Exception e)
            {
                HandleException(e, "Payees", "");
                return View("Error");
            }
        }  //Payees

        [HttpPost]
        public ActionResult Payees(PayeesViewModel model)
        {
            return View("Payees", model);
        }  //Payees(model)
        #endregion
        #region API
        [OutputCache(Location = OutputCacheLocation.None)]
        public JsonResult GetPayees()
        {
            payees = dbRepo.Payees();
            return Json(payees, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add/Delete/Update
        public void AddPayee(PayeesViewModel model, ISessionRepository sr)
        {
            try
            {
                ISessionRepository sessionRepo = sr;
                dbRepo.AddAccount(model.NewPayee);
                sessionRepo.SetPayeeVariables();
            }
            catch (Exception e)
            {
                HandleException(e, "AddPayee", "");
            }
        }  //AddPayee

        public void DeletePayee(int id, ISessionRepository sr)
        {
            try
            {
                dbRepo.DeleteAccount(id);
                ISessionRepository sessionRepo = sr;
                sessionRepo.SetPayeeVariables();
            }
            catch (Exception e)
            {
                HandleException(e, "DeletePayee", "");
            }
        }  //DeletePayee

        public void UpdatePayee(PayeesViewModel model, int id, ISessionRepository sr)
        {
            try
            {
                dbRepo.UpdatePayee(model.Payees[id]);
                ISessionRepository sessionRepo = sr;
                sessionRepo.SetPayeeVariables();
            }
            catch (Exception e)
            {
                HandleException(e, "UpdatePayee", "");
            }
        }  //UpdatePayee
        #endregion
        #region Infrastructure
        private string SanitizeHTML(string input)
        {
            input = input.Replace("<", " less than ").Replace("<=", " less or equal to ").Replace(">", " greater than ").Replace(">=", " greater or equal to ").Replace("\\", " [backslash] ");
            return AntiXssEncoder.HtmlEncode(input, true);
        }

        private void HandleException(Exception e, string method, string userMessage)
        {
            Debug.WriteLine("An error occurred in PayeesController/" + method + ".\n" + e.Message + ".\n" + userMessage);
            logger.Error("An error occurred in PayeesController/" + method + ".\n" + e.Message + ".\n" + userMessage);
        }  //HandleException
        #endregion
    }  //class
}  //namespace