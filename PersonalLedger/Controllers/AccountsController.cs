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
    public class AccountsController : Controller
    {
        #region Fields
        private List<Account> accounts;
        private readonly IDatabaseRepository dbRepo;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        public AccountsController(IDatabaseRepository idbr)
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
        public ViewResult Accounts()
        {
            try
            {
                AccountsViewModel model = new AccountsViewModel();
                PopulateAccounts(model);
                return View("Accounts", model);
            }
            catch (Exception e)
            {
                HandleException(e, "Accounts", "");
                return View("Error");
            }
        }  //Accounts

        [HttpPost]
        public ViewResult Accounts(AccountsViewModel model)
        {
            return View("Accounts", model);
        }
        #endregion
        #region API
        [OutputCache(Location = OutputCacheLocation.None)]
        public JsonResult GetAccounts()
        {
            accounts = dbRepo.Accounts();
            return Json(accounts, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add/Delete/Update
        public ViewResult AddAccount(AccountsViewModel model, ISessionRepository sr)
        {
            try
            {
                dbRepo.AddAccount(model.NewAccount);
                ISessionRepository sessionRepo = sr;
                sessionRepo.SetAccountVariables();
                PopulateAccounts(model);
                return View("Accounts", model);
            }
            catch (Exception e)
            {
                HandleException(e, "AddAccount", "");
                return View("Error");
            }
        }  //AddAccount

        public ViewResult DeleteAccount(int id, ISessionRepository sr)
        {
            try
            {
                AccountsViewModel model = new AccountsViewModel();
                dbRepo.DeleteAccount(id);
                ISessionRepository sessionRepo = sr;
                sessionRepo.SetAccountVariables();
                PopulateAccounts(model);
                return View("Accounts", model);
            }
            catch (Exception e)
            {
                HandleException(e, "DeleteAccount", "");
                return View("Error");
            }
        }  //DeleteAccount

        public ViewResult UpdateAccount(AccountsViewModel model, int id, ISessionRepository sr)
        {
            try
            {
                dbRepo.UpdateAccount(model.Accounts[id]);
                ISessionRepository sessionRepo = sr;
                sessionRepo.SetAccountVariables();
                PopulateAccounts(model);
                return View("Accounts", model);
            }
            catch (Exception e)
            {
                HandleException(e, "UpdateAccount", "");
                return View("Error");
            }
        }  //UpdateAccount
        #endregion
        #region Filter/Sort
        public ViewResult Sort(string id)
        {
            try
            {
                AccountsViewModel model = new AccountsViewModel();
                model.SortOrder = id;
                PopulateAccounts(model);
                return View("Accounts", model);
            }
            catch (Exception e)
            {
                HandleException(e, "Sort", "");
                return View("Error");
            }
        }  //Sort

        private void SortAccounts(AccountsViewModel model)
        {
            try
            {
                if (model.SortOrder != null && model.Accounts != null)
                {
                    switch (model.SortOrder)
                    {
                        case "name":
                            model.Accounts = model.Accounts.OrderBy(x => x.Name).ToList();
                            break;
                        case "name_D":
                            model.Accounts = model.Accounts.OrderByDescending(x => x.Name).ToList();
                            break;
                        case "type":
                            model.Accounts = model.Accounts.OrderBy(x => x.Type).ToList();
                            break;
                        case "type_D":
                            model.Accounts = model.Accounts.OrderByDescending(x => x.Type).ToList();
                            break;
                        case "institution":
                            model.Accounts = model.Accounts.OrderBy(x => x.Institution).ToList();
                            break;
                        case "institution_D":
                            model.Accounts = model.Accounts.OrderByDescending(x => x.Institution).ToList();
                            break;
                        case "number":
                            model.Accounts = model.Accounts.OrderBy(x => x.Number).ToList();
                            break;
                        case "number_D":
                            model.Accounts = model.Accounts.OrderByDescending(x => x.Number).ToList();
                            break;
                        case "openbal":
                            model.Accounts = model.Accounts.OrderBy(x => x.OpeningBalance).ToList();
                            break;
                        case "openbal_D":
                            model.Accounts = model.Accounts.OrderByDescending(x => x.OpeningBalance).ToList();
                            break;
                        case "balance":
                            model.Accounts = model.Accounts.OrderBy(x => x.Balance).ToList();
                            break;
                        case "balance_D":
                            model.Accounts = model.Accounts.OrderByDescending(x => x.Balance).ToList();
                            break;
                        case "interest":
                            model.Accounts = model.Accounts.OrderBy(x => x.InterestRate).ToList();
                            break;
                        case "interest_D":
                            model.Accounts = model.Accounts.OrderByDescending(x => x.InterestRate).ToList();
                            break;
                        case "limit":
                            model.Accounts = model.Accounts.OrderBy(x => x.Limit).ToList();
                            break;
                        case "limit_D":
                            model.Accounts = model.Accounts.OrderByDescending(x => x.Limit).ToList();
                            break;
                        case "opened":
                            model.Accounts = model.Accounts.OrderBy(x => x.DateOpened).ToList();
                            break;
                        case "opened_D":
                            model.Accounts = model.Accounts.OrderByDescending(x => x.DateOpened).ToList();
                            break;
                        case "closed":
                            model.Accounts = model.Accounts.OrderBy(x => x.DateClosed).ToList();
                            break;
                        case "closed_D":
                            model.Accounts = model.Accounts.OrderByDescending(x => x.DateClosed).ToList();
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                HandleException(e, "SortAccounts", "");
            }
        }  //SortAccounts
        #endregion
        #region Populate
        private void PopulateAccounts(AccountsViewModel model)
        {
            model.Accounts = dbRepo.Accounts();
            model.NewAccount = new Account();
            SortAccounts(model);
        }  //PopulateAccounts
        #endregion
        #region Infrastructure
        private string SanitizeHTML(string input)
        {
            input = input.Replace("<", " less than ").Replace("<=", " less or equal to ").Replace(">", " greater than ").Replace(">=", " greater or equal to ").Replace("\\", " [backslash] ");
            return AntiXssEncoder.HtmlEncode(input, true);
        }

        private void HandleException(Exception e, string method, string userMessage)
        {
            Debug.WriteLine("An error occurred in AccountController/" + method + ".\n" + e.Message + ".\n" + userMessage);
            logger.Error("An error occurred in AccountController/" + method + ".\n" + e.Message + ".\n" + userMessage);
        }  //HandleException
        #endregion
    }  //class
}  //namespace