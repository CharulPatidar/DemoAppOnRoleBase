namespace DemoApp.Models
{
    public class Permission
    {
        public Guid Id { get; set; }

        public string PermissionName { get; set; }

        public Permission()
        {
            Id = Guid.NewGuid();
        }


        // navigation property
        public List<RolePermission> RolePermissions { get; set; }


    }
}
