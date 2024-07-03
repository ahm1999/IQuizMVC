using Microsoft.AspNetCore.Identity;

namespace IQuizMVC.Models
{
    public class User:IdentityUser
    {
        public ICollection<Quiz> Quizzes { get; set; }
    }
}
