using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalLedger.DomainModels
{
    public class Payee
    {
        public int Id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}")]
        public decimal Balance { get; set; }
        public string Name { get; set; }
        public int TranCount { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}", NullDisplayText = "---")]
        public decimal? TranFirstAmount { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}", NullDisplayText = "---")]
        public DateTime? TranFirstDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}", NullDisplayText = "---")]
        public decimal? TranLastAmount { get; set; }
        [DisplayFormat(NullDisplayText = "---")]
        public string TranLastCategory { get; set; }
        [DisplayFormat(NullDisplayText = "---")]
        public int TranLastCategoryId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}", NullDisplayText = "---")]
        public DateTime? TranLastDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}", NullDisplayText = "---")]
        public decimal? TranMax { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}", NullDisplayText = "---")]
        public decimal? TranMean { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}", NullDisplayText = "---")]
        public decimal? TranMin { get; set; }

        public Payee() { }  //default constructor

        public Payee(int id, string name, List<Transaction> transactions)
        {
            Id = id;
            Name = name;
            if (transactions != null)
            {
                if (transactions.Count > 0)
                {
                    TranCount = transactions.Count;
                    TranFirstAmount = transactions.OrderBy(x => x.OnDate).First().Amount;
                    TranFirstDate = transactions.OrderBy(x => x.OnDate).First().OnDate;
                    TranLastAmount = transactions.OrderByDescending(x => x.OnDate).First().Amount;
                    TranLastCategory = transactions.OrderByDescending(x => x.OnDate).First().Category;
                    TranLastDate = transactions.OrderByDescending(x => x.OnDate).First().OnDate;
                    TranMax = transactions.Select(x => x.Amount).Max();
                    TranMean = transactions.Select(x => x.Amount).Average();
                    TranMin = transactions.Select(x => x.Amount).Min();
                    decimal bal = 0;
                    foreach (Transaction t in transactions.Where(x => x.PayeeId == Id && x.FromAccount != null))
                    {
                        bal += t.Amount;
                    }
                    foreach (Transaction t in transactions.Where(x => x.PayeeId == Id && x.ToAccount != null))
                    {
                        bal -= t.Amount;
                    }
                    Balance = bal;
                }
            }
        }  //constructor
    }  //class
}  //namespace