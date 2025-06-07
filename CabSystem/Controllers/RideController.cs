// Controllers/RideController.cs
using AutoMapper;
using CabSystem.DTOs;
using CabSystem.Models;
using CabSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CabSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideController : ControllerBase
    {
        private readonly IRideRepository _rideRepository;
        private readonly IMapper _mapper;

        public RideController(IRideRepository rideRepository, IMapper mapper)
        {
            _rideRepository = rideRepository;
            _mapper = mapper;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRidesByUserId(int userId)
        {
            var rides = await _rideRepository.GetRidesByUserIdAsync(userId);
            var rideDtos = _mapper.Map<IEnumerable<RideDTO>>(rides);
            return Ok(rideDtos);
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookRide([FromBody] CreateRideDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ride = _mapper.Map<Ride>(dto);
            var result = await _rideRepository.BookRideAsync(ride);
            var rideDto = _mapper.Map<RideDTO>(result);
            return Ok(rideDto);
        }

        [HttpPost("complete/{rideId}")]
        public async Task<IActionResult> CompleteRide(int rideId)
        {
            var ride = await _rideRepository.CompleteRideAsync(rideId);

            if (ride == null)
                return NotFound($"Ride with ID {rideId} not found.");

            var response = _mapper.Map<CompleteRideResponseDTO>(ride);
            return Ok(response);
        }

    }
}
