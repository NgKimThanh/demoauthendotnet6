using DemoWebApiDotNet6.DTO.Auth;
using DotNet6Authen.DTO;
using DotNet6Authen.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace DotNet6Authen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DemoAuthenContext _demoAuthencontext;
        private readonly IConfiguration _configuration;

        public AuthController(DemoAuthenContext demoAuthencontext, IConfiguration configuration)
        {
            _demoAuthencontext = demoAuthencontext;
            _configuration = configuration;
        }

        #region Đăng ký
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = await _demoAuthencontext.User.FirstOrDefaultAsync(c => c.UserName == request.UserName);
            if (user == null)
            {
                user = new User();
                user.UserName = request.UserName;
                _demoAuthencontext.User?.Add(user);
            }
            user.Password = request.Password;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Role = request.Role;
            user.PasswordHash = Convert.ToBase64String(passwordHash);
            user.PasswordSalt = Convert.ToBase64String(passwordSalt);
            await _demoAuthencontext.SaveChangesAsync();

            return Ok(user);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        #endregion Đăng ký

        #region Đăng nhập
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDTO request)
        {
            string token = "";
            try
            {
                #region Verify user
                // Lấy thông tin user
                var user = await _demoAuthencontext.User.FirstOrDefaultAsync(c => c.UserName == request.UserName);
                if (user == null || user.UserName != request.UserName)
                {
                    return BadRequest("User not found.");
                }

                // Lấy passwordHash & passwordSalt từ user
                var passwordHash = Convert.FromBase64String(user.PasswordHash);
                var passwordSalt = Convert.FromBase64String(user.PasswordSalt);

                // Verify Password từ passwordHash & passwordSalt
                if (!VerifyPasswordHash(request.Password, passwordHash, passwordSalt))
                {
                    return BadRequest("Wrong password.");
                }
                #endregion

                #region Tạo token
                // Tạo token
                token = CreateToken(user);
                // Tạo refresh token
                var refreshToken = GenerateRefreshToken();
                SetRefreshToken(refreshToken);

                user.RefreshToken = refreshToken.Token;
                user.TokenCreated = refreshToken.Created;
                user.TokenExpires = refreshToken.Expires;
                await _demoAuthencontext.SaveChangesAsync();
                #endregion

            }
            catch (Exception ex)
            {
            }

            return Ok(token);
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        #endregion Đăng nhập

        #region Refresh token
        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken(string userName)
        {
            // Lấy thông tin user
            var user = await _demoAuthencontext.User.FirstOrDefaultAsync(c => c.UserName == userName);
            if (user == null || user.UserName != userName)
            {
                return BadRequest("User not found.");
            }

            var refreshToken = Request.Cookies["refreshToken"];
            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
            await _demoAuthencontext.SaveChangesAsync();

            return Ok(token);
        }
        #endregion Refresh token

        private string CreateToken(User user)
        {
            // Tạo ra danh sách các quyền có trong 1 token
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // Tạo 1 key
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:SecretKey").Value));

            // Ký mã jwt bằng thuật toán SecurityAlgorithms.HmacSha512Signature
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Tạo mã jwt
            var token = new JwtSecurityToken(
                claims: claims, // Danh sách quyền
                expires: DateTime.Now.AddDays(1), // Thời hạn
                signingCredentials: creds // Ký mã jwt
                );

            // Tạo chuỗi jwt
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7).ToUniversalTime(),
                Created = DateTime.Now.ToUniversalTime()
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        }
    }
}
