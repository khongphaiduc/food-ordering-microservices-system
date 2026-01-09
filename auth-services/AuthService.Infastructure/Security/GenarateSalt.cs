using auth_services.AuthService.Application.Interfaces;

namespace auth_services.AuthService.Infastructure.Security
{
    public class GenarateSalt : IGenarateSalt
    {
        string IGenarateSalt.GenarateSalt()
        {
           return Guid.NewGuid().ToString().Replace("-", "");   
        }
    }
}
