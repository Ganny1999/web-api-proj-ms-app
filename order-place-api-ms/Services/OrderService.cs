using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using order_place_api_ms.Data;
using order_place_api_ms.DomainModel.Dtos;
using order_place_api_ms.DomainModel.IServices;
using order_place_api_ms.DomainModel.Models;
using order_place_api_ms.Services.IExternalServices;

namespace order_place_api_ms.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly AppDbContext _appDbContext;
        public OrderService(AppDbContext appDbContext,ICartService cartService, IProductService productService)
        {
            _appDbContext = appDbContext;
            _cartService = cartService;
            _productService = productService;
        }
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            var allOrders = await (from OrderList in _appDbContext.Orders
                            orderby OrderList.CreatedDate descending
                            select OrderList).ToListAsync();
            var allOrder = _appDbContext.Orders.OrderByDescending(u=>u.CreatedDate);

            return allOrders;
        }

        public async Task<Order> GetOrderByOrderIdAsync(Guid orderId)
        {
            if(orderId == Guid.Empty)
            {
                return null;
            }
            var isOrderExist = await _appDbContext.Orders.FirstOrDefaultAsync(u => u.OrderID == orderId);
            if(isOrderExist != null) 
            {
                return isOrderExist;
            }
            return null;
        }

        public async Task<Order> PlaceOrderAsync(int CustomerID)
        {
            try
            {
                // Need to call GetCart Api based on the custmer ID.
                var CartDetailsFromDb = await _cartService.GetCartDetails(CustomerID);

                // calling product details by based in the Product ID.
                var cartproductList = new List<ProductDto>();

                foreach(var item in CartDetailsFromDb.CartItems)
                {
                    var product = await _productService.GetProductByID(item.ProductID);
                    cartproductList.Add(product);
                }

                // Create order data in table 
                if (CartDetailsFromDb.Cart != null && CartDetailsFromDb.CartItems != null)
                {
                    var order = new Order()
                    {
                        CustomerID = CartDetailsFromDb.Cart.CustomerID,
                        TotalAmount = (int)CartDetailsFromDb.totalAmount,
                        PementMethod = "Cash",
                        PaymentStatus = "Success",
                        CreatedDate = DateTime.Now
                    };

                    var IsOrderAdded = await _appDbContext.Orders.AddAsync(order);
                    await _appDbContext.SaveChangesAsync();

                    // fetch the OrderID from database
                    var OrderDetailsFromDb = await _appDbContext.Orders.FirstOrDefaultAsync(u => u.CustomerID == CustomerID && u.CreatedDate == order.CreatedDate);
                    if (OrderDetailsFromDb != null && cartproductList!=null)
                    {
                        var OrderItemsList = new List<OrderItems>();
                        
                        foreach (var CartItem in CartDetailsFromDb.CartItems)
                        {
                            var product = cartproductList.FirstOrDefault(u=>u.ProductID==CartItem.ProductID);
                            var OrderItems = new OrderItems()
                            {
                                OrderID = OrderDetailsFromDb.OrderID,
                                ProductID = CartItem.ProductID,
                                Quntity = CartItem.Quantity,
                                Price = product !=null ? product.Price :0,
                                ProductName = product != null ? product.ProductName :""
                            };
                            OrderItemsList.Add(OrderItems);
                        }
                        if (CartDetailsFromDb.CartItems.Count() == OrderItemsList.Count())
                        {
                            // Remove Cart logic
                            var cart = await _cartService.RemoveCart(CartDetailsFromDb.Cart.CartID);
                        }
                        await _appDbContext.OrderItems.AddRangeAsync(OrderItemsList);
                        await _appDbContext.SaveChangesAsync();
                        return OrderDetailsFromDb;
                    }
                }
                return null;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
