using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonalLedger.DomainModels;
using System.Web.Mvc;
using PersonalLedger.Repositories;

namespace PersonalLedger.ViewModels
{
    public class ReportsViewModel
    {
        public List<Account> Accounts { get; set; }
        public List<Category> Categories { get; set; }
        public Report EditReport { get; set; }
        public Report NewReport { get; set; }
        public List<Report> Reports { get; set; }
        public List<Transaction> Transactions { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public List<int> IncludeCategories { get; set; }
        public List<int> IncludeAccounts { get; set; }
        public bool TaxOnly { get; set; }
        public string SortOrder { get; set; }

        public ReportsViewModel()
        {
            Accounts = new List<Account>();
            Categories = new List<Category>();
            Reports = new List<Report>();
            EditReport = new Report();
            IncludeAccounts = new List<int>();
            IncludeAccounts = new List<int>();
            Transactions = new List<Transaction>();
        }  //constructor
    }  //class
}  //namespace