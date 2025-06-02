using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Domain.People;
using Models.Domain.Services;

namespace Project.Extentions;

public static class DataInitializer
{
    public static async Task SeedRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = { "Admin", "Patient", "Doctor", "Nurse", "LabTech" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    public static async Task SeedDepartments(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (!await dbContext.Set<Department>().AnyAsync())
        {
            var departments = new List<Department>
            {
                new Department
                {
                    Name = "Cardiology",
                    Description = "Handles heart-related treatments.",
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.Now),
                },
                new Department
                {
                    Name = "Neurology",
                    Description = "Focuses on nervous system disorders.",
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.Now),
                },
                new Department
                {
                    Name = "Orthopedics",
                    Description = "Deals with musculoskeletal system issues.",
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.Now),
                }
            };

            await dbContext.Set<Department>().AddRangeAsync(departments);
            await dbContext.SaveChangesAsync();
        }
    }

    public static async Task SeedSpecializations(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (!await dbContext.Set<Specialization>().AnyAsync())
        {
            var specializations = new List<Specialization>
            {
                new Specialization
                {
                    Name = "Interventional Cardiology",
                    Description = "Specializes in catheter-based treatment of heart diseases.",
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.Now),
                    DepartmentId = 1 // Cardiology
                },
                new Specialization
                {
                    Name = "Electrophysiology",
                    Description = "Focuses on electrical activities of the heart.",
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.Now),
                    DepartmentId = 1 // Cardiology
                },
                new Specialization
                {
                    Name = "Stroke Management",
                    Description = "Specializes in treating stroke-related conditions.",
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.Now),
                    DepartmentId = 2 // Neurology
                },
                new Specialization
                {
                    Name = "Spinal Surgery",
                    Description = "Deals with surgeries for spinal disorders.",
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.Now),
                    DepartmentId = 3 // Orthopedics
                }
            };

            await dbContext.Set<Specialization>().AddRangeAsync(specializations);
            await dbContext.SaveChangesAsync();
        }
    }

        public static async Task UseAllDataSeed(IServiceProvider serviceProvider)
    {
        await SeedRoles(serviceProvider);
        await SeedDepartments(serviceProvider);
        await SeedSpecializations(serviceProvider);
    }
}
