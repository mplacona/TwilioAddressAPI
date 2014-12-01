using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Twilio;

namespace NumbersApp.Controllers
{
    public class NumbersController : Controller
    {
        //
        // GET: /Numbers/

        public ActionResult Index(String IsoCountry)
        {
            var accountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
            var authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            var twilio = new TwilioRestClient(accountSid, authToken);

            var addresses = twilio.ListAddresses();

            if (addresses.RestException != null)
            {
                return new HttpStatusCodeResult(500, addresses.RestException.Message);
            }

            // Return ISO codes only once
            var availableCountries = addresses.Addresses.Select(m => m.IsoCountry).Distinct();

            if (IsoCountry != null)
            {
                var options = new AvailablePhoneNumberListRequest { VoiceEnabled = true };
                var numbers = twilio.ListAvailableLocalPhoneNumbers(IsoCountry, options);

                if (numbers.RestException != null)
                {
                    return new HttpStatusCodeResult(500, numbers.RestException.Message);
                }

                ViewBag.numbers = numbers.AvailablePhoneNumbers;
            }

            ViewBag.countries = availableCountries;

            return View();
        }

        public ActionResult Buy(string PhoneNumber)
        {
            var accountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
            var authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            var twilio = new TwilioRestClient(accountSid, authToken);

            var purchaseOptions = new PhoneNumberOptions { PhoneNumber = PhoneNumber };

            var purchase = twilio.AddIncomingPhoneNumber(purchaseOptions);

            if (purchase.RestException != null)
            {
                return new HttpStatusCodeResult(500, purchase.RestException.Message);
            }

            return RedirectToAction("Index", "Numbers", new { number = PhoneNumber });
        }
    }
}
