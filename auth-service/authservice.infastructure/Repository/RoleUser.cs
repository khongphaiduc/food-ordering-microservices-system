using auth_service.authservice.api.CustomExceptionSerives;
using auth_service.authservice.domain.Interfaces;
using auth_service.authservice.infastructure.dbcontexts;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace auth_service.authservice.infastructure.Repository
{
    public class RoleUser : IRoleUser
    {
        private readonly FoodAuthContext _db;

        public RoleUser(FoodAuthContext context)
        {
            _db = context;
        }

        // map role  cho thanwgf customer
        public async Task<bool> AddMappingRoleToUser(string email)
        {

            var user = await _db.Users.FirstOrDefaultAsync(s => s.Email == email);

            if (user == null)
            {
                throw new NotFoundException("Not found user");
            }

            var roleUser = await _db.Roles.FirstOrDefaultAsync(r => r.Name == "Customer");

            if (roleUser == null)
            {
                throw new NotFoundException("Not found role");
            }
            user.Roles.Add(roleUser);

            var row = await _db.SaveChangesAsync();

            return row > 0;
        }
        

    }
}
