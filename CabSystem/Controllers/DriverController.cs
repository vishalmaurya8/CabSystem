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

        [Authorize(Roles = "Driver")]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateDriverProfileDTO dto)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Invalid profile data.");

            var userId = GetUserIdFromToken(); // Get userId from JWT
            await _driverRepo.UpdateDriverProfileAsync(userId, dto);
            return Ok(new
            {
                message = "Profile updated successfully."
            });
        }



        /*[HttpGet("available-rides")]
        public async Task<IActionResult> GetAvailableRides()
        {
            var userId = GetUserIdFromToken();

            var driverId = await _driverRepo.GetDriverIdByUserIdAsync(userId);
            if (driverId == null)
                throw new NotFoundException("Driver profile not found");

            var rides = await _rideRepo.GetRequestedRidesByDriverIdAsync(driverId.Value);

            if (rides == null || !rides.Any())
                throw new NotFoundException("No rides available for assignment.");

            var result = _mapper.Map<List<RequestedRideDTO>>(rides);
            return Ok(result);
        }*/

        /*[HttpPost("accept-ride")]
        public async Task<IActionResult> AcceptRide([FromBody] AcceptRideDTO dto)
        {
            var driverId = GetUserIdFromToken(); // extract from JWT claims
            var updatedRide = await _rideRepo.AcceptRideAsync(dto.RideId, driverId);

            if (updatedRide == null)
                throw new NotFoundException("Ride not available or already assigned.");

            return Ok(new { message = "Ride accepted successfully", RideId = updatedRide.RideId });
        }*/

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token.");
            return int.Parse(userIdClaim.Value);
        }
    }
}