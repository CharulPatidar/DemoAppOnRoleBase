namespace DemoApp.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }  
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }

        public string UserSalt { get; set; }
        public string UserHashedPassword { get; set; }


        public User()
        {
            Id = Guid.NewGuid();
        }

        // navigation property
        public UserRole? UserRoles { get; set; } 
        public List<RolePermission>? RolePermissions { get; set; } 

        public List<UserNotes>? UserNotes { get; set; }

    }
}
