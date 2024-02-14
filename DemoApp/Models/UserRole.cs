using System.ComponentModel.DataAnnotations.Schema;

namespace DemoApp.Models
{
    public class UserRole
    {
        public Guid Id { get; set; }


        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [ForeignKey("Role")]
        public Guid RoleId { get; set; }



        public UserRole()
        {
            Id = Guid.NewGuid();
        }



        // navigation properties
        public User User { get; set; }
        public Role Role { get; set; }


    }
}
