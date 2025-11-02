using Postgrest.Models;
using Postgrest.Attributes;
using System;

namespace SneakerShop.Models
{
    [Table("brands")]
    public class Brand : BaseModel
    {
        [PrimaryKey("brand_id")] // ← ADD THIS
        public string Id { get; set; }

        [Column("brand_name")] // ← ADD THIS
        public string BrandName { get; set; }

        [Column("created_at")] // ← ADD THIS
        public DateTime CreatedAt { get; set; }

        public Brand()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
        }
    }
}