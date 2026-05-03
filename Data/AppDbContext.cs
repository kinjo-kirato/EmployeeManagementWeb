using EmployeeManagementWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        public void SeedData()
        {
            if (!Users.Any())
            {
                Users.AddRange(
                    new User
                    {
                        UserId = "admin",
                        Password = "1234",
                        UserName = "管理者"
                    },
                    new User
                    {
                        UserId = "user01",
                        Password = "1111",
                        UserName = "山田太郎"
                    }
                );
            }

            if (!Departments.Any())
            {
                Departments.AddRange(
                    new Department
                    {
                        DepartmentName = "営業部",
                        CreatedBy = "初期データ",
                        UpdatedBy = "初期データ"
                    },
                    new Department
                    {
                        DepartmentName = "開発部",
                        CreatedBy = "初期データ",
                        UpdatedBy = "初期データ"
                    },
                    new Department
                    {
                        DepartmentName = "総務部",
                        CreatedBy = "初期データ",
                        UpdatedBy = "初期データ"
                    }
                );
            }

            SaveChanges();

            if (!Employees.Any())
            {
                var sales = Departments.FirstOrDefault(d => d.DepartmentName == "営業部");
                var dev = Departments.FirstOrDefault(d => d.DepartmentName == "開発部");

                Employees.AddRange(
                    new Employee
                    {
                        EmployeeName = "佐藤一郎",
                        DepartmentId = sales!.DepartmentId,
                        CreatedBy = "初期データ",
                        UpdatedBy = "初期データ"
                    },
                    new Employee
                    {
                        EmployeeName = "鈴木花子",
                        DepartmentId = dev!.DepartmentId,
                        CreatedBy = "初期データ",
                        UpdatedBy = "初期データ"
                    }
                );
            }

            SaveChanges();
        }
    }
}