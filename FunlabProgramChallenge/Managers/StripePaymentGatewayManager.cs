using FunlabProgramChallenge.Core;
using FunlabProgramChallenge.Helpers;
using FunlabProgramChallenge.Utility;
using FunlabProgramChallenge.ViewModels;
using Microsoft.Extensions.Options;
using Stripe;

namespace FunlabProgramChallenge.Managers
{
    public class StripePaymentGatewayManager : IStripePaymentGatewayManager
    {
        private readonly ILogger<StripePaymentGatewayManager> _iLogger;
        private readonly StripePaymentGatewayConfig _stripePaymentGatewayConfig;

        public StripePaymentGatewayManager(ILogger<StripePaymentGatewayManager> iLogger, IOptions<StripePaymentGatewayConfig> stripePaymentGatewayConfig)
        {
            _iLogger = iLogger;
            _stripePaymentGatewayConfig = stripePaymentGatewayConfig.Value;

            //EmailAddress: raselofficial102010@gmail.com
            //Password: RaB!@#123456
            //PublishableKey: pk_test_51PJXZPIttUFv1EMOxn9U0xwaymdlmmdRtwhDhYYiOyKV4xG6Et9oR5rRuuhHS5flsLXtdeuIRPyNHgCE774yQ9vS000LTVw0oC
            //SecretKey: sk_test_51PJXZPIttUFv1EMOJ6cCLoz7oSQmrhoDN4GG9xww5O1EZmabm0pWO8xnAMXvLAJI6tVApF7LbEoFl3kwfziQp3E200sheDV8sa
        }

        public async Task<StripePaymentGatewayResult> ProcessAsync(StripePaymentGatewayViewModel model)
        {
            var stripePaymentGatewayResult = new StripePaymentGatewayResult();

            try
            {
                var customers = new CustomerService();
                var charges = new ChargeService();

                var publishableKeyOptions = new RequestOptions
                {
                    ApiKey = _stripePaymentGatewayConfig.PublishableKey
                    //ApiKey = _stripePaymentGatewayConfig.SecretKey 
                };

                var secretKeyOptions = new RequestOptions
                {
                    //ApiKey = _stripePaymentGatewayConfig.PublishableKey
                    ApiKey = _stripePaymentGatewayConfig.SecretKey
                };

                #region Card payment checkout                              

                var tokenCreateOptions = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Number = model.CardNumber,
                        ExpMonth = model.CardExpirationMonth,
                        ExpYear = model.CardExpirationYear,
                        Cvc = model.CardCvc,
                        Name = model.CardName,
                        Currency = model.CardCurrencyCode
                    },
                };
                var tokenService = new TokenService();
                Token paymentToken = await tokenService.CreateAsync(tokenCreateOptions, publishableKeyOptions);

                _iLogger.LogInformation("StripePaymentGatewayManager - ProcessAsync - Create token: " + paymentToken.Id);

                #endregion

                #region Stripe Customer

                var paymentCustomer = new Customer();
                var customerEmail = model.CustomerEmailAddress;
                // Search customer in Stripe
                var stripeCustomer = await customers.ListAsync(new CustomerListOptions
                {
                    Email = customerEmail,
                    Limit = 1
                }, secretKeyOptions);

                if (stripeCustomer.Data.Count == 0)
                {
                    // create new customer
                    paymentCustomer = await customers.CreateAsync(new CustomerCreateOptions
                    {
                        Source = paymentToken.Id,
                        Phone = model.CustomerPhoneNumber,
                        Name = model.CustomerName,
                        Email = customerEmail,
                        Description = model.CustomerDescription,
                    }, secretKeyOptions);

                }
                else
                {
                    // use existing customer
                    paymentCustomer = stripeCustomer.FirstOrDefault();
                }

                _iLogger.LogInformation("StripePaymentGatewayManager - ProcessAsync - Create customer: " + paymentCustomer.Id);

                #endregion

                #region Stripe charges                             

                //var paymentCharge = await charges.CreateAsync(new ChargeCreateOptions
                //{
                //    Source = paymentToken.Id,//Customer = customer.Id,
                //    Amount = model.CardAmount,
                //    Currency = model.CardCurrencyCode?.ToLower(),
                //    ReceiptEmail = paymentCustomer?.Email,
                //    Description = model.CustomerDescription

                //}, secretKeyOptions);

                //_iLogger.LogInformation("StripePaymentGatewayManager - ProcessAsync - Create charge amount: " + paymentCharge.Id);

                //if (paymentCharge.Status.ToLower().Equals("succeeded"))
                //{
                //    stripePaymentGatewayResult = StripePaymentGatewayResult.Ok(MessageHelper.Payment, "Paid", paymentCharge.Id);
                //    return stripePaymentGatewayResult;
                //}
                //else
                //{
                //    string message = ("We are facing some problem while processing payment. (" + paymentCharge.FailureMessage + ")");
                //    stripePaymentGatewayResult = StripePaymentGatewayResult.Fail(message);
                //    return stripePaymentGatewayResult;
                //}

                #endregion

                stripePaymentGatewayResult = StripePaymentGatewayResult.Ok(MessageHelper.Payment);
                return stripePaymentGatewayResult;

            }
            catch (Exception ex)
            {
                _iLogger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "ProcessAsync"));
                string message = ("We are facing some problem while processing payment. (" + ex.Message + ")");
                stripePaymentGatewayResult = StripePaymentGatewayResult.Fail(message);
                return stripePaymentGatewayResult;
            }
        }

    }

    public interface IStripePaymentGatewayManager
    {
        Task<StripePaymentGatewayResult> ProcessAsync(StripePaymentGatewayViewModel model);
    }
}
