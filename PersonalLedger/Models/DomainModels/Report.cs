using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonalLedger.Repositories;
using System.ComponentModel.DataAnnotations;
using Unity;

namespace PersonalLedger.DomainModels
{
    public class Report
    {
        public int Id { get; set; }
        public List<string> Accounts { get; set; }
        public List<int> AccountIds { get; set; }
        public List<string> Categories { get; set; }
        public List<int> CategoryIds { get; set; }
        [DisplayFormat(NullDisplayText = "---")]
        public int? DayScope { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}", NullDisplayText = "---")]
        public decimal? MinAmount { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}", NullDisplayText = "---")]
        public decimal? MaxAmount { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}", NullDisplayText = "---")]
        public DateTime? MinDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}", NullDisplayText = "---")]
        public DateTime? MaxDate { get; set; }
        public string Name { get; set; }
        public bool TaxOnly { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }

        public Report() { }  //default constructor

        public Report(int id, List<int> accountIds, List<int> categoryIds, int? dayScope, decimal? maxAmount, DateTime? maxDate, decimal? minAmount, DateTime? minDate, string name, bool tax, int typeId)
        {
            using (IUnityContainer container = UnityConfig.Container)
            {
                ISessionRepository sessionRepo = container.Resolve<ISessionRepository>();
                Id = id;
                AccountIds = accountIds;
                if(accountIds != null)
                {
                    if(accountIds.Count > 0)
                    {
                        Accounts = accountIds.Select(x => sessionRepo.AccountName(x)).ToList();
                    }
                }
                CategoryIds = categoryIds;
                if (categoryIds != null)
                {
                    if (categoryIds.Count > 0)
                    {
                        Categories = categoryIds.Select(x => sessionRepo.CategoryName(x)).ToList();
                    }
                }
                DayScope = dayScope;
                MinAmount = minAmount;
                MaxAmount = maxAmount;
                MinDate = minDate;
                MaxDate = maxDate;
                Name = name;
                TaxOnly = tax;
                TypeId = typeId;
                Type = ApplicationRepository.ReportType(typeId);
            }  //using UnityContainer
        }  //constructor
    }  //class
}  //namespace