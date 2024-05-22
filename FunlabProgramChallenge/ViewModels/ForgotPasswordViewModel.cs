using System.ComponentModel.DataAnnotations;

namespace FunlabProgramChallenge.ViewModels
{
    public partial class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
