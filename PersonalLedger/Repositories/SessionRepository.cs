using PersonalLedger.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Diagnostics;
using System.Web.Security.AntiXss;
using NLog;
using PersonalLedger.Controllers;

namespace PersonalLedger.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        #region Fields
        private readonly IDatabaseRepository dbRepo;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Dictionary<string, int> accountId;
        private Dictionary<int, string> accountName;
        private Dictionary<string, int> categoryId;
        private Dictionary<int, string> categoryName;
        private Dictionary<int, int> categoryTypeLookup;
        private static readonly List<Category> defaultCategories = new List<Category>{
            //ADJUSTMENT(OTHER)
            new Category
            {
                Name = "Adjustment",
                Tax = false,
                TypeId= 1
            },
            //TRANSFER(OTHER)
            new Category
            {
                Name = "Transfer",
                Tax = false,
                TypeId= 2
            },
            //INCOME
            new Category
            {
                Name = "Salary",
                Tax = true,
                TypeId= 3
            },
            new Category
            {
                Name = "Bonus",
                Tax = true,
                TypeId= 3
            },
            new Category
            {
                Name = "Commission",
                Tax = true,
                TypeId= 3
            },
            new Category
            {
                Name = "Ordinary Dividend",
                Tax = true,
                TypeId= 3
            },
            new Category
            {
                Name = "Qualified Dividend",
                Tax = true,
                TypeId= 3
            },
            new Category
            {
                Name = "Interest Income",
                Tax = true,
                TypeId= 3
            },
            new Category
            {
                Name = "Gift Income",
                Tax = true,
                TypeId= 3
            },
            new Category
            {
                Name = "Other Income",
                Tax = true,
                TypeId= 3
            },
            //EXPENSE
            new Category
            {
                Name = "Automobile",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Business",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Children",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Clothing",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Eating Out",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Education",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Electricity",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Entertainment",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Fee",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Garbage",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Gasoline",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Gift Expense",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Grocery and Houseware",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Health and Grooming",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Home Maintenance",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Insurance",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Interest Expense",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Internet",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Legal",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Medical",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Mortgage Principal",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Mortgage Interest",
                Tax = true,
                TypeId= 4
            },
            new Category
            {
                Name = "Natural Gas",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Other Expense",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Parking",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Pet",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Phone",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Rent",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Tax",
                Tax = true,
                TypeId= 4
            },
            new Category
            {
                Name = "Television",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Travel",
                Tax = false,
                TypeId= 4
            },
            new Category
            {
                Name = "Water and Sewer",
                Tax = false,
                TypeId= 4
            }
        };
        private Dictionary<int, decimal> payeeAmount;
        private Dictionary<int, int> payeeCategoryId;
        private Dictionary<string, int> payeeId;
        private Dictionary<int, string> payeeName;
        private string payees;
        private List<SelectListItem> sL_Accounts;
        private List<SelectListItem> sL_AddCategories;
        private List<SelectListItem> sL_Categories;
        private List<SelectListItem> sL_Payees;
        #endregion
        public SessionRepository(IDatabaseRepository dr)
        {
            try
            {
                dbRepo = dr;
            }
            catch (Exception e)
            {
                HandleException(e, "constructor", "");
            }
        }  //constructor
        #region Set Session Variables
        public void SetAllVariables()
        {
            try
            {
                SetAccountVariables();
                SetCategoryVariables();
                SetPayeeVariables();
            }
            catch (Exception e)
            {
                HandleException(e, "SetSessionVariables", "");
            }
        }  //SetSessionVariables

        public void SetAccountVariables()
        {
            try
            {
                Dictionary<int, string> accounts = dbRepo.AccountDictionary();
                accountId = new Dictionary<string, int>();
                accountName = new Dictionary<int, string>();
                sL_Accounts = new List<SelectListItem>();
                foreach (KeyValuePair<int, string> a in accounts)
                {
                    accountId.Add(a.Value, a.Key);
                    accountName.Add(a.Key, a.Value);
                    sL_Accounts.Add(new SelectListItem { Value = a.Key.ToString(), Text = a.Value });
                }

                HttpContext.Current.Session["AccountId"] = accountId as Dictionary<string, int>;
                HttpContext.Current.Session["AccountName"] = accountName as Dictionary<int, string>;
                HttpContext.Current.Session["SL_Accounts"] = sL_Accounts as List<SelectListItem>;
            }
            catch (Exception e)
            {
                HandleException(e, "SetAccountVariables", "");
            }
        }  //SetAccountVariables

        public void SetCategoryVariables()
        {
            try
            {
                List<Category> categories = dbRepo.Categories();
                //if the db contains no categories (first-use) then populate it with the default categories.
                if (categories.Count == 0)
                {
                    dbRepo.AddCategories(defaultCategories);
                    categories = dbRepo.Categories();
                }
                categoryId = new Dictionary<string, int>();
                categoryName = new Dictionary<int, string>();
                categoryTypeLookup = new Dictionary<int, int>();
                sL_AddCategories = new List<SelectListItem>();
                sL_Categories = new List<SelectListItem>();
                foreach (Category c in categories)
                {
                    categoryId.Add(c.Name, c.Id);
                    categoryName.Add(c.Id, c.Name);
                    categoryTypeLookup.Add(c.Id, c.TypeId);
                    sL_Categories.Add(new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
                }
                //the select list does not include Adjustment and Transfer (these are buttons) 
                foreach (Category c in categories.Where(x => x.TypeId != 1 || x.TypeId != 2))
                {
                    sL_AddCategories.Add(new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
                }

                HttpContext.Current.Session["CategoryId"] = categoryId as Dictionary<string, int>;
                HttpContext.Current.Session["CategoryName"] = categoryName as Dictionary<int, string>;
                HttpContext.Current.Session["D_CategoryType"] = categoryTypeLookup as Dictionary<int, int>;
                HttpContext.Current.Session["SL_AddCategories"] = sL_AddCategories as List<SelectListItem>;
                HttpContext.Current.Session["SL_Categories"] = sL_Categories as List<SelectListItem>;
            }
            catch (Exception e)
            {
                HandleException(e, "SetCategoryVariables", "");
            }
        }  //SetCategoryVariables

        public void SetPayeeVariables()
        {
            try
            {
                //Part One is a prerequisite for instantiating Transactions, which is performed by dbRepo.Payees in Part Two
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                payeeId = new Dictionary<string, int>();
                payeeName = new Dictionary<int, string>();
                sL_Payees = new List<SelectListItem>();
                Dictionary<int, string> payeeDictionary = dbRepo.PayeeDictionary();
                if (payeeDictionary != null)
                {
                    foreach (KeyValuePair<int, string> p in payeeDictionary)
                    {
                        payeeId.Add(p.Value, p.Key);
                        payeeName.Add(p.Key, p.Value);
                        sL_Payees.Add(new SelectListItem { Value = p.Key.ToString(), Text = p.Value });
                        sb.Append("'" + p.Value + "', ");
                    }
                    sb.Remove(sb.Length - 2, 2);
                }
                sb.Append("]");
                payees = sb.ToString();
                HttpContext.Current.Session["PayeeId"] = payeeId as Dictionary<string, int>;
                HttpContext.Current.Session["PayeeName"] = payeeName as Dictionary<int, string>;
                HttpContext.Current.Session["Payees"] = payees as string;
                HttpContext.Current.Session["SL_Payees"] = sL_Payees as List<SelectListItem>;

                //Part Two
                payeeAmount = new Dictionary<int, decimal>();
                payeeCategoryId = new Dictionary<int, int>();
                List<Payee> dbPayees = dbRepo.Payees();
                if (payees != null)
                {
                    foreach (Payee p in dbPayees)
                    {
                        payeeAmount.Add(p.Id, p.TranLastAmount ?? 0);
                        payeeCategoryId.Add(p.Id, p.TranLastCategoryId);
                    }
                }
                HttpContext.Current.Session["PayeeAmount"] = payeeAmount as Dictionary<int, decimal>;
                HttpContext.Current.Session["PayeeCategory"] = payeeCategoryId as Dictionary<int, int>;
            }
            catch (Exception e)
            {
                HandleException(e, "SetPayeeVariables", "");
            }
        }  //SetPayeeVariables

        public void UpdatePayeeDefaults(int payeeId, decimal amount, int categoryId)
        {
            try
            {
                payeeCategoryId[payeeId] = categoryId;
                payeeAmount[payeeId] = amount;
            }
            catch (Exception e)
            {
                HandleException(e, "UpdatePayeeDefaults", "");
            }
        }  //UpdatePayee
        #endregion
        #region Retrieve Session Variables           
        public int AccountId(string accountName)
        {
            try
            {
                int aId = 0;
                if (!String.IsNullOrEmpty(accountName))
                {
                    Dictionary<string, int> accountId = (Dictionary<string, int>)HttpContext.Current.Session["AccountId"];
                    if (accountId.Keys.Contains(accountName))
                    {
                        aId = accountId[accountName];
                    }
                }
                return aId;
            }
            catch (Exception e)
            {
                HandleException(e, "AccountId", "");
                return 0;
            }
        }  //AccountId

        public string AccountName(int accountId)
        {
            try
            {
                string aName = "";
                Dictionary <int, string> accountName = (Dictionary<int, string>)HttpContext.Current.Session["AccountName"];
                if (accountName.Keys.Contains(accountId))
                {
                    aName = accountName[accountId];
                }
                return aName;
            }
            catch (Exception e)
            {
                HandleException(e, "AccountName", "");
                return "";
            }
        }  //AccountName

        public int CategoryId(string categoryName)
        {
            try
            {
                int cId = 0;
                if (!String.IsNullOrEmpty(categoryName))
                {
                    Dictionary<string, int> categoryId = (Dictionary<string, int>)HttpContext.Current.Session["CategoryId"];
                    if (categoryId.Keys.Contains(categoryName))
                    {
                        cId = categoryId[categoryName];
                    }
                }
                return cId;
            }
            catch (Exception e)
            {
                HandleException(e, "CategoryId", "");
                return 0;
            }
        }  //CategoryId

        public string CategoryName(int categoryId)
        {
            try
            {
                string cName = "";
                Dictionary<int, string> categoryName = (Dictionary<int, string>)HttpContext.Current.Session["CategoryName"];
                if (categoryName.Keys.Contains(categoryId))
                {
                    cName = categoryName[categoryId];
                }
                return cName;
            }
            catch (Exception e)
            {
                HandleException(e, "CategoryName", "");
                return "";
            }
        }  //CategoryName

        public int CategoryType(int categoryId)
        {
            try
            {
                int ctId = 0;
                Dictionary<int, int> categoryType = (Dictionary < int, int > )HttpContext.Current.Session["D_CategoryType"];
                if (categoryType.Keys.Contains(categoryId))
                {
                    ctId = categoryType[categoryId];
                }
                return ctId;
            }
            catch (Exception e)
            {
                HandleException(e, "CategoryType", "");
                return 0;
            }
        }  //CategoryType

        public decimal PayeeAmount(int payeeId)
        {
            try
            {
                decimal pa = 0;
                Dictionary<int, decimal> payeeAmount = (Dictionary<int, decimal>)HttpContext.Current.Session["PayeeAmount"];
                if (payeeAmount.Keys.Contains(payeeId))
                {
                    pa = payeeAmount[payeeId];
                }
                return pa;
            }
            catch (Exception e)
            {
                HandleException(e, "PayeeAmount", "");
                return 0;
            }
        }  //PayeeAmount

        public int PayeeCategory(int payeeId)
        {
            try
            {
                int pcId = 0;
                Dictionary<int, int> payeeCategory = (Dictionary<int, int>)HttpContext.Current.Session["PayeeCategory"];
                if (payeeCategory.Keys.Contains(payeeId))
                {
                    pcId = payeeCategory[payeeId];
                }
                return pcId;
            }
            catch (Exception e)
            {
                HandleException(e, "PayeeCategory", "");
                return 0;
            }
        }  //PayeeCategory

        public int PayeeId(string payeeName)
        {
            try
            {
                int pId = 0;
                if (!String.IsNullOrEmpty(payeeName))
                {
                    Dictionary<string, int> payeeId = (Dictionary<string, int>)HttpContext.Current.Session["PayeeId"];
                    if (payeeId.Keys.Contains(payeeName))
                    {
                        pId = payeeId[payeeName];
                    }
                }
                return pId;
            }
            catch (Exception e)
            {
                HandleException(e, "PayeeId", "");
                return 0;
            }
        }  //PayeeId

        public string PayeeName(int payeeId)
        {
            try
            {
                string pName = "";
                Dictionary<int, string> payeeName = (Dictionary<int, string>)HttpContext.Current.Session["PayeeName"];
                if (payeeName.Keys.Contains(payeeId))
                {
                    pName = payeeName[payeeId];
                }
                return pName;
            }
            catch (Exception e)
            {
                HandleException(e, "PayeeName", "");
                return "";
            }
        }  //PayeeName

        public string Payees()
        {
            try
            {
                string p = "";
                string payees = (string)HttpContext.Current.Session["Payees"];
                if (payees != null)
                {
                    p = payees;
                }
                return p;
            }
            catch (Exception e)
            {
                HandleException(e, "Payees", "");
                return "";
            }
        }  //Payees

        public List<SelectListItem> SL_Accounts()
        {
            try
            {
                List<SelectListItem> sL_Accounts = (List<SelectListItem>)HttpContext.Current.Session["SL_Accounts"];
                if (sL_Accounts != null)
                {
                    return sL_Accounts;
                }
                else
                {
                    return new List<SelectListItem>();
                }
            }
            catch (Exception e)
            {
                HandleException(e, "SL_Accounts", "");
                return new List<SelectListItem>();
            }
        }  //SL_Accounts

        public List<SelectListItem> SL_AddCategories()
        {
            try
            {
                List<SelectListItem> sL_AddCategories = (List<SelectListItem>)HttpContext.Current.Session["SL_AddCategories"];
                if (sL_AddCategories != null)
                {
                    return sL_AddCategories;
                }
                else
                {
                    return new List<SelectListItem>();
                }
            }
            catch (Exception e)
            {
                HandleException(e, "SL_AddCategories", "");
                return new List<SelectListItem>();
            }
        }  //SL_AddCategories

        public List<SelectListItem> SL_Categories()
        {
            try
            {
                List<SelectListItem> sL_Categories = (List<SelectListItem>)HttpContext.Current.Session["SL_Categories"];
                if (sL_Categories != null)
                {
                    return sL_Categories;
                }
                else
                {
                    return new List<SelectListItem>();
                }
            }
            catch (Exception e)
            {
                HandleException(e, "SL_Categories", "");
                return new List<SelectListItem>();
            }
        }  //SL_Categories

        public List<SelectListItem> SL_Payees()
        {
            try
            {
                List<SelectListItem> sL_Payees = (List<SelectListItem>)HttpContext.Current.Session["SL_Payees"];
                if (sL_Payees != null)
                {
                    return sL_Payees;
                }
                else
                {
                    return new List<SelectListItem>();
                }
            }
            catch (Exception e)
            {
                HandleException(e, "SL_Payees", "");
                return new List<SelectListItem>();
            }
        }  //SL_Payees
        #endregion
        #region Infrastructure
        private string SanitizeHTML(string input)
        {
            input = input.Replace("<", " less than ").Replace("<=", " less or equal to ").Replace(">", " greater than ").Replace(">=", " greater or equal to ").Replace("\\", " [backslash] ");
            return AntiXssEncoder.HtmlEncode(input, true);
        }

        private void HandleException(Exception e, string method, string userMessage)
        {
            Debug.WriteLine("An error occurred in SessionRepository." + method + ".\n" + e.Message + ".\n" + userMessage);
            logger.Error("An error occurred in SessionRepository." + method + ".\n" + e.Message + ".\n" + userMessage);
        }  //HandleException
        #endregion
    }  //class
}  //namespace