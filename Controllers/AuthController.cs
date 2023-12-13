using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using e_commerce_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.Scripting;
using System.Security.Cryptography;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly EcommerceContext _context;

    public AuthController(IConfiguration configuration, EcommerceContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Employee model)
    {
        var existingUser = await _context.Employees.FirstOrDefaultAsync(u => u.Email == model.Email);

        if (existingUser != null)
        {
            return Conflict(new { Message = "Email is already in use" });
        }

        var user = new Employee
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            BirthDate = model.BirthDate,
            Password = model.Password, 
        };

        _context.Employees.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Registration successful" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Employee model)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

        if (employee == null)
        {
            return Unauthorized(new { Message = "Invalid credentials" });
        }

        // Generate JWT token
        var token = GenerateJwtToken(employee);

        return Ok(new { Token = token });
    }

    private string GenerateJwtToken(Employee user)
    {
        var claims = new List<Claim>
        {
        new Claim("id", user.Id.ToString()),
        new Claim("name", $"{user.FirstName}{user.LastName}"),
        new Claim("email", user.Email ?? ""),
        new Claim("phoneNumber", user.PhoneNumber ?? ""),
         new Claim("employeeImage", user.EmployeeImage ?? ""),
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["JWT:ExpiresInHours"]));
        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}