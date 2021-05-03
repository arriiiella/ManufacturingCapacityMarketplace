using API.Models;

namespace API.Interfaces
{
    // A contract between itself and a class who implements it
    public interface ITokenService
    {
        string CreateToken(User user);
        
    }
}