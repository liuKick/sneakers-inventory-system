using Postgrest.Models;
using Postgrest.Attributes;
using System;

namespace SneakerShop.Models
{
    [Table("sneakers")]
    public class Sneaker : BaseModel
    {
        [PrimaryKey("sneaker_id", false)]  // Added false to indicate not auto-increment
        public string Id { get; set; }

        [Column("display_id")]
        public int DisplayId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("brand_id")]
        public string BrandId { get; set; }

        [Column("size")]
        public string Size { get; set; }

        [Column("color")]
        public string Color { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("stock_quantity")]
        public int StockQuantity { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public Sneaker()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
        }
    }
}