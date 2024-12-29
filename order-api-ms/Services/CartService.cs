using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using order_api_ms.Data;
using order_api_ms.DomainModel.IServices;
using order_api_ms.DomainModel.Models;
using order_api_ms.DomainModel.Models.Dtos;
using order_api_ms.Services.IExternalServices;
using System.Reflection.Metadata.Ecma335;

namespace order_api_ms.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _dbContext;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public CartService(AppDbContext dbContext, IProductService productService, IMapper mapper)
        {
            _dbContext = dbContext; 
            _productService = productService;
            _mapper = mapper;
        }
        public async Task<CartDetails> GetCart(int CustomerID)
        {
            var cart = new CartDetails()
            {
                Cart = _dbContext.Carts.First(u => u.CustomerID == CustomerID)
            };
            cart.CartItems = _dbContext.CartItems.Where(u => u.CartID == cart.Cart.CartID);

            IEnumerable<ProductDto> ProductListDto =  await _productService.GetProducts();
            var productList = _mapper.Map<IEnumerable<Product>>(ProductListDto);
            if(productList!= null)
            {
                foreach (var itesm in cart.CartItems)
                {
                    itesm.Product = productList.FirstOrDefault(u => u.ProductID == itesm.ProductID);
                    cart.totalAmount += itesm.Product.Price * cart.CartItems.First().Quantity;
                }
            }
            return cart;
        }
        public async Task<Cart> UpsertCartAsync(CartDetails cartDetails)
        {
            /*
                 1. user add first item to the cart

                 2. user add new item to the cart (user already exists)
                 - find a cart
                 - Add Cart details for same cartItem,

                 3. user update quantity of an existing items in the cart
                  - find CartItem
                  - Update quantity of existing Item
            */
            var CartFromDb = await _dbContext.Carts.AsNoTracking().FirstOrDefaultAsync(u => u.CustomerID == cartDetails.Cart.CustomerID);
            if(CartFromDb == null)
            {
                // Create cart & CartItem ()if cart is empty)
                var cart = _dbContext.Carts.Add(cartDetails.Cart);
                await _dbContext.SaveChangesAsync();
                var CartWhichAddedInDb = await _dbContext.Carts.AsNoTracking().FirstOrDefaultAsync(u => u.CustomerID == cartDetails.Cart.CustomerID);

                var cartIteamToAddInDb = new CartItems()
                {
                    CartID = CartWhichAddedInDb.CartID,
                    ProductID = cartDetails.CartItems.First().ProductID,
                    Quantity = cartDetails.CartItems.First().Quantity
                };
                await _dbContext.CartItems.AddAsync(cartIteamToAddInDb);
                await _dbContext.SaveChangesAsync();

                return CartWhichAddedInDb;
            }
            else
            {
                var CartItemsFromDb = await _dbContext.CartItems.AsNoTracking().FirstOrDefaultAsync(
                    u => u.ProductID == cartDetails.CartItems.First().ProductID &&
                         u.CartID == CartFromDb.CartID);

                // Cart having data but cart details with perticulare product is empty
                if (CartItemsFromDb == null)
                {
                    // Create New CartItem
                    var cartItemToAdd = new CartItems
                    {
                        CartID = CartFromDb.CartID,
                        ProductID = cartDetails.CartItems.First().ProductID,
                        Quantity = cartDetails.CartItems.First().Quantity
                    };
                    await _dbContext.CartItems.AddAsync(cartItemToAdd);
                    await _dbContext.SaveChangesAsync();
                    return CartFromDb;
                }
                else
                {
                    // Update Count if cartid and product Id already exist
                    
                    CartItemsFromDb.Quantity += cartDetails.CartItems.First().Quantity;
                    _dbContext.CartItems.Update(CartItemsFromDb);
                    await _dbContext.SaveChangesAsync();
                     return CartFromDb;
                }
            }
            //return new Cart { };
        }
        public async Task<bool> RemoveCartItemAsync(int CartDetailsID)
        {
            // Seach for CartItem to delete
            var CartItemsFromDb = await _dbContext.CartItems.AsNoTracking().FirstOrDefaultAsync(u => u.CartItemId == CartDetailsID);
            if(CartItemsFromDb!=null)
            {
                // Count of CartItems for same CartID, if 1 delete CartItems and Cart
                var totalCartItemsCount = _dbContext.CartItems.Where(u => u.CartID == CartItemsFromDb.CartID).Count();

                var CartItemToDeleted = _dbContext.CartItems.Remove(CartItemsFromDb);
                if (totalCartItemsCount == 1)
                {
                    // Remove Cart as only 1 record for the CartItems
                    var CartFromDb = await _dbContext.Carts.AsNoTracking().FirstOrDefaultAsync(u => u.CartID == CartItemsFromDb.CartID);
                    var CartDeleted = _dbContext.Carts.Remove(CartFromDb);
                }
                await _dbContext.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<Cart> RemoveCart(int CartID)
        {
            var IsCartExists = await _dbContext.Carts.FirstOrDefaultAsync(u=>u.CartID == CartID);
            
            if(IsCartExists != null)
            {
                var CartItemLIst = await _dbContext.CartItems.Where(u => u.CartID == IsCartExists.CartID).ToListAsync();
                _dbContext.CartItems.RemoveRange(CartItemLIst);
                _dbContext.Carts.Remove(IsCartExists);

                await _dbContext.SaveChangesAsync();

                return IsCartExists;
            }
            return null;
        }
    }
}
