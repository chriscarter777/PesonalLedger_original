using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonalLedger.DomainModels;
using System.Web.Mvc;
using System.Diagnostics;
using PersonalLedger.Controllers;
using System.Text;
using System.Web.Security.AntiXss;
using NLog;

namespace PersonalLedger.Repositories
{
    public static class ApplicationRepository
    {
        #region Fields
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Dictionary<int, string> accountTypes = new Dictionary<int, string>
        {
            { 1, "Asset"},
            { 2, "Cash"},
            { 3, "Checking"},
            { 4, "Credit"},
            { 5, "Equity"},
            { 6, "Liability"},
            { 7, "Savings"}
        };

        private static Dictionary<string, int> accountTypeIds = new Dictionary<string, int>
        {
            { "Asset", 1},
            { "Cash", 2},
            { "Checking", 3},
            { "Credit", 4},
            { "Equity", 5},
            { "Liability", 6},
            { "Savings", 7}
        };

        private static Dictionary<int, string> categoryTypes = new Dictionary<int, string>
        {
            { 1, "Adjustment"},
            { 2, "Transfer"},
            { 3, "Income"},
            { 4, "Expense"}
        };

        private static Dictionary<string, int> categoryTypeIds = new Dictionary<string, int>
        {
            { "Adjustment", 1},
            { "Transfer", 2},
            { "Income", 3},
            { "Expense", 4}
        };


        private static Dictionary<int, string> reportTypes = new Dictionary<int, string>
        {
            { 1, "Bar"},
            { 2, "Line"},
            { 3, "List"},
            { 4, "Pie"},
            { 5, "Surface"}
        };

        private static Dictionary<string, int> reportTypeIds = new Dictionary<string, int>
        {
            { "Bar", 1},
            { "Line", 2},
            { "List", 3},
            { "Pie", 4},
            { "Surface", 5}
        };
        #endregion
        #region Properties
        public static readonly List<SelectListItem> AccountTypeList;
        public static readonly List<SelectListItem> ReportTypeList;
        #endregion

        static ApplicationRepository()
        {
            AccountTypeList = accountTypes.Select(x => new SelectListItem { Text = x.Value, Value = x.Key.ToString() }).ToList();
            ReportTypeList = reportTypes.Select(x => new SelectListItem { Text = x.Value, Value = x.Key.ToString() }).ToList();
        }  //ctor

        #region Methods
        public static string AccountType(int accountTypeId)
        {
            try
            {
                string aType = "";
                if (accountTypes.Keys.Contains(accountTypeId))
                {
                    aType = accountTypes[accountTypeId];
                }
                return aType;
            }
            catch (Exception e)
            {
                HandleException(e, "AccountType", "");
                return "";
            }
        }  //AccountType

        public static int AccountTypeId(string accountType)
        {
            try
            {
                int atId = 0;
                if (accountTypeIds.Keys.Contains(accountType))
                {
                    atId = accountTypeIds[accountType];
                }
                return atId;
            }
            catch (Exception e)
            {
                HandleException(e, "AccountTypeId", "");
                return 0;
            }
        }  //AccountTypeId

        public static string CategoryType(int categoryTypeId)
        {
            try
            {
                string cType = "";
                if (categoryTypes.Keys.Contains(categoryTypeId))
                {
                    cType = categoryTypes[categoryTypeId];
                }
                return cType;
            }
            catch (Exception e)
            {
                HandleException(e, "CategoryType", "");
                return "";
            }
        }  //CategoryType

        public static int CategoryTypeId(string categoryType)
        {
            try
            {
                int cId = 0;
                if (categoryTypeIds.Keys.Contains(categoryType))
                {
                    cId = categoryTypeIds[categoryType];
                }
                return cId;
            }
            catch (Exception e)
            {
                HandleException(e, "CategoryTypeId", "");
                return 0;
            }
        }  //CategoryTypeId

        public static string ReportType(int reportTypeId)
        {
            try
            {
                string rType = "";
                if (reportTypes.Keys.Contains(reportTypeId))
                {
                    rType = reportTypes[reportTypeId];
                }
                return rType;
            }
            catch (Exception e)
            {
                HandleException(e, "ReportType", "");
                return "";
            }
        }  //ReportType

        public static int ReportTypeId(string reportType)
        {
            try
            {
                int rId = 0;
                if (reportTypeIds.Keys.Contains(reportType))
                {
                    rId = reportTypeIds[reportType];
                }
                return rId;
            }
            catch (Exception e)
            {
                HandleException(e, "ReportTypeId", "");
                return 0;
            }
        }  //ReportTypeId
        #endregion
        #region Infrastructure
        private static string SanitizeHTML(string input)
        {
            input = input.Replace("<", " less than ").Replace("<=", " less or equal to ").Replace(">", " greater than ").Replace(">=", " greater or equal to ").Replace("\\", " [backslash] ");
            return AntiXssEncoder.HtmlEncode(input, true);
        }

        private static void HandleException(Exception e, string method, string userMessage)
        {
            Debug.WriteLine("An error occurred in ApplicationRepository/" + method + ".\n" + e.Message + ".\n" + userMessage);
            logger.Error("An error occurred in ApplicationRepository/" + method + ".\n" + e.Message + ".\n" + userMessage);
        }  //HandleException
        #endregion
    }  //class
}  //namespace