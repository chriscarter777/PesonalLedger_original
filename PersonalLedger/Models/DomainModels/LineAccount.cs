using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalLedger.DomainModels
{
    public class LineAccount
    {
        public int AcctId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}")]
        public decimal Change { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}")]
        public decimal Balance { get; set; }

        public LineAccount() { }  //default constructor

        public LineAccount(int acctId, decimal change, decimal balance)
        {
            AcctId = acctId;
            Change = change;
            Balance = balance;
        }  //constructor
    }  //class
}  //namespace