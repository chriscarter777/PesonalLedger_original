using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PersonalLedger.DomainModels;
using System.Diagnostics;
using System.Web.Security.AntiXss;
using NLog;

namespace PersonalLedger.Controllers
{
    public class IdentityController : Controller, IIdentityController
    {
        #region Fields
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion
        #region Methods
        [AllowAnonymous]
        public void SetUserId()
        {
            int userId = 1;
            System.Web.HttpContext.Current.Session["UserId"] = userId as int?;
        }  //SetUserId

        #endregion
        #region Infrastructure
        private string SanitizeHTML(string input)
        {
            input = input.Replace("<", " less than ").Replace("<=", " less or equal to ").Replace(">", " greater than ").Replace(">=", " greater or equal to ").Replace("\\", " [backslash] ");
            return AntiXssEncoder.HtmlEncode(input, true);
        }

        private void HandleException(Exception e, string method, string userMessage)
        {
            Debug.WriteLine("An error occurred in IdentityController/" + method + ".\n" + e.Message + ".\n" + userMessage);
            logger.Error("An error occurred in IdentityController/" + method + ".\n" + e.Message + ".\n" + userMessage);
        }  //HandleException
        #endregion
    }  //class
}  //namespace