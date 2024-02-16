namespace DemoApp.DTO
{
 
    public class UserDto
    {
        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public string UserPassword { get; set; }
    }

    public class NotesDto
    {
        public string NoteId { get; set; }

        public string NoteTopic { get; set; }

        public string NoteDescription { get; set; }
    }

    public class RoleDto
    {
        public string RoleId { get; set; }

        public string RoleName { get; set; }
    }


    public class PermissionDto
    {
        public string PermissionId { get; set; }

        public string PermissionName { get; set; }
    }

    public class UserRoleDto
    {
        public string UserId { get; set; }

        public string RoleId { get; set; }

    }

    public class RolePermissionDto
    {
        public string RoleId { get; set; }
        public string PermissionId { get; set; }

    }

    


}
