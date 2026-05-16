PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS Departments (
    DepartmentId INTEGER PRIMARY KEY AUTOINCREMENT,
    DepartmentName TEXT NOT NULL,
    CreatedBy TEXT NOT NULL DEFAULT '',
    UpdatedBy TEXT NOT NULL DEFAULT '',
    CONSTRAINT UQ_Departments_DepartmentName UNIQUE (DepartmentName)
);

CREATE TABLE IF NOT EXISTS Employees (
    EmployeeId INTEGER PRIMARY KEY AUTOINCREMENT,
    EmployeeNumber TEXT NOT NULL,
    EmployeeName TEXT NOT NULL,
    Email TEXT NOT NULL,
    HireDate TEXT NOT NULL,
    Position TEXT NOT NULL,
    DepartmentId INTEGER NOT NULL,
    CreatedBy TEXT NOT NULL DEFAULT '',
    UpdatedBy TEXT NOT NULL DEFAULT '',
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NOT NULL,
    IsDeleted INTEGER NOT NULL DEFAULT 0 CHECK (IsDeleted IN (0, 1)),
    CONSTRAINT UQ_Employees_EmployeeNumber UNIQUE (EmployeeNumber),
    CONSTRAINT FK_Employees_Departments FOREIGN KEY (DepartmentId)
        REFERENCES Departments(DepartmentId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);
