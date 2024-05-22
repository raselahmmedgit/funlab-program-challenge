using System.ComponentModel.DataAnnotations;

namespace FunlabProgramChallenge.ViewModels
{
    public partial class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string UserEmail { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string UserPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string UserConfirmPassword { get; set; }

        public string RoleName { get; set; }

        public string CardNumber { get; set; }

        public string CardExpiration { get; set; }

        public string CardCvc { get; set; }

        public string CardCountry { get; set; }

        public string ReturnUrl { get; set; }
    }
}
