using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonalLedger.DomainModels;
using System.Web.Mvc;
using PersonalLedger.Repositories;

namespace PersonalLedger.ViewModels
{
    public class TransactionsViewModel
    {
        public List<Account> Accounts { get; set; }
        public List<Category> Categories { get; set; }
        public List<LedgerLine> LedgerLines { get; set; }
        public List<Transaction> Transactions { get; set; }
        public Transaction EditTransaction { get; set; }
        public Transaction NewTransaction { get; set; }
        public string SortOrder { get; set; }

        public TransactionsViewModel()
        {
            Accounts = new List<Account>();
            Categories = new List<Category>();
            LedgerLines = new List<LedgerLine>();
            Transactions = new List<Transaction>();
            EditTransaction = new Transaction();
            NewTransaction = new Transaction { OnDate = DateTime.Today};
        }  //constructor
    }  //class
}  //namespace