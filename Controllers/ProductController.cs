using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private static readonly List<Product> Products = new()
        {
            new Product { Id = "1", Name = "Cocacola", Price = 1.2 },
            new Product { Id = "2", Name = "Pepsi", Price = 1.0 },
            new Product { Id = "3", Name = "Fanta", Price = 0.9 },
            new Product { Id = "4", Name = "Sprite", Price = 0.95 },
            new Product { Id = "5", Name = "7Up", Price = 0.85 }
        };

        // GET: api/products
        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            return Ok(Products);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound(new { message = $"Không tìm thấy sản phẩm với Id = {id}" });

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] Product product)
        {
            if (product == null)
                return BadRequest(new { message = "Dữ liệu sản phẩm không hợp lệ" });

            // Kiểm tra Id đã tồn tại chưa
            bool idExists = Products.Any(p => p.Id == product.Id);
            if (idExists)
                return Conflict(new { message = $"Sản phẩm với Id = {product.Id} đã tồn tại" });

            Products.Add(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Update(string id, [FromBody] Product updatedProduct)
        {
            if (updatedProduct == null)
                return BadRequest(new { message = "Dữ liệu sản phẩm không hợp lệ" });

            var existingProduct = Products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
                return NotFound(new { message = $"Không tìm thấy sản phẩm với Id = {id}" });

            existingProduct.Name = updatedProduct.Name;
            existingProduct.Price = updatedProduct.Price;

            return Ok(existingProduct);
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(string id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound(new { message = $"Không tìm thấy sản phẩm với Id = {id}" });

            Products.Remove(product);
            return Ok(new { message = $"Đã xóa sản phẩm '{product.Name}' thành công" });
        }
    }
}
