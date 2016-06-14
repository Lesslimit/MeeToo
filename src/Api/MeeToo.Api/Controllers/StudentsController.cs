using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MeeToo.DataAccess.DocumentDb;

namespace MeeToo.Api.Controllers
{
    [Route("api/v1.1/[controller]")]
    public class StudentsController : Controller
    {
        private readonly IStorage storage;

        public StudentsController(IStorage storage)
        {
            this.storage = storage;
        }

        [Route("all")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var db = storage.Db();//.CollectionAsync<Student>();

            await Task.Delay(500);

            return Ok(db.ToString());
        }
    }
}
