using OnlineCookBook.MongoDB_DataLayer;
using OnlineCookBook.MongoDB_DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using System.IO;
using OnlineCookBook.Models;

namespace OnlineCookBook.Controllers
{
    public class HomeController : Controller
    {
        DataLayer storage = new DataLayer();
        public ActionResult Index()
        {
            ViewBag.User = HttpContext.Application["User"];
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            ViewBag.User = HttpContext.Application["User"];
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            ViewBag.User = HttpContext.Application["User"];
            return View();
        }

         [HttpGet]
        public ActionResult RecepiesView(string type)
        {
            ViewBag.User = HttpContext.Application["User"];
            List<Recipe> r;
            if (type != null)
                r = storage.ReturnRecipesByCategory(type);
            else
            {
                r = TempData["recepiesByTags"] as List<Recipe>;
                TempData["recepiesByTags"] = r;
            }
                
            return View(r);
        }
        [HttpPost]
        public ActionResult RecepiesView(List<string> filter)
        {
            ViewBag.User = HttpContext.Application["User"];
            List<Recipe> r = storage.ReturnRecipeByTags(filter);
            
            TempData["recepiesByTags"] = r;
            return Json(new { url = Url.Action("RecepiesView") });
        }

        public ActionResult FullRecepieView(string recepieId)
        {
            ViewBag.User = HttpContext.Application["User"];
            Recipe r = storage.ReturnRecipeById(recepieId);
            FullRecipeViewModel vm = new FullRecipeViewModel();
            vm.Recipe = r;
            if (HttpContext.Application["User"]!=null)
            {
                User u = storage.FindUserByUsername(HttpContext.Application["User"].ToString());
                vm.User = u;
            }
            ViewBag.Comments = storage.FindCommentByRecipeId(ObjectId.Parse(recepieId));
            return View(vm);
        }

        public ActionResult RegisterView()
        {
            
            return View();
        }

        public ActionResult RegisterUser(User user)
        {
            string message = storage.RegisterFindUserByUsername(user);
            if (message == "Valid username")
                HttpContext.Application["User"] = user.Username;
            return Json("{ \"message\" :\"" + message + "\"  }");
        }
        public ActionResult Login(string username, string password)
        {
            string message = storage.LogInFindUserByUsername(username, password);
            if(message=="Success!")
                HttpContext.Application["User"] = username;
            return Json("{ \"message\" :\"" + message + "\"  }");
            
        }
        public ActionResult Logout()
        {
           
            HttpContext.Application["User"] = null;
            return RedirectToAction("Index", "Home");
          

        }
        public ActionResult AddRecepie()
        {
            ViewBag.User = HttpContext.Application["User"];
            return View();
        }
        [HttpGet]
        public ActionResult UploadImgPartialView()
        {
            List<Ingredient> ingr = storage.FindAllIngredients();
            ViewBag.ingredients = ingr;
            ViewBag.User = HttpContext.Application["User"];
            return View();
        }
        [HttpPost]
        public ActionResult UploadImgPartialView(HttpPostedFileBase file)
        {
            var path = "";
            if (file != null)
            {
                if(file.ContentLength>0)
                {
                    if(Path.GetExtension(file.FileName).ToLower()==".jpg" || Path.GetExtension(file.FileName).ToLower()==".png")
                    {
                        path = Path.Combine(Server.MapPath("~/Content/Images"), file.FileName);
                        file.SaveAs(path);
                        ViewBag.image = file.FileName;
                        ViewBag.UploadSuccess = true;
                        List<Ingredient> ingr = storage.FindAllIngredients();
                        ViewBag.User = HttpContext.Application["User"];
                        ViewBag.ingredients = ingr;
                    }
                    else
                    {
                        ViewBag.UploadSuccess = false;
                        ViewBag.message = "Only .png and .jpg files suported!";
                    }
                }
                else
                {
                    ViewBag.UploadSuccess = false;
                    ViewBag.message = "Something went wwrong. Please, try again.";
                }
            }
            else
            {
                ViewBag.UploadSuccess = false;
                ViewBag.message = "Something went wwrong. Please, try again.";
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddRecipe(Recipe recipe, string username)
        {
            User u = storage.FindUserByUsername(username);
            recipe.UserId = u.Id;
            string message = "Success!";
            storage.InsertRecipeToDB(recipe);
            return Json("{ \"message\" :\"" + message + "\"  }");
        }
         [HttpPost]
        public ActionResult AddComment(Comment comment, string recipeId)
        {
            User u = storage.FindUserByUsername(comment.Username);
            comment.UserId = u.Id;
            comment.Date = DateTime.Now;
            comment.RecipeId = ObjectId.Parse(recipeId);
            string message = "Success!";
            storage.InsertNewCommentToDB(comment);
            return Json("{ \"message\" :\"" + message + "\"  }");
        }
         [HttpPost]
         public ActionResult AddOrder(Order order, string username)
         {
             User u = storage.FindUserByUsername(username);
             order.UserId = u.Id;
             order.Date = DateTime.Now;
             
             string message = "Success!";
             storage.InsertNewOrderToDB(order);
             return Json("{ \"message\" :\"" + message + "\"  }");
         }
         public ActionResult AdminView()
         {
             AdminViewModel vm = new AdminViewModel();
             vm.Recipes = storage.ReturnAllRecipes();
             vm.Orders = storage.FindOrdersByStatus("pending");
             vm.DeliveredOrders = storage.FindOrdersByStatus("Delivered");
             return View(vm);
         }
         public ActionResult AdminFullRecipePartialView(string recepieId)
         {
             Recipe r = storage.ReturnRecipeById(recepieId);
             return View(r);
         }
         public ActionResult ApproveRecipe(string recipeId)
         {

             storage.UpdateRecipeStatus(ObjectId.Parse(recipeId));
             
             string message = "Success!";
            
             return Json("{ \"message\" :\"" + message + "\"  }");
         }

         public ActionResult DeleteRecipe(string recipeId)
         {

             storage.DeleteRecipeFromDB(ObjectId.Parse(recipeId));
             
             string message = "Success!";
            
             return Json("{ \"message\" :\"" + message + "\"  }");
         }
         public ActionResult ConfirmOrder(string orderId)
         {

             storage.UpdateOrderStatus(ObjectId.Parse(orderId));

             string message = "Success!";

             return Json("{ \"message\" :\"" + message + "\"  }");
         }
         public ActionResult DeclineOrder(string orderId)
         {

             storage.DeleteOrderWithId(ObjectId.Parse(orderId));

             string message = "Success!";

             return Json("{ \"message\" :\"" + message + "\"  }");
         }
        
        
    }
}