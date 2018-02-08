using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonalLedger.DomainModels;

namespace PersonalLedger.ViewModels
{
    public class AdminViewModel
    {
        public List<Account> Accounts { get; set; }
        public List<Category> ExpenseCategories { get; set; }
        public List<Category> IncomeCategories { get; set; }
        public List<Category> OtherCategories { get; set; }
        public List<Payee> Payees { get; set; }
        public List<Report> Reports { get; set; }
        public List<Transaction> Transactions { get; set; }

        public AdminViewModel()
        {
            Accounts = new List<Account>();
            ExpenseCategories = new List<Category>();
            IncomeCategories = new List<Category>();
            OtherCategories = new List<Category>();
            Payees = new List<Payee>();
            Reports = new List<Report>();
            Transactions = new List<Transaction>();
        }  //constructor
    }  //class
}  //namespace