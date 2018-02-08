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
using System.Web.UI;

namespace PersonalLedger.Controllers
{
    public class AdminController : Controller
    {
        #region Fields
        private List<Category> categories;
        private readonly IDatabaseRepository dbRepo;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        public AdminController(IDatabaseRepository dr)
        {
            if (dr == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                dbRepo = dr;
            }
        }  //ctor

        [OutputCache(Location = OutputCacheLocation.None)]
        public JsonResult GetCategories()
        {
            CategoryCollection cc = new CategoryCollection();
            categories = dbRepo.Categories();
            cc.ExpenseCategories = categories.Where(x => x.TypeId == 4).ToList();
            cc.IncomeCategories = categories.Where(x => x.TypeId == 3).ToList();
            cc.OtherCategories = categories.Where(x => x.TypeId == 1 || x.TypeId == 2).ToList();
            return Json(cc, JsonRequestBehavior.AllowGet);
        }


        #region Views
        public ActionResult Admin()
        {
            try
            {
                return View("Admin");
            }
            catch (Exception e)
            {
                HandleException(e, "Admin", "");
                return View("Error");
            }
        }  //Admin
        #endregion
        #region Infrastructure
        private string SanitizeHTML(string input)
        {
            input = input.Replace("<", " less than ").Replace("<=", " less or equal to ").Replace(">", " greater than ").Replace(">=", " greater or equal to ").Replace("\\", " [backslash] ");
            return AntiXssEncoder.HtmlEncode(input, true);
        }

        private void HandleException(Exception e, string method, string userMessage)
        {
            Debug.WriteLine("An error occurred in AdminControllery/" + method + ".\n" + e.Message + ".\n" + userMessage);
            logger.Error("An error occurred in AdminControllery/" + method + ".\n" + e.Message + ".\n" + userMessage);
        }  //HandleException
        #endregion
    }  //class
}  //namespace