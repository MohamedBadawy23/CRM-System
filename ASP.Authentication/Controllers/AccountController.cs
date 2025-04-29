using Core.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ASP.Authentication.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ASP.Authentication.Controllers;
[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly JwtHandler _jwtHandler;

    public AccountController(UserManager<User> userManager, IMapper mapper, JwtHandler jwtHandler)
    {
        _userManager = userManager;
        _mapper = mapper;
        _jwtHandler = jwtHandler;
    }

    //[HttpPost("register")]
    //public async Task<ActionResult<RegisterationResponseDto>> RegisterUser(
    //    [FromBody] UserForRegisterationDto model)
    //{
    //    var user = _mapper.Map<User>(model);
    //    var result = await _userManager.CreateAsync(user, model.Password!);

    //    if (!result.Succeeded)
    //    {
    //        var errors = result.Errors.Select(e => e.Description);
    //        return BadRequest(new RegisterationResponseDto(false, errors.ToList()));
    //    }

    //    await _userManager.AddToRoleAsync(user, "Visitor");

    //    return Ok(new RegisterationResponseDto(true));

    //}

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponseDto>> AuthenticateUser(
        [FromBody] UserForAuthenticationDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email!);
        if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password!))
            return Unauthorized(new AuthenticationResponseDto(false, "Invalid Authentication"));
        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtHandler.CreateToken(user, roles, null); // Pass null for the 'orderId' parameter
        return Ok(new AuthenticationResponseDto(true, token));
    }

    

    [HttpPost("register-staff")]
    public async Task<ActionResult<RegisterationResponseDto>> RegisterStaff(
        [FromBody] UserForRegisterationDto model)
    {
        var user = _mapper.Map<User>(model);
        var result = await _userManager.CreateAsync(user, model.Password!);

        var usere = new User()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            UserName = model.Email!.Split('@')[0]
      
            // Add other properties as needed

        };
        string email = model.Email; // الإيميل اللي المستخدم دخلّه
        string username = email.Split('@')[0]; // ناخد الجزء قبل @


        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new RegisterationResponseDto(false, errors.ToList()));
        }

        var roleManager = HttpContext.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();
        
        // Create Admin role if it doesn't exist
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }


        // Create Sales role if it doesn't exist
        if (!await roleManager.RoleExistsAsync("Sales"))
        {
            await roleManager.CreateAsync(new IdentityRole("Sales"));
        }

        // Add the selected role based on the user's choice
        if (model.Role?.ToLower() == "admin")
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }
        else if (model.Role?.ToLower() == "sales")
        {
            await _userManager.AddToRoleAsync(user, "Sales");
        }
        else
        {
            return BadRequest(new RegisterationResponseDto(false, new List<string> { "Invalid role. Please choose either 'Admin' or 'Sales'." }));
        }

        return Ok(new RegisterationResponseDto(true));
    }

    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames = { "Admin", "Sales" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
    [Authorize]
    [HttpGet("Profile")]
    public async Task<IActionResult> GetUserProfileAsync()
    {
        var userId = User.FindFirstValue("id");

        if (userId is null)
            return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);

        // Assuming the user's role needs to be fetched from the UserManager
        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault(); // Get the first role if available

        var response = new UserForRegisterationDto
        {
            Email = user?.Email!,
            Role = role, // Assign the fetched role here
       
            FirstName = user?.FirstName,
            LastName = user?.LastName,
         

        };

        return Ok(response);
    }

}
