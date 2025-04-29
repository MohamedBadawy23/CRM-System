using ASP.Authentication.DTOs;

using AutoMapper;
using Core.Entities;
using Core.Repository;
using Core.Specification.Client_and_Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.Authentication.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IGenericRepository<Clients_Management> _clients;
        private readonly IGenericRepository<Orders_Management> _ordet;
        private readonly IMapper _mappe;
        private readonly UserManager<User> _userManager;
        private readonly JwtHandler _jwtHandler;
      

        public OrdersController(
            IGenericRepository<Clients_Management> Clients,
            IGenericRepository<Orders_Management> ordet,
            IMapper Mappe,
            UserManager<User> userManager,
            JwtHandler jwtHandler
           )
        {
            _clients = Clients;
            _ordet = ordet;
            _mappe = Mappe;
            _userManager = userManager;
            _jwtHandler = jwtHandler;
           
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin,Sales")]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrders([FromQuery] string? sort)
        {
            var spec = new ClientAndOrderSpec(sort);
            var orders = await _ordet.GetAllWithSpecification(spec);

            var orderDtos = orders.Select(order => new OrderResponseDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber, // Fix: Convert nullable int to string
                Price = order.Price ?? 0,
                Product = order.Product ,
                OrderDate = order.OrderDate ?? DateTime.Now,
                Status = order.Status
            });

            return Ok(orderDtos);
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin,Sales")]
        public async Task<ActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest("Order data is required");
            }

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return BadRequest("User ID not found or invalid.");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var order = _mappe.Map<Orders_Management>(orderDto);
            order.UserId = userId.ToString();
            await _ordet.AddAsync(order);

            var roles = await _userManager.GetRolesAsync(user);
            string token = _jwtHandler.CreateToken(user, roles, order.Id.ToString());

            return Ok(new
            {
                message = "Successful",
                token = token
            });
        }
      //[HttpPut]
      //[Authorize(Roles = "Admin,Sales")]
      //  public async Task<ActionResult> UpdateOrder([FromBody] OrderDto orderDto)
      //  {
      //      if (orderDto == null)
      //      {
      //          return BadRequest("Order data is required");
      //      }

      //      if (orderDto.Id == 0)
      //      {
      //          return BadRequest("Order ID is required");
      //      }

      //      var order = await _ordet.GetByIdAsync(orderDto.Id);
      //      if (order == null)
      //      {
      //          return NotFound(new { message = $"Order with ID {orderDto.Id} not found." });
      //      }

      //      var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
      //      if (userIdClaim == null)
      //      {
      //          return Unauthorized(new { message = "User ID not found in token." });
      //      }

      //      // Check if user is admin or the owner of the order
      //      var user = await _userManager.FindByIdAsync(userIdClaim.Value);
      //      if (user == null)
      //      {
      //          return Unauthorized(new { message = "User not found." });
      //      }

      //      var roles = await _userManager.GetRolesAsync(user);
      //      if (!roles.Contains("Admin") && userIdClaim.Value != order.UserId)
      //      {
      //          return Unauthorized(new { message = "You are not authorized to update this order." });
      //      }

      //      try
      //      {
      //          order.OrderNumber = orderDto.OrderNumber;
      //          order.Price = orderDto.Price;
      //          order.Product = orderDto.Product;
      //          order.Status = orderDto.Status;
      //          order.OrderDate = orderDto.OrderDate;

      //          await _ordet.UpdateAsync(order);
      //          return Ok(new { message = "Order updated successfully", order = order });
      //      }
      //      catch (Exception ex)
      //      {
      //          return StatusCode(500, new { message = "Error updating order", error = ex.Message });
      //      }
      //  }
        
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteOrder()
        {
            var orderIdClaim = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "orderId")?.Value;
            if (string.IsNullOrEmpty(orderIdClaim) || !int.TryParse(orderIdClaim, out int orderId))
            {
                return Unauthorized(new { message = "Order ID not found in token or invalid." });
            }

            var order = await _ordet.GetByIdAsync(orderId);
            if (order == null)
            {
                return NotFound(new { message = "Order not found." });
            }

            await _ordet.DeleteAsync(order);
            return Ok(new { message = "Order deleted successfully." });
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Sales")]
        public async Task<ActionResult> UpdateOrderStatus([FromBody] OrderDto orderDto)
        {
            var problemIdClaim = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == "orderId")?.Value;
            if (string.IsNullOrEmpty(problemIdClaim) || !int.TryParse(problemIdClaim, out int problemId))
            {
                return Unauthorized(new { message = "Problem ID not found in token or invalid." });
            }


            //  جلب المشكلة من قاعدة البيانات
            var problem = await _ordet.GetByIdAsync(problemId);
            if (problem == null)
            {
                return NotFound(new { message = "Problem not found." });
            }

            problem.OrderNumber = orderDto.OrderNumber;
            problem.Price = orderDto.Price;
            problem.Product = orderDto.Product;
            problem.Status = orderDto.Status;
            problem.OrderDate = orderDto.OrderDate;

            await _ordet.UpdateAsync(problem);
        

            return Ok(new { message = "Problem updated successfully." });
        }

    }
}
