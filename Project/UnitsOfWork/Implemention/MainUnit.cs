
using Models.Data;
using Models.Domain.People;
using Models.Domain.Services;
using Project.Repos.Abstract;
using Project.Repos.Implemention;

namespace UnitsOfWork.Implemention;


public class MainUnit : IMainUnit
{

    private readonly AppDbContext _db;
    public MainUnit(AppDbContext db)
    {
        _db = db;
        People = new MainRepo<Person>(_db);
        Admins = new MainRepo<Admin>(_db);
        Doctors = new MainRepo<Doctor>(_db);
        LabsTech = new MainRepo<LabTech>(_db);
        Nurses = new MainRepo<Nurse>(_db);
        Patients = new MainRepo<Patient>(_db);

        Appointments = new MainRepo<Appointment>(_db);
        Departments = new MainRepo<Department>(_db);
        Specializations = new MainRepo<Specialization>(_db);
        LaboratoryTests = new MainRepo<LaboratoryTest>(_db);
        MedicalRecords = new MainRepo<MedicalRecord>(_db);
        Payments = new MainRepo<Payment>(_db);
        Prescriptions = new MainRepo<Prescription>(_db);
    }

    public IMainRepo<Person> People { get; private set; }

    public IMainRepo<Admin> Admins { get; private set; }

    public IMainRepo<Doctor> Doctors { get; private set; }

    public IMainRepo<LabTech> LabsTech { get; private set; }

    public IMainRepo<Nurse> Nurses { get; private set; }

    public IMainRepo<Patient> Patients { get; private set; }
    
    
    public IMainRepo<Appointment> Appointments { get; set ; }
    public IMainRepo<Department> Departments { get; set ; }
    public IMainRepo<Specialization> Specializations { get; set ; }
    public IMainRepo<LaboratoryTest> LaboratoryTests { get; set ; }
    public IMainRepo<MedicalRecord> MedicalRecords { get; set ; }
    public IMainRepo<Payment> Payments { get; set ; }
    public IMainRepo<Prescription> Prescriptions { get; set ; }
}