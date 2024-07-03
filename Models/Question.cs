using System.ComponentModel.DataAnnotations.Schema;

namespace IQuizMVC.Models
{
    public class Question
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public string Answer { get; set; }

        [ForeignKey("Quiz")]
        public Guid QuizId { get; set; }
    }
}
