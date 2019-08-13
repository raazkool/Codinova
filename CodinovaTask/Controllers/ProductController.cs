using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodinovaTask.DataEntities;
using CodinovaTask.Helpers;
using CodinovaTask.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CodinovaTask.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductController : Controller
    {
        private CodinovaContextEntities _dbContext;

        public ProductController(CodinovaContextEntities context)
        {
            _dbContext = context;
        }

        [HttpGet]
        [Route("GetProduct")]
        public async Task<ActionResult> GetProduct()
        {
            try
            {
                var result = await _dbContext.Products.ToListAsync();
                if (result != null)
                {
                    var response = new ReturnResultStatus(Status.Success, "Product List", "Product list", result);
                    return Ok(response.Data);
                }
                else
                {
                    var response = new ReturnResultStatus(Status.Failed, "Product Not Found", "Product not found");
                    return Ok(response.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
        [HttpGet("{id}")]
        [Route("GetProductById")]
        public async Task<ActionResult> GetProductById(int? id)
        {
            try
            {
                var result = await _dbContext.Products.Where(x => x.ProductId == id).FirstOrDefaultAsync();
                if (result != null)
                {
                    var response = new ReturnResultStatus(Status.Success, "Product List", "Product list", result);
                    return Ok(response.Data);
                }
                else
                {
                    var response = new ReturnResultStatus(Status.Failed, "Product Not Found", "Product not found");
                    return Ok(response.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("CreateProduct")]
        public async Task<ActionResult> CreateProduct([FromBody]ProductDetails productDetails)
        {
            try
            {
                if (productDetails != null)
                {
                    await _dbContext.Products.AddAsync(productDetails);
                    _dbContext.SaveChanges();
                    var response = new ReturnResultStatus(Status.Success, "Product Created", "Product created successfully.");
                    return new OkObjectResult(response.Message);
                }

                else
                {
                    var response = new ReturnResultStatus(Status.Failed, "Product Not Created", "Unable to create product.");
                    return new OkObjectResult(response.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("EditProduct")]
        public async Task<IActionResult> EditProduct([FromBody]ProductDetails productDetails)
        {
            try
            {
                var result = await _dbContext.Products.Where(x => x.ProductId == productDetails.ProductId).SingleOrDefaultAsync();
                if (result != null)
                {
                    result.ProductName = productDetails.ProductName;
                    result.Price = productDetails.Price;
                    result.Description = productDetails.Description;
                    result.ProductImage = productDetails.ProductImage;
                    result.Quantity = productDetails.Quantity;
                    _dbContext.Products.Update(result);
                    _dbContext.SaveChanges();
                    var response = new ReturnResultStatus(Status.Success, "Product Updated", "Product updated successfully.");
                    return new OkObjectResult(response.Message);
                }
                else
                {
                    var response = new ReturnResultStatus(Status.Failed, "Product Not found", "Product details not found.");
                    return Ok(response.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct([FromBody]ProductDetails productDetails)
        {
            try
            {
                var result = await _dbContext.Products.Where(x => x.ProductId == productDetails.ProductId).SingleOrDefaultAsync();
                if (result != null)
                {
                    _dbContext.Remove(result);
                    _dbContext.SaveChanges();
                    var response = new ReturnResultStatus(Status.Success, "Product Deleted", "Product deleted successfully.");
                    return Ok(response.Message);
                }
                else
                {
                    var response = new ReturnResultStatus(Status.Failed, "Product not found", "Unable to delete Product.");
                    return Ok(response.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
