using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MinimalApis.Migrations
{
    /// <inheritdoc />
    public partial class Initital : Migration
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
                name: "Currencies",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: true),
                    CurrencyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumOfDecimal = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntityLists",
                columns: table => new
                {
                    EntityCode = table.Column<string>(type: "varchar(25)", nullable: false),
                    ModuleCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowNo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderNum = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsParent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityLists", x => x.EntityCode);
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
                name: "Countries",
                columns: table => new
                {
                    CountryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ACountryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryID);
                    table.ForeignKey(
                        name: "FK_Countries_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    CompanyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutletsCount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryID = table.Column<int>(type: "int", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyURLName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PricePlan = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.CompanyID);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Countries_CountryID",
                        column: x => x.CountryID,
                        principalTable: "Countries",
                        principalColumn: "CountryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EntityListDetails",
                columns: table => new
                {
                    RowNo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityCode = table.Column<string>(type: "varchar(25)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    SubscriptionCompanyID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityListDetails", x => x.RowNo);
                    table.ForeignKey(
                        name: "FK_EntityListDetails_EntityLists_EntityCode",
                        column: x => x.EntityCode,
                        principalTable: "EntityLists",
                        principalColumn: "EntityCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityListDetails_Subscriptions_SubscriptionCompanyID",
                        column: x => x.SubscriptionCompanyID,
                        principalTable: "Subscriptions",
                        principalColumn: "CompanyID");
                });

            migrationBuilder.CreateTable(
                name: "GroupRolesMaster",
                columns: table => new
                {
                    GroupID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupRolesMaster", x => x.GroupID);
                    table.ForeignKey(
                        name: "FK_GroupRolesMaster_Subscriptions_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Subscriptions",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Outlets",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutletName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    OutletNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArabicOutletName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArabicAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArabicArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyID = table.Column<int>(type: "int", nullable: true),
                    OutletSequanceNo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outlets", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Outlets_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Outlets_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Outlets_Subscriptions_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Subscriptions",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupRolesDetails",
                columns: table => new
                {
                    RoleDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupID = table.Column<int>(type: "int", nullable: true),
                    EntityCode = table.Column<string>(type: "varchar(25)", nullable: true),
                    Allow = table.Column<bool>(type: "bit", nullable: true),
                    New = table.Column<bool>(type: "bit", nullable: true),
                    Edit = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupRolesDetails", x => x.RoleDetailID);
                    table.ForeignKey(
                        name: "FK_GroupRolesDetails_EntityLists_EntityCode",
                        column: x => x.EntityCode,
                        principalTable: "EntityLists",
                        principalColumn: "EntityCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupRolesDetails_GroupRolesMaster_GroupID",
                        column: x => x.GroupID,
                        principalTable: "GroupRolesMaster",
                        principalColumn: "GroupID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MainUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupID = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OutletIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    DefaultOutlet = table.Column<int>(type: "int", nullable: true),
                    GroupRoleGroupID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainUsers", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_MainUsers_GroupRolesMaster_GroupRoleGroupID",
                        column: x => x.GroupRoleGroupID,
                        principalTable: "GroupRolesMaster",
                        principalColumn: "GroupID");
                    table.ForeignKey(
                        name: "FK_MainUsers_Subscriptions_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Subscriptions",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPaymentDetails",
                columns: table => new
                {
                    SubPaymentDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyID = table.Column<int>(type: "int", nullable: true),
                    OutletID = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPaymentDetails", x => x.SubPaymentDetailID);
                    table.ForeignKey(
                        name: "FK_SubscriptionPaymentDetails_Outlets_OutletID",
                        column: x => x.OutletID,
                        principalTable: "Outlets",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_SubscriptionPaymentDetails_Subscriptions_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Subscriptions",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    GroupRoleGroupId = table.Column<int>(type: "int", nullable: false),
                    OutletsId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_AspNetUsers_GroupRolesDetails_GroupRoleGroupId",
                        column: x => x.GroupRoleGroupId,
                        principalTable: "GroupRolesDetails",
                        principalColumn: "RoleDetailID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_GroupRolesMaster_GroupId",
                        column: x => x.GroupId,
                        principalTable: "GroupRolesMaster",
                        principalColumn: "GroupID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Subscriptions_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Subscriptions",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Restrict);
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
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "ID", "CreatedBy", "CreatedOn", "CurrencyCode", "CurrencyImage", "CurrencyName", "IsDefault", "ModifyBy", "ModifyOn", "NumOfDecimal", "Rate" },
                values: new object[] { 1, "1", new DateTime(2025, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc), "PKR", null, "Pakistani Rupee", true, "1", new DateTime(2025, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc), 2, 75m });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "HR" },
                    { 2, "IT" }
                });

            migrationBuilder.InsertData(
                table: "EntityLists",
                columns: new[] { "EntityCode", "Active", "EntityName", "Icon", "IsParent", "ModuleCode", "OrderNum", "Path", "RowNo" },
                values: new object[,]
                {
                    { "Dashboard", true, "Dashboard", "dashboard", false, "Dashboard", 1, "/dashboard", 1L },
                    { "Outlet", true, "Outlet", "store", false, "Outlet", 2, "/outlets", 2L },
                    { "Reports", true, "Reports", "report", false, "Reports", 4, "/report", 4L },
                    { "UserProfile", true, "UserProfile", "person", false, "UserProfile", 3, "/profile", 3L }
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryID", "ACountryName", "Active", "CountryCode", "CountryName", "CreatedBy", "CreatedOn", "CurrencyId", "ModifyBy", "ModifyOn" },
                values: new object[] { 1, null, true, "PK", "Pakistan", "1", new DateTime(2025, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1, "1", new DateTime(2025, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "CompanyID", "CompanyName", "CompanyURLName", "ContactPerson", "CountryCode", "CountryID", "CreatedBy", "CreatedOn", "IsActive", "Mobile", "ModifyBy", "ModifyOn", "OutletsCount", "PricePlan" },
                values: new object[] { 1, "Demo Company", null, "Abdul", "PK", 1, "1", new DateTime(2025, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc), true, "0341", "1", new DateTime(2025, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc), "10", null });

            migrationBuilder.InsertData(
                table: "GroupRolesMaster",
                columns: new[] { "GroupID", "CompanyId", "CreatedBy", "CreatedOn", "GroupName", "ModifyBy", "ModifyOn" },
                values: new object[] { 1, 1, "1", new DateTime(2025, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Manager", "1", new DateTime(2025, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Outlets",
                columns: new[] { "ID", "Address", "ArabicAddress", "ArabicArea", "ArabicOutletName", "CompanyId", "CountryId", "CreatedBy", "CreatedOn", "CurrencyID", "Email", "ImageName", "ImagePath", "IsActive", "ModifyBy", "ModifyOn", "OutletName", "OutletNumber", "OutletSequanceNo", "Phone" },
                values: new object[] { 1, "Karachi", null, null, null, 1, 1, "1", new DateTime(2025, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1, "outlet1@gmail.com", null, null, true, "1", new DateTime(2025, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Outlet1", null, null, "0341" });

            migrationBuilder.InsertData(
                table: "GroupRolesDetails",
                columns: new[] { "RoleDetailID", "Allow", "Edit", "EntityCode", "GroupID", "New" },
                values: new object[,]
                {
                    { 1, true, true, "Dashboard", 1, true },
                    { 2, true, true, "Outlet", 1, true },
                    { 3, true, true, "UserProfile", 1, true },
                    { 4, true, true, "Reports", 1, true }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "CompanyId", "ConcurrencyStamp", "CreatedBy", "Email", "EmailConfirmed", "GroupId", "GroupRoleGroupId", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OutletsId", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedBy", "UpdatedDate", "UserName" },
                values: new object[] { "378c0b06-88dd-420a-9c5c-b0fe6d56a683", 0, 1, "540c892e-c2f2-418e-a428-1671a3f5b88e", null, "admin@library.com", true, 1, 1, true, true, null, "ADMIN@LIBRARY.COM", "ADMIN123", 0L, "AQAAAAIAAYagAAAAENzabTFoyAiqITPlTOu0L/ViZ5+xk4r7NWvFVEhqqGB3jRGs+UibYuqmRPnsjR7oEQ==", null, false, "620ea8f8-601a-4cf1-8931-bb3cfd24d568", false, null, null, "Admin123" });

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
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GroupId",
                table: "AspNetUsers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GroupRoleGroupId",
                table: "AspNetUsers",
                column: "GroupRoleGroupId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CurrencyId",
                table: "Countries",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityListDetails_EntityCode",
                table: "EntityListDetails",
                column: "EntityCode");

            migrationBuilder.CreateIndex(
                name: "IX_EntityListDetails_SubscriptionCompanyID",
                table: "EntityListDetails",
                column: "SubscriptionCompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupRolesDetails_EntityCode",
                table: "GroupRolesDetails",
                column: "EntityCode");

            migrationBuilder.CreateIndex(
                name: "IX_GroupRolesDetails_GroupID",
                table: "GroupRolesDetails",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupRolesMaster_CompanyId",
                table: "GroupRolesMaster",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MainUsers_CompanyId",
                table: "MainUsers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MainUsers_GroupRoleGroupID",
                table: "MainUsers",
                column: "GroupRoleGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Outlets_CompanyId",
                table: "Outlets",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Outlets_CountryId",
                table: "Outlets",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Outlets_CurrencyID",
                table: "Outlets",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPaymentDetails_CompanyID",
                table: "SubscriptionPaymentDetails",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPaymentDetails_OutletID",
                table: "SubscriptionPaymentDetails",
                column: "OutletID");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_CountryID",
                table: "Subscriptions",
                column: "CountryID");
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
                name: "EntityListDetails");

            migrationBuilder.DropTable(
                name: "MainUsers");

            migrationBuilder.DropTable(
                name: "SubscriptionPaymentDetails");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Outlets");

            migrationBuilder.DropTable(
                name: "GroupRolesDetails");

            migrationBuilder.DropTable(
                name: "EntityLists");

            migrationBuilder.DropTable(
                name: "GroupRolesMaster");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
