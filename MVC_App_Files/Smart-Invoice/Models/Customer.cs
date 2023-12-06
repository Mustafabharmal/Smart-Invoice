using Invoice_web_app.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Invoice_web_app.Models
{
	public class Customer
	{
        [Key]
        public int customer_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string description { get; set; }
        // public byte[] avatar { get; set; } // Commented out as you mentioned it's not used
        public int store_id { get; set; }
        public int status { get; set; }
        //public Store Store { get; set; } // Navigation property
        public int user_id { get; set; }
        //public User User { get; set; } // Navigation property
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}

