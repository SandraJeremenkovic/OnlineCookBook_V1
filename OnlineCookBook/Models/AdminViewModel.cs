using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineCookBook.MongoDB_DataLayer.Entities;

namespace OnlineCookBook.Models
{
    public class AdminViewModel
    {
        public List<Order> Orders { get; set; }
        public List<Order> DeliveredOrders { get; set; }
        public List<Recipe> Recipes { get; set; }
    }
}