using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DATN.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly AccountApplication _accountApplication;

        public RegisterController(AccountApplication application)
        {
            _accountApplication = application;
        }

        [HttpPost]
        public bool Register([FromBody] AccountViewModel model) {
            return _accountApplication.Register(model);
        }
    }
}