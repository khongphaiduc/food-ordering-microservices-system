using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using user_service.UserService.Application.DTOS;
using user_service.UserService.Application.Services;

namespace user_service.UserService.API.UserServiceControllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserProfile _iUserCreateProfileService;

        public UsersController(IUserProfile userProfile)
        {
            _iUserCreateProfileService = userProfile;
        }


        [HttpPost]
        public async Task<IActionResult> CreateNewUser(RequestUserProfile requestUserProfile)
        {
            var result = await _iUserCreateProfileService.UserProfilHandle(requestUserProfile);

            return Ok(result);
        }

    }
}
