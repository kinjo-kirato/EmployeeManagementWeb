using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementWeb.Models
{
    public class ProfileViewModel
    {
        [Display(Name = "ユーザーID")]
        public string UserId { get; set; } = string.Empty;

        [Display(Name = "氏名")]
        public string UserName { get; set; } = string.Empty;

        [Display(Name = "権限")]
        public string Role { get; set; } = string.Empty;

        [Required(ErrorMessage = "現在のパスワードを入力してください。")]
        [DataType(DataType.Password)]
        [Display(Name = "現在のパスワード")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "新しいパスワードを入力してください。")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "パスワードは4文字以上で入力してください。")]
        [DataType(DataType.Password)]
        [Display(Name = "新しいパスワード")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "新しいパスワード（確認）を入力してください。")]
        [Compare("NewPassword", ErrorMessage = "新しいパスワードが一致しません。")]
        [DataType(DataType.Password)]
        [Display(Name = "新しいパスワード（確認）")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
