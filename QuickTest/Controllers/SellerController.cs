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
using Newtonsoft.Json;
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
        public List<Order> getorderswithstart_end_Date(string start,string end,List<Order> list)
        {
            //  DateTime sdate = new DateTime("");  "2020/01/01","2020/01/31"
            DateTime sdate = DateTime.ParseExact(start, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            DateTime enddate = DateTime.ParseExact(end, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            //  DateTime enddate = new DateTime(2020, 7, 30,00,00,00);
      List<Order> filteredorders= list.Where((a => a.Date.Value.Date >= sdate && a.Date.Value.Date <= enddate)).ToList();

           // List<Order> ol = db2.Order.Where(a => a.Date.Value.Date >= sdate && a.Date.Value.Date <= enddate).ToList();
           
          // List<Order> ol= db2.Order.Where(a => a.Date.Value >= sdate && a.Date.Value <= enddate).ToList();

            return filteredorders;
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
        public double getsalewithorderId(int oid,int s_id)
        {
            
            double sum_of_unittotals = 0.0;
            List<double> unittotals=db2.OrderItems.Where(a => a.OrderId.Equals(oid) && a.SellerId.Equals(s_id)).Select(b => b.unitTotal.Value).ToList();
            foreach(double s in unittotals)
            {
                sum_of_unittotals = sum_of_unittotals + s;

            }

            return sum_of_unittotals;
        }
        public double getsalesoforders(List<Order> list)
        {
            string phone = HttpContext.Session.GetString("phone");
           int sid= db2.Usertable.Where(a => a.PhoneNo.Equals(phone) && a.UserType.Equals("S")).Select(b => b.UserId).SingleOrDefault();


            double sales = 0.0;
            foreach(Order o in list)
            {
                sales= sales +getsalewithorderId(o.OrderId,sid);
            }
            return sales;
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
        public JsonResult test()
        {


            DateTime td = DateTime.Now;
            int year = td.Year;

            string stryear = Convert.ToString(year);

            return Json(stryear+"/01/31");
        }
        public IActionResult Dashboard()
        {

            List<Order> jan = new List<Order>();

            List<Order> fab = new List<Order>();
            List<Order> march = new List<Order>();
            List<Order> april = new List<Order>();
            List<Order> may = new List<Order>();

            List<Order> june = new List<Order>();
            List<Order> july = new List<Order>();
            List<Order> aug = new List<Order>();
            List<Order> sep= new List<Order>();
            List<Order> oct = new List<Order>();
            List<Order> nov = new List<Order>();
            List<Order> dec = new List<Order>();
            double[] sale = new double[12];
            string[] months = new string[12];
            //double jansale = 0.0;
            //double fabsale = 0.0;
            //double marchsale = 0.0;
            //double aprilsale = 0.0;
            //double maysale = 0.0;
            //double junesale = 0.0;
            //double julysale = 0.0;
            //double augsale = 0.0;
            //double sepsale = 0.0;
            //double octsale = 0.0;
            //double novsale = 0.0;
            //double decsale = 0.0;
            //months[0] = "Jan";
            //months[1] = "Fab";
            //months[2] = "Mar";
            //months[3] = "Apr";
            //months[4] = "May";
            //months[5] = "June";
            //months[6] = "July";
            //months[7] = "Aug";
            //months[8] = "Sep";
            //months[9] = "Oct";
            //months[10] = "Nov";
            //months[11] = "Dec";
            





            ViewBag.key = "D";
            int totalproducts = 0;
            DashboardViewModel obj = new DashboardViewModel();
            
            try
            {
                string sellerphone = HttpContext.Session.GetString("phone");
                if (sellerphone != null)
                {
                  Usertable user=  db2.Usertable.Where(a => a.PhoneNo.Equals(sellerphone) && a.UserType.Equals("S")).SingleOrDefault();
                    int sid = db2.Usertable.Where(a => a.PhoneNo.Equals(sellerphone) && a.UserType.Equals("S")).Select(b => b.UserId).SingleOrDefault();
                    totalproducts = db2.Product.Where(a => a.UserId.Value.Equals(sid)).Count();              
                    List<OrderItems> olist=db2.OrderItems.Where(a => a.SellerId.Equals(sid)).ToList();

                    

                    double TEarnings = 0.0;
                    foreach(OrderItems i in olist)
                    {
                        TEarnings = TEarnings + i.unitTotal.Value;              
                       

                    }

                    obj.Totalearnings = TEarnings;                               /// Earnings
                    obj.Salesitems = olist.Select(a => a.Quantity.Value).Sum();                           // Sales items
                       
                   // db2.Order.Where(a => a.UserId.Equals(sid)).Count();

                    obj.Totalproducts = totalproducts;                               // Total products
                    Basedashboard basedashboard = new Basedashboard();
                    basedashboard.shopname = user.ShopName;
                    basedashboard.logo = user.Logo;

                    List<int>  orderids = db2.OrderItems.Where(a => a.SellerId.Equals(sid)).Select(b => b.OrderId.Value).ToList();
                       List<int> removedublicate = orderids.Distinct().ToList();
                     int _totalorders=removedublicate.Count();



                    obj.Totalorders = _totalorders;                                         // total orders





                    //// for graph
                    ///
                    db2.SaveChanges();
                    //List<GraphViewModel> graphdata = new List<GraphViewModel>();
                   // GraphViewModel gvmodel;
                    List<Order> _orlist=getorderwithorderids(removedublicate);
                    int index = 0;
                    foreach(Order o in _orlist)
                    {

                        //  o.OrderItems = null;
                        _orlist[index].OrderItems = null;
                        index++;

                    }
                   // return Json(_orlist);
                    

                        DateTime td = DateTime.Now;
                        int year = td.Year;
                   
                        string stryear = Convert.ToString(year);
                    //jan = getorderswithstart_end_Date(stryear+"/08/04", stryear+"/08/31", _orlist);
                  //  return Json(jan);

                    // return Json(stryear);
                    // return Json(stryear + "/01/01");
                       //  jan = getorderswithstart_end_Date("2020/01/01", "2020/01/31", _orlist);

                    /*  foreach(Order o in jan)
                      {
                          o.Status = "sds";
                      }

                     */


                    // comparedate("2020/01/01", "2020/", i.Date.Value);


                    jan =getorderswithstart_end_Date(stryear+"/01/01", stryear+"/01/31",_orlist);
                    fab = getorderswithstart_end_Date((stryear+"/02/01"), stryear+"/02/29", _orlist);

                    march= getorderswithstart_end_Date(stryear+"/03/01", stryear+"/03/31", _orlist);
                    april = getorderswithstart_end_Date(stryear+"/04/01", stryear+"/04/30", _orlist);
                    may = getorderswithstart_end_Date(stryear+"/05/01", stryear+"/05/31", _orlist);
                    june = getorderswithstart_end_Date(stryear+"/06/01", stryear+"/06/30", _orlist);
                    july = getorderswithstart_end_Date(stryear+"/07/01", stryear+"/07/31", _orlist);
                    aug = getorderswithstart_end_Date(stryear+"/08/01", stryear+"/08/31", _orlist);
                    sep = getorderswithstart_end_Date(stryear+"/09/01", stryear+"/09/30", _orlist);
                    oct= getorderswithstart_end_Date(stryear+"/10/01", stryear+"/10/31", _orlist);
                    nov = getorderswithstart_end_Date(stryear+"/11/01", stryear+"/11/30", _orlist);
                    dec = getorderswithstart_end_Date(stryear+"/12/01", stryear+"/12/31", _orlist);




                    //jansale = getsalesoforders(jan);
                    //fabsale = getsalesoforders(fab);
                    //marchsale = getsalesoforders(march);
                    //aprilsale = getsalesoforders(april);
                    //maysale = getsalesoforders(may);
                    //junesale = getsalesoforders(june);
                    //julysale = getsalesoforders(july);
                    //augsale = getsalesoforders(aug);
                    //sepsale = getsalesoforders(sep);
                    //octsale = getsalesoforders(oct);
                    //novsale = getsalesoforders(nov);
                    //decsale = getsalesoforders(dec);

                    sale[0] = getsalesoforders(jan);
                    sale[1] = getsalesoforders(fab);
                    sale[2] = getsalesoforders(march);
                    sale[3] = getsalesoforders(april);
                    sale[4] = getsalesoforders(may);
                    sale[5] = getsalesoforders(june);
                    sale[6] = getsalesoforders(july);
                    sale[7] = getsalesoforders(aug);
                    sale[8] = getsalesoforders(sep);
                    sale[9] = getsalesoforders(oct);
                    sale[10] = getsalesoforders(nov);
                    sale[11] = getsalesoforders(dec);
                   /* int indx = 0;
                    foreach(double d in sale)
                    {
                        gvmodel = new GraphViewModel();
                        gvmodel.dataitem = sale[indx];
                        gvmodel.day = months[indx];
                        graphdata.Add(gvmodel);
                        indx++;

                    }*/










                //    return Json(graphdata);





                    var t = new Tuple<DashboardViewModel,Basedashboard,double[]>(obj, basedashboard,sale);






                    return View(t);
                }
                else
                {
                    return RedirectToAction("login");
                }
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;

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
        public IActionResult temp2()
        {
            Usertable i = new Usertable();

            return View("temp",i);
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

            string s = "0";
            string tempnumber = s + number;

            Usertable user = null;
                 user = db2.Usertable.Where(a => a.PhoneNo.Equals(tempnumber) && a.UserType.Equals("S")).SingleOrDefault();
            if (user != null)
            {
                ViewBag.key = "a";
                TempData["error"] = "This Number is allready registered as Seller";
                return View("signup");
            }
            else
            {



                // number=number.Substring(1, 10);
                //  number= String.Concat("92", number);
                //  number = "92" + number;
                Random n = new Random();
                String randomnumber = (n.Next(100000, 999999)).ToString();


                String MessageText = "Your varification code is " + randomnumber;

                String URI = "http://sendpk.com" +
    "/api/sms.php?" +
    "username=" + "923006103303" +
    "&password=" + "ahmedishtiaq9777" +
    "&sender=" + "QuickMart" +
    "&mobile=" + number + "&message=" + Uri.UnescapeDataString(MessageText); // Visual Studio 10-15 

                try
                {



                    WebRequest req = WebRequest.Create(URI);
                    WebResponse resp = await req.GetResponseAsync();
                    var sr = new System.IO.StreamReader(resp.GetResponseStream());

                    String result = sr.ReadToEnd().Trim();
                    char firstch = result[0];
                    if (result.Contains("OK"))
                    {
                        HttpContext.Session.SetString("Varification", randomnumber);
                        // HttpContext.Session.SetString("phone", number);

                        CookieOptions option = new CookieOptions();
                        option.Expires = DateTime.Now.AddDays(1);
                        Response.Cookies.Append("tempphone", tempnumber, option);
                        //  TempData["phone"] = number;

                        return RedirectToAction("Entercode");
                    }
                    else if (firstch == '7')
                    {

                        ViewBag.error = "7";
                        //return View();
                        //   return Json("Your Phone Number is invalid");
                        return Json(result);
                    }
                    else if (firstch == '8')
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
            }
            return null;



            



        }

        public IActionResult Login()
        {

            ViewBag.error=TempData["error"];

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
        public IActionResult RemoveProduct(int id,int userid)
        {
            try
            {
                if(HttpContext.Session.GetString("phone")!=null)
                {
                   OrderItems orderItems= db2.OrderItems.Where(a => a.UserId.Equals(userid) && a.ProId.Equals(id)).SingleOrDefault();
                    db2.Remove(orderItems);
                    db2.SaveChanges();
                    return RedirectToAction(nameof(OrderList));
                }
                else
                {
                    return RedirectToAction("login");


                }

            }catch(Exception e)
            {
                ViewBag.error = e.Message;
                return View("errorpage");

            }

        }



        [HttpPost]
        public IActionResult EditOrderInfo(OrderViewModel model)
        {
            try
            {
                if(HttpContext.Session.GetString("phone")!=null)
                {
                    Order order=db2.Order.Where(a => a.OrderId.Equals(model.Orderid)).SingleOrDefault();
                    order.Status = model.status;
                    db2.SaveChanges();

                    return RedirectToAction(nameof(OrderList));


                }
                else
                {
                    return RedirectToAction("login");

                }



            }catch(Exception e)
            {
                ViewBag.error = e.Message;
                return View("errorpage");
            }


        }
        public IActionResult EditOrderInfo(int id)
        {
            try
            {
                if(HttpContext.Session.GetString("phone")!=null)
                {
                    string phone=HttpContext.Session.GetString("phone");
                    Usertable user = db2.Usertable.Where(a => a.PhoneNo.Equals(phone) && a.UserType.Equals("S")).SingleOrDefault();

                    Order order = db2.Order.Where(a => a.OrderId.Equals(id)).SingleOrDefault();
                   int userid = db2.Order.Where(a => a.OrderId.Equals(id)).Select(b => b.UserId).SingleOrDefault();
                    string NAME = db2.Usertable.Where(a => a.UserId.Equals(userid)).Select(b => b.Firstname).SingleOrDefault();
                    string address = db2.Usertable.Where(a => a.UserId.Equals(userid)).Select(b => b.Address).SingleOrDefault();

                    OrderViewModel orderViewModel = new OrderViewModel();
                    orderViewModel.Orderid = id;
                    orderViewModel.status = order.Status;
                    orderViewModel.userid = order.UserId;
                    orderViewModel.user_name = NAME;
                    orderViewModel.address = address;
                    orderViewModel.total = order.Total.Value;



                    Basedashboard basedashboard = new Basedashboard();
                    basedashboard.logo = user.Logo;
                    basedashboard.shopname = user.ShopName;

                    var t = new Tuple<OrderViewModel, Basedashboard>(orderViewModel, basedashboard);

                    return View(t);
                    //  db2.OrderItems.Where(a=>a.OrderId.Equals(id)).

                }
                else
                {
                    return RedirectToAction("login");

                }


            }catch(Exception e)
            {
                ViewBag.error = e.Message;
                return View("errorpage");
            }


        }
       
        public IActionResult CancelOrder(int id)
        {
            try
            {
                if(HttpContext.Session.GetString("phone")!=null)
                {
                    List<OrderItems> items= db2.OrderItems.Where(a => a.OrderId.Equals(id)).ToList();
                    db2.RemoveRange(items);
                    db2.SaveChanges();
                    Order order=  db2.Order.Where(a => a.OrderId.Equals(id)).SingleOrDefault();
                    db2.Remove(order);
                    db2.SaveChanges();
                    return RedirectToAction("Orderlist");
                           
                }
                else
                {
                    return RedirectToAction("login");

                }





            }catch(Exception e)
            {
                ViewBag.error = e.Message;
                return View("errorpage");
            }

        }
        public IActionResult OrderDetail(int id)
        {
            try
            {
                if (HttpContext.Session.GetString("phone") != null)
                {

                    string phone = HttpContext.Session.GetString("phone");

                    Usertable user = db2.Usertable.Where(a => a.PhoneNo.Equals(phone) && a.UserType.Equals("S")).SingleOrDefault();
                    List<int> productids = db2.OrderItems.Where(a => a.OrderId.Equals(id) &&a.SellerId.Equals(user.UserId)).Select(b => b.ProId.Value).ToList();

                    List<Product> _prolist = new List<Product>();

                    for (int i = 0; i < productids.Count; i++)
                    {
                        _prolist = _prolist.Concat(db2.Product.Where(a => a.ProductId.Equals(productids[i]))).ToList();
                    }
                    Basedashboard basedashboard = new Basedashboard();
                    basedashboard.logo = user.Logo;
                    basedashboard.shopname = user.ShopName;

                    var t = new Tuple<List<Product>, Basedashboard>(_prolist, basedashboard);

                    return View(t);
                }
                else
                {
                    return RedirectToAction("login");
                }
            }catch(Exception E)
            {
                ViewBag.error = E.Message;
                return View("errorpage");
            }
     


        }
        public IActionResult OrderList()
        {
            string sellerphone = HttpContext.Session.GetString("phone");
            if (sellerphone != null)
            {

              Usertable user=  db2.Usertable.Where(a => a.PhoneNo.Equals(sellerphone) && a.UserType.Equals("S")).SingleOrDefault();
                Basedashboard basedashboard = new Basedashboard();
                basedashboard.logo = user.Logo;
                basedashboard.shopname = user.ShopName;
                db2.SaveChanges();


                int sid = db2.Usertable.Where(a => a.PhoneNo.Equals(sellerphone) && a.UserType.Equals("S")).Select(b => b.UserId).SingleOrDefault();

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



                var t = new Tuple<List<OrderViewModel>,Basedashboard>(olist, basedashboard);




                return View(t);
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
                    Basedashboard basedashboard = new Basedashboard();

                    String phone = HttpContext.Session.GetString("phone");
                    Usertable Seller = db2.Usertable.Where(a => a.PhoneNo == phone && a.UserType.Equals("S")).SingleOrDefault();
                    int userid = Seller.UserId;
                    IList<Product> list = db2.Product.Where(a => a.UserId == userid).ToList();
                    try
                    {
                        basedashboard.logo = Seller.Logo;
                        basedashboard.shopname = Seller.ShopName;
                    }catch(Exception e)
                    {

                    }
                    var t = new Tuple<IList<Product>,Basedashboard>(list, basedashboard);


                    // HttpContext.Session.Remove("procode");
                    // IList<Product> list = pro.Products.ToList();
                    //return View("testlist", list);   
                    return View(t);
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


        public IActionResult tempsignup()
        {

            string phone = null;
            //  string phone = HttpContext.Session.GetString("phone");
            try
            {
                phone = Request.Cookies["tempphone"];
            }catch(Exception e)
            {
                TempData["error"] = "Cookie Expired... Please Enter Phone Number Again";
                return View("signup");
            }
            string step1 = null;
            try
            {
                step1 = Request.Cookies["form1data"];
            }catch(NullReferenceException e)
            {
                ViewBag.error = "Cookie is Expired... Enter data Again";
                
                return View("/Views/Registeration/step1.cshtml");
            }
                Step1 security = JsonConvert.DeserializeObject<Step1>(step1);
                //ViewBag.password = security.password;
                //ViewBag.cnfrompass = security.conformpassword;
                Usertable user = new Usertable();

                user.PhoneNo = phone;
                user.Password = security.password;
            user.SellerDetails = 0;
            user.UserType = "S";
                if (!security.email.Equals(null))
                {
                    user.Email = security.email;
                    // ViewBag.email = security.email;
                }
                db2.Usertable.Add(user);
                db2.SaveChanges();
            HttpContext.Session.SetString("phone", phone);
                return RedirectToAction("Dashboard");

            





        }

        public IActionResult Addproduct()
        {



            if(HttpContext.Session.GetString("phone")!=null)
            {
                Basedashboard basedashboard = new Basedashboard();
              string phone=  HttpContext.Session.GetString("phone");
               Usertable user= db2.Usertable.Where(a => a.PhoneNo.Equals(phone) && a.UserType.Equals("S")).SingleOrDefault();
                int detail = db2.Usertable.Where(a => a.PhoneNo.Equals(phone) && a.UserType.Equals("S")).Select(b => b.SellerDetails.Value).SingleOrDefault();
                if(detail==1)
                {
              basedashboard.shopname  =    user.ShopName;
                    basedashboard.logo=user.Logo;
                    var t = new Tuple<Product,Basedashboard>(new Product(), basedashboard);
                    return View(t);
                }
                else 
                {
                    return RedirectToAction("step2", "Registeration");
                }
            }
            else
            {
                return RedirectToAction("login");
            }



           
        }
        private string Normalize(string text)
        {
            return string.Join("",
                from ch in text
                where char.IsLetterOrDigit(ch) && !char.IsWhiteSpace(ch)
                select ch);
        }
        public string temp3()
        {
            Random r = new Random();
            int Rnumber = r.Next(100);
            string randomnumber = Convert.ToString(Rnumber);
            randomnumber = "_" + randomnumber;

            string sdate = DateTime.Now.ToString();
           string ssdate= Normalize(sdate);


          //  string ssdate = sdate.Trim();
//            string concatstr = String.Concat(ssdate, randomnumber);
            return ssdate;
        }
        [HttpPost]
        public IActionResult Addproduct(Product obj, IFormFile img)
        {
            if (HttpContext.Session.GetString("phone") != null)
            {
                try
                {

                    Random r = new Random();
                    int Rnumber = r.Next(100);
                    string randomnumber = Convert.ToString(Rnumber);
                    randomnumber = "_" + randomnumber;

                    string sdate = DateTime.Now.ToString();

                  string ssdate = Normalize(sdate);
                    string concatstr = String.Concat(ssdate, randomnumber);




                    string path = host2.WebRootPath + "/img/products/";
                    string tempname = Path.GetFileName(img.FileName);
                    string type=Path.GetExtension(tempname);
                    string fname=String.Concat(concatstr, type);


                    var image=Image.Load(img.OpenReadStream());
                    image.Mutate(a => a.Resize(256, 256));
                    image.Save(path + fname);
                    // string[] array = new string[3];
                    // int a = 0;
                  
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




                    
                   
                    


                  //  img.CopyTo(new System.IO.FileStream(path + fname, System.IO.FileMode.Create));

                    obj.ProductImage = "/img/products/" + fname;
                
                    String phone = HttpContext.Session.GetString("phone");
                   int userid = db2.Usertable.Where(a => a.PhoneNo == phone && a.UserType.Equals("S")).Select(a => a.UserId).SingleOrDefault();
                    obj.UserId = userid;

                    db2.Product.Add(obj);

                    db2.SaveChanges();
                    int id = obj.ProductId;
                    HttpContext.Session.SetInt32("proid", id);
                    
                    TempData["proid"] = id;
                    return RedirectToAction("prosizecolor");

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
        public IActionResult prosizecolor()
        {
            try
            {
                if (HttpContext.Session.GetString("phone") != null)
                {

                    if (HttpContext.Session.GetInt32("proid") != null)
                    {
                        TempData["proid"] = HttpContext.Session.GetInt32("proid");
                    }
                    else
                    {
                        return RedirectToAction("productlist");
                    }
                    ProductSpecification p = new ProductSpecification();
                    Basedashboard basedashboard = new Basedashboard();
                    string phone = HttpContext.Session.GetString("phone");
                    Usertable user = db2.Usertable.Where(a => a.PhoneNo.Equals(phone) && a.UserType.Equals("S")).SingleOrDefault();

                    basedashboard.shopname = user.ShopName;
                    basedashboard.logo = user.Logo;
                    var t = new Tuple<ProductSpecification, Basedashboard>(p, basedashboard);
                    return View(t);
                }
                else
                {
                    return RedirectToAction("login");
                }
            }catch(Exception e)
            {

                ViewBag.error = e.Message;
                return View("errorpage");

            }
        }
        [HttpPost]
        public IActionResult prosizecolor(ProductSpecification obj)
        {
            try
            {
                if (HttpContext.Session.GetString("phone") != null)
                {
                    //   obj.ProductId=HttpContext.Session.GetInt32("product_id");

                    db2.ProductSpecification.Add(obj);
                    db2.SaveChanges();
                    TempData["proid"] = HttpContext.Session.GetInt32("proid");
                    return RedirectToAction("prosizecolor");
                }
                else
                {
                    return RedirectToAction("login");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ViewBag.error = e.Message;
                return View();
            }
        }

        public IActionResult EditProduct(int proid)
        {
            if (HttpContext.Session.GetString("phone") != null)
            {
               string phone= HttpContext.Session.GetString("phone");
                /////.................//sidebar data
                Usertable user = db2.Usertable.Where(a => a.PhoneNo.Equals(phone) && a.UserType.Equals("S")).SingleOrDefault();
                Basedashboard basedashboard = new Basedashboard();
                basedashboard.logo = user.Logo;
                basedashboard.shopname = user.ShopName;
                ///////............//Edit object
                Product p = db2.Product.Where(a => a.ProductId == proid).SingleOrDefault();


                var t = new Tuple<Product, Basedashboard>(p, basedashboard);

                return View(t);
            }
            else
            {

               return RedirectToAction("Login");
            }
                

        }

        //public string  temp3()
        //{
        //    string tempname = Path.GetFileName("/img/products/sss.png");
          
        //    string type = Path.GetExtension(tempname);
        //    return type;
        //}
        [HttpPost]
        public IActionResult Editproduct(Product obj,IFormFile img)
        {
            Random r = new Random();
            int Rnumber = r.Next(100);
            string randomnumber = Convert.ToString(Rnumber);
            randomnumber = "_" + randomnumber;

            string sdate = DateTime.Now.ToString();
            string sndate = Normalize(sdate);
            string concatstr = String.Concat(sndate, randomnumber);


            string path = host2.WebRootPath + "/img/products/";
            string fname = null;
            if (img != null)
            {
                

               string tempname = Path.GetFileName(img.FileName);
             
               string type= Path.GetExtension(tempname);
                fname= String.Concat(concatstr, type);
                var image = Image.Load(img.OpenReadStream());
                image.Mutate(a => a.Resize(256, 256));
                image.Save(path + fname);


               // fname = Path.GetFileName(img.FileName);
                //img.CopyTo(new System.IO.FileStream(path + fname, System.IO.FileMode.Create));

            }

            
            Product p = db2.Product.Where(a => a.ProductId == obj.ProductId).SingleOrDefault();
           int id= obj.ProductId;
            p.Title = obj.Title;
            p.Code = obj.Code;
            p.Description = obj.Description;
            if (img != null)
            {
                p.ProductImage = "/img/products/" + fname;
            }
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
                Basedashboard basedashboard = new Basedashboard();
                string phone = HttpContext.Session.GetString("phone");
                Usertable user = db2.Usertable.Where(a => a.PhoneNo.Equals(phone) && a.UserType.Equals("S")).SingleOrDefault();



                Product p = db2.Product.Where(a => a.ProductId == proid).SingleOrDefault();


                basedashboard.shopname = user.ShopName;
                basedashboard.logo = user.Logo;
                var t = new Tuple<Product, Basedashboard>(p, basedashboard);


                return View(t);
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
            ViewBag.key = "2";
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
            TempData["k"] = "1212122323";

            return View();
        }
        [HttpGet]
        public IActionResult Entercode()
        {

           // String phone = TempData["phone"].ToString();
            //TempData["phone"] = phone;
            


            ViewBag.key = "1";
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
                    HttpContext.Session.Remove("Varification");
                    return RedirectToAction("step1", "Registeration");
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