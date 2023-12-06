using System;
using Invoice_web_app.Models;
using System.ComponentModel.DataAnnotations;
namespace Invoice_web_app.Models
{
	public class Product
	{
        [Key]
        public int product_id { get; set; }
        public string product_name { get; set; }
        public int category_id { get; set; }
        public int min_quantity { get; set; }
        public int quantity { get; set; }
        public string description { get; set; }
        public string price { get; set; }
        public int status { get; set; }
        public byte[] image { get; set; }

        // Foreign keys
        public int store_id { get; set; }

        public int user_id { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}

