using MVCAPIWithBearerToken.DTOs;
using MVCAPIWithBearerToken.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MVCAPIWithBearerToken.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class OrderController : ApiController
    {
        private readonly AppDbContext db = new AppDbContext();

        public IHttpActionResult GetOrder()
        {
            var orders = db.Orders.ToList();
            return Ok(orders);
        }
        public IHttpActionResult GetOrderById(int id)
        {
            var order = (from o in db.Orders
                         where o.OrderId == id
                         select new
                         {
                             o.OrderId,
                             o.OrderNo,
                             o.CustomerName,
                             o.IsPaid,
                             o.ImageUrl
                         }).FirstOrDefault();
            var orderItems = (from o in db.OrderedItems
                              join p in db.Products on o.ProductId equals p.ProductId
                              where o.OrderId == id
                              select new { o.OrderId, o.OrderedItemId, o.ProductId, p.ProductName, o.Price, o.Quantity });
            return Ok(new { order, orderItems });
        }
        public IHttpActionResult DeleteOrder(int id)
        {
            var order = db.Orders.Find(id);
            var orderItems = db.OrderedItems.Where(o => o.OrderId == id).ToList();
            foreach (var item in orderItems)
            {
                db.OrderedItems.Remove(item);
            }
            db.Orders.Remove(order);
            db.SaveChanges();
            return Ok("Order and related items deleted successfully");
        }
        public IHttpActionResult PostOrder(OrderRequest request)
        {
            if (request.Order == null)
                return BadRequest("Order Data is missing");
            Order obj = request.Order;
            byte[] imageFile = request.ImageFile;
            if (imageFile != null && imageFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                string filePath = Path.Combine("~/Images/", fileName);
                File.WriteAllBytes(HttpContext.Current.Server.MapPath(filePath), imageFile);
                obj.ImageUrl = filePath;
            }
            Order order = new Order
            {
                OrderNo = obj.OrderNo,
                OrderDate = obj.OrderDate,
                CustomerName = obj.CustomerName,
                IsPaid = obj.IsPaid,
                ImageUrl = obj.ImageUrl,

            };
            db.Orders.Add(order);
            db.SaveChanges();
            var orderObj = db.Orders.FirstOrDefault(x => x.OrderNo == obj.OrderNo);
            if (orderObj != null && obj.OrderedItems != null)
            {
                foreach (var item in obj.OrderedItems)
                {
                    OrderedItem orderedItem = new OrderedItem
                    {
                        OrderId = orderObj.OrderId,
                        ProductId = item.ProductId,
                        Price = item.Price,
                        Quantity = item.Quantity,
                    };
                    db.OrderedItems.Add(orderedItem);
                }
                db.SaveChanges();
            }
            return Ok("Order Saved Successfully");
        }

        public IHttpActionResult PutOrder(int id, OrderRequest request)
        {
            Order order = db.Orders.FirstOrDefault(x => x.OrderId == id);
            if (id != request.Order.OrderId) return BadRequest();
            if (order == null) return NotFound();
            if (request.Order == null) return BadRequest("Order Data missing");
            Order obj = request.Order;
            byte[] imageFile = request.ImageFile;
            if (imageFile != null && imageFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                string filePath = Path.Combine("~/Images/", fileName);
                File.WriteAllBytes(HttpContext.Current.Server.MapPath(filePath), imageFile);
                obj.ImageUrl = filePath;
            }
            order.OrderNo = obj.OrderNo;
            order.OrderDate = obj.OrderDate;
            order.CustomerName = obj.CustomerName;
            order.IsPaid = obj.IsPaid;
            order.ImageUrl = obj.ImageUrl;
            var existingOrderItems = db.OrderedItems.Where(x => x.OrderId == order.OrderId);
            db.OrderedItems.RemoveRange(existingOrderItems);
            if (obj.OrderedItems != null)
            {
                foreach (var item in obj.OrderedItems)
                {
                    OrderedItem orderedItem = new OrderedItem
                    {
                        OrderId = order.OrderId,
                        ProductId = item.ProductId,
                        Price = item.Price,
                        Quantity = item.Quantity
                    };
                    db.OrderedItems.Add(orderedItem);
                }
            }
            db.SaveChanges();
            return Ok("Order and related items have been updated");
        }
    }
}
