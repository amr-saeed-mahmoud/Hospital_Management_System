using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Domain.People;
using Models.Domain.Services;

namespace Models.Data;


public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder.Entity<Person>().UseTptMappingStrategy();

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var foreignKey in entityType.GetForeignKeys())
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
        base.OnModelCreating(builder);
    }


    public DbSet<Admin> Admins { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<LabTech> LabTechs { get; set; }
    public DbSet<Nurse> Nurses { get; set; }
    public DbSet<Patient> Patient { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<Appointment> Appointment { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<LaboratoryTest> LaboratoryTests { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Specialization> Specialization { get; set; }
}