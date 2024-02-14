using System.ComponentModel.DataAnnotations.Schema;

namespace DemoApp.Models
{
    public class RolePermission
    {
        public Guid Id { get; set; }


        [ForeignKey("Role")]
        public Guid RoleId { get; set; }

        [ForeignKey("Permission")]
        public Guid PermissionId { get; set; }



        public RolePermission()
        {
            Id = Guid.NewGuid();
        }


        // navigation properties
        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }
}
