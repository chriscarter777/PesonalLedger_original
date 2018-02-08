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
    public class CategoriesController : Controller
    {
        #region Fields
        private List<Category> categories;
        private readonly IDatabaseRepository dbRepo;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        public CategoriesController(IDatabaseRepository idbr)
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
        public ActionResult Categories()
        {
            try
            {
                CategoriesViewModel model = new CategoriesViewModel();
                List<Category> Categories = dbRepo.Categories();
                model.ExpenseCategories = Categories.Where(x => x.TypeId == ApplicationRepository.CategoryTypeId("Expense")).ToList();
                model.IncomeCategories = Categories.Where(x => x.TypeId == ApplicationRepository.CategoryTypeId("Income")).ToList();
                model.OtherCategories = Categories.Where(x => x.TypeId == ApplicationRepository.CategoryTypeId("Adjustment") || x.TypeId == ApplicationRepository.CategoryTypeId("Transfer")).ToList();
                return View("Categories", model);
            }
            catch (Exception e)
            {
                HandleException(e, "Category", "");
                return View("Error");
            }
        }  //Category

        [HttpPost]
        public ActionResult Categories(CategoriesViewModel model)
        {
            return View("Categories", model);
        }  //Category(model)
        #endregion
        #region API
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
        #endregion
        #region Add/Delete/Update
        public ActionResult AddCategory(CategoriesViewModel model, ISessionRepository sr)
        {
            try
            {
                dbRepo.AddCategory(model.NewCategory);
                ISessionRepository sessionRepo = sr;
                sessionRepo.SetCategoryVariables();
                return RedirectToAction("Categories");
            }
            catch (Exception e)
            {
                HandleException(e, "AddCategory", "");
                return View("Error");
            }
        }  //AddCategory

        public ActionResult DeleteCategory(int id, ISessionRepository sr)
        {
            try
            {
                dbRepo.DeleteCategory(id);
                ISessionRepository sessionRepo = sr;
                sessionRepo.SetCategoryVariables();
                return RedirectToAction("Categories");
            }
            catch (Exception e)
            {
                HandleException(e, "DeleteCategory", "");
                return View("Error");
            }
        }  //DeleteCategory

        public ActionResult UpdateExpenseCategory(CategoriesViewModel model, int id, ISessionRepository sr)
        {
            try
            {
                dbRepo.UpdateCategory(model.ExpenseCategories[id]);
                ISessionRepository sessionRepo = sr;
                sessionRepo.SetCategoryVariables();
                return RedirectToAction("Categories");
            }
            catch (Exception e)
            {
                HandleException(e, "UpdateExpenseCategory", "");
                return View("Error");
            }
        }  //UpdateExpenseCategory

        public ActionResult UpdateIncomeCategory(CategoriesViewModel model, int id, ISessionRepository sr)
        {
            try
            {
                dbRepo.UpdateCategory(model.IncomeCategories[id]);
                ISessionRepository sessionRepo = sr;
                sessionRepo.SetCategoryVariables();
                return RedirectToAction("Categories");
            }
            catch (Exception e)
            {
                HandleException(e, "UpdateIncomeCategory", "");
                return View("Error");
            }
        }  //UpdateIncomeCategory

        public ActionResult UpdateOtherCategory(CategoriesViewModel model, int id, ISessionRepository sr)
        {
            try
            {
                dbRepo.UpdateCategory(model.OtherCategories[id]);
                ISessionRepository sessionRepo = sr;
                sessionRepo.SetCategoryVariables();
                return RedirectToAction("Categories");
            }
            catch (Exception e)
            {
                HandleException(e, "UpdateOtherCategory", "");
                return View("Error");
            }
        }  //UpdateOtherCategory
        #endregion
        #region Infrastructure
        private string SanitizeHTML(string input)
        {
            input = input.Replace("<", " less than ").Replace("<=", " less or equal to ").Replace(">", " greater than ").Replace(">=", " greater or equal to ").Replace("\\", " [backslash] ");
            return AntiXssEncoder.HtmlEncode(input, true);
        }

        private void HandleException(Exception e, string method, string userMessage)
        {
            Debug.WriteLine("An error occurred in CategoriesController/" + method + ".\n" + e.Message + ".\n" + userMessage);
            logger.Error("An error occurred in CategoriesController/" + method + ".\n" + e.Message + ".\n" + userMessage);
        }  //HandleException
        #endregion
    }  //class
}  //namespace