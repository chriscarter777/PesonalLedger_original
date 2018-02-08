using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonalLedger.Repositories;
using System.ComponentModel.DataAnnotations;
using Unity;

namespace PersonalLedger.DomainModels
{
    public class Transaction
    {
        public int Id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}")]
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public int? CheckNo { get; set; }
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
        public string Memo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime OnDate { get; set; }
        public string Payee { get; set; }
        public int? PayeeId { get; set; }
        public bool Reconciled { get; set; }
        public bool Tax { get; set; }
        public string ToAccount { get; set; }
        public int? ToAccountId { get; set; }

        public Transaction()
        {
            using(IUnityContainer container = UnityConfig.Container)
            {
                ISessionRepository sessionRepo = container.Resolve<ISessionRepository>();
                CategoryId = sessionRepo.CategoryId(Category);
            }
            Category = "Adjustment";
            OnDate = DateTime.Today;
        }  //default constructor

        public Transaction(int id, decimal amount, int categoryId, int? checkNo, string flagString, int? fromAccountId, string memo, DateTime onDate, int? payeeId, bool reconciled, bool tax, int? toAccountId)
        {
            using (IUnityContainer container = UnityConfig.Container)
            {
                ISessionRepository sessionRepo = container.Resolve<ISessionRepository>();
                Id = id;
                Amount = amount;
                CategoryId = categoryId;
                Category = sessionRepo.CategoryName(categoryId);
                CheckNo = checkNo;
                if (String.IsNullOrEmpty(flagString))
                {
                    Flag0 = false;
                    Flag1 = false;
                    Flag2 = false;
                    Flag3 = false;
                    Flag4 = false;
                    Flag5 = false;
                    Flag6 = false;
                    Flag7 = false;
                }
                else
                {
                    Flag0 = (flagString[0] == '1');
                    Flag1 = (flagString[1] == '1');
                    Flag2 = (flagString[2] == '1');
                    Flag3 = (flagString[3] == '1');
                    Flag4 = (flagString[4] == '1');
                    Flag5 = (flagString[5] == '1');
                    Flag6 = (flagString[6] == '1');
                    Flag7 = (flagString[7] == '1');
                }
                FromAccountId = fromAccountId;
                if (fromAccountId != null)
                {
                    FromAccount = sessionRepo.AccountName((int)fromAccountId);
                }
                if (memo == null)
                {
                    Memo = String.Empty;
                }
                else
                {
                    Memo = memo;
                }
                OnDate = onDate;
                if (payeeId != null)
                {
                    Payee = sessionRepo.PayeeName((int)payeeId);
                    PayeeId = payeeId;
                }
                Reconciled = reconciled;
                Tax = tax;
                ToAccountId = toAccountId;
                if (toAccountId != null)
                {
                    ToAccount = sessionRepo.AccountName((int)toAccountId);
                }
            }  //using UnityContainer
        }  //constructor
    }  //class
}  //namespace