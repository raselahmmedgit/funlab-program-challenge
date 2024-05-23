namespace FunlabProgramChallenge.Utility
{
    public class StripePaymentGatewayConfig
    {
        public static string Name = "StripePaymentGatewayConfig";
        public string? EmailAddress { get; set; }
        public string? Password { get; set; }
        public string? PublishableKey { get; set; }
        public string? SecretKey { get; set; }
    }
}
