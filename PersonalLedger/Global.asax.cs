using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PersonalLedger.Repositories;
using PersonalLedger.Controllers;
using Unity;

namespace PersonalLedger
{
    public class MvcApplication : System.Web.HttpApplication
    {
         protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }  //Application_Start

        protected void Session_Start()
        {
            IIdentityController identCtrl = UnityConfig.Container.Resolve<IIdentityController>();
            identCtrl.SetUserId();
            ISessionRepository sessRepo = UnityConfig.Container.Resolve<ISessionRepository>();
            sessRepo.SetAllVariables();
        }  //Session_Start
    }  //class
}  //namespace
