using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementWeb.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "社員番号を入力してください。")]
        [Display(Name = "社員番号")]
        public string EmployeeNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "社員名を入力してください。")]
        [Display(Name = "社員名")]
        public string EmployeeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "メールアドレスを入力してください。")]
        [EmailAddress(ErrorMessage = "メールアドレスの形式が正しくありません。")]
        [Display(Name = "メールアドレス")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "入社日を入力してください。")]
        [DataType(DataType.Date)]
        [Display(Name = "入社日")]
        public DateTime HireDate { get; set; }

        [Required(ErrorMessage = "役職を入力してください。")]
        [Display(Name = "役職")]
        public string Position { get; set; } = string.Empty;

        [Required(ErrorMessage = "部門を選択してください。")]
        [Display(Name = "部門")]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        [Display(Name = "登録者")]
        public string CreatedBy { get; set; } = string.Empty;

        [Display(Name = "更新者")]
        public string UpdatedBy { get; set; } = string.Empty;

        [Display(Name = "登録日時")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "更新日時")]
        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}
