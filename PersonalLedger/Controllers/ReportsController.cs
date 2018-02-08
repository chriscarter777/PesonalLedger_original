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
    public class ReportsController : Controller
    {
        #region Fields
        private readonly IDatabaseRepository dbRepo;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private List<Report> reports;
        #endregion

        public ReportsController(IDatabaseRepository idbr)
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
        public ActionResult Reports()
        {
            try
            {
                ReportsViewModel model = new ReportsViewModel();
                model.Reports = dbRepo.Reports();
                return View("Reports", model);
            }
            catch (Exception e)
            {
                HandleException(e, "Reports", "");
                return View("Error");
            }
        }  //Reports

        [HttpPost]
        public ActionResult Reports(ReportsViewModel model)
        {
            return View("Reports", model);
        }  //Reports(model)
        #endregion
        #region API
        [OutputCache(Location = OutputCacheLocation.None)]
        public JsonResult GetReports()
        {
            reports = dbRepo.Reports();
            return Json(reports, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add/Delete/Update
        public ActionResult AddReport(ReportsViewModel model)
        {
            try
            {
                dbRepo.AddReport(model.NewReport);
                return RedirectToAction("Reports");
            }
            catch (Exception e)
            {
                HandleException(e, "AddReport", "");
                return View("Error");
            }
        }  //AddReport

        public ActionResult DeleteReport(int id)
        {
            try
            {
                dbRepo.DeleteReport(id);
                return RedirectToAction("Reports");
            }
            catch (Exception e)
            {
                HandleException(e, "DeleteReport", "");
                return View("Error");
            }
        }  //DeleteReport

        public ActionResult UpdateReport(ReportsViewModel model, int id)
        {
            try
            {
                dbRepo.UpdateReport(model.Reports[id]);
                return RedirectToAction("Reports");
            }
            catch (Exception e)
            {
                HandleException(e, "UpdateReport", "");
                return View("Error");
            }
        }  //UpdateReport
        #endregion
        #region Run Reports
        public ActionResult RunCustom()
        {
            try
            {
                ReportsViewModel model = new ReportsViewModel();
                model.Transactions = dbRepo.Transactions().Where(x => x.CategoryId != ApplicationRepository.CategoryTypeId("Adjustment") && x.CategoryId != ApplicationRepository.CategoryTypeId("Transfer")).ToList();
                return View("Report_Custom", model);
            }
            catch (Exception e)
            {
                HandleException(e, "RunCustom", "");
                return View("Error");
            }
        }  //RunCustom

        public ActionResult RunIE30()
        {
            try
            {
                ReportsViewModel model = new ReportsViewModel();
                model.Transactions = dbRepo.Transactions().Where(x => x.CategoryId != ApplicationRepository.CategoryTypeId("Adjustment") && x.CategoryId != ApplicationRepository.CategoryTypeId("Transfer")).ToList();

                return View("Report_IE", model);
            }
            catch (Exception e)
            {
                HandleException(e, "RunIE30", "");
                return View("Error");
            }
        }  //RunIE30

        public ActionResult RunIE90()
        {
            try
            {
                ReportsViewModel model = new ReportsViewModel();
                model.Transactions = dbRepo.Transactions().Where(x => x.CategoryId != ApplicationRepository.CategoryTypeId("Adjustment") && x.CategoryId != ApplicationRepository.CategoryTypeId("Transfer")).ToList();

                return View("Report_IE", model);
            }
            catch (Exception e)
            {
                HandleException(e, "RunIE90", "");
                return View("Error");
            }
        }  //RunIE90

        public ActionResult RunIE365()
        {
            try
            {
                ReportsViewModel model = new ReportsViewModel();
                model.Transactions = dbRepo.Transactions().Where(x => x.CategoryId != ApplicationRepository.CategoryTypeId("Adjustment") && x.CategoryId != ApplicationRepository.CategoryTypeId("Transfer")).ToList();

                return View("Report_IE", model);
            }
            catch (Exception e)
            {
                HandleException(e, "RunIE365", "");
                return View("Error");
            }
        }  //RunIE365

        public ActionResult RunRNW365()
        {
            try
            {
                ReportsViewModel model = new ReportsViewModel();
                model.Transactions = dbRepo.Transactions().Where(x => x.CategoryId != ApplicationRepository.CategoryTypeId("Adjustment") && x.CategoryId != ApplicationRepository.CategoryTypeId("Transfer")).ToList();
                model.Accounts = dbRepo.Accounts();

                return View("Report_NW", model);
            }
            catch (Exception e)
            {
                HandleException(e, "RunRNW365", "");
                return View("Error");
            }
        }  //RunRNW365

        public ActionResult RunNW()
        {
            try
            {
                ReportsViewModel model = new ReportsViewModel();
                model.Transactions = dbRepo.Transactions().Where(x => x.CategoryId != ApplicationRepository.CategoryTypeId("Adjustment") && x.CategoryId != ApplicationRepository.CategoryTypeId("Transfer")).ToList();
                model.Accounts = dbRepo.Accounts();
                return View("Report_NW", model);
            }
            catch (Exception e)
            {
                HandleException(e, "RunNW", "");
                return View("Error");
            }
        }  //RunNW

        public ActionResult RunCF365()
        {
            try
            {
                ReportsViewModel model = new ReportsViewModel();
                model.Transactions = dbRepo.Transactions().Where(x => x.CategoryId != ApplicationRepository.CategoryTypeId("Adjustment") && x.CategoryId != ApplicationRepository.CategoryTypeId("Transfer")).ToList();
                return View("Report_CF", model);
            }
            catch (Exception e)
            {
                HandleException(e, "RunCF365", "");
                return View("Error");
            }
        }  //RunCF365

        public ActionResult RunCF()
        {
            try
            {
                ReportsViewModel model = new ReportsViewModel();
                model.Transactions = dbRepo.Transactions().Where(x => x.CategoryId != ApplicationRepository.CategoryTypeId("Adjustment") && x.CategoryId != ApplicationRepository.CategoryTypeId("Transfer")).ToList();

                return View("Report_CF", model);
            }
            catch (Exception e)
            {
                HandleException(e, "RunCF", "");
                return View("Error");
            }
        }  //RunCF
        #endregion
        #region Infrastructure
        private string SanitizeHTML(string input)
        {
            input = input.Replace("<", " less than ").Replace("<=", " less or equal to ").Replace(">", " greater than ").Replace(">=", " greater or equal to ").Replace("\\", " [backslash] ");
            return AntiXssEncoder.HtmlEncode(input, true);
        }

        private void HandleException(Exception e, string method, string userMessage)
        {
            Debug.WriteLine("An error occurred in ReportsController/" + method + ".\n" + e.Message + ".\n" + userMessage);
            logger.Error("An error occurred in ReportsController/" + method + ".\n" + e.Message + ".\n" + userMessage);
        }  //HandleException
        #endregion
    }  //class
}  //namespace