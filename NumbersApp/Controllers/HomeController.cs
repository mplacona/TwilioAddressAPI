using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;

namespace NumbersApp.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var accountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
            var authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            var twilio = new TwilioRestClient(accountSid, authToken);

            var addresses = twilio.ListAddresses();

            ViewBag.Addresses = addresses.Addresses;

            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        public ActionResult Detail(String Sid)
        {
            var accountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
            var authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            var twilio = new TwilioRestClient(accountSid, authToken);

            var address = twilio.GetAddress(Sid);
            
            ViewBag.address = address;

            return View();
        }

        public ActionResult Update(String Sid, String FriendlyName, String CustomerName, String Street, String City, String Region, String PostalCode)
        {
            var accountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
            var authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            var twilio = new TwilioRestClient(accountSid, authToken);

            var options = new AddressOptions { FriendlyName = FriendlyName, CustomerName = CustomerName,Street = Street, City = City, Region = Region, PostalCode = PostalCode};
            var address = twilio.UpdateAddress(Sid, options);

            ViewBag.address = address;
            return View(viewName: "~/Views/Home/Detail.cshtml");
        }


        public ActionResult Create(String FriendlyName, String CustomerName, String Street, String City, String Region, String PostalCode, String IsoCountry)
        {
            var accountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
            var authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            var twilio = new TwilioRestClient(accountSid, authToken);

            var address = twilio.AddAddress(FriendlyName, CustomerName, Street, City, Region, PostalCode, IsoCountry);
            
            ViewBag.address = address;
            return View(viewName: "~/Views/Home/Detail.cshtml");
        }
    }
}
