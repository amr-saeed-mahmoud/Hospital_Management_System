using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Models.Domain.People;

public class AppUser : IdentityUser
{

    public int PersonId { get; set; }

    [ForeignKey("PersonId")]
    public Person? CurrentPerson { get; set; }
    
}