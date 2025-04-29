using ASP.Authentication.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IGenericRepository<Product> _product;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly JwtHandler _jwtHandler;

        public ProductController(
            IGenericRepository<Product> product,
            IMapper mapper,
            UserManager<User> userManager,
            JwtHandler jwtHandler)
        {
            _product = product;
            _mapper = mapper;
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Sales")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _product.GetAllAsync();
            return Ok(products);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Sales")]
        public async Task<ActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Product data is required");
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

            var product = _mapper.Map<Product>(productDto);
            product.UserId = userId.ToString();
            await _product.AddAsync(product);

            var roles = await _userManager.GetRolesAsync(user);
            string token = _jwtHandler.CreateToken(user, roles, product.Id.ToString());

            return Ok(new
            {
                message = "Product created successfully",
                token = token
            });
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Sales")]
        public async Task<ActionResult> UpdateProduct([FromBody] ProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Product data is required");
            }

            if (productDto.Id == 0)
            {
                return BadRequest("Product ID is required");
            }

            var product = await _product.GetByIdAsync(productDto.Id);
            if (product == null)
            {
                return NotFound(new { message = $"Product with ID {productDto.Id} not found." });
            }

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }

            // Check if user is admin or the owner of the product
            var user = await _userManager.FindByIdAsync(userIdClaim.Value);
            if (user == null)
            {
                return Unauthorized(new { message = "User not found." });
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Admin") && userIdClaim.Value != product.UserId)
            {
                return Unauthorized(new { message = "You are not authorized to update this product." });
            }

            try
            {
                product.Name = productDto.Name;
                product.Description = productDto.Description;
                product.Price = productDto.Price;
                product.Stock = productDto.Stock;
                product.Category = productDto.Category;

                await _product.UpdateAsync(product);
                return Ok(new { message = "Product updated successfully", product = product });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating product", error = ex.Message });
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _product.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            await _product.DeleteAsync(product);
            return Ok(new { message = "Product deleted successfully." });
        }
    }
} 