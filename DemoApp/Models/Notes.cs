namespace DemoApp.Models
{
    public class Notes
    {
        public Guid Id { get; set; }

        public string Topic { get; set; }

        public string Description { get; set; }

        public List<UserNotes> UserNotes { get; set; }

    }
}
