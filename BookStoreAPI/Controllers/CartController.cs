﻿using BookStore.DB;
using BookStore.Interfaces;
using BookStore.Model;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Model;
using BookStoreAPI.Model.Lists;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Net;

namespace BookStoreAPI.Controllers
{
    [Route("/cart")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly DbConnection db;
        public CartController(DbConnection dbContext)
        {
            db = dbContext;
        }

        [HttpGet("all")]
        public async Task<ActionResult<ICartList>> GetAllCarts()//получение списка всех корзин
        {
            var carts = await db.Cart.Include(c => c.CartItems).ToListAsync();
            if (carts == null || !carts.Any())return NotFound("No carts found");
            ICartList response = new CartList { Carts = carts };
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ICartItemList>> GetCartContent(int id)//получить содержимое корзины по айди
        {
            var cart = await db.Cart.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.CartId == id);
            if (cart == null)  return NotFound("Cart with id " + id + " not found");
            if (cart.CartItems == null || !cart.CartItems.Any())
                return Ok(new CartItemList { CartItems = new List<CartItem>() });
            ICartItemList response = new CartItemList { CartItems = cart.CartItems };
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id) //удаление содержимого корзины 
        {
            var cart = await db.Cart.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.CartId == id);
            if (cart == null) return NotFound("Cart with id " + id + " not found.");
            db.CartItem.RemoveRange(cart.CartItems);
            await db.SaveChangesAsync();
            return NoContent(); 
        }

        [HttpDelete("{id}/item/{itemid}")]
        public async Task<IActionResult> DeleteCartItem(int id, int itemid) //удаление одного предмета из корзины
        {
            var cart = await db.Cart.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.CartId == id);
            if (cart == null) return NotFound("Cart with id " + id + " not found");
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == itemid);
            if (cartItem == null)return NotFound("Cart item with id " + itemid + " not found in cart with id " + id);
            db.CartItem.Remove(cartItem);
            await db.SaveChangesAsync();
            return NoContent(); 
        }

        [HttpGet("{id}/item/{itemid}")]
        public async Task<ActionResult<IBook>> GetCartItemInfo(int id, int itemid) //получение информации об одном элементе корзины
        {
            var cart = await db.Cart.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.CartId == id);
            if (cart == null) return NotFound("Cart with id " + id + " not found.");
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == itemid);
            if (cartItem == null) return NotFound("Cart item with id " + itemid + " not found in cart with id " + id);
            IBook product = await db.BookProduct.FindAsync(cartItem.ProductId); 
            if (product == null) return NotFound("Product with id " + cartItem.ProductId + " not found");
            return Ok(product);
        }

        [HttpPost("{id}/item")]
        public async Task<IActionResult> PostCartItem(int id, [FromBody] CartItem newCartItem) //добавить продукт в корзину
        {
            var cart = await db.Cart.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.CartId == id);
            if (cart == null)return NotFound("Cart with id " + id + " not found");
            var bookProduct = await db.BookProduct.FindAsync(newCartItem.ProductId);
            if (bookProduct == null) return NotFound("Product with id " + newCartItem.ProductId + " not found");

            if (newCartItem.ProductQuantity > bookProduct.AvailableQuantity) //если количество товара меньше чем хочет добавить пользователь
                return BadRequest("Not enough available quantity for product with id " + newCartItem.ProductId + ", available quantity: " + bookProduct.AvailableQuantity);
            if (newCartItem.ProductQuantity <=0 )  return BadRequest("Product quantity must be greater than 0");

            newCartItem.CartId = cart.CartId;
            cart.CartItems.Add(newCartItem);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCartItemInfo), new { id = cart.CartId, itemid = newCartItem.CartItemId }, newCartItem);
        }

        //[HttpGet("")]
        //public async Task<ICart> GetCartContent( [FromQuery] string sessionId = null, [FromQuery] int? userId = null) {
        //    if (userId.HasValue)
        //    {
        //        var userCart = await db.Cart
        //            .Include(c => c.CartItems)
        //            .FirstOrDefaultAsync(c => c.UserId == userId);
        //        if (userCart != null) return new CartItemList { CartItems = userCart.CartItems };
        //        else
        //        {
        //            var newCart = new Cart
        //            {
        //                UserId = userId,
        //                SessionId = Guid.NewGuid().ToString()
        //            };
        //            db.Cart.Add(newCart);
        //            await db.SaveChangesAsync();
        //            return new CartItemList { CartItems = new List<CartItem>() };
        //        }
        //    }

        //    if (string.IsNullOrEmpty(sessionId))  sessionId = Guid.NewGuid().ToString();
        //    var cart = await db.Cart
        //        .Include(c => c.CartItems)
        //        .FirstOrDefaultAsync(c => c.SessionId == sessionId);
        //    if (cart == null)
        //    {
        //        var newCart = new Cart
        //        {
        //            SessionId = sessionId
        //        };
        //        db.Cart.Add(newCart);
        //        await db.SaveChangesAsync();

        //        return new CartItemList { CartItems = new List<CartItem>() };
        //    }
        //    return new CartItemList { CartItems = cart.CartItems };
        //}
    }
}
