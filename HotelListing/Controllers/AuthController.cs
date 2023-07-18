using AutoMapper;
using HotelListing.DTOs.UserDTO;
using HotelListing.Services.AuthService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<AuthController> _logger;
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;

    public AuthController(UserManager<User> userManager, ILogger<AuthController> logger, IMapper mapper, IAuthService authService)
    {
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
        _authService = authService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUserDTO) 
    {
        _logger.LogInformation($"Registration attempt for {registerUserDTO.Email}");
        if (!ModelState.IsValid) // If the addUserDTO failed any validation annotations
        {
            return BadRequest(ModelState);
        }
        try
        {
            var user = _mapper.Map<User>(registerUserDTO);
            user.UserName = registerUserDTO.Email;
            var result = await _userManager.CreateAsync(user, registerUserDTO.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            await _userManager.AddToRolesAsync(user, registerUserDTO.Roles);
            return Ok($"User registration successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {nameof(Register)}");
            return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500);
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
    {
        _logger.LogInformation($"Login attempt for {loginUserDTO.Email}");
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            if (!await _authService.ValidateUser(loginUserDTO))
            {
                return Unauthorized("Invalid username or password");
            }

            return Accepted(new { Token = await _authService.CreateToken() });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {nameof(Login)}");
            return Problem($"Something went wrong in the {nameof(Login)}", statusCode: 500);
        }
    }
}
