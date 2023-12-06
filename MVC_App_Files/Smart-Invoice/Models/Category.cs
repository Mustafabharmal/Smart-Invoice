using System;
using Invoice_web_app.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoice_web_app.Models
{
	public class Category
	{
        [Key]
        public int category_id { get; set; }
        public string product_name { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public byte[]? image { get; set; }
        public int store_id { get; set; }
        public int status { get; set; }
        [ForeignKey("store_id")]
        public Store Store { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}

