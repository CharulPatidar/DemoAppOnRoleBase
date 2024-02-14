namespace DemoApp.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }

        public Role()
        {
            Id = Guid.NewGuid();
        }

        // navigation properties
        public List<RolePermission> RolePermissions { get; set; } 

        public List<UserRole> UserRoles { get; set; }


    }
}
