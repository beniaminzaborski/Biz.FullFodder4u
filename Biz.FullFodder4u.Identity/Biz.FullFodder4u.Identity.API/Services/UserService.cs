using Biz.FullFodder4u.Identity.API.DTOs;
using Biz.FullFodder4u.Identity.API.Entities;
using Biz.FullFodder4u.Identity.API.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Biz.FullFodder4u.Identity.API.Services;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;

    private static readonly IList<User> users = new List<User>
    { 
        new User { Id = new Guid("C4779B70-6D3B-4FA3-9B7D-03FD7A28A9E7"), Email = "admin@fodder4u.com", PasshowrdHash = "" }
    };

    public UserService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SignUp(SignUpDataDto payload)
    {
        if (payload is null)
        {
            throw new ValidationException("Empty data");
        }

        if (string.IsNullOrEmpty(payload.Email))
        {
            throw new ValidationException("E-mail is empty");
        }

        if (string.IsNullOrEmpty(payload.Password) || payload.Password.Length < 8)
        {
            throw new ValidationException("Password must have at least 8 characters length");
        }

        if (payload.Password != payload.Password2)
        {
            throw new ValidationException("Passwords are not equal");
        }

        var user = users.FirstOrDefault(u => u.Email == payload.Email);
        if (user != null)
        {
            throw new ValidationException("User already exists");
        }

        users.Add(new User { Id = Guid.NewGuid(), Email = payload.Email, PasshowrdHash = "" });
    }

    public async Task<string> SignIn(SignInDataDto payload)
    {
        if (payload is null)
        {
            throw new ValidationException("Empty data");
        }

        if (string.IsNullOrEmpty(payload.Email))
        {
            throw new ValidationException("E-mail is empty");
        }

        if (string.IsNullOrEmpty(payload.Password) || payload.Password.Length < 8)
        {
            throw new ValidationException("Password must have at least 8 characters length");
        }

        var user = users.FirstOrDefault(u => u.Email == payload.Email);
        if (user == null)
        {
            throw new NotFoundException("User not exists");
        }

        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim("Email", payload.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: signIn);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
