using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CodinovaTask.DataEntities;
using CodinovaTask.Helpers;
using CodinovaTask.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CodinovaTask.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OrderController : Controller
    {
        private CodinovaContextEntities _dbContext;

        public OrderController(CodinovaContextEntities context)
        {
            _dbContext = context;
        }

        [HttpPost]
        [Route("PlaceOrder")]
        public IActionResult PlaceOrder([FromBody]OrderDetails orderDetails)
        {
            try
            {
                if (orderDetails != null)
                {
                    var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                    orderDetails.OrderBy = int.Parse(UserId);
                    var product = _dbContext.Products.SingleOrDefault(x => x.ProductId == orderDetails.ProductId);
                    var availableQuantity = product.Quantity;
                    var appliedQuantity = orderDetails.Quantity;

                    if (appliedQuantity > availableQuantity)
                    {
                        var response = new ReturnResultStatus(Status.Failed, "Product Quantity Applied",
                            "Product available quantity is less then product applied quantity.");
                        return Ok(response.Message);
                    }

                    else if (appliedQuantity == availableQuantity)
                    {
                        _dbContext.Orders.Add(orderDetails);
                        _dbContext.SaveChanges();
                        var data = _dbContext.Products.SingleOrDefault(x => x.ProductId == orderDetails.ProductId);
                        _dbContext.Remove(data);
                        _dbContext.SaveChanges();
                        var response = new ReturnResultStatus(Status.Success, "Order Purchase", "Order placed successfully.");
                        return Ok(response.Message);
                    }
                    else if (appliedQuantity < availableQuantity)
                    {
                        _dbContext.Orders.Add(orderDetails);
                        _dbContext.SaveChanges();
                        var data = _dbContext.Products.SingleOrDefault(x => x.ProductId == orderDetails.ProductId);
                        var CurrentQuantity = availableQuantity - appliedQuantity;
                        data.Quantity = CurrentQuantity;
                        _dbContext.Update(data);
                        _dbContext.SaveChanges();
                        var response = new ReturnResultStatus(Status.Success, "Order Purchase", "Order placed successfully.");
                        return Ok(response.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return null;
        }
    }
}
