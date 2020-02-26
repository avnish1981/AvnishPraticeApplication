using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTWEBAPI.Controllers
{
	[Route("api/[controller]")]
	public class AuthController : Controller
    {
		[HttpPost("token")]
        public IActionResult Token()
        {
			//string tokenString = "Test";
			var header = Request.Headers["Authorization"];
			if (header.ToString().StartsWith("Basic"))
			{
				var credvalue = header.ToString().Substring("Basic".Length).Trim();
				var usernameandpassword = Encoding.UTF8.GetString(Convert.FromBase64String(credvalue)); //admin:password
				var userandpass = usernameandpassword.Split(":");
				//check in db for Username and password
				if (userandpass[0] == "admin" && userandpass[1] == "password")
				{
					var claimdata = new[] { new Claim(ClaimTypes.Name, userandpass[0]) };
					var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ajdkjskjksjfksjfkjdfjdhddsadsasf"));
					var signIncred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
					var token = new JwtSecurityToken(
						issuer: "mysite.com",
						audience: "mysite.com", expires: DateTime.Now.AddMinutes(3),
						claims: claimdata,
						signingCredentials: signIncred
						);
					var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
					return Ok(tokenString);
				}
			}

			return BadRequest("Wrong Request");
        }
    }
}