using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MongoDB.Bson;
using MongoDB.Driver;

namespace OnlineCookBook.MongoDB_DataLayer.Entities
{
    public class Recipe
    {
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public int PrepareTime { get; set; }

        public string LongDsc { get; set; }

        public string ShortDsc { get; set; }

        public string Status { get; set; }

        public string Image { get; set; }
        public List<string> Tags { get; set; }
        public ObjectId UserId { get; set; }

        public List<Ingredient> Ingredients { get; set; }

        public Recipe()
        {
            Ingredients = new List<Ingredient>();
        }
    }
}