using System.ComponentModel.DataAnnotations;

namespace Microservice.Web.Models
{
    //kullanıcıdan veri alacağımız model
    public class SignInInput
    {
        [Required]
        [Display(Name = "Email adresiniz")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Şifreniz")]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool IsRemember { get; set; }
    }
}
