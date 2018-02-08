using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PersonalLedger.DomainModels;
using PersonalLedger.ViewModels;
using PersonalLedger.Repositories;
using System.Text;
using System.Diagnostics;
using System.Web.Security.AntiXss;
using NLog;

namespace PersonalLedger.Controllers
{
    public class HomeController : Controller
    {
        #region Fields
        private readonly IDatabaseRepository dbRepo;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion
        public HomeController(IDatabaseRepository idbr)
        {
            if (idbr == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                dbRepo = idbr;
            }
        }  //constructor
        #region Views
        public ActionResult Index()
        {
            return View("Index");
        }
        #endregion
        #region Infrastructure
        private string SanitizeHTML(string input)
        {
            input = input.Replace("<", " less than ").Replace("<=", " less or equal to ").Replace(">", " greater than ").Replace(">=", " greater or equal to ").Replace("\\", " [backslash] ");
            return AntiXssEncoder.HtmlEncode(input, true);
        }

        private void HandleException(Exception e, string method, string userMessage)
        {
            Debug.WriteLine("An error occurred in HomeControllery/" + method + ".\n" + e.Message + ".\n" + userMessage);
            logger.Error("An error occurred in HomeControllery/" + method + ".\n" + e.Message + ".\n" + userMessage);
        }  //HandleException
        #endregion
    }  //class
}  //namespace