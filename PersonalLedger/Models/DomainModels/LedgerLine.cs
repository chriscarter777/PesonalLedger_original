using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalLedger.DomainModels
{
    public class LedgerLine
    {
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}")]
        public decimal Amount { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}")]
        public decimal Assets { get; set; }
        public int TransactionId { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public bool Flag0 { get; set; }
        public bool Flag1 { get; set; }
        public bool Flag2 { get; set; }
        public bool Flag3 { get; set; }
        public bool Flag4 { get; set; }
        public bool Flag5 { get; set; }
        public bool Flag6 { get; set; }
        public bool Flag7 { get; set; }
        public string FromAccount { get; set; }
        public int? FromAccountId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}")]
        public decimal Liabilities { get; set; }
        public List<LineAccount> LineAccounts { get; set; }
        public string Memo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}")]
        public decimal NetWorth { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime OnDate { get; set; }
        public string Payee { get; set; }
        public int? PayeeId { get; set; }
        public bool Reconciled { get; set; }
        public bool Tax { get; set; }
        public string ToAccount { get; set; }
        public int? ToAccountId { get; set; }

        public LedgerLine() { }  //default constructor

        public LedgerLine(Transaction tran, List<Account> accounts)
        {
            Amount = tran.Amount;
            TransactionId = tran.Id;
            Category = tran.Category;
            CategoryId = tran.CategoryId;
            Flag0 = tran.Flag0;
            Flag1 = tran.Flag1;
            Flag2 = tran.Flag2;
            Flag3 = tran.Flag3;
            Flag4 = tran.Flag4;
            Flag6 = tran.Flag6;
            Flag7 = tran.Flag7;
            FromAccount = tran.FromAccount;
            FromAccountId = tran.FromAccountId;
            Memo = tran.Memo;
            OnDate = tran.OnDate;
            Payee = tran.Payee;
            Reconciled = tran.Reconciled;
            Tax = tran.Tax;

            LineAccounts = new List<LineAccount>();
            decimal assets = 0;
            decimal liabilities = 0;

            foreach (Account acct in accounts)
            {
                LineAccount linacct = new LineAccount{ AcctId = acct.Id };
                if (tran.FromAccountId == linacct.AcctId)
                {
                    linacct.Change = tran.Amount * (-1);
                    linacct.Balance = acct.OpeningBalance - tran.Amount;
                }
                else if (tran.ToAccountId == linacct.AcctId)
                {
                    linacct.Change = tran.Amount;
                    linacct.Balance = acct.OpeningBalance + tran.Amount;
                }
                else
                {
                    linacct.Change = 0;
                    linacct.Balance = acct.OpeningBalance;
                }
                LineAccounts.Add(linacct);

                assets = assets + acct.OpeningBalance;
                liabilities = liabilities + 0;
            }
            Assets = assets;
            Liabilities = liabilities;
            NetWorth = assets - liabilities;
        }  //constructor for first line

        public LedgerLine(Transaction tran, LedgerLine priorLine)
        {
            Amount = tran.Amount;
            TransactionId = tran.Id;
            Category = tran.Category;
            CategoryId = tran.CategoryId;
            Flag0 = tran.Flag0;
            Flag1 = tran.Flag1;
            Flag2 = tran.Flag2;
            Flag3 = tran.Flag3;
            Flag4 = tran.Flag4;
            Flag6 = tran.Flag6;
            Flag7 = tran.Flag7;
            FromAccount = tran.FromAccount;
            FromAccountId = tran.FromAccountId;
            Memo = tran.Memo;
            OnDate = tran.OnDate;
            Payee = tran.Payee;
            PayeeId = tran.PayeeId;
            Reconciled = tran.Reconciled;
            Tax = tran.Tax;
            ToAccount = tran.ToAccount;
            ToAccountId = tran.ToAccountId;

            LineAccounts = new List<LineAccount>();
            decimal assets = 0;
            decimal liabilities = 0;

            foreach (LineAccount priorLineAcct in priorLine.LineAccounts)
            {
                LineAccount linacct = new LineAccount { AcctId = priorLineAcct.AcctId };
                if (tran.FromAccountId == linacct.AcctId)
                {
                    linacct.Change = tran.Amount * (-1);
                    linacct.Balance = priorLineAcct.Balance - tran.Amount;
                }
                else if (tran.ToAccountId == linacct.AcctId)
                {
                    linacct.Change = tran.Amount;
                    linacct.Balance = priorLineAcct.Balance + tran.Amount;
                }
                else
                {
                    linacct.Change = 0;
                    linacct.Balance = priorLineAcct.Balance;
                }
                LineAccounts.Add(linacct);

                assets = assets + linacct.Balance;
                liabilities = liabilities + 0;
            }
            Assets = assets;
            Liabilities = liabilities;
            NetWorth = assets - liabilities;
        }  //constructor for subsequent lines
    }  //class
}  //namespace