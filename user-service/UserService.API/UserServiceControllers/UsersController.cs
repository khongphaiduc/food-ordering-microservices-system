using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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


        [HttpGet]
        public IActionResult Test()
        {
            var activity = Activity.Current;

            Console.WriteLine(">>> HIT CONTROLLER");

            if (activity == null)
            {
                Console.WriteLine("❌ Activity is NULL");
            }
            else
            {
                Console.WriteLine($"✅ TraceId: {activity.TraceId}");
                Console.WriteLine($"✅ SpanId : {activity.SpanId}");
                Console.WriteLine($"✅ Parent : {activity.ParentSpanId}");
                Console.WriteLine($"✅ Name   : {activity.DisplayName}");
            }

            return Ok("Xin chào");
        }
    }
}
