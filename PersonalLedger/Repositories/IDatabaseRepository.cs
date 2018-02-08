using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonalLedger.DomainModels;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web.Security.AntiXss;
using PersonalLedger.Controllers;
using NLog;

namespace PersonalLedger.Repositories
{
    public interface IDatabaseRepository
    {
        List<Account> Accounts();
        Account Account(int id);
        Dictionary<int, string> AccountDictionary();
        int AddAccount(Account a);
        void DeleteAccount(int acctId);
        void UpdateAccount(Account a);

        List<Category> Categories();
        int AddCategory(Category c);
        void AddCategories(List<Category> categories);
        void DeleteCategory(int id);
        void UpdateCategory(Category c);

        List<Payee> Payees();
        Dictionary<int, string> PayeeDictionary();
        int AddPayee(Payee p);
        int AddPayee(string name);
        void DeletePayee(int id);
        void UpdatePayee(Payee p);

        List<Report> Reports();
        Report Report(int id);
        int AddReport(Report r);
        void DeleteReport(int id);
        void UpdateReport(Report r);

        List<Transaction> Transactions();
        List<Transaction> Transactions(int? acctId, int? catId, int? payeeId, DateTime? fromDt, DateTime? toDt, decimal? fromAmt, decimal? toAmt);
        List<Transaction> TransactionsForAccount(int accountId);
        List<Transaction> TransactionsForPayee(int payeeId);
        Transaction Transaction(int tranId);
        int AddTransaction(Transaction t);
        void DeleteTransaction(int tranId);
        void UpdateTransaction(Transaction t);
    }  //interface
}  //namespace