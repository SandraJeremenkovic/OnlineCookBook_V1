using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MongoDB.Bson;
using MongoDB.Driver;

namespace OnlineCookBook.MongoDB_DataLayer.Entities
{
    public class Order
    {
        public ObjectId Id { get; set; }

        public DateTime Date { get; set; }

        public float Fprice { get; set; }//full price

        public ObjectId UserId { get; set; }
        public string UserInfo { get; set; }

        public string Status { get; set; } //pending ili delivered

        public List<Ingredient> Ingredients { get; set; }

        public Order()
        {
            Ingredients = new List<Ingredient>();
        }
    }
}