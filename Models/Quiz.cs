using System.ComponentModel.DataAnnotations.Schema;

namespace IQuizMVC.Models
{
    public class Quiz
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<Question> Questions { get; set; }

        public string Accessability { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
