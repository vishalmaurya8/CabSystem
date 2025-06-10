using AutoMapper;
using CabSystem.DTOs;
using CabSystem.Exceptions;
using CabSystem.Models;
using CabSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User")]
public class RatingController : ControllerBase
{
    private readonly IRatingRepository _ratingRepo;
    private readonly IMapper _mapper;

    public RatingController(IRatingRepository ratingRepo, IMapper mapper)
    {
        _ratingRepo = ratingRepo;
        _mapper = mapper;
    }

    private int GetUserIdFromToken()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User ID not found in token.");
        return int.Parse(userIdClaim.Value);
    }

    [HttpPost]
    public async Task<IActionResult> AddRating([FromBody] CreateRatingDTO dto)
    {
        if (!ModelState.IsValid) throw new BadRequestException("Invalid rating data");

        var userId = GetUserIdFromToken();

        // 🔐 Make sure this ride belongs to the user
        var isOwner = await _ratingRepo.IsRideOwnedByUserAsync(dto.RideId, userId);
        if (!isOwner)
            throw new UnauthorizedAccessException("You cannot rate a ride not booked by you.");

        // 🛑 Check for existing rating
        var existing = await _ratingRepo.GetRatingByRideIdAsync(dto.RideId);
        if (existing != null)
            throw new ConflictException("Rating already exists for this ride.");

        var rating = _mapper.Map<Rating>(dto);
        var result = await _ratingRepo.AddRatingAsync(rating);

        return Ok(_mapper.Map<RatingDTO>(result));
    }

    [HttpPut("{rideId}")]
    public async Task<IActionResult> UpdateRating(int rideId, [FromBody] UpdateRatingDto dto)
    {
        if (!ModelState.IsValid) throw new BadRequestException("Invalid update input");

        var userId = GetUserIdFromToken();

        var updated = await _ratingRepo.UpdateRatingAsync(rideId, userId, dto.Score, dto.Comments);
        if (updated == null)
            throw new NotFoundException("You cannot update rating for this ride.");

        return Ok(_mapper.Map<RatingDTO>(updated));
    }

    [HttpGet]
    public async Task<IActionResult> GetMyRatings()
    {
        var userId = GetUserIdFromToken();
        var ratings = await _ratingRepo.GetRatingsByUserIdAsync(userId);
        return Ok(_mapper.Map<IEnumerable<RatingDTO>>(ratings));
    }
}
