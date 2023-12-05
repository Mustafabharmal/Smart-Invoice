using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace Invoice_web_app.Models
{
	public class Store
	{
        [Key]
        public int store_id { get; set; }
        public string store_name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string Address { get; set; }
        public string Tax_ID { get; set; }
        public int status { get; set; }
        public byte[] logo { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}

