using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace auth_services.AuthService.Infastructure.Models
{
    [Table("OutBoxMessageTable")]
    public class OutBoxMessageTable
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string TypeMessage { get; set; }

        public string PayLoad { get; set; }

        public bool IsProccessed { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
