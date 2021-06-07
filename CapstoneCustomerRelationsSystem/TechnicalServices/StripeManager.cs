using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CapstoneCustomerRelationsSystem.TechnicalServices
{
    public class StripeManager
    {
        public async Task<dynamic> PayAsync(string CardNumber, int Month, int Year, string CVC, int Value)
        {
            try
            {
                Configurations Config = new Configurations();
                StripeConfiguration.ApiKey = Config.GetConfiguration("Stripe", "SecretKey");

                var optionsToken = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Number = CardNumber,
                        ExpMonth = Month,
                        ExpYear = Year,
                        Cvc = CVC
                    }
                };

                var serviceToken = new TokenService();
                Token stripeToken = await serviceToken.CreateAsync(optionsToken);

                var chargeOptions = new ChargeCreateOptions
                {
                    Amount = Value,
                    Currency = "cad",
                    Description = "Capstone Comics and Games",
                    Source =  stripeToken.Id,
                    
                };

                var service = new ChargeService();
                Charge charge = await service.CreateAsync(chargeOptions);

                if (charge.Paid)
                {
                    return "Success";
                }
                else
                {
                    return "Failed";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }//End PayAsync

        public StripeList<TaxRate> GetTaxRates()
        {
            Configurations Config = new Configurations();
            StripeConfiguration.ApiKey = Config.GetConfiguration("Stripe", "SecretKey");
            var options = new TaxRateListOptions
            {
                Limit = 13
            };

            var service = new TaxRateService();
            StripeList<TaxRate> taxRates = service.List(options);

            return taxRates;
        }//End GetTaxRates
    }
}
