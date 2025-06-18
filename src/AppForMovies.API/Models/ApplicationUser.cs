using Microsoft.AspNetCore.Identity;

namespace AppForMovies.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
    }
    public ApplicationUser(string id, string name, string surname, string userName)
    {
        Id = id;
        Name = name;
        Surname = surname;
        UserName = userName;
        Email = userName;
    }

    [Display(Name = "Name")]
    public string? Name
    {
        get;
        set;
    }

    [Display(Name = "Surname")]
    public string? Surname
    {
        get;
        set;
    }
}

