using System;
using System.Configuration;
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

            if (addresses.RestException != null)
            {
                return new HttpStatusCodeResult(500, addresses.RestException.Message);
            }
            ViewBag.Addresses = addresses.Addresses;
            
            return View();
        }

        public ActionResult AddAddress()
        {
            return View();
        }

        public ActionResult Details(string Sid)
        {
            var accountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
            var authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            var twilio = new TwilioRestClient(accountSid, authToken);

            var address = twilio.GetAddress(Sid);

            if (address.RestException != null)
            {
                return new HttpStatusCodeResult(500, address.RestException.Message);
            }
            
            ViewBag.address = address;

            return View();
        }

        public ActionResult Update(string Sid, string FriendlyName, string CustomerName, string Street, string City, string Region, string PostalCode)
        {
            var accountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
            var authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            var twilio = new TwilioRestClient(accountSid, authToken);

            var options = new AddressOptions { FriendlyName = FriendlyName, CustomerName = CustomerName,Street = Street, City = City, Region = Region, PostalCode = PostalCode};
            var address = twilio.UpdateAddress(Sid, options);

            if (address.RestException != null)
            {
                return new HttpStatusCodeResult(500, address.RestException.Message);
            }

            ViewBag.address = address;
            return View(viewName: "~/Views/Home/Details.cshtml");
        }


        public ActionResult Create(string FriendlyName, string CustomerName, string Street, string City, string Region, string PostalCode, string IsoCountry)
        {
            var accountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
            var authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            var twilio = new TwilioRestClient(accountSid, authToken);

            var address = twilio.AddAddress(FriendlyName, CustomerName, Street, City, Region, PostalCode, IsoCountry);

            if (address.RestException != null)
            {
                return new HttpStatusCodeResult(500, address.RestException.Message);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
