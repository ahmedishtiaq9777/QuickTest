using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickTest.Models;
using QuickTest.Models.ManageViewModels;
using QuickTest.Models.ReturnModelForAndroid;
using QuickTest.Models.SellerViewModels;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using tryproj1._1.Models;

namespace tryproj1._1.Controllers
{
    public class SellerController : Controller
    {

        DB_A56F7A_Quickmart2Context db2 = null;
        IHostingEnvironment host2 = null;
        public SellerController(DB_A56F7A_Quickmart2Context context, IHostingEnvironment obj)
        {
            db2 = context;
            host2 = obj;


        }
       public List<Usertable> addphones()
        {
            List<Usertable> list = null;
            
            
              
                list = db2.Usertable.Where(a=>a.UserType.Equals("C")).ToList();
                
                int c = 1;
                foreach (Usertable i in list)
                {

                    i.PhoneNo = "0323983223" + Convert.ToString(c);
                    i.Address = "nekapura street no " + c;
                    c++;

                    db2.Usertable.Update(i);
                    db2.SaveChanges();
                }
                
            
            return list;
        }///temprary data entry user tabel "phone  address"
        public List<Order> comparedate(string start,string end,DateTime odate)
        {
            //  DateTime sdate = new DateTime("");  "2020/01/01","2020/01/31"
            DateTime sdate = DateTime.ParseExact(start, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            DateTime enddate = DateTime.ParseExact(end, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            //  DateTime enddate = new DateTime(2020, 7, 30,00,00,00);

            List<Order> ol = db2.Order.Where(a => a.Date.Value.Date >= sdate && a.Date.Value.Date <= enddate).ToList();
           
          // List<Order> ol= db2.Order.Where(a => a.Date.Value >= sdate && a.Date.Value <= enddate).ToList();

            return ol;
        }

        public int getsellerIdofProduct(int pid)
        {
            int seller_id=db2.Product.Where(e => e.ProductId.Equals(pid)).Select(a => a.UserId).SingleOrDefault().Value;
            return seller_id;


        }
        public  JsonResult addsellerId()
        {
            List<OrderItems> list=db2.OrderItems.ToList();
            foreach(OrderItems i in list)
            {
                int sellerid=getsellerIdofProduct(i.ProId.Value);
                i.SellerId = sellerid;
                db2.Update(i);
                db2.SaveChanges();
            }

            return Json("success");
        }
        public IActionResult tempregister()
        {
            return View();

             }

        public PartialViewResult returnOrders()
        {

            return PartialView("order_partial");


        }
        
        public List<Order> getorderwithorderids(List<int> list)
        {
            List<Order> olist = new List<Order>();
            Order o;
            
            foreach (int id in list )
            {
                o = new Order();
                o = db2.Order.Where(a => a.OrderId.Equals(id)).SingleOrDefault();
                olist.Add(o);

            }
            return olist;
        }
        public IActionResult Dashboard()
        {
            int totalproducts = 0;
            DashboardViewModel obj = new DashboardViewModel();
            
            try
            {
                string sellerphone = HttpContext.Session.GetString("phone");
                if (sellerphone != null)
                {
                    int sid = db2.Usertable.Where(a => a.PhoneNo.Equals(sellerphone)).Select(b => b.UserId).SingleOrDefault();
                    totalproducts = db2.Product.Where(a => a.UserId.Value.Equals(sid)).Count();              
                    List<OrderItems> olist=db2.OrderItems.Where(a => a.SellerId.Equals(sid)).ToList();

                    

                    double TEarnings = 0.0;
                    foreach(OrderItems i in olist)
                    {
                        TEarnings = TEarnings + i.unitTotal.Value;              
                       

                    }

                    obj.Totalearnings = TEarnings;                               /// Earnings
                    obj.Salesitems = olist.Count();                               // Sales items
                       
                   // db2.Order.Where(a => a.UserId.Equals(sid)).Count();

                    obj.Totalproducts = totalproducts;                               // Total products


                    List<int>  orderids = db2.OrderItems.Where(a => a.SellerId.Equals(sid)).Select(b => b.OrderId.Value).ToList();
                       List<int> removedublicate = orderids.Distinct().ToList();
                     int _totalorders=removedublicate.Count();



                    obj.Totalorders = _totalorders;                                         // total orders





                    //// for graph
                    ///
                    List<GraphViewModel> graphdata = new List<GraphViewModel>();
                    GraphViewModel gvmodel;
                    List<Order> _orlist=getorderwithorderids(removedublicate);

                    foreach(Order i in _orlist)
                    {



                        DateTime td = DateTime.Now;
                        int year = td.Year;

                        string stryear = Convert.ToString(year);

                        int month = i.Date.Value.Month;

                        if (month==1)
                        {
                           // comparedate("2020/01/01", "2020/", i.Date.Value);

                        }
                        else if(month==2)
                        {

                        }else if(month==3)
                        {

                        }else if(month==4)
                        {

                        }else if(month==5)
                        {

                        }else if(month==6)
                        {

                        }
                        else if (month == 7)
                        {

                        }
                        else if (month == 8)
                        {

                        }
                        else if (month == 9)
                        {

                        }
                        else if (month == 10)
                        {

                        }
                        else if (month == 11)
                        {

                        }
                        else if (month == 12)
                        {

                        }




                    }


                    













                    return View(obj);
                }
                else
                {
                    return RedirectToAction("login");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("login");

            }

           

           
        }
       

        public IActionResult Index()
        {
            int totalproducts = 0;

            try
            {
                string sellerphone = HttpContext.Session.GetString("phone");
                if(sellerphone!=null)
                {
                    int sid = db2.Usertable.Where(a => a.PhoneNo.Equals(sellerphone)).Select(b => b.UserId).SingleOrDefault();
                    totalproducts = db2.Product.Where(a => a.UserId.Value.Equals(sid)).Count();




                    IndexViewModel indexview = new IndexViewModel
                    {
                        
                    };
                    
                    return View();
                }
                else
                {
                    return RedirectToAction("login");
                }
            }catch(Exception e)
            {
                return RedirectToAction("login");

            }


           
        }
        
        public IActionResult signup()
        {
            ViewBag.key = "1";

            return View();

        }

        public IActionResult Signup2()
        {
            ViewBag.error = "none";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Signup2(string number)
        {
            number = number;
            // number=number.Substring(1, 10);
            //  number= String.Concat("92", number);
            //  number = "92" + number;
            Random n = new Random();
            String randomnumber = (n.Next(100000, 999999)).ToString();
            HttpContext.Session.SetString("Varification", randomnumber);
            String MessageText = "Your varification code is " + randomnumber;

            String URI = "http://sendpk.com" +
"/api/sms.php?" +
"username=" + "923338767324" +
"&password=" + "Ahmadbutt321" +
"&sender=" + "QuickMart" +
"&mobile=" + number + "&message=" + Uri.UnescapeDataString(MessageText); // Visual Studio 10-15 

            try
            {



                WebRequest req = WebRequest.Create(URI);
                WebResponse resp = await req.GetResponseAsync();
                var sr = new System.IO.StreamReader(resp.GetResponseStream());

                String result = sr.ReadToEnd().Trim();
                if (result.Contains("OK"))
                    return RedirectToAction("Entercode");
                else if (result.Contains("7"))
                {
                    ViewBag.error = "7";
                    //return View();
                    return Json("Your Phone Number is invalid");
                }
                else if (result.Contains("8"))
                {
                    System.Diagnostics.Debug.WriteLine("SMS Balance Out");
                    return Json("SmS Balance out");
                    // Console.Write("SMS Balance Out");
                }

            }
            catch (WebException ex)
            {
                var httpWebResponse = ex.Response as HttpWebResponse;
                if (httpWebResponse != null)
                {
                    switch (httpWebResponse.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            return Json("404:URL not found :" + URI);
                            break;
                        case HttpStatusCode.BadRequest:
                            return Json("400:Bad Request");
                            break;
                        case HttpStatusCode.OK:
                            return Json("ok");
                        default:
                            return Json(httpWebResponse.StatusCode.ToString());







                    }
                }
            }
            return null;



            



        }

        public IActionResult Login()
        {



            ViewData["Message"] = "Your application description page.";

            return View();
        }// get request of UploadProducts
        [HttpPost]
        public IActionResult Loginn(Usertable seller)
        {
            Usertable obj = db2.Usertable.Where(a => a.PhoneNo == seller.PhoneNo && a.Password == seller.Password).SingleOrDefault();
            if (obj != null)
            {

                // seller.UserType = "S";
                HttpContext.Session.SetString("phone", seller.PhoneNo);
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.error = "Your Phone or password is incurrect";

                return View("login");

            }


        }


        [HttpGet]
        public IActionResult Upload()
        {
            if (HttpContext.Session.GetString("phone") != null)
            {
                return View("Uploadproducts");
            }
            else
            {
                return RedirectToAction("login");
            }
            //ViewData["Message"] = "Your application description page.";


        }
        [HttpPost]
        public IActionResult Upload(IList<IFormFile> img)

        {

            String email = HttpContext.Session.GetString("email");
            Usertable user = db2.Usertable.Where(a => a.Email == email).SingleOrDefault();
            int userid = user.UserId;
            Product product;



            string path = host2.WebRootPath + "/img/products/";
            foreach (var i in img)
            {
                product = new Product()
                {
                    ProductId = 0
                };
                // product.ProductId = 0;


                string fname = Path.GetFileName(i.FileName);

                i.CopyTo(new System.IO.FileStream(path + fname, System.IO.FileMode.Create));

                String image = "/img/products/" + fname;
                product.ProductImage = image;
                product.UserId = userid;

                db2.Product.Add(product);
                db2.SaveChanges();



            }


            return RedirectToAction("productlist");

        }
        public IActionResult GetAddproductPartial()
        {
            if (HttpContext.Session.GetString("phone") != null)
            {
                return PartialView("Addproductpartial");
            }
            else
            {

                return Json(new { result = "notlogin" });
            }

        }

        public IActionResult getproductlistpartial()
        {
            try
            {

                if (HttpContext.Session.GetString("phone") != null)
                {


                    String email = HttpContext.Session.GetString("email");
                    Usertable Seller = db2.Usertable.Where(a => a.Email == email).SingleOrDefault();
                    int userid = Seller.UserId;
                    IList<Product> list = db2.Product.Where(a => a.UserId == userid).ToList();

                    // HttpContext.Session.Remove("procode");
                    // IList<Product> list = pro.Products.ToList();
                    //return View("testlist", list);   
                    // return View(list);
                    return PartialView("productlistpartial",list);
                }
                else
                {
                  return Json(new { result = "notlogin" });
                }
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
                return PartialView("productlistpartial");
            }







           
        }
        public List<Title_proid> getproductswithorderid(int orderid)
        {
            List<Title_proid> list = new List<Title_proid>();
           List<int> proids= db2.OrderItems.Where(a => a.OrderId.Equals(orderid)).Select(a => a.ProId.Value).ToList();
            foreach(int pid in proids)
            {
               list.Add( getproductwithid(pid));
            }
            return list;

        }
        public Title_proid  getproductwithid(int i)
        {
            var result = db2.Product.Where(a => a.ProductId.Equals(i)).Select(a => new { pid = a.ProductId, title = a.Title }).SingleOrDefault();
            Title_proid titem = new Title_proid();


            titem.id = result.pid;
                titem.title = result.title;

            return titem;
        }
        
       public IActionResult  temp()
        {
            List<Product> p = db2.Product.ToList();
            var l = p.Take(5);

           
            return View("templist",l);
        }

        public IActionResult OrderList()
        {
            string sellerphone = HttpContext.Session.GetString("phone");
            if (sellerphone != null)
            {
                int sid = db2.Usertable.Where(a => a.PhoneNo.Equals(sellerphone)).Select(b => b.UserId).SingleOrDefault();

                List<int> orderids = db2.OrderItems.Where(a => a.SellerId.Equals(sid)).Select(b => b.OrderId.Value).ToList();
                List<int> removedublicate = orderids.Distinct().ToList();
                List<Order> _orlist = getorderwithorderids(removedublicate);

                var data = (from Order in _orlist
                            join Usertable in db2.Usertable
                            on Order.UserId equals Usertable.UserId
                            
                            
                            select new
                            {
                                orderid = Order.OrderId,
                                userid = Usertable.UserId,
                                name = Usertable.Firstname + " " + Usertable.Lastname,
                                phone = Usertable.PhoneNo,
                                address = Usertable.Address,
                                total = Order.Total,
                                state = Order.Status,
                                date = Order.Date


                            }).ToList();

                List<OrderViewModel> olist = new List<OrderViewModel>();

                OrderViewModel obj;

                foreach (var i in data)
                {
                    obj = new OrderViewModel();
                    obj.Orderid = i.orderid;
                    obj.userid = i.userid;
                    obj.user_name = i.name;
                    obj.phone = i.phone;
                    obj.address = i.address;
                    obj.total = i.total.Value;
                    obj.status = i.state;
                    obj.orderdate = i.date.Value;

                    olist.Add(obj);

                }

            
            List<OrderViewModel> olist2 = new List<OrderViewModel>();
            List<Title_proid> list = null;

            foreach (OrderViewModel i in olist)
            {
                list = getproductswithorderid(i.Orderid);
                i.products = list;

                olist2.Add(i);



            }








            return View(olist2);
            }
            else
            {
                return RedirectToAction("login");
            }
            
         //   List<Order> list = db2.Order.ToList();
           
        }

        public IActionResult Productlist()
        {
            try
            {

                if (HttpContext.Session.GetString("phone") != null)
                {
                    

                    String phone = HttpContext.Session.GetString("phone");
                    Usertable Seller = db2.Usertable.Where(a => a.PhoneNo == phone).SingleOrDefault();
                    int userid = Seller.UserId;
                    IList<Product> list = db2.Product.Where(a => a.UserId == userid).ToList();

                    // HttpContext.Session.Remove("procode");
                    // IList<Product> list = pro.Products.ToList();
                    //return View("testlist", list);   
                    return View(list);
                }
                else
                {
                    return RedirectToAction("login");
                }
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
                return View();
            }
        }
        public IActionResult Addproduct()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Addproduct(Product obj, IFormFile img)
        {
            if (HttpContext.Session.GetString("phone") != null)
            {
                try
                {
                    // string[] array = new string[3];
                    // int a = 0;
                    string path = host2.WebRootPath + "/img/products/";
                    /* foreach (var i in img)
                     {


                         string fname = Path.GetFileName(i.FileName);
                         i.CopyTo(new System.IO.FileStream(path + fname, System.IO.FileMode.Create));
                         array[a] = "/img/products/" + fname;
                         a++;
                     }




                     obj.ProductImg1 = array[0];
                     obj.ProductImg2 = array[1];
                     obj.ProductImg3 = array[2];*/

                  /*  using var image = Image.Load(img.OpenReadStream()){
                        image.Mutate(x => x.Resize(256, 256));
                        image.Save("...");

                    }
                   */




                    string fname = Path.GetFileName(img.FileName);

                   
                    


                    img.CopyTo(new System.IO.FileStream(path + fname, System.IO.FileMode.Create));

                    obj.ProductImage = "/img/products/" + fname;
                
                    String email = HttpContext.Session.GetString("email");
                    int userid = db2.Usertable.Where(a => a.Email == email).Select(a => a.UserId).SingleOrDefault();
                    obj.UserId = userid;

                    db2.Product.Add(obj);

                    db2.SaveChanges();
                    return RedirectToAction("Productlist");

                }
                catch (Exception e)
                {
                    ViewBag.error = e.Message;
                    return View("errorpage");

                    // View

                }
                //HttpContext.Session.SetString("proid", obj.ProductCode);
                //    Console.WriteLine("product id:" + obj.ProductId);
                //ViewBag.pro = "procode";







            }
            else
            {
                return RedirectToAction("login");

            }
        }
        public IActionResult EditProduct(int proid)
        {
            if (HttpContext.Session.GetString("phone") != null)
            {
                Product p = db2.Product.Where(a => a.ProductId == proid).SingleOrDefault();
                return View(p);
            }
            else
            {

               return RedirectToAction("Login");
            }
                

        }
        [HttpPost]
        public IActionResult Editproduct(Product obj,IFormFile img)
        {
            
           
            string path = host2.WebRootPath + "/img/products/";

            string fname = Path.GetFileName(img.FileName);
            img.CopyTo(new System.IO.FileStream(path + fname, System.IO.FileMode.Create));
            
             

            
            Product p = db2.Product.Where(a => a.ProductId == obj.ProductId).SingleOrDefault();
            p.Title = obj.Title;
            p.Code = obj.Code;
            p.Description = obj.Description;
            p.ProductImage = "/img/products/" + fname;
            p.Price = obj.Price;
            p.Category = obj.Category;
            db2.SaveChanges();
            return RedirectToAction("Productlist");
        }


        [HttpGet]
        public IActionResult DeleteProduct(int proid)
        {
            if (HttpContext.Session.GetString("phone")!=null)
            {
                Product p = db2.Product.Where(a => a.ProductId == proid).SingleOrDefault();
                return View(p);
            }else
            {
                return RedirectToAction("Login");
            }


        }

        [HttpGet]
        public IActionResult ConfirmDelete(int proid)
        {
            if (HttpContext.Session.GetString("phone") != null)
            {
                try
                {
                    List<OrderItems> list = db2.OrderItems.Where(a => a.ProId.Equals(proid)).ToList();
                    db2.OrderItems.RemoveRange(list);
                    db2.SaveChanges();
                }catch(NullReferenceException e)
                {

                }
                try
                {
                    List<CartProdescriptionPivot> cpdp = db2.CartProdescriptionPivot.Where(a => a.ProductId.Equals(proid)).ToList();
                    db2.CartProdescriptionPivot.RemoveRange(cpdp);
                    db2.SaveChanges();



                        }catch(NullReferenceException e)
                {

                }
                try
                {
                    Product obj = db2.Product.Where(a => a.ProductId.Equals(proid)).SingleOrDefault();
                    db2.Product.Remove(obj);
                    db2.SaveChanges();
                }catch(Exception e)
                {
                    ViewBag.error = e.Message;
                    return View("errorpage");
                }
                return RedirectToAction("Productlist");
            }
            else { return RedirectToAction("login"); }
        }

        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");

        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        public IActionResult about()
        {
            //  System.Net.HttpWebRequest httpWebRequest;
            TempData["data"] = "about";
            return View();
        }


        public IActionResult Registeration()
        { 

            return View();


        }
        [HttpPost]
        public JsonResult validateshopname(string shopname)
        {
            try
            {
               Usertable OBJ= db2.Usertable.Where(a => a.ShopName == shopname).SingleOrDefault();
                if(OBJ!=null)
                {
                    return Json("1");
                }
                else
                {
                    return Json("0");
                }
            }
            catch(Exception e)
            {
                return Json("Error:" + e.Message);
            }

          //  return Json("sds");
        }
        public IActionResult errorpage()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Entercode()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Entercode(String code)
        {
            try
            {

                String tcode = HttpContext.Session.GetString("Varification");
                if(code.Equals(tcode))
                {
                   return RedirectToAction("Registeration");
                }
                else
                {
                    ViewBag.error = "NotMatch";
                    return View();
                }
            }catch(NullReferenceException e)
            {
                ViewBag.error = e.Message;
                return View();

            }

           // return View();
        }

        
        [HttpPost]
       public IActionResult forgetpassword(string Email )
        {
           
                ViewBag.email = Email;
                return View();
               // HttpContext.Session.se

                //db2.Usertable.Where(a=>a.em)
           
        }
        public IActionResult HomePage()
        {
            Usertable o = new Usertable();

            return View(o);
        }
        //[HttpPost]
        //public async Task<IActionResult> about(string n)
        //{
        //    //  System.Net.HttpWebRequest httpWebRequest;
        //    TempData["data"] = "about";
        //    return Json("ahmed");
        //}

    }
}