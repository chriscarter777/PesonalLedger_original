using PersonalLedger.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalLedger.DomainModels
{
    public class CategoryCollection
    {
        public List<Category> ExpenseCategories { get; set; }
        public List<Category> IncomeCategories { get; set; }
        public List<Category> OtherCategories { get; set; }
    }
}