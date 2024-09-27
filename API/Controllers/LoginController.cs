using API.Context;
using API.Models;
using API.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public IConfiguration configuration;
        private readonly MGFContext mGFContext;
        public LoginController(MGFContext mGFContext, IConfiguration configuration)
        {
            this.mGFContext = mGFContext;
            this.configuration = configuration;
        }

        [Route("User")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            MsUser? msUser = await mGFContext.MsUsers.Where(obj => obj.UserName == loginVM.EmailOrUsername && obj.Password == loginVM.Password && obj.IsActive == true).FirstOrDefaultAsync();
            if (msUser != null) {
            // Implement JWT
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, msUser.UserName),
                    new Claim(ClaimTypes.Sid, msUser.UserId.ToString())
                };


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                        configuration["Jwt:Issuer"],
                        configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(60),
                        signingCredentials: signIn
                    );
                var idToken = new JwtSecurityTokenHandler().WriteToken(token);
                claims.Add(new Claim("TokenSecurity", idToken.ToString()));

                HttpContext.Session.SetString("JWToken", idToken);

                return Ok(new { Message = "Login Sukses", Token = idToken });
            }
			return BadRequest(new { Message = "Password salah", Token = "" });

          
        }

        [Route("register-user")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser(LoginVM loginVM)
        {
            MsUser? msUser = await mGFContext.MsUsers.Where(obj => obj.UserName == loginVM.EmailOrUsername ).FirstOrDefaultAsync();
            if (msUser != null)
            {
                return BadRequest(new { Message = "User sudah terdaftar sebelumnya."});
            
            }
            MsUser user = new MsUser();
            user.UserName = loginVM.EmailOrUsername;
            user.Password = loginVM.Password;
            user.IsActive = true;

            await mGFContext.MsUsers.AddAsync(user);
            await mGFContext.SaveChangesAsync();
            
            return Ok(new { Message = "User berhasil terdaftar." });


        }


    }
}
