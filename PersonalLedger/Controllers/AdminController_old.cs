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
    public class AdminController_old : Controller
    {
        #region Fields
        private readonly IDatabaseRepository dbRepo;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion
        public AdminController_old(IDatabaseRepository dr)
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
        #region Views
        public ActionResult Admin()
        {
            try
            {
                AdminViewModel model = new AdminViewModel();
                model.Accounts = dbRepo.Accounts();
                List<Category> Categories = dbRepo.Categories();
                model.ExpenseCategories = Categories.Where(x => x.TypeId == 4).ToList();
                model.IncomeCategories = Categories.Where(x => x.TypeId == 3).ToList();
                model.OtherCategories = Categories.Where(x => x.TypeId == 1 || x.TypeId == 2).ToList();
                model.Payees = dbRepo.Payees();
                model.Reports = dbRepo.Reports();
                model.Transactions = dbRepo.Transactions();
                return View("Admin", model);
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