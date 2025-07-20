using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HumanCapitalManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Designations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Designations_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdentityUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    DesignationId = table.Column<int>(type: "int", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeTypeId = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_Designations_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "Designations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_EmployeeTypes_EmployeeTypeId",
                        column: x => x.EmployeeTypeId,
                        principalTable: "EmployeeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, false, "IT" },
                    { 2, false, "HR" },
                    { 3, false, "Sales" },
                    { 4, false, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "EmployeeTypes",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, false, "Permanent" },
                    { 2, false, "Temporary" },
                    { 3, false, "Contract" },
                    { 4, false, "Intern" }
                });

            migrationBuilder.InsertData(
                table: "Designations",
                columns: new[] { "Id", "DepartmentId", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, 1, false, "Software Developer" },
                    { 2, 1, false, "System Administrator" },
                    { 3, 1, false, "Network Engineer" },
                    { 4, 2, false, "HR Specialist" },
                    { 5, 2, false, "HR Manager" },
                    { 6, 2, false, "Talent Acquisition Coordinator" },
                    { 7, 3, false, "Sales Executive" },
                    { 8, 3, false, "Sales Manager" },
                    { 9, 3, false, "Account Executive" },
                    { 10, 4, false, "Office Manager" },
                    { 11, 4, false, "Executive Assistant" },
                    { 12, 4, false, "Receptionist" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "DateOfBirth", "DepartmentId", "DesignationId", "Email", "EmployeeTypeId", "FullName", "Gender", "HireDate", "IdentityUserId", "Password", "Salary" },
                values: new object[,]
                {
                    { 1, new DateTime(1990, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "john@example.com", 1, "John Doe", "Male", new DateTime(2020, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "John123!", 60000m },
                    { 2, new DateTime(1985, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 5, "jane@example.com", 1, "Jane Smith", "Female", new DateTime(2018, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Jane123!", 80000m },
                    { 3, new DateTime(1992, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 7, "sam@example.com", 3, "Sam Wilson", "Male", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Sam123!", 50000m },
                    { 4, new DateTime(1995, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 11, "anna@example.com", 2, "Anna Taylor", "Female", new DateTime(2022, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Anna123!", 40000m },
                    { 5, new DateTime(1989, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, "tom@example.com", 1, "Tom Brown", "Male", new DateTime(2019, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Tom123!", 70000m },
                    { 6, new DateTime(1987, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 4, "emma@example.com", 1, "Emma Davis", "Female", new DateTime(2017, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Emma123!", 75000m },
                    { 7, new DateTime(1990, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 8, "luke@example.com", 3, "Luke Miller", "Male", new DateTime(2020, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Luke123!", 85000m },
                    { 8, new DateTime(1993, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 10, "olivia@example.com", 1, "Olivia Johnson", "Female", new DateTime(2021, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Olivia123!", 65000m },
                    { 9, new DateTime(1997, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "mia@example.com", 4, "Mia Moore", "Female", new DateTime(2022, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Mia123!", 30000m },
                    { 10, new DateTime(1986, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 6, "chris@example.com", 2, "Chris Evans", "Other", new DateTime(2018, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Chris123!", 55000m },
                    { 11, new DateTime(1994, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 7, "sophia@example.com", 1, "Sophia White", "Female", new DateTime(2019, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Sophia123!", 52000m },
                    { 12, new DateTime(1996, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 12, "liam@example.com", 2, "Liam Green", "Male", new DateTime(2020, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Liam123!", 38000m },
                    { 13, new DateTime(1991, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "noah@example.com", 1, "Noah Black", "Male", new DateTime(2018, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Noah123!", 65000m },
                    { 14, new DateTime(1988, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 4, "isabella@example.com", 1, "Isabella Blue", "Female", new DateTime(2017, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Isabella123!", 76000m },
                    { 15, new DateTime(1993, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 9, "james@example.com", 3, "James Brown", "Male", new DateTime(2021, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "James123!", 62000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Designations_DepartmentId",
                table: "Designations",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DesignationId",
                table: "Employees",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeTypeId",
                table: "Employees",
                column: "EmployeeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_IdentityUserId",
                table: "Employees",
                column: "IdentityUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Designations");

            migrationBuilder.DropTable(
                name: "EmployeeTypes");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
