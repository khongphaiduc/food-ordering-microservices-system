using Microsoft.Identity.Client;

namespace auth_services.AuthService.Application.DTOS
{
    public class OutBoxMessageInternalDTO
    {

        public Guid Id { get; set; }

        public string TypeMessage { get; set; }

        public string Payload { get; set; }

        public bool IsProccessed { get; set; }


        public DateTime CreateAt { get; set; }

        public OutBoxMessageInternalDTO(string typeMessage, string payload)
        {
            Id = Guid.NewGuid();
            TypeMessage = typeMessage;
            Payload = payload;
            IsProccessed = false;
            CreateAt = DateTime.UtcNow;
        }

        public OutBoxMessageInternalDTO(Guid id, string typeMessage, string payload, bool isProccessed, DateTime createAt)
        {
            Id = id;
            TypeMessage = typeMessage;
            Payload = payload;
            IsProccessed = isProccessed;
            CreateAt = createAt;
        }
    }
}
