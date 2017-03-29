using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineCookBook.MongoDB_DataLayer.Entities;

namespace OnlineCookBook.Models
{
    public class FullRecipeViewModel
    {
        public Recipe Recipe { get; set; }
        public User User { get; set; }
    }
}