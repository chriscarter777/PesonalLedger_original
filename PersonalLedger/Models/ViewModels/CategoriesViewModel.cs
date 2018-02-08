using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonalLedger.DomainModels;

namespace PersonalLedger.ViewModels
{
    public class CategoriesViewModel
    {
        public List<Category> ExpenseCategories { get; set; }
        public List<Category> IncomeCategories { get; set; }
        public List<Category> OtherCategories { get; set; }
        public Category NewCategory { get; set; }
        public string SortOrder { get; set; }

        public CategoriesViewModel()
        {
            ExpenseCategories = new List<Category>();
            IncomeCategories = new List<Category>();
            OtherCategories = new List<Category>();
            NewCategory = new Category();
        }  //constructor
    }  //class
}  //namespace