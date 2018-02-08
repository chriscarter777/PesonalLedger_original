using PersonalLedger.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Diagnostics;
using System.Web.Security.AntiXss;
using NLog;
using PersonalLedger.Controllers;

namespace PersonalLedger.Repositories
{
    public interface ISessionRepository
    {
        void SetAllVariables();
        void SetAccountVariables();
        void SetCategoryVariables();
        void SetPayeeVariables();
        void UpdatePayeeDefaults(int payeeId, decimal amount, int categoryId);

        int AccountId(string accountName);
        string AccountName(int accountId);
        int CategoryId(string categoryName);
        string CategoryName(int categoryId);
        int CategoryType(int categoryId);
        decimal PayeeAmount(int payeeId);
        int PayeeCategory(int payeeId);
        int PayeeId(string payeeName);
        string PayeeName(int payeeId);
        string Payees();
        List<SelectListItem> SL_Accounts();
        List<SelectListItem> SL_AddCategories();
        List<SelectListItem> SL_Categories();
        List<SelectListItem> SL_Payees();
    }  //interface
}  //namespace