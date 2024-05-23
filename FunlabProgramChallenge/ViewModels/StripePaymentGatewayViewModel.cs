using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FunlabProgramChallenge.ViewModels
{
    public class StripePaymentGatewayViewModel
    {
        public string? CustomerName { get; set; }
        
        public string? CustomerEmailAddress { get; set; }

        public string? CustomerPhoneNumber { get; set; }

        public string? CustomerDescription { get; set; }

        public string? CardName { get; set; }

        public string? CardNumber { get; set; }

        public string? CardExpirationYear { get; set; }

        public string? CardExpirationMonth { get; set; }

        public string? CardCvc { get; set; }

        public string? CardCountry { get; set; }

        public string? CardCurrencyCode { get; set; }

        public long? CardAmount { get; set; }
    }
}
