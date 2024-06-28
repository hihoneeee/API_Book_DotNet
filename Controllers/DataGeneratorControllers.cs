using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestWebAPI.Repositories;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataGeneratorControllers : ControllerBase
    {
        private readonly FakeDataRepositories _fakeDataRepo;

        public DataGeneratorControllers(FakeDataRepositories fakeDataRepo) {
            _fakeDataRepo = fakeDataRepo;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateData()
        {
            await _fakeDataRepo.GenerateAndInsertData();
            return Ok(new { message = "Data has been generated and inserted." });
        }
    }
}
