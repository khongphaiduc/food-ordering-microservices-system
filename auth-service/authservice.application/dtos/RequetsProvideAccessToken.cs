using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace auth_service.authservice.application.dtos
{
    public class RequetsProvideAccessToken
    {
        [JsonRequired]
        public Guid UserId { get; set; } 
    }
}
