using EmployeeManagementWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>().HasQueryFilter(e => !e.IsDeleted);
        }

        public void SeedData()
        {
            EnsureLegacySchemaUpdated();

            var hasher = new PasswordHasher<User>();

            if (!Users.Any())
            {
                var admin = new User { UserId = "admin", UserName = "管理者", Role = "Admin" };
                admin.PasswordHash = hasher.HashPassword(admin, "1234");

                var user01 = new User { UserId = "user01", UserName = "山田太郎", Role = "User" };
                user01.PasswordHash = hasher.HashPassword(user01, "1111");

                Users.AddRange(admin, user01);
            }
            else
            {
                var legacyUsers = Users.Where(u => string.IsNullOrEmpty(u.PasswordHash)).ToList();
                foreach (var user in legacyUsers)
                {
                    var plainPassword = GetLegacyPassword(user.Id);
                    user.PasswordHash = hasher.HashPassword(user, plainPassword);
                    user.Role = string.IsNullOrWhiteSpace(user.Role) ? "User" : user.Role;
                }
            }

            if (!Departments.Any())
            {
                Departments.AddRange(
                    new Department { DepartmentName = "営業部", CreatedBy = "初期データ", UpdatedBy = "初期データ" },
                    new Department { DepartmentName = "開発部", CreatedBy = "初期データ", UpdatedBy = "初期データ" },
                    new Department { DepartmentName = "総務部", CreatedBy = "初期データ", UpdatedBy = "初期データ" }
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
                        EmployeeNumber = "E0001",
                        EmployeeName = "佐藤一郎",
                        Email = "satou@example.com",
                        HireDate = new DateTime(2020, 4, 1),
                        Position = "主任",
                        DepartmentId = sales!.DepartmentId,
                        CreatedBy = "初期データ",
                        UpdatedBy = "初期データ",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new Employee
                    {
                        EmployeeNumber = "E0002",
                        EmployeeName = "鈴木花子",
                        Email = "suzuki@example.com",
                        HireDate = new DateTime(2021, 4, 1),
                        Position = "担当",
                        DepartmentId = dev!.DepartmentId,
                        CreatedBy = "初期データ",
                        UpdatedBy = "初期データ",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    }
                );
            }

            SaveChanges();
        }

        private void EnsureLegacySchemaUpdated()
        {
            AddColumnIfMissing("Employees", "IsDeleted", "INTEGER NOT NULL DEFAULT 0");
            AddColumnIfMissing("Employees", "EmployeeNumber", "TEXT NOT NULL DEFAULT ''");
            AddColumnIfMissing("Employees", "Email", "TEXT NOT NULL DEFAULT ''");
            AddColumnIfMissing("Employees", "HireDate", "TEXT NOT NULL DEFAULT '2000-01-01'");
            AddColumnIfMissing("Employees", "Position", "TEXT NOT NULL DEFAULT ''");
            AddColumnIfMissing("Employees", "CreatedAt", "TEXT NOT NULL DEFAULT '2000-01-01 00:00:00'");
            AddColumnIfMissing("Employees", "UpdatedAt", "TEXT NOT NULL DEFAULT '2000-01-01 00:00:00'");
            AddColumnIfMissing("Users", "PasswordHash", "TEXT NOT NULL DEFAULT ''");
            AddColumnIfMissing("Users", "Role", "TEXT NOT NULL DEFAULT 'User'");
        }

        private void AddColumnIfMissing(string tableName, string columnName, string definition)
        {
            using var connection = new SqliteConnection(Database.GetConnectionString());
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = $"PRAGMA table_info({tableName});";

            var exists = false;
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader.GetString(1) == columnName)
                    {
                        exists = true;
                        break;
                    }
                }
            }

            if (!exists)
            {
                using var alterCmd = connection.CreateCommand();
                alterCmd.CommandText = $"ALTER TABLE {tableName} ADD COLUMN {columnName} {definition};";
                alterCmd.ExecuteNonQuery();
            }
        }

        private string GetLegacyPassword(int userId)
        {
            using var connection = new SqliteConnection(Database.GetConnectionString());
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Password FROM Users WHERE Id = $id LIMIT 1;";
            cmd.Parameters.AddWithValue("$id", userId);
            var value = cmd.ExecuteScalar();
            return value?.ToString() ?? "";
        }
    }
}
