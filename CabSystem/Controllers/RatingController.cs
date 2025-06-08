using AutoMapper;
using CabSystem.DTOs;
using CabSystem.Models;
using CabSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CabSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RatingController : ControllerBase
    {
        private readonly IRatingRepository _ratingRepo;
        private readonly IMapper _mapper;

        public RatingController(IRatingRepository ratingRepo, IMapper mapper)
        {
            _ratingRepo = ratingRepo;
            _mapper = mapper;
        }


        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> AddRating([FromBody] CreateRatingDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _ratingRepo.GetRatingByRideIdAsync(dto.RideId);
            if (existing != null)
                return Conflict("Rating for this ride already exists.");

            var rating = _mapper.Map<Rating>(dto);
            var result = await _ratingRepo.AddRatingAsync(rating);
            return Ok(_mapper.Map<RatingDTO>(result));
        }


        [Authorize(Roles = "User")]
        [HttpPut("{rideId}")]
        public async Task<IActionResult> UpdateRating(int rideId, [FromBody] UpdateRatingDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _ratingRepo.UpdateRatingAsync(rideId, dto.Score, dto.Comments);
            if (updated == null)
                return NotFound("Rating not found for the given ride ID.");

            return Ok(_mapper.Map<RatingDTO>(updated));
        }


        [Authorize(Roles = "User")]
        [HttpGet("{rideId}")]
        public async Task<IActionResult> GetRatingByRideId(int rideId)
        {
            var rating = await _ratingRepo.GetRatingByRideIdAsync(rideId);
            if (rating == null)
                return NotFound("Rating not found.");

            return Ok(_mapper.Map<RatingDTO>(rating));
        }


        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> GetAllRatings()
        {
            var ratings = await _ratingRepo.GetAllRatingsAsync();
            var ratingDtos = _mapper.Map<IEnumerable<RatingDTO>>(ratings);
            return Ok(ratingDtos);
        }


        [Authorize(Roles = "Driver")]
        [HttpGet("driver/{driverId}/average")]
        public async Task<IActionResult> GetAverageRatingForDriver(int driverId)
        {
            var result = await _ratingRepo.GetAverageRatingForDriverAsync(driverId);

            if (result == null)
                return NotFound($"No ratings found for driver with ID {driverId}.");

            return Ok(result);
        }

    }
}
