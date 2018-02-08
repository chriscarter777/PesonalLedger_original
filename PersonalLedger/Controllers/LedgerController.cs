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

namespace PersonalLedger.Controllers
{
    public class LedgerController : Controller
    {
        #region Fields
        private readonly IDatabaseRepository dbRepo;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion
        public LedgerController(IDatabaseRepository idbr)
        {
            if (idbr == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                dbRepo = idbr;
            }
        }  //default constructor
        #region Views
        [HttpGet]
        public ActionResult Ledger()
        {
            try
            {
                TransactionsViewModel model = new TransactionsViewModel();
                model.Transactions = dbRepo.Transactions();
                model.NewTransaction = new Transaction();
                model.Accounts = dbRepo.Accounts();
                model.LedgerLines = ConstructLedger(model);
                return View("Ledger", model);
            }
            catch (Exception e)
            {
                HandleException(e, "Ledger", "");
                return View("Error");
            }
        }  //Ledger

        [HttpPost]
        public ActionResult Ledger(TransactionsViewModel model)
        {
            try
            {
                model.Transactions = dbRepo.Transactions();
                model.Accounts = dbRepo.Accounts();
                model.LedgerLines = ConstructLedger(model);
                return View("Ledger", model);
            }
            catch (Exception e)
            {
                HandleException(e, "Ledger(model)", "");
                return View("Error");
            }
        }  //Ledger(model)
        #endregion
        #region Add/Delete/Update
        public ActionResult AddTransaction(TransactionsViewModel model)
        {
            try
            {
                dbRepo.AddTransaction(model.NewTransaction);
                return RedirectToAction("Ledger");
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
                return RedirectToAction("Ledger");
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
                return PartialView("_EditLedgerLine", model);
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
                return RedirectToAction("Ledger");
            }
            catch (Exception e)
            {
                HandleException(e, "UpdateTransaction", "");
                return View("Error");
            }
        }  //UpdateTransaction
        #endregion
        #region Infrastructure
        private List<LedgerLine> ConstructLedger(TransactionsViewModel model)
        {
            List<LedgerLine> ledgerLines = new List<LedgerLine>();
            if (model.Transactions != null && model.Accounts != null)
            {
                if (model.Transactions.Count > 0)
                {
                    ledgerLines.Add(new LedgerLine(model.Transactions[0], model.Accounts));  //first line constructor
                    if (model.Transactions.Count > 1)
                    {
                        for (int i = 1; i < model.Transactions?.Count; i++)
                        {
                            ledgerLines.Add(new LedgerLine(model.Transactions[i], ledgerLines[i - 1]));  //subsequent line constructor
                        }
                    }
                }
            }
            return ledgerLines;
        }  //ConstructLedger

        private string SanitizeHTML(string input)
        {
            input = input.Replace("<", " less than ").Replace("<=", " less or equal to ").Replace(">", " greater than ").Replace(">=", " greater or equal to ").Replace("\\", " [backslash] ");
            return AntiXssEncoder.HtmlEncode(input, true);
        }

        private void HandleException(Exception e, string method, string userMessage)
        {
            Debug.WriteLine("An error occurred in LedgerController/" + method + ".\n" + e.Message + ".\n" + userMessage);
            logger.Error("An error occurred in LedgerController/" + method + ".\n" + e.Message + ".\n" + userMessage);
        }  //HandleException
        #endregion
    }  //class
}  //namespace