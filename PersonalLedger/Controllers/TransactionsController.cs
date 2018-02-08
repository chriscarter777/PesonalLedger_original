using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PersonalLedger.DomainModels;
using PersonalLedger.ViewModels;
using PersonalLedger.Repositories;
using System.Diagnostics;
using System.Web.Security.AntiXss;
using NLog;
using System.Web.UI;

namespace PersonalLedger.Controllers
{
    public class TransactionsController : Controller
    {
        #region Fields
        private readonly IDatabaseRepository dbRepo;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private List<Transaction> transactions;
        #endregion

        public TransactionsController(IDatabaseRepository idbr)
        {
            if(idbr == null)
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
        public ActionResult Transactions()
        {
            try
            {
                TransactionsViewModel model = new TransactionsViewModel();
                model.Transactions = dbRepo.Transactions();
                model.NewTransaction = new Transaction();
                return View("Transactions", model);
            }
            catch (Exception e)
            {
                HandleException(e, "Transactions", "");
                return View("Error");
            }
        }  //Transactions

        [HttpPost]
        public ActionResult Transactions(TransactionsViewModel model)
        {
            return View("Transactions(model)", model);
        }  //Transactions(model)
        #endregion
        #region API
        [OutputCache(Location = OutputCacheLocation.None)]
        public JsonResult GetTransactions()
        {
            transactions = dbRepo.Transactions();
            return Json(transactions, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add/Delete/Update
        public ActionResult AddTransaction(TransactionsViewModel model, ISessionRepository sr)
        {
            //add the payee if it's new.  Otherwise, update it if necessary
            try
            {
                ISessionRepository sessionRepo = sr;
                if (model.NewTransaction.Payee != null)
                {
                    if (sessionRepo.PayeeId(model.NewTransaction.Payee) > 0)
                    {
                        model.NewTransaction.PayeeId = sessionRepo.PayeeId(model.NewTransaction.Payee);
                        if (sessionRepo.PayeeAmount((int)model.NewTransaction.PayeeId) != model.NewTransaction.Amount || sessionRepo.PayeeCategory((int)model.NewTransaction.PayeeId) != model.NewTransaction.CategoryId)
                        {
                            sessionRepo.UpdatePayeeDefaults((int)model.NewTransaction.PayeeId, model.NewTransaction.Amount, model.NewTransaction.CategoryId);
                        }
                    }
                    else
                    {
                        dbRepo.AddPayee(model.NewTransaction.Payee);
                        sessionRepo.SetPayeeVariables();
                    }
                }
                dbRepo.AddTransaction(model.NewTransaction);
                return RedirectToAction("Transactions");
            }
            catch (Exception e)
            {
                HandleException(e, "AddTransaction", "");
                return View("Error");
            }
        }  //AddTransaction

        public ActionResult DeleteTransaction(int id)
        {
            try
            {
                dbRepo.DeleteTransaction(id);
                return RedirectToAction("Transactions");
            }
            catch (Exception e)
            {
                HandleException(e, "DeleteTransaction", "");
                return View("Error");
            }
        }  //DeleteTransaction

        [HttpGet]
        public ActionResult EditTransaction(int id)
        {
            try
            {
                TransactionsViewModel model = new TransactionsViewModel();
                model.EditTransaction = dbRepo.Transaction(id);

                return PartialView("_EditTransaction", model);
            }
            catch (Exception e)
            {
                HandleException(e, "EditTransaction", "");
                return View("Error");
            }
        }  //EditTransaction

        [HttpPost]
        public ActionResult UpdateTransaction(TransactionsViewModel model)
        {
            try
            {
                dbRepo.UpdateTransaction(model.EditTransaction);
                return RedirectToAction("Transactions");
            }
            catch (Exception e)
            {
                HandleException(e, "UpdateTransaction", "");
                return View("Error");
            }
        }  //UpdateTransaction
        #endregion
        #region Ajax
        public int CategoryType(int id, ISessionRepository sr)
        {
            try
            {
                ISessionRepository sessionRepo = sr;
                if (sessionRepo.CategoryType(id) > 0)
                {
                    return sessionRepo.CategoryType(id);
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception e)
            {
                HandleException(e, "CategoryType", "");
                return 3;
            }
        }  //CategoryType

        public string PayeeData(string id, ISessionRepository sr)
        {
            try
            {
                ISessionRepository sessionRepo = sr;
                decimal amount = 0;
                int catId = 0;
                if (sessionRepo.PayeeId(id) > 0)
                {
                    int payeeId = sessionRepo.PayeeId(id);
                    if (sessionRepo.PayeeAmount(payeeId) > 0)
                    {
                        amount = sessionRepo.PayeeAmount(payeeId);
                    }
                    if (sessionRepo.PayeeCategory(payeeId) > 0)
                    {
                        catId = sessionRepo.PayeeCategory(payeeId);
                    }
                    return "{amount: " + amount + ", catId: " + catId + ", id: " + id + "}";
                }
                else
                {
                    return "{amount: 0, catId: 0, id: 0}";
                }
            }
            catch (Exception e)
            {
                HandleException(e, "PayeeData", "");
                return "{amount: 0, catId: 0, id: 0}";
            }
        } //PayeeData
        #endregion
        #region Infrastructure
        private string SanitizeHTML(string input)
        {
            input = input.Replace("<", " less than ").Replace("<=", " less or equal to ").Replace(">", " greater than ").Replace(">=", " greater or equal to ").Replace("\\", " [backslash] ");
            return AntiXssEncoder.HtmlEncode(input, true);
        }

        private void HandleException(Exception e, string method, string userMessage)
        {
            Debug.WriteLine("An error occurred in TransactionsController/" + method + ".\n" + e.Message + ".\n" + userMessage);
            logger.Error("An error occurred in TransactionsController/" + method + ".\n" + e.Message + ".\n" + userMessage);
        }  //HandleException
        #endregion
    }  //class
}  //namespace