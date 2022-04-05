using System.ComponentModel.DataAnnotations.Schema;

namespace Downgrooves.Domain
{
    [Table("user")]
    public class User
    {
        [Column("UserId")]
        public int Id { get; set; }

        public string Password { get; set; }

        [NotMapped]
        public string Role { get; set; }

        public string UserName { get; set; }
    }
}