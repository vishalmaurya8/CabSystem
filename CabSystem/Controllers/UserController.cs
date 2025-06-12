using AutoMapper;
using CabSystem.Data;
using CabSystem.DTOs;
using CabSystem.Exceptions;
using CabSystem.Models;
using CabSystem.Repositories; // Assuming IJwtTokenServices is here
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CAB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly CabSystemContext _dbContext;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper mapper;

        public UserController(CabSystemContext dbContext, IJwtTokenService jwtTokenService, IMapper mapper)
        {
            _dbContext = dbContext;
            _jwtTokenService = jwtTokenService;
            this.mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Role != "User" && dto.Role != "Driver")
                throw new BadRequestException("Invalid role. Must be 'User' or 'Driver'.");

            if (await _dbContext.Users.AnyAsync(u => u.Email == dto.Email))
                throw new BadRequestException("Email already in use.");

            if (await _dbContext.Users.AnyAsync(u => u.Phone == Convert.ToInt64(dto.Phone)))
                return BadRequest("Phone already in use.");

            var user = mapper.Map<User>(dto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.CreatedAt = DateTime.UtcNow;

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            if (dto.Role == "Driver")
            {
                if (string.IsNullOrEmpty(dto.LicenseNumber) ||
                    string.IsNullOrEmpty(dto.VehicleDetails) ||
                    string.IsNullOrEmpty(dto.Status))
                {
                    return BadRequest("Driver-specific fields are required.");
                }

                //var driver = new Driver
                //{
                //    UserId = user.UserId,
                //    LicenseNo = dto.LicenseNumber,
                //    VehicleDetails = dto.VehicleDetails,
                //    Status = dto.Status
                //};
                var driver = mapper.Map<Driver>(dto);
                driver.UserId = user.UserId; // Set manually after user is saved


                _dbContext.Drivers.Add(driver);
                await _dbContext.SaveChangesAsync();
            }

            return Ok("Registration successful.");
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegistrationDTO dto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    if (dto.Role != "User" && dto.Role != "Driver")
        //        throw new BadRequestException("Invalid role. Must be 'User' or 'Driver'.");

        //    // Check for existing email or phone
        //    if (await _dbContext.Users.AnyAsync(u => u.Email == dto.Email))
        //        throw new BadRequestException("Email already in use.");

        //    if (await _dbContext.Users.AnyAsync(u => u.Phone == dto.Phone))
        //        return BadRequest("Phone already in use.");


        //    // Create user
        //    var user = new User
        //    {
        //        Name = dto.Name,
        //        Email = dto.Email,
        //        PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        //        Phone = Convert.ToInt64(dto.Phone),
        //        Role = dto.Role,
        //        CreatedAt = DateTime.UtcNow
        //    };

        //    _dbContext.Users.Add(user);
        //    await _dbContext.SaveChangesAsync(); // UserId is generated here

        //    // If Driver, insert into Drivers table
        //    if (dto.Role == "Driver")
        //    {
        //        if (string.IsNullOrEmpty(dto.LicenseNumber) ||
        //            string.IsNullOrEmpty(dto.VehicleDetails) ||
        //            string.IsNullOrEmpty(dto.Status))
        //        {
        //            return BadRequest("Driver-specific fields are required.");
        //        }

        //        var driver = new Driver
        //        {
        //            UserId = user.UserId,
        //            LicenseNo = dto.LicenseNumber,
        //            VehicleDetails = dto.VehicleDetails,
        //            Status = dto.Status
        //        };

        //        _dbContext.Drivers.Add(driver);
        //        await _dbContext.SaveChangesAsync();
        //    }

        //    return Ok("Registration successful.");
        //}

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO dto)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            var token = _jwtTokenService.GenerateJwtToken(user.Email, user.Role, user.UserId);
            return Ok(new
            {
                message = "Login successful",
                token = token
            });
        }

    }
}