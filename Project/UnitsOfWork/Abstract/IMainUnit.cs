
using Models.Domain.People;
using Models.Domain.Services;
using Project.Repos.Abstract;

namespace UnitsOfWork;


public interface IMainUnit
{
    public IMainRepo<Person> People { get; }
    public IMainRepo<Admin> Admins { get; }
    public IMainRepo<Doctor> Doctors { get; }
    public IMainRepo<LabTech> LabsTech { get; }
    public IMainRepo<Nurse> Nurses { get; }
    public IMainRepo<Patient> Patients { get; }

    public IMainRepo<Appointment> Appointments { get; set; }
    public IMainRepo<Department> Departments { get; set; }
    public IMainRepo<Specialization> Specializations { get; set; }
    public IMainRepo<LaboratoryTest> LaboratoryTests { get; set; }
    public IMainRepo<MedicalRecord> MedicalRecords { get; set; }
    public IMainRepo<Payment> Payments { get; set; }
    public IMainRepo<Prescription> Prescriptions { get; set; }
}