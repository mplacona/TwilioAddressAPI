using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;

namespace NumbersApp.Controllers
{
    public class NumbersController : Controller
    {
        //
        // GET: /Numbers/

        public ActionResult Index(String IsoCountry = "GB")
        {
            var accountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
            var authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            var twilio = new TwilioRestClient(accountSid, authToken);

            var addresses = twilio.ListAddresses().Addresses;
            // Return ISO codes only once
            var availableCountries = addresses.Select(m => m.IsoCountry).Distinct();

            var options = new AvailablePhoneNumberListRequest { VoiceEnabled = true };
            var numbers = twilio.ListAvailableLocalPhoneNumbers(IsoCountry, options);

            ViewBag.countries = availableCountries;
            ViewBag.numbers = numbers.AvailablePhoneNumbers;

            return View();
        }

        public ActionResult Buy(String PhoneNumber)
        {
            var accountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
            var authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            var twilio = new TwilioRestClient(accountSid, authToken);

            var purchaseOptions = new PhoneNumberOptions { PhoneNumber = PhoneNumber };

            twilio.AddIncomingPhoneNumber(purchaseOptions);

            return RedirectToAction("Index", "Numbers", new { number = PhoneNumber });
        }
    }
}
