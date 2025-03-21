﻿using BookStoreAPI.Interfaces;
using BookStoreAPI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookStore.Model
{
    public class Cart : ICart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        [Column("cart_id")]
        public int CartId { get; set; }
        [Column("session_id")]
        public string? SessionId { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
