using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MongoDB.Bson;
using MongoDB.Driver;

namespace OnlineCookBook.MongoDB_DataLayer.Entities
{
    public class Ingredient
    {
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public float Quantity { get; set; }

        
    }
}