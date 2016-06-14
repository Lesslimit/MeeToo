using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MeeToo.Api.Models;
using MeeToo.DataAccess.DocumentDb;
using MeeToo.Domain;

namespace MeeToo.Api.Controllers
{
    [Authorize]
    [Route("api/v1.1/tests")]
    public class TestsController : Controller
    {
        private readonly IStorage storage;

        public TestsController(IStorage storage)
        {
            this.storage = storage;
        }

        [Route("getAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tests = await storage.Db().CollectionAsync<Test>();

            return Ok(tests.Query().AsEnumerable());
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]TestCreationModel model)
        {
            var tests = await storage.Db().CollectionAsync<Test>();

            var newTest = new Test
            {
                Id = Guid.NewGuid().ToString(),
                Duration = model.Duration
            };

            foreach (var questionModel in model.Questions)
            {
                var newQuestion = new Question
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = questionModel.Title
                };

                foreach (var option in questionModel.Options)
                {
                    newQuestion.Options.Add(new Option
                    {
                        Id = Guid.NewGuid().ToString(),
                        Text = option.Text,
                        IsCorrect = option.IsCorrect
                    });
                }

                newTest.Questions.Add(newQuestion);
            }

            await tests.AddAsync(newTest);

            return Created("", "");
        }
    }
}
