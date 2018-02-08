using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PersonalLedger.DomainModels;
using PersonalLedger.ViewModels;
using PersonalLedger.Repositories;
using System.Text;
using System.Diagnostics;
using NLog;

namespace PersonalLedger.Controllers
{
    public class TestController : Controller
    {
        #region Fields
        private readonly IDatabaseRepository dbRepo;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        public TestController(IDatabaseRepository dr)
        {
            if (dr == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                dbRepo = dr;
            }
        }  //ctor

        public void TestMethod()
        {
            //get the info from the db
            DateTime start1 = DateTime.Now;

            for(int i = 0; i < 100000; i++)
            {
                List<SelectListItem> categories1 = new List<SelectListItem>();
                List<Category> categories = dbRepo.Categories();
                foreach (Category c in categories)
                {
                    categories1.Add(new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
                }
            }

            DateTime end1 = DateTime.Now;
            Debug.WriteLine("Time required to create 100,000 select lists from Db");
            Debug.WriteLine(end1 - start1);

            //get the info from the session
            DateTime start2 = DateTime.Now;

            for (int i = 0; i < 100000; i++)
            {
                List<SelectListItem> categories2 = new List<SelectListItem>();
                categories2 = (List<SelectListItem>)Session["SL_Categories"];
            }

            DateTime end2 = DateTime.Now;
            Debug.WriteLine("Time required to create 100,000 select lists from Session");
            Debug.WriteLine(end2 - start2);

        }
    }
}