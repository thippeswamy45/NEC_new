using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using thippeswamy.Models;
namespace thippeswamy.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {
            List<int> rackIds = new List<int>();
            List<int> shelfIds = new List<int>();
            DbAccess db = new DbAccess();
            db.GetRackShelfDetails(out rackIds, out shelfIds);
            ViewBag.rackids = rackIds;
            ViewBag.shelfids = shelfIds;
            
            return View(new ShelfContext());
        }

        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            DbAccess db = new DbAccess();
            ShelfContext context = new ShelfContext();

            List<int> rackIds = new List<int>();
            List<int> shelfIds = new List<int>();

            db.GetRackShelfDetails(out rackIds, out shelfIds);
            ViewBag.rackids = rackIds;
            ViewBag.shelfids = shelfIds;
            short rowCount = 0;
            short columnCount = 0;
          
            if (Int16.TryParse(formCollection["row_box"], out rowCount) && Int16.TryParse(formCollection["column_box"], out columnCount))
            {
                if(rowCount > 0 && columnCount > 0)
                { 

                db.AddShelf(rowCount, columnCount);
                context.shelfCreatedSuccessMsg = "shelf created successfully!!";
                return View(context);
                }
            }
           

            if (formCollection["rackid"] != "rack Id" && formCollection["shelfid"] != "shelf Id")
            {
                int shelfid = Convert.ToInt16( formCollection["shelfid"]);
                List<int> sensors = new List<int>();
                List<string> productNames = new List<string>();
               // short rowCount;
              //  short columnCount;
                db.GetShelfDetails(shelfid,out sensors,out productNames,out rowCount,out columnCount );
                context.sensors = sensors;
                context.productNames = productNames;
                context.rowCount = rowCount;
                context.columnCount = columnCount;
                return View(context);
            }
            if (formCollection["productName"] != "select product")
            {

                context.productName = formCollection["productName"];
                return View(context);
            }
            else
            {
                context.shelfCreatedSuccessMsg = "shelf creation failed!!";
                return View(context);
            }

          
        }

        public ActionResult RegisterProduct()
        {
            DbAccess db = new DbAccess();
            int sensorId =Convert.ToInt32( Request.Params["sensorId"]);
            string productName = Request.Params["productName"];

            db.ProductSensorMapping(productName,sensorId);
            return RedirectToAction("Index");
            return View();

        }
    }
}