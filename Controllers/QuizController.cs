using IQuizMVC.Data;
using IQuizMVC.DTO;
using IQuizMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks.Dataflow;

namespace IQuizMVC.Controllers
{
    public class QuizController : Controller
    {
        private readonly ILogger<QuizController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManeger;
        public QuizController(ILogger<QuizController> logger, ApplicationDbContext context, UserManager<User> userManager)
        {
            _logger = logger;
            _context = context;
            _userManeger = userManager;
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        [Route("{controller}/Create_p")]
        public async Task<IActionResult> CreateP([FromForm] QuizDTO userData)
        {
            string userId = _userManeger.GetUserId(User);
            Guid quizId = Guid.NewGuid();
            Quiz quiz = new Quiz
            {
                Id = quizId,
                Title = userData.Title,
                Description = userData.Description,
                UserId = userId,
                Accessability = userData.Accessability

            };
            await _context.Quizzes.AddAsync(quiz);
            await _context.SaveChangesAsync();
            return Redirect($"EditQuestion/{quizId.ToString()}");
        }


        [Authorize]
        [Route("{controller}/EditQuestion/{QuizId:guid}")]
        public async Task<IActionResult> EditQuestion([FromRoute] Guid QuizId) {
            Quiz quiz = await _context.Quizzes.Include(q => q.Questions).FirstOrDefaultAsync(q => q.Id == QuizId && q.UserId == _userManeger.GetUserId(User));
            if (quiz == null) {
                _logger.LogInformation("this is error state");
                return BadRequest();
            }
            return View(quiz);
        }

        [HttpPost]
        [Authorize]
        [Route("{controller}/AddQuestion/{QuizId:guid}")]
        public async Task<IActionResult> AddQuestion([FromRoute] Guid QuizId, [FromForm] QuestionDTO userData) {
            Quiz quiz = await _context.Quizzes.Include(q => q.Questions).FirstOrDefaultAsync(q => q.Id == QuizId && q.UserId == _userManeger.GetUserId(User));
            if (quiz == null)
            {
                _logger.LogInformation("this is error state");
                return BadRequest();
            }
            Question question = new Question()
            {
                Id = Guid.NewGuid(),
                Content = userData.Content,
                Answer = userData.Answer,
                QuizId = quiz.Id,

            };

            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
            return Redirect($"/Quiz/EditQuestion/{quiz.Id.ToString()}");

        }

        [HttpPost]
        [Authorize]
        [Route("{controller}/DeleteQuestion/{QuizId:guid}/{QuestionId:guid}")]
        public async Task<IActionResult> DeleteQuestion([FromRoute] Guid QuizId, [FromRoute] Guid QuestionId) {

            if (!await _context.Quizzes.AnyAsync(q => q.Id == QuizId && q.UserId == _userManeger.GetUserId(User))) {
                return Unauthorized();
            }
            Question q = new Question()
            {
                Id = QuestionId,
            };

            _context.Questions.Remove(q);
            await _context.SaveChangesAsync();

            return Ok("question deleted");
        
        }

        [Route("{controller}/Play/{QuizId:guid}")]
        public async Task<IActionResult> Play([FromRoute] Guid QuizId) {

            

            //var quizJson = JsonSerializer.Serialize(quiz);

            return View();
        }

        [Route("{controller}/{QuizId:guid}")]
        public async Task<IActionResult> GetQuiz([FromRoute] Guid QuizId) {
            _logger.LogInformation(QuizId.ToString());
            Quiz quiz = await _context.Quizzes.Include(q => q.Questions).FirstOrDefaultAsync(q => q.Id == QuizId);
            if (quiz is null)
            {
                _logger.LogInformation("1");
                return BadRequest();

            }
            if (quiz.Accessability == "private")
            {
                _logger.LogInformation("2");
                if (!(quiz.UserId == _userManeger.GetUserId(User)))
                {
                    _logger.LogInformation("3");
                    return BadRequest();
                }
            }

            return Ok(quiz);

        }
        [Authorize]
        public async Task<IActionResult> YourQuizes() {
            List<Quiz> quizes = await _context.Quizzes.Where(q => q.UserId == _userManeger.GetUserId(User)).ToListAsync();
            return View(quizes);
        }
            




    }
}