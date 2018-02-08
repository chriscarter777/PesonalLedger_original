using PersonalLedger.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalLedger.DomainModels
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Tax { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }

        public Category() { }  //default constructor

        public Category(int id, string name, bool tax, int typeId)
        {
            Id = id;
            Name = name;
            Tax = tax;
            TypeId = typeId;
            Type = ApplicationRepository.CategoryType(typeId);
        }  //constructor
    }   //class
}  //namespace