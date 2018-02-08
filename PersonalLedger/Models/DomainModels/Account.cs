 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonalLedger.Repositories;
using System.ComponentModel.DataAnnotations;

namespace PersonalLedger.DomainModels
{
    public class Account
    {
        public int Id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}")]
        public decimal Balance { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}", NullDisplayText = "---")]
        public DateTime? DateClosed { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateOpened { get; set; }
        public string Institution { get; set; }
        [DisplayFormat(DataFormatString = "{0}%", NullDisplayText = "---")]
        public decimal? InterestRate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}", NullDisplayText = "---")]
        public decimal? Limit { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}")]
        public decimal OpeningBalance { get; set; }
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
        public int TypeId { get; set; }
        public string Type { get; set; }

        public Account()
        {
            DateOpened = DateTime.Today;
        }  //default constructor

        public Account(int id, string name)
        {
            Id = id;
            Name = name;
        }  //constructor for AccountList use

        public Account(int id, DateTime? dateClosed, DateTime dateOpened, string institution, decimal? interest, decimal? limit, string name, string number, decimal openingBal, int typeId, List<Transaction> transactions)
        {
            Id = id;
            DateClosed = dateClosed;
            DateOpened = dateOpened;
            Institution = institution;
            InterestRate = interest;
            Limit = limit;
            Name = name;
            Number = number;
            OpeningBalance = openingBal;
            Type = ApplicationRepository.AccountType(typeId);
            TypeId = typeId;
            if(transactions != null)
            {
                if(transactions.Count > 0)
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
                    decimal bal = openingBal;
                    foreach(Transaction t in transactions.Where(x => x.FromAccountId == Id))
                    {
                        bal -= t.Amount;
                    }
                    foreach (Transaction t in transactions.Where(x => x.ToAccountId == Id))
                    {
                        bal += t.Amount;
                    }
                    Balance = bal;
                }
            }
        }  //constructor
    }  //class
}  //namespace