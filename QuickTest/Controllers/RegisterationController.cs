﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuickTest.Models;
using QuickTest.Models.SellerViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuickTest.Controllers
{
    public class RegisterationController : Controller
    {


        DB_A56F7A_Quickmart2Context db3 = null;
        IHostingEnvironment host3 = null;

        public RegisterationController(DB_A56F7A_Quickmart2Context context, IHostingEnvironment obj)
        {
            db3 = context;
            host3 = obj;


        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult step1()
        {
            ViewBag.key = "1";
            return View();
        }
        public IActionResult step2()
        {
            
            ViewBag.key = "1";
            step2 obj = new step2();
            return View(obj);
        }
        public IActionResult step3()
        {
            ViewBag.key = "1";
            return View();
        }[HttpPost]
        public IActionResult step3(Step3 obj, IFormFile img)
        {
            string phone = null;
            if (obj!=null)
            {
                try
                {
                     phone = HttpContext.Session.GetString("phone");
                }catch(Exception e)
                {
                    ViewBag.key = "a";
                    TempData["error"] = "Cookie Expired... Please Enter Phone Number Again";
                    return View("/Views/Seller/signup.cshtml");

                }
                //  String  step1 = Request.Cookies["form1data"];
                //Step1 security=  JsonConvert.DeserializeObject<Step1>(step1);
                // ViewBag.password=security.password;
                //ViewBag.cnfrompass= security.conformpassword;
                //ViewBag.email = security.email;

            

                String step2 = Request.Cookies["form2data"];
                step2 Shopdetails = JsonConvert.DeserializeObject<step2>(step2);
             //   ViewBag.shopname = Shopdetails.shopname;


               

                string path = host3.WebRootPath + "/img/logos/";
               string fname = Path.GetFileName(img.FileName);
                img.CopyTo(new System.IO.FileStream(path + fname, System.IO.FileMode.Create));
                Usertable SELLER = db3.Usertable.Where(a => a.PhoneNo.Equals(phone) && a.UserType.Equals("S")).SingleOrDefault();
               
                SELLER.Address = obj.Address;
                SELLER.Latitude = Convert.ToDecimal(obj.lat);
                SELLER.Longitude = Convert.ToDecimal(obj.longitude);
                SELLER.Logo = "/img/logos/" + fname;
                SELLER.ShopName = Shopdetails.shopname;
               
                SELLER.UserType = "S";
                SELLER.SellerDetails = 1;
                try
                {

                    db3.Usertable.Update(SELLER);
                    db3.SaveChanges();
                }catch(Exception e)
                {
                       ViewBag.error = e.InnerException;
                      return View("/Views/Seller/errorpage.cshtml");



                    //return Json(SELLER);
                }


               


                // ViewBag.lat = obj.lat;
                //ViewBag.longitude = obj.longitude;




                //string[] array=Shopdetails.category;
                // ViewBag["Modes"]= Shopdetails.category;




                // JObject json = JObject.Parse(step1);
                //ViewBag.error = step1;
                //  TempData["Data"] = step1;
                // return RedirectToAction("errorpage", "Seller");
                //return View("successfully",obj);
                return RedirectToAction("dashboard", "seller");

            }

            return View();
        }


    }
}
