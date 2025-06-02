using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.IdentityModel.Tokens;
using Models.Domain.People;
using Models.DTOs;
using UnitsOfWork;

namespace Controllers;

[ApiController]
[Route("api/Account")]
public class AccountController : ControllerBase
{
    private readonly IMainUnit _MainUnit;
    private readonly UserManager<AppUser> _UserManager;
    private readonly IConfiguration _Config;
    public AccountController(UserManager<AppUser> userManager, IConfiguration configuration, IMainUnit mainUnit)
    {
        _UserManager = userManager;
        _Config = configuration;
        _MainUnit = mainUnit;
    }

    private void FillPersonalData(dynamic person, dynamic Dto)
    {
        person.FirstName = Dto.FirstName;
        person.LastName = Dto.LastName;
        person.Gender = Dto.Gender;
        person.Age = Dto.Age;
        person.Address = Dto.Address;
        person.Email = Dto.Email;
        person.Phone = Dto.Phone;
        if(person.CreatedAt == new DateOnly())
        {
            person.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
        }
        person.UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
    }

    private async Task<IActionResult> CompleteSignIn(string Role, string UserName, string Password, int PersonId)
    {
        AppUser User = new AppUser()
        {
            UserName = UserName,
            PersonId = PersonId
        };
        
        var Result = await _UserManager.CreateAsync(User, Password);
        if(!Result.Succeeded)
        {
            foreach(var error in Result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return BadRequest(ModelState);
        }

        Result = await _UserManager.AddToRoleAsync(User, Role);
        
        if(!Result.Succeeded)
        {
            foreach(var error in Result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return BadRequest(ModelState);
        }
        return Ok("Succeeded");
    }

    [AllowAnonymous]
    [HttpPost("PatientSignIn", Name = "PatientSignIn")]
    public async Task<IActionResult> PatientSignIn(PatientSignInDto Info)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest("the data is not valid");
        }
        Patient NewPatient = new Patient();
        FillPersonalData(NewPatient, Info);
        int PatientId = await _MainUnit.Patients.AddNew(NewPatient);
        return await CompleteSignIn("Patient", Info.UserName!, Info.Password!, PatientId);
    }

    [AllowAnonymous]
    // [Authorize(Roles = "Admin")]
    [HttpPost("AdminSignIn", Name = "AdminSIngIn")]
    public async Task<IActionResult> AdminSIgnIn(AdminSingInDto Info)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest("data is not valid.");
        }
        Admin NewAdmin = new Admin();
        FillPersonalData(NewAdmin, Info);
        NewAdmin.IsActive = Info.IsActive;
        NewAdmin.Salary = Info.Salary;
        int PersonId = await _MainUnit.Admins.AddNew(NewAdmin);
        return await CompleteSignIn("Admin", Info.UserName!, Info.Password!, PersonId);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("DoctorSignIn", Name = "DoctorSIgnIn")]
    public async Task<IActionResult> DoctorSignIn(DoctorSignInDto Info)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest("data is not valid.");
        }
        Doctor NewDoctor = new Doctor();
        FillPersonalData(NewDoctor, Info);
        NewDoctor.IsActive = Info.IsActive;
        NewDoctor.Salary = Info.Salary;
        NewDoctor.SpecializationId = Info.SpecializationId;
        int PersonId = await _MainUnit.Doctors.AddNew(NewDoctor);
        return await CompleteSignIn("Doctor", Info.UserName!, Info.Password!, PersonId);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("NurseSIgnIn", Name = "NurseSignIn")]
    public async Task<IActionResult> NurseSignIn(NurseSIgnInDto Info)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest("data is not valid");
        }
        Nurse NewNurse = new Nurse();
        FillPersonalData(NewNurse, Info);
        NewNurse.IsActive = Info.IsActive;
        NewNurse.Salary = Info.Salary;
        NewNurse.DepartmentId = Info.DepartmentId;
        int PersonId = await _MainUnit.Nurses.AddNew(NewNurse);
        return await CompleteSignIn("Nurse", Info.UserName!, Info.Password!, PersonId);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("LabTechSignIn", Name = "LabTechSignIn")]
    public async Task<IActionResult> LabTechSignIn(LabTechSignInDto Info)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest("data is not valid.");
        }
        LabTech NewLabTech = new LabTech();
        FillPersonalData(NewLabTech, Info);
        NewLabTech.Salary = Info.Salary;
        NewLabTech.IsActive = Info.IsActive;
        int PersonId = await _MainUnit.LabsTech.AddNew(NewLabTech);
        return await CompleteSignIn("LabTech", Info.UserName!, Info.Password!, PersonId);
    }

    [AllowAnonymous]
    [HttpPost("LogIn", Name = "LogIn")]
    public async Task<IActionResult> LogIn(LogInDto Info)
    {
        if(!ModelState.IsValid)
        {    
            return BadRequest(ModelState);
        }
        var User = await _UserManager.FindByNameAsync(Info.UserName ?? "");
        if(User != null)
        {
            if(await _UserManager.CheckPasswordAsync(User, Info.Password ?? ""))
            {
                JwtSecurityToken TokenDescriptor = await CreateTokenDescriptor(User);
                var token = new 
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(TokenDescriptor)
                };
                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }   
        }
        else
        {
            ModelState.AddModelError("", "user name not exists.");
        }
        return BadRequest(ModelState);
    }
    private async Task<JwtSecurityToken> CreateTokenDescriptor(AppUser User)
    {
        var Claims = new List<Claim>();
        Claims.Add(new Claim(ClaimTypes.NameIdentifier, User.PersonId.ToString()));
        Claims.Add(new Claim(ClaimTypes.Name, User.UserName!));
        Claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        // get the roles for this user
        var Roles = await _UserManager.GetRolesAsync(User);
        foreach(var role in Roles)
        {
            Claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
        }
        // add singing credentials
        var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Config["JWT:SecretKey"]!));
        var sc = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

        var TokenDescriptor = new JwtSecurityToken(
            claims: Claims,
            issuer: _Config["JWT:Issuer"],
            audience: _Config["JWT:Audience"],
            signingCredentials: sc,
            expires: DateTime.Now.AddHours(1)
        );
        
        return TokenDescriptor;
    }

    [Authorize(Policy = "OwnProfile")]
    [HttpPatch("UpdateProfileInfo/{Id}", Name = "UpdateProfileInfo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProfileInfo([FromRoute]int Id, [FromBody] PersonUpdateDto Info)
    {
        var Person = await _MainUnit.People.FindByIdAsync(Id);
        if(Person == null)
        {
            return NotFound("Person not found.");
        }
        FillPersonalData(Person, Info);
        bool IsUpdated = await _MainUnit.People.UpdateAsync(Person);
        if(IsUpdated)
        {
            return Ok("Profile updated successfully.");
        }
        return BadRequest("Profile is not updated.");
    }
}