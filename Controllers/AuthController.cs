using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using learn.Configs;
using learn.InputModels;
using learn.Models;
using learn.Services;
using learn.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace learn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LearnContext _context;
        private readonly ConfigJwt _configJwt;

        public AuthController(
            LearnContext context,
            IOptions<ConfigJwt> configJwt
        )
        {
            _context = context;
            _configJwt = configJwt.Value;
        }

        [HttpPost("[action]")] // new
        public async Task<IActionResult> Login(
            [FromBody] // new
            AuthInputModel.Login input)
        {
            // check is _context null or not 
            if (_context.User == null) return BadRequest("context.user is null");

            // get user data from db
            var user = await _context.User
                    .Where(m => (m.Username == input.Username) &&
                        (m.Password == input.Password) &&
                        (!m.IsDelete))
                    .SingleOrDefaultAsync();

            // check if user found or not 
            if (user == null) return BadRequest("user not found, please check username or the password");

            // save user info to claim
            var claim = ClaimInputModel.Save(user);

            // create JWT token
            var Jwt = TokenService.Jwt(_configJwt, claim, 60);

            // generate refresh token
            var RefreshToken = TokenService.RefreshToken(16);

            // save refresh token and access token to model auth
            var res = AuthViewModel.Success(Jwt, RefreshToken);

            // update data user
            user.RefreshToken = RefreshToken;

            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();

                return Ok(res);
            }
            catch (System.Exception e)
            {

                return BadRequest($"exception occured while save the refresh token in the DB {e}");
            }
        }
    }
}
