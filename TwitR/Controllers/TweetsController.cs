using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TwitR.Models.Concrete;
using TwitR.Repositories.Abstract;

namespace TwitR.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private readonly IEntityRepository<User> _userRepository;
        public TweetsController(IEntityRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public IActionResult AddUser(string model)
        {
            User addedUser = JsonConvert.DeserializeObject<User>(model);
            addedUser.CreatedDate = DateTime.Now;
            _userRepository.AddAsync(addedUser).Wait();
            return Content("evet");
        }
    }
}
