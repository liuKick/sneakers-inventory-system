using Postgrest.Models;
using Postgrest.Attributes;
using System;

namespace SneakerShop.Models
{
    [Table("customers")]
    public class Customer : BaseModel  // ← ADDED : BaseModel
    {
        [PrimaryKey("customer_id", false)]
        public string Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public Customer()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
        }
    }
}