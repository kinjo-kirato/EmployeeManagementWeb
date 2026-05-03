using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementWeb.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "社員名を入力してください。")]
        [Display(Name = "社員名")]
        public string EmployeeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "部門を選択してください。")]
        [Display(Name = "部門")]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        [Display(Name = "登録者")]
        public string CreatedBy { get; set; } = string.Empty;

        [Display(Name = "更新者")]
        public string UpdatedBy { get; set; } = string.Empty;
    }
}