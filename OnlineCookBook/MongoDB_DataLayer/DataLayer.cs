using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using OnlineCookBook.MongoDB_DataLayer.Entities;


namespace OnlineCookBook.MongoDB_DataLayer

{
    public static class DLayer
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
       (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var known = new HashSet<TKey>();
            return source.Where(element => known.Add(keySelector(element)));
        }
    }
    public class DataLayer
    {


        //funkcije za manipulisanje receptima i povezivanje sa bazom********************************************************************************************************************

        public List<Recipe> ReturnRecipesByCategory(string category)//vraca recepte iz odredjene kategorije
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Recipe>("recipe");

            var filter1 = Builders<Recipe>.Filter.Eq(f => f.Status, "Approved");
            var filter2 = Builders<Recipe>.Filter.Eq(f => f.Category, category);
            var filter = Builders<Recipe>.Filter.And(filter2, filter1);
            List<Recipe> r = collection.Find(filter).ToList();


            return r;
        }


        public Recipe ReturnRecipeById(string id)//vraca recept po Id koji je zadat kao string
        {
            ObjectId id1 = ObjectId.Parse(id);
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Recipe>("recipe");

            Recipe r = collection.Find(f => f.Id == id1).First();
            return r;
        }

        public Recipe ReturnRecipeByIdObjectId(ObjectId id)//vraca recept po Id koji je zadat kao ObjectId
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Recipe>("recipe");

            Recipe r = collection.Find(f => f.Id == id).First();
            return r;
        }


        public List<Recipe> ReturnAllRecipes() //vraca sve recepte svih kategorija
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Recipe>("recipe");

            List<Recipe> r = collection.Find(_ => true).ToList();
            return r;
        }



      



        public void InsertRecipeToDB(Recipe p)//dodaje recept u bazu
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Recipe>("recipe");

            collection.InsertOne(p);
        }

        public void DeleteRecipeFromDB(ObjectId id)//brise recept iz baze po Id
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Recipe>("recipe");


            collection.DeleteOne(f => f.Id == id);
        }

        public List<Recipe> ReturnRecipeByTag(string tag)//vraca listu recepata po tagu
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Recipe>("recipe");

            var filter1 = Builders<Recipe>.Filter.Eq(f => f.Status, "Approved");
            var filter2 = Builders<Recipe>.Filter.Where(f => f.Tags.Contains(tag));
            var filter = Builders<Recipe>.Filter.And(filter2, filter1);
            List<Recipe> r = collection.Find(filter).ToList();

            return r;
        }

        public List<Recipe> ReturnRecipeByTags(List<string> tags)//vraca recepte po listi tagova
        {
            List<Recipe> p = new List<Recipe>();
            foreach (string t in tags)
            {
                List<Recipe> m = this.ReturnRecipeByTag(t);
                p.AddRange(m);
            }
            return p;
        }

        public void UpdateRecipeStatus(ObjectId id)//updejtuje status recepta na Approved, difoltno svi novi su pending
        {
            string s = "Approved";
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Recipe>("recipe");

            var filter = Builders<Recipe>.Filter.Eq(f => f.Id, id);
            var update = Builders<Recipe>.Update.Set("Status", s);
            collection.FindOneAndUpdate(filter, update);
        }
        public List<Recipe> FindRecipesByStatus(string s)//vraca recepte po statusu
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Recipe>("recipe");

            List<Recipe> r = collection.Find(p => p.Status == s).ToList();

            return r;
        }

        public List<Recipe> FindRecipesByUserId(ObjectId s)//vraca recepte po korisnikovom Id-ju koji ih he kreirao
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Recipe>("recipe");

            List<Recipe> r = collection.Find(p => p.UserId == s).ToList();

            return r;
        }


        //funkcije za manipulisanje narudzbinama i povezivanje sa bazom********************************************************************************************************************

        public void InsertNewOrderToDB(Order u)//dodavanje narudzbine u bazu
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Order>("order");

            collection.InsertOne(u);

        }
        public List<Order> FindAllOrders()//vraca sve narudzbine
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Order>("order");

            List<Order> r = collection.Find(_ => true).ToList();
            return r;
        }
        public List<Order> FindOrdersByUserId(ObjectId id)//vraca narudzbine za odredjenog korisnika
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Order>("order");

            List<Order> r = collection.Find(p => p.UserId == id).ToList();
            return r;
        }

        public List<Order> FindOrdersByStatus(String s)//vraca narudzbine za adminu po statusu-pending, delivered
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Order>("order");

            List<Order> r = collection.Find(p => p.Status == s).ToList();
            return r;
        }

        public void UpdateOrderStatus(ObjectId id)//updejtuje status narudzbine na Delivered, difoltno sve nove su pending
        {
            string s = "Delivered";
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Order>("order");

            var filter = Builders<Order>.Filter.Eq(f => f.Id, id);
            var update = Builders<Order>.Update.Set("Status", s);
            collection.FindOneAndUpdate(filter, update);
        }

        public void DeleteOrderWithId(ObjectId id)//brisanje narudzbine sa odredjenim Id-jem
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Order>("order");

            collection.DeleteOne(f => f.Id == id);
        }

        //funkcije za manipulisanje korisnicima i povezivanje sa bazom********************************************************************************************************************


        public User FindUserWithId(ObjectId id)//vraca korisnika po Id-ju
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<User>("user");

            User u = collection.Find(f => f.Id == id).First();

            return u;

        }
        public void InsertNewUserToDB(User u)//dodavanje korisnika u bazu
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<User>("user");

            collection.InsertOne(u);

        }

        public void DeleteUserWithId(ObjectId id)//brisanje korisnika sa odredjenim Id-jem
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<User>("user");

            collection.DeleteOne(f => f.Id == id);
        }
   

        public User FindUserByUsername(string username)//vraca usera sa istim username
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<User>("user");

            bool result = collection.Find(f => f.Username == username).Any();
            if (result == true)
                return collection.Find(f => f.Username == username).First();
            else
                return null;
        }

        public string LogInFindUserByUsername(string user, string password)//login forma
        {
            User u = FindUserByUsername(user);

            if (u == null)
            {
                return "Non-existing username!";
            }
            else if (u.Password != password)
            {
                return "Wrong password!";
            }
            else
                return "Success!";

        }

        public string RegisterFindUserByUsername(User user)//register forma
        {
            User u = FindUserByUsername(user.Username);

            if (u != null)
            {
                return "Username already taken!";
            }
            else
            {
                InsertNewUserToDB(user);
                return "Valid username";
            }

        }

        //funkcije za manipulisanje komentarima i povezivanje sa bazom********************************************************************************************************************

        public void InsertNewCommentToDB(Comment u)//dodavanje komentara u bazu
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Comment>("comment");

            collection.InsertOne(u);

        }

        public List<Comment> FindCommentByUserId(ObjectId id)//vraca komentare za odredjenog korisnika
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Comment>("comment");

            List<Comment> r = collection.Find(p => p.UserId == id).ToList();
            return r;
        }


        //vraca komentare po statusu
        //update status komentara
        //brisanje komentara koji nisu approved


        //funkcije za manipulisanje komentarima i sastojcima i povezivanje sa bazom********************************************************************************************************************

        public Ingredient FindIngredientById(ObjectId id)//vraca sastojak sa odredjenim id
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Ingredient>("ingredient");

            Ingredient r = collection.Find(p => p.Id == id).First();
            return r;
        }

        public Ingredient FindIngredientByName(string name)//vraca sastojak sa odredjenim imenom
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Ingredient>("ingredient");

            Ingredient r = collection.Find(p => p.Name == name).First();
            return r;
        }

        public List<Ingredient> FindAllIngredients()//vraca sve sastojke
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Ingredient>("ingredient");

            List<Ingredient> r = collection.Find(_ => true).ToList();
            List<Ingredient> m = DLayer.DistinctBy(r, p => p.Name).ToList();
            return m;
        }

        public void InsertNewIngredientToDB(Ingredient u)//dodavanje sastojka u bazu
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Ingredient>("ingredient");

            collection.InsertOne(u);

        }


        public List<Comment> FindCommentByRecipeId(ObjectId id)//vraca komentare za odredjeni recept
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = new MongoClient(connectionString);
            var db = server.GetDatabase("cookbook");

            var collection = db.GetCollection<Comment>("comment");

            List<Comment> r = collection.Find(p => p.RecipeId == id).ToList();
            return r;
        }
    }
}