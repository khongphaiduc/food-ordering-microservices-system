namespace auth_service.authservice.domain.Interfaces
{
    public interface IRoleUser
    {
        public Task<bool> AddMappingRoleToUser(string roleName);

    }
}
