using ASP.Authentication.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Repository;
using Core.Specification;
using Core.Specification.Client_and_Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Authentication.Controllers
{

    public class ClientController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtHandler _jwtHandler;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Clients_Management> _client;

        public ClientController(UserManager<User> userManager, JwtHandler jwtHandler, IMapper mapper, IGenericRepository<Clients_Management> Client)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
            _mapper = mapper;
            _client = Client;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients([FromQuery] string? sort)
        {
            // Update the specification type to match the expected type for Clients_Management
            var spec = new BaseSpecification<Clients_Management>
            {
                OrderBy = sort == "asc" ? (x => x.Name) : null,
                OrderByDesce = sort == "desc" ? (x => x.Name) : null
            };
            // var spec = new ClientAndOrderSpec(sort);

            var clients = await _client.GetAllWithSpecification(spec);
            return Ok(clients);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateClient([FromBody] Client clientDto)
        {
            if (clientDto == null)
            {
                return BadRequest("Client data is required");
            }

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return BadRequest("User ID not found or invalid.");
            }

            // البحث عن المستخدم
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var client = _mapper.Map<Clients_Management>(clientDto);
            client.UserId = userId.ToString(); // Set the UserId from the authenticated user
            await _client.AddAsync(client);

            var roles = await _userManager.GetRolesAsync(user);

            // ** إنشاء التوكن يحتوي على `ClientId` فقط**
            string token = _jwtHandler.CreateToken(user, roles, client.Id.ToString());

            // ** إرجاع الاستجابة مع التوكن فقط**
            return Ok(new
            {
                message = "Successful",
                token = token // التوكن يحتوي على `ClientId`
            });
        }
        //[HttpPut]
        //[Authorize(Roles = "Admin")]
        //public async Task<ActionResult> UpdateClient([FromBody] Client clientDto)
        //{
        //    if (clientDto == null)
        //    {
        //        return BadRequest("Client data is required");
        //    }

        //    // Validate the client ID
        //    if (clientDto.Id == 0)
        //    {
        //        return BadRequest("Client ID is required");
        //    }

        //    var client = await _client.GetByIdAsync(clientDto.Id);
        //    if (client == null)
        //    {
        //        return NotFound(new { message = $"Client with ID {clientDto.Id} not found." });
        //    }

        //    // Verify that the authenticated user owns this client
        //    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
        //    if (userIdClaim == null)
        //    {
        //        return Unauthorized(new { message = "User ID not found in token." });
        //    }

        //    if (userIdClaim.Value != client.UserId)
        //    {
        //        return Unauthorized(new { message = "You are not authorized to update this client." });
        //    }

        //    try
        //    {
        //        // Update only the fields that can be changed
        //        client.Name = clientDto.Name;
        //        client.Phone = clientDto.Phone;
        //        client.Email = clientDto.Email;
        //        client.Address = clientDto.Address;
        //        client.Date = clientDto.Date;

        //        await _client.UpdateAsync(client);
        //        return Ok(new { message = "Client updated successfully", client = client });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "Error updating client", error = ex.Message });
        //    }
        //}
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateClient([FromBody] Client clientDto)
        {
            var problemIdClaim = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "orderId")?.Value;
            if (string.IsNullOrEmpty(problemIdClaim) || !int.TryParse(problemIdClaim, out int problemId))
            {
                return Unauthorized(new { message = "Problem ID not found in token or invalid." });
            }


            //  جلب المشكلة من قاعدة البيانات
            var problem = await _client.GetByIdAsync(problemId);
            if (problem == null)
            {
                return NotFound(new { message = "Problem not found." });
            }

            //  تحديث المشكلة
            problem.Name = clientDto.Name;
            problem.Phone = clientDto.Phone;
            problem.Email = clientDto.Email;
            problem.Address = clientDto.Address;
            problem.Date = clientDto.Date;


            await _client.UpdateAsync(problem);


            return Ok(new { message = "Problem updated successfully." });

        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteClient()
        {
            var clientIdClaim = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "orderId")?.Value;
            if (string.IsNullOrEmpty(clientIdClaim) || !int.TryParse(clientIdClaim, out int clientId))
            {
                return Unauthorized(new { message = "Client ID not found in token or invalid." });
            }

            var client = await _client.GetByIdAsync(clientId);
            if (client == null)
            {
                return NotFound(new { message = "Client not found." });
            }

            await _client.DeleteAsync(client);
            return Ok(new { message = "Client deleted successfully." });
        }

    }
}
