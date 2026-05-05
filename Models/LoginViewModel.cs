using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementWeb.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "ユーザーIDを入力してください。")]
        [Display(Name = "ユーザーID")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "パスワードを入力してください。")]
        [Display(Name = "パスワード")]
        public string Password { get; set; } = string.Empty;
    }
}