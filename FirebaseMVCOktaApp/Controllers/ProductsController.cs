using FirebaseMVCOktaApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Linq;

namespace FirebaseMVCOktaApp.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public async Task<ActionResult> Index()
        {
            var firebaseClient = new FirebaseClient("https://fir-mvcoktasample-fc21c.firebaseio.com/");
            var dbProduct = await firebaseClient
              .Child("Product")
              .OnceAsync<Product>();
            
            var proListObjekt = new List<Product>();

            foreach (var n in dbProduct)
            {
                proListObjekt.Add( new Product { TimestampUtc = n.Object.TimestampUtc, Name = n.Object.Name, Price = n.Object.Price });
            }
            return View(proListObjekt);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TimestampUtc ,Name,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                var currentLoginTime = DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss");
                product.TimestampUtc = currentLoginTime;
                var firebaseClient = new FirebaseClient("https://fir-mvcoktasample-fc21c.firebaseio.com/");
                var result = await firebaseClient
                .Child("Product/")
                .PostAsync(product);

                return RedirectToAction("Index");
            }
            return View(product);
        }

        public async Task<ActionResult> Edit(string time)
        {
            var firebaseClient = new FirebaseClient("https://fir-mvcoktasample-fc21c.firebaseio.com/");
            var dbProduct = await firebaseClient
              .Child("Product")
              .OnceAsync<Product>();

            var product = dbProduct.Where(x => x.Object.TimestampUtc == time);

            return View(product);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string time, [Bind(Include = "TimestampUtc ,Name,Price")] Product product)
        {

            return View(product);
        }

        // GET: Movies/Delete/5
        public async Task<ActionResult> Delete(string time)
        {
            var firebaseClient = new FirebaseClient("https://fir-mvcoktasample-fc21c.firebaseio.com/");
            var dbProduct = await firebaseClient
              .Child("Product")
              .OnceAsync<Product>();

            var proListObjekt = new List<Product>();

            foreach (var n in dbProduct)
            {
                proListObjekt.Add(new Product { TimestampUtc = n.Object.TimestampUtc, Name = n.Object.Name, Price = n.Object.Price });
            }

            var x = (from t in proListObjekt
                    where t.TimestampUtc == time
                    select new Product{ TimestampUtc = t.TimestampUtc, Name = t.Name, Price = t.Price }).FirstOrDefault();

            return View(x);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string time)
        {
            var firebaseClient = new FirebaseClient("https://fir-mvcoktasample-fc21c.firebaseio.com/");
            await firebaseClient
              .Child("Product")
              .DeleteAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}