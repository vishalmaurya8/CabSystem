using AutoMapper;
using CabSystem.DTOs;
using CabSystem.Exceptions;
using CabSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CabSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository customerRepository;
        //private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerRepository customerRepository, IMapper mapper)
        {
            this.customerRepository = customerRepository;
            _mapper = mapper;
        }

        private int GetUserIdFromToken()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new BadRequestException("User ID not found in token.");
            return int.Parse(userId);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetUserIdFromToken();
            var user = await customerRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User not found.");

            var dto = _mapper.Map<UserProfileDTO>(user);
            return Ok(dto);
        }

        [HttpGet("rides")]
        public async Task<IActionResult> GetMyRides()
        {
            var userId = GetUserIdFromToken();
            var rides = await customerRepository.GetRidesByUserIdAsync(userId);

            if (rides == null || !rides.Any())
                throw new NotFoundException("No rides found for this user.");

            var dto = _mapper.Map<IEnumerable<UserRideDTO>>(rides);
            return Ok(dto);
        }

        [HttpGet("ratings")]
        public async Task<IActionResult> GetMyRatings()
        {
            var userId = GetUserIdFromToken();
            var ratings = await customerRepository.GetRatingsByUserIdAsync(userId);

            if (ratings == null || !ratings.Any())
                throw new NotFoundException("No ratings found for this user.");

            var dto = _mapper.Map<IEnumerable<UserRatingDTO>>(ratings);
            return Ok(dto);
        }
    }
}
