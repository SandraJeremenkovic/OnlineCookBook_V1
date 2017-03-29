using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MongoDB.Bson;
using MongoDB.Driver;

namespace OnlineCookBook.MongoDB_DataLayer.Entities
{
    public class User
    {
        public ObjectId Id { get; set; }
        
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Phonenum { get; set; }

        public string Password { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

    }
}