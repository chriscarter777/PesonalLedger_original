using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonalLedger.DomainModels;

namespace PersonalLedger.ViewModels
{
    public class PayeesViewModel
    {
        public List<Payee> Payees { get; set; }
        public Account NewPayee { get; set; }
        public string SortOrder { get; set; }

        public PayeesViewModel()
        {
            Payees = new List<Payee>();
            NewPayee = new Account();
        }  //constructor
    }  //class
}  //namespace