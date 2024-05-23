using FunlabProgramChallenge.Helpers;
using Stripe;

namespace FunlabProgramChallenge.Core
{
    public class StripePaymentGatewayResult
    {
        public bool Success { get; }

        public string? Message { get; }

        public string? MessageType { get; }

        public int ParentId { get; }

        public string? ParentName { get; }

        public int ResultCount { get; }

        public string? ResultId { get; }

        public string? RedirectUrl { get; set; }

        public string? PaymentStatus { get; set; }

        public string? ChargeId { get; set; }

        public StripePaymentGatewayResult()
        {
        }

        private StripePaymentGatewayResult(bool success, string message, string messageType)
        {
            Success = success;
            Message = message;
            MessageType = messageType;
        }

        private StripePaymentGatewayResult(bool success, string message, string messageType, string paymentStatus, string chargeId)
        {
            Success = success;
            Message = message;
            MessageType = messageType;
            PaymentStatus = paymentStatus;
            ChargeId = chargeId;
        }

        public static StripePaymentGatewayResult Fail()
        {
            return new StripePaymentGatewayResult(false, MessageHelper.Error, MessageHelper.MessageTypeDanger);
        }

        public static StripePaymentGatewayResult Fail(string message)
        {
            return new StripePaymentGatewayResult(false, message, MessageHelper.MessageTypeDanger);
        }

        public static StripePaymentGatewayResult Ok()
        {
            return new StripePaymentGatewayResult(true, MessageHelper.Success, MessageHelper.MessageTypeSuccess);
        }

        public static StripePaymentGatewayResult Ok(string message)
        {
            return new StripePaymentGatewayResult(true, message, MessageHelper.MessageTypeSuccess);
        }

        public static StripePaymentGatewayResult Ok(string message, string paymentStatus, string chargeId)
        {
            return new StripePaymentGatewayResult(true, message, MessageHelper.MessageTypeSuccess, paymentStatus, chargeId);
        }

    }
}
