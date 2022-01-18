using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using URSAPI.DataAccessLayer;
using URSAPI.ModelDTO;

namespace URSAPI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }
        [HttpPost]
        [Route("api/Login/LoginValidations")]
        public dynamic CheckUserNameandPassword([FromBody] UserModel inputCredential)
        {
            var result = LoginAuthDAL.CheckUserCredential(inputCredential);
            if (result.Status)
            {
              
                UserModel userDetails = result.ResultOP;
                string token = GenerateJSONWebToken(userDetails);
                userDetails.Token = token;
                result.ResultOP = userDetails;
                
            }
            return result;
        }

        

        public string GenerateJSONWebToken(UserModel userInfo)
        {
            string _timezone = _config.GetValue<string>("TimeZone");
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(_timezone);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.FamilyName, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserId.ToString()),
                // new Claim("DateOfJoing", userInfo.DateOfJoing.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                   
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: dateTime_Indian.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateJWT()
        {
            string _timezone = _config.GetValue<string>("TimeZone");
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(_timezone);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var expiry = dateTime_Indian.AddMinutes(120);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: issuer, audience: audience, expires: expiry, signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }

        [HttpPost]
        [Route("api/Login/LoadMenu")]
        public dynamic LoadMenuBasedUser([FromBody] UserModel inputCredential)
        {
            string _timezone = _config.GetValue<string>("TimeZone");
            var result = LoginAuthDAL.LoadMenuBasedUser(inputCredential,_timezone);
            return result;
        }

        [HttpPost]
        [Route("api/Login/RoleandPermission")]
        public dynamic LoadButtonpermission([FromBody] RolePermissionDTO1 inputCredential)
        {
            string _timezone = _config.GetValue<string>("TimeZone");
            var result = LoginAuthDAL.LoadButtonpermission(inputCredential,_timezone);
            return result;
        }

        
        [HttpPost]
        [Route("api/Login/ChangePassword")]
        public dynamic ChangePassword([FromBody] Password inputCredential)
        {
            string _timezone = _config.GetValue<string>("TimeZone");
            var result = LoginAuthDAL.ChangePassword(inputCredential,_timezone);
            return result;
        }


    }
}