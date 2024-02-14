using System.ComponentModel.DataAnnotations.Schema;

namespace DemoApp.Models
{
    public class UserNotes
    {
        public Guid Id { get; set; }

        [ForeignKey("Notes")]
        public Guid NoteId { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }


        public UserNotes() 
        {
            Id = Guid.NewGuid();
        }


        public User User { get; set; }

        public  Notes Note { get; set; }


    }
}
