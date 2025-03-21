﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace BookStore.Model
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        [Column("order_item_id")]
        public int CartItemId { get; set; }

        [JsonIgnore]
        [Column("order_id")]
        public int OrderId { get; set; } // Ensure this matches the database column name

        [Column("product_id")]
        public int ProductId { get; set; }

        [Required]
        [Column("productquantity")]
        public int ProductQuantity { get; set; }

    }
}
