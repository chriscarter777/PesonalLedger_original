using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonalLedger.DomainModels;
using System.Web.Mvc;
using PersonalLedger.Repositories;

namespace PersonalLedger.ViewModels
{
    public class AccountsViewModel
    {
        public List<Account> Accounts { get; set; }
        public Account NewAccount { get; set; }
        public string SortOrder { get; set; }

        public AccountsViewModel()
        {
            Accounts = new List<Account>();
            NewAccount = new Account();
        }  //constructor
    }  //class
}  //namespace