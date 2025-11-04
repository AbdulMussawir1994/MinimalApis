using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using MinimalApis.Entities.Model;

namespace MinimalApis.DataDbContext;

public class AppDbContext : IdentityDbContext<AppUser>
{
    private readonly ILogger<AppDbContext> _logger;

    public AppDbContext(DbContextOptions<AppDbContext> options, ILogger<AppDbContext> logger) : base(options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        try
        {
            var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            if (databaseCreator is not null)
            {
                if (!databaseCreator.CanConnect())
                {
                    databaseCreator.Create();
                }
                if (!databaseCreator.HasTables())
                {
                    databaseCreator.CreateTables();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
        }
    }
    // DbSets
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<EntityList> EntityLists => Set<EntityList>();
    public DbSet<EntityListDetailCompanyWise> EntityListDetails => Set<EntityListDetailCompanyWise>();
    public DbSet<GroupRoleDetail> GroupRolesDetails => Set<GroupRoleDetail>();
    public DbSet<GroupRoleMaster> GroupRolesMaster => Set<GroupRoleMaster>();
    public DbSet<Outlet> Outlets => Set<Outlet>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<SubscriptionPaymentDetail> SubscriptionPaymentDetails => Set<SubscriptionPaymentDetail>();
    public DbSet<User> MainUsers => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //  ---AppUser Configuration-- -
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.Property(e => e.DateCreated).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.IsActive).HasDefaultValue(true).IsRequired();
        });

        // ---Relationships-- -
        modelBuilder.Entity<Department>()
            .HasMany(d => d.Employees)
            .WithOne(e => e.Department)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AppUser>()
            .HasMany(u => u.Employees)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AppUser>()
    .HasOne(u => u.Company)
    .WithMany(s => s.AppUsers)
    .HasForeignKey(u => u.CompanyId)
    .OnDelete(DeleteBehavior.Restrict); // ✅ Fix 1

        modelBuilder.Entity<AppUser>()
            .HasOne(u => u.Group)
            .WithMany()
            .HasForeignKey(u => u.GroupId)
            .OnDelete(DeleteBehavior.Restrict); // ✅ Fix 2

        modelBuilder.Entity<AppUser>()
            .HasOne(u => u.GroupRoleId)
            .WithMany()
            .HasForeignKey(u => u.GroupRoleGroupId)
            .OnDelete(DeleteBehavior.Restrict); // ✅ Fix 3

        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.Country)
            .WithMany(c => c.Subscriptions)
            .HasForeignKey(s => s.CountryID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SubscriptionPaymentDetail>()
            .HasOne(p => p.Subscription)
            .WithMany(s => s.Payments)
            .HasForeignKey(p => p.CompanyID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Subscription)
            .WithMany(s => s.Users)
            .HasForeignKey(u => u.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GroupRoleMaster>()
            .HasOne(g => g.Subscription)
            .WithMany(s => s.GroupRoles)
            .HasForeignKey(g => g.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GroupRoleDetail>()
            .HasOne(d => d.GroupRole)
            .WithMany(m => m.RoleDetails)
            .HasForeignKey(d => d.GroupID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EntityList>()
            .HasKey(e => e.EntityCode);

        modelBuilder.Entity<GroupRoleDetail>()
            .HasOne(d => d.Entity)
            .WithMany(e => e.GroupRoleDetails)
            .HasForeignKey(d => d.EntityCode)
            .HasPrincipalKey(e => e.EntityCode)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EntityListDetailCompanyWise>()
            .HasOne(e => e.Entity)
            .WithMany(l => l.CompanyWiseDetails)
            .HasForeignKey(e => e.EntityCode)
            .HasPrincipalKey(l => l.EntityCode)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Country>()
            .HasOne(c => c.Currency)
            .WithMany(cr => cr.Countries)
            .HasForeignKey(c => c.CurrencyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Outlet>()
            .HasOne(o => o.Subscription)
            .WithMany(s => s.Outlets)
            .HasForeignKey(o => o.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Outlet>()
            .HasOne(o => o.Country)
            .WithMany(c => c.Outlets)
            .HasForeignKey(o => o.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Outlet>()
            .HasOne(o => o.Currency)
            .WithMany(c => c.Outlets)
            .HasForeignKey(o => o.CurrencyID)
            .OnDelete(DeleteBehavior.Restrict);

        // --- Seed Data ---
        var createdOn = new DateTime(2025, 11, 4, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Currency>().HasData(
            new Currency
            {
                ID = 1,
                CurrencyCode = "PKR",
                Rate = 75m,
                IsDefault = true,
                CurrencyName = "Pakistani Rupee",
                CreatedBy = "1",
                CreatedOn = createdOn,
                ModifyBy = "1",
                ModifyOn = createdOn,
                NumOfDecimal = 2
            }
        );

        modelBuilder.Entity<Country>().HasData(
            new Country
            {
                CountryID = 1,
                CountryName = "Pakistan",
                Active = true,
                CreatedBy = "1",
                CreatedOn = createdOn,
                ModifyBy = "1",
                ModifyOn = createdOn,
                CountryCode = "PK",
                CurrencyId = 1
            }
        );

        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "HR" },
            new Department { Id = 2, Name = "IT" }
        );

        modelBuilder.Entity<EntityList>().HasData(
            new EntityList { EntityCode = "Dashboard", ModuleCode = "Dashboard", EntityName = "Dashboard", RowNo = 1, Path = "/dashboard", OrderNum = 1, Active = true, Icon = "dashboard" },
            new EntityList { EntityCode = "Outlet", ModuleCode = "Outlet", EntityName = "Outlet", RowNo = 2, Path = "/outlets", OrderNum = 2, Active = true, Icon = "store" }
        );

        modelBuilder.Entity<GroupRoleMaster>().HasData(
            new GroupRoleMaster
            {
                GroupID = 1,
                GroupName = "Manager",
                CreatedBy = "1",
                CreatedOn = createdOn,
                ModifyBy = "1",
                ModifyOn = createdOn,
                CompanyId = 1
            }
        );

        modelBuilder.Entity<GroupRoleDetail>().HasData(
            new GroupRoleDetail
            {
                RoleDetailID = 1,
                GroupID = 1, // FIXED to match seeded GroupRoleMaster
                EntityCode = "Dashboard",
                Allow = true,
                New = true,
                Edit = true
            }
        );

        modelBuilder.Entity<Subscription>().HasData(
            new Subscription
            {
                CompanyID = 1,
                CompanyName = "Demo Company",
                OutletsCount = "10",
                CountryID = 1,
                Mobile = "0341",
                CreatedBy = "1",
                CreatedOn = createdOn,
                ModifyBy = "1",
                ModifyOn = createdOn,
                CountryCode = "PK",
                ContactPerson = "Abdul",
                IsActive = true
            }
        );

        modelBuilder.Entity<Outlet>().HasData(
            new Outlet
            {
                ID = 1,
                OutletName = "Outlet1",
                CountryId = 1,
                Email = "outlet1@gmail.com",
                Phone = "0341",
                Address = "Karachi",
                IsActive = true,
                CreatedBy = "1",
                CreatedOn = createdOn,
                ModifyBy = "1",
                ModifyOn = createdOn,
                CompanyId = 1,
                CurrencyID = 1
            }
        );

        // --- Default Admin User Seed ---
        var userId = Guid.NewGuid().ToString();
        var hasher = new PasswordHasher<AppUser>();
        var adminUser = new AppUser
        {
            Id = userId,
            UserName = "Admin123",
            NormalizedUserName = "ADMIN123",
            Email = "admin@library.com",
            NormalizedEmail = "ADMIN@LIBRARY.COM",
            EmailConfirmed = true,
            DateCreated = createdOn,
            LockoutEnabled = true,
            GroupId = 1,
            CompanyId = 1,
            GroupRoleGroupId = 1,
        };
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin@123");

        modelBuilder.Entity<AppUser>().HasData(adminUser);
    }
}