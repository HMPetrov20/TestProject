namespace PractiseApp.BLL.Identity.Constants;

public class DefaultRoles
{
    public const string Admin = "Admin";
    public const string Moderator = "Moderator";
    public const string User = "User";
    
    public static readonly List<string> List = new()
    {
        Admin,
        Moderator,
        User
    };
}