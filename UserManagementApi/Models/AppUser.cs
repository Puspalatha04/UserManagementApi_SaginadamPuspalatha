
namespace UserManagementAPI.Models;

public class AppUser
{
    public int Id { get; set; }        // 0 allowed
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
}
