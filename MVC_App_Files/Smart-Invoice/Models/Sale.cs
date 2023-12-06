using System;
using Invoice_web_app.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoice_web_app.Models
{
	public class Sale
	{
        [Key]
        public int sale_id { get; set; }

        public DateTime sale_date { get; set; }

        public string customer_name { get; set; }

        public string product_name { get; set; }


        public string quantity { get; set; }


        public string price { get; set; }

        public string discount { get; set; }

        public string tax { get; set; }


        public string subtotal { get; set; }

        public int paid_with { get; set; }

        public int store_id { get; set; }

       
        public int user_id { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }
}

