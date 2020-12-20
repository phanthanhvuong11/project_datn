using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DATN.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountApplication _accountApplication;

        public AccountController(AccountApplication application)
        {
            _accountApplication = application;
        }

        [HttpGet("Terminal")]
        public string GetTerminalContent() {
            return _accountApplication.GetTerminalContent();
        }

        [HttpGet]
        public async Task<List<AccountViewModel>> GetAccounts() {
            return await _accountApplication.GetAccounts();
        }

        [HttpPost("Update")]
        public async Task<bool> UpdateAccount([FromBody] AccountViewModel model) {
            return await _accountApplication.UpdateAccount(model);
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> DeleteAccount([FromRoute] int id) {
            return await _accountApplication.DeleteAccount(id);
        }
    }
}