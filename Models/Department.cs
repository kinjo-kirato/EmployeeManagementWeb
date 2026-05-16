using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementWeb.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "部門名を入力してください。")]
        [Display(Name = "部門名")]
        public string DepartmentName { get; set; } = string.Empty;

        [Display(Name = "登録者")]
        public string CreatedBy { get; set; } = string.Empty;

        [Display(Name = "更新者")]
        public string UpdatedBy { get; set; } = string.Empty;

        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}