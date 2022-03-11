using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace DB_First_Tutorial.Controllers
{
    public class DBFirstController : Controller
    {
        DBFirstTutorialEntities _context;
        public DBFirstController()
        {
            _context = new DBFirstTutorialEntities();
        }
        
        public ActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
        public ActionResult Create()
        {
            return View(new Product { Id = 0 }) ;
        }
        [HttpPost]
        public ActionResult Create(Product _product)
        {
            if(ModelState.IsValid)
            {
                if (_product.Id > 0)
                    _context.Entry(_product).State = System.Data.Entity.EntityState.Modified;
                else
                    _context.Products.Add(_product);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View("Create",_product);
            
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Product product = _context.Products.Find(id);
            if (product == null)
                return HttpNotFound();
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var product = _context.Products.Find(id);
            if(product==null)
                return HttpNotFound();
            return View("Create",product);
        }
        public ActionResult Search(string search)
        {
            //var _products=from p in _context.Products
            //              where p.Name.Contains(search) 
            //              select p;
            var _products=_context.Products.Where(p=>p.Name.Contains(search));
            return View("Index", _products);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();
            base.Dispose(disposing);
        }
    }
}