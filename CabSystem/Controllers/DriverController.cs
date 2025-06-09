using AutoMapper;
using CabSystem.DTOs;
using CabSystem.Exceptions;
using CabSystem.Models;
using CabSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CabSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Driver")]
    public class DriverController : ControllerBase
    {
        private readonly IDriverRepository _driverRepo;
        private readonly IRideRepository _rideRepo;
        private readonly IMapper _mapper;

        public DriverController(IDriverRepository driverRepo, IRideRepository rideRepo, IMapper mapper)
        {
            _driverRepo = driverRepo;
            _rideRepo = rideRepo;
            _mapper = mapper;
        }

        private int GetUserIdFromToken()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(id))
                throw new BadRequestException("User ID not found in token.");
            return int.Parse(id);
        }

        // GET /api/driver/me
        [HttpGet("me")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetUserIdFromToken();
            var driver = await _driverRepo.GetDriverByUserIdAsync(userId);
            if (driver == null)
                throw new NotFoundException("Driver profile not found.");

            var dto = _mapper.Map<DriverProfileDTO>(driver);
            return Ok(dto);
        }

        // PUT /api/driver/status
        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateDriverStatusDTO dto)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Invalid status input.");

            if (string.IsNullOrWhiteSpace(dto.Status))
                throw new BadRequestException("Status cannot be null or empty");

            var userId = GetUserIdFromToken();
            await _driverRepo.UpdateDriverStatusAsync(userId, dto.Status);
            return Ok("Driver status updated.");
        }

        // GET /api/driver/stats
        [HttpGet("stats")]
        public async Task<IActionResult> GetDriverStats()
        {
            var userId = GetUserIdFromToken();
            var stats = await _driverRepo.GetDriverStatsAsync(userId);
            return Ok(stats);
        }
    }
}