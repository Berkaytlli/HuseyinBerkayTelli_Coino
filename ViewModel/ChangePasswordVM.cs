using System.ComponentModel.DataAnnotations;

namespace ViewModel.Authentication
{
    public class ChangePasswordVM
    {
        [Required(ErrorMessage = "Lütfen eski şifrenizi giriniz.")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Lütfen yeni şifrenizi giriniz.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Şifreniz en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Lütfen yeni şifrenizi tekrar giriniz.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Girilen yeni şifreler uyuşmuyor.")]
        public string ConfirmPassword { get; set; }
    }
}
