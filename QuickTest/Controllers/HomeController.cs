using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;



using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using tryproj1._1.Models;
using tryproj1._1.Models.ModelsForAndroid;






using Newtonsoft.Json.Linq;
using System.Net.Http;
using QuickTest.Models;
using QuickTest.Models.ReturnModelForAndroid;
using QuickTest.Models.ModelsForAndroid;
using SixLabors.ImageSharp;
using Microsoft.AspNetCore.Http.Internal;
using QuickTest.Models.MvcModels;

namespace QuickTest.Controllers
{
    public class HomeController : Controller
    {

        DB_A56F7A_Quickmart2Context db = null;
        IHostingEnvironment host = null;
       public HomeController(DB_A56F7A_Quickmart2Context context,IHostingEnvironment obj)
        {
            db = context;
            host = obj;
          

        }

        public IActionResult  Index()
        {
            // IList<Product> prolist=db.Product.ToList();
            //return Json(prolist);
            return View();

        }
        [HttpPost]
        public JsonResult SearchNearByProducts(string idsarray)
        {

            IList<String> sellerids = JsonConvert.DeserializeObject<List<String>>(idsarray);
            List<int> int_sellerids = new List<int>();
            foreach(string s in sellerids)
            {

                int_sellerids.Add(int.Parse(s));


            }
         List<Product> prolist=   db.Product.Where(a => int_sellerids.Contains(a.UserId.Value)).ToList();

            return Json(prolist);

        }
    
        public JsonResult getsizecolors(Userid_Proid_Raiting_Feedback_RecieveModel model)
        {
            List<ProductSpecification> pd = new List<ProductSpecification>();
            try
            {
                 pd = db.ProductSpecification.Where(a => a.ProductId.Equals(model.proid)).ToList();
              
                
                    return Json(pd);

               
            }
            catch (Exception e)
            {
                StringResult result = new StringResult();
                result.error = e.Message;
                result.Strresult = "error";
                return Json(result);
            }
        }
        public JsonResult getuserInfo(UserIdRecieveModelForAndroid Model)
        {
            try {
                Usertable user = db.Usertable.Where(a => a.UserId.Equals(Model.userid)).SingleOrDefault();

                ShippingDetail_ModelAndroid userdetail = new ShippingDetail_ModelAndroid();
                if (user.Address != null)
                {
                    userdetail.address = user.Address;
                    userdetail.contact = user.PhoneNo;
                   // userdetail.name = user.Firstname + " "+user.Lastname;

                    return Json(userdetail);
                }
                else
                {
                    userdetail.address = "null";
                   return Json(userdetail);

                }
            } catch(Exception e)
            {
                ShippingDetail_ModelAndroid userdetail = new ShippingDetail_ModelAndroid();
                userdetail.address = "null";
                return Json(userdetail);
            }
       

        }


        [HttpPost]
        public JsonResult SaveOrderForDidectBuy(UserIdRecieveModelForAndroid obj)
        {
            try
            {

                int orderid = 0;

                Order order = new Order();
                order.Status = "Not Ready";
                order.UserId = obj.userid;
                order.Date = DateTime.Now;
                order.Total = obj.total;

                //order.SellerId = obj.sellerid;
                db.Order.Add(order);
                db.SaveChanges();
                try
                {
                    orderid = order.OrderId;
                }
                catch (NullReferenceException e)
                {
                    StringResult result2 = new StringResult();
                    result2.error = e.Message;
                    result2.Strresult = "Order id is Null";
                    return Json(result2);
                }

            


                //IList<String> idList = JsonConvert.DeserializeObject<List<String>>(idsarray);

                OrderItems transaction;
                List<ProductModelForAndroid> list = JsonConvert.DeserializeObject<List<ProductModelForAndroid>>(obj.orderedproducts);
                foreach (ProductModelForAndroid i in list)
                {
                    ProductSpecification spec = db.ProductSpecification.Where(a => a.ProductId.Equals(obj.proid) && a.ProductColor.Equals(obj.color) && a.ProductSize.Equals(obj.size)).SingleOrDefault();

                    transaction = new OrderItems();
                    transaction.UserId = obj.userid;
                    transaction.OrderId = orderid;
                    transaction.ProId = i.productId;
                    transaction.Quantity = i.userQuantity;
                    transaction.SellerId = i.sellerId;
                    transaction.Viewed = 0;
                    transaction.SpecificationId = spec.SpecificationId;
                    double utotal = transaction.Quantity.Value * i.price;
                    transaction.unitTotal = utotal;
                   // ProductSpecification spec = db.ProductSpecification.Where(a => a.SpecificationId.Equals(i.specificationid)).SingleOrDefault();
                    //  Product p= db.Product.Where(a => a.ProductId.Equals(i.productId)).SingleOrDefault();
                    // p.Quantity = p.Quantity - i.userQuantity;
                    spec.Quantity = spec.Quantity - i.userQuantity;
                    db.OrderItems.Add(transaction);
                    db.SaveChanges();




                }


               // int cartid = db.Cart.Where(a => a.UserId.Equals(obj.userid)).Select(b => b.CartId).SingleOrDefault();
                //List<CartProdescriptionPivot> cpdp = db.CartProdescriptionPivot.Where(b => b.CartId.Equals(cartid)).ToList();
                //db.CartProdescriptionPivot.RemoveRange(cpdp);
                db.SaveChanges();

                StringResult result = new StringResult();
                result.Strresult = "OrderSaved";
                return Json(result);
            }
            catch (Exception e)
            {
                StringResult result = new StringResult();
                result.error = e.Message;
                result.Strresult = "OrderNOtAdded";
                return Json(result);


            }


        }




        [HttpPost]
        public JsonResult SaveOrder(UserIdRecieveModelForAndroid obj)
        {
            try
            {
               
                int orderid = 0;

                Order order = new Order();
                order.Status = "Not Ready";
                order.UserId = obj.userid;
                order.Date = DateTime.Now;
                order.Total = obj.total;
               
                //order.SellerId = obj.sellerid;
                db.Order.Add(order);
                db.SaveChanges();
                try
                {
                    orderid = order.OrderId;
                }catch(NullReferenceException e)
                {
                    StringResult result2 = new StringResult();
                    result2.error = e.Message;
                    result2.Strresult = "Order id is Null";
                    return Json(result2);
                }
                

                  


                //IList<String> idList = JsonConvert.DeserializeObject<List<String>>(idsarray);

                OrderItems transaction;
                List<ProductModelForAndroid> list = JsonConvert.DeserializeObject<List<ProductModelForAndroid>>(obj.orderedproducts);
                foreach (ProductModelForAndroid i in list)
                {
                    transaction = new OrderItems();
                    transaction.UserId =obj.userid;
                    transaction.OrderId = orderid;
                    transaction.ProId =i.productId;
                    transaction.Quantity = i.userQuantity;
                    transaction.SellerId = i.sellerId;
                    transaction.Viewed = 0;
                    transaction.SpecificationId = i.specificationid;
                   double utotal = transaction.Quantity.Value * i.price;
                    transaction.unitTotal = utotal;
                    ProductSpecification spec = db.ProductSpecification.Where(a => a.SpecificationId.Equals(i.specificationid)).SingleOrDefault();
                    //  Product p= db.Product.Where(a => a.ProductId.Equals(i.productId)).SingleOrDefault();
                    // p.Quantity = p.Quantity - i.userQuantity;
                    spec.Quantity = spec.Quantity - i.userQuantity;
                    db.OrderItems.Add(transaction);
                    db.SaveChanges();
                   



                }


                int cartid = db.Cart.Where(a => a.UserId.Equals(obj.userid)).Select(b => b.CartId).SingleOrDefault();
                List<CartProdescriptionPivot> cpdp=db.CartProdescriptionPivot.Where(b => b.CartId.Equals(cartid)).ToList();
                db.CartProdescriptionPivot.RemoveRange(cpdp);
                db.SaveChanges();

                StringResult result = new StringResult();
                result.Strresult = "OrderSaved";
                return Json(result);
            }catch(Exception e)
            {
                StringResult result = new StringResult();
                result.error = e.Message;
                result.Strresult = "OrderNOtAdded";
                return Json(result);


            }

           
        }
        [HttpPost]
        public JsonResult SaveShippingDetail(UserIdRecieveModelForAndroid obj)
        {
            try
            {
                String shipping_details=obj.shippingdetail;
                Usertable customer=db.Usertable.Where(a => a.UserId.Equals(obj.userid)).SingleOrDefault();
                customer.ShippingDetail = obj.shippingdetail;

         ShippingDetail_ModelAndroid shippingdeatil=  JsonConvert.DeserializeObject<ShippingDetail_ModelAndroid>(obj.shippingdetail);
                customer.Address = shippingdeatil.address;
                
                db.SaveChanges();
                StringResult result = new StringResult();
                result.Strresult = "SuccessfullySave";
                return Json(result);


            }catch(Exception e)
            {
                StringResult result = new StringResult();
                result.Strresult = "Notsaved";
                result.error = e.Message;
                return Json(result);

            }

            
        }

        [HttpPost]
        public double GetAvgRatingOfProduct(Userid_Proid_Raiting_Feedback_RecieveModel obj)
        {
            try
            {
                List<double?> list = db.Review.Where(a => a.ProductId.Equals(obj.proid)).Select(b => b.RatingStars).ToList();

                List<double> list2 = new List<double>();
                foreach(double? i in list)
                {
                    if(i!=null)
                    list2.Add(i.Value);
                }



                if (list2.Count>0)
                {

                    double sum = 0.0;
                    double avg = 0.0;
                    foreach (double i in list)
                    {
                        sum = sum + i;
                    }
                    avg = sum / list.Count;
                    if (avg >= 4.75 && avg < 5.0)
                        avg = 5.0;
                    else if (avg >= 4.5 && avg < 4.75)
                        avg = 4.5;
                    else if (avg >= 4.25 && avg < 4.5)
                        avg = 4.5;
                    else if (avg >= 4.0 && avg < 4.25)
                        avg = 4.0;
                    else if (avg >= 3.75 && avg < 4.0)
                        avg = 4.0;
                    else if (avg >= 3.5 && avg < 3.75)
                        avg = 3.5;
                    else if (avg >= 3.25 && avg < 3.5)
                        avg = 3.5;
                    else if (avg >= 3.0 && avg < 3.25)
                        avg = 3.0;
                    else if (avg >= 2.75 && avg < 3.0)
                        avg = 3.0;
                    else if (avg >= 2.5 && avg < 2.75)
                        avg = 2.5;
                    else if (avg >= 2.25 && avg < 2.5)
                        avg = 2.5;
                    else if (avg >= 2.0 && avg < 2.25)
                        avg = 2.0;
                    else if (avg >= 1.75 && avg < 2.0)
                        avg = 2.0;
                    else if (avg >= 1.5 && avg < 1.75)
                        avg = 1.5;
                    else if (avg >= 1.25 && avg < 1.5)
                        avg = 1.5;
                    else if (avg >= 1.0 && avg < 1.25)
                        avg = 1.0;
                    else if (avg >= 0.75 && avg < 1.0)
                        avg = 1.0;
                    else if (avg >= 0.5 && avg < 0.75)
                        avg = 0.5;
                    else if (avg >= 0.25 && avg < 0.5)
                        avg = 0.5;
                    else
                        avg = 0.0;

                    return avg;
                    //  return Json(avg);
                }
                else
                {
                    return 0.0;
                }
            }
            catch (Exception e)
            {
                return 0.0;

            }


        }
        /*
       public JsonResult  GetAvgRatingOfProduct(Userid_Proid_Raiting_Feedback_RecieveModel obj)
        {
            try
            {
                List<double?> list = db.Review.Where(a => a.ProductId.Equals(obj.proid)).Select(b => b.RatingStars).ToList();

                double sum = 0.0;
                double avg = 0.0;
               foreach(double i in list)
                {
                    sum = sum + i;
                }
                avg = sum / list.Count;
                return Json(avg);
            } catch(Exception e)
            {
                return Json(e.Message);

            }


        }*/
        [HttpPost]
        public JsonResult SaveFeedback(Userid_Proid_Raiting_Feedback_RecieveModel obj)
        {
            try
            {

               Review r= db.Review.Where(a => a.UserId.Equals(obj.userid) && a.ProductId.Equals(obj.proid)).SingleOrDefault();
                if (r == null)
                {

                    Review review = new Review();
                    review.ProductId = obj.proid;
                    review.UserId = obj.userid;
                    review.RatingStars = obj.rating;
                    review.Feedback = obj.feedback;
                    db.Review.Add(review);
                    db.SaveChanges();

                }
                else
                {
                    r.RatingStars = obj.rating;
                    db.SaveChanges();
                }
                Userid_Proid_Raiting_Feedback_RecieveModel temp = new Userid_Proid_Raiting_Feedback_RecieveModel();
                temp.proid = obj.proid;
                double avg=GetAvgRatingOfProduct(temp);
                Product p = db.Product.Where(a => a.ProductId.Equals(obj.proid)).SingleOrDefault();
                p.AvgRating = avg;
                db.SaveChanges();

                StringResult result = new StringResult();
                result.Strresult = "Feedback added with Rating";
                return Json(result);
            }catch(Exception e)
            {
                StringResult result = new StringResult();
                result.Strresult = e.Message;
                return Json(result);

            }

        }


        [HttpPost]
        public JsonResult GetCartQuantities(UserIdRecieveModelForAndroid obj)
        {
            try
            {
               Cart c= db.Cart.Where(a => a.UserId.Equals(obj.userid)).SingleOrDefault();
                if(c!=null)
                {
                    int?[] quantities = db.CartProdescriptionPivot.Where(a => a.CartId.Equals(c.CartId)).Select(a => a.Quantity).ToArray();
                    String [] final_quantities = new String[quantities.Length];
                    int index = 0;
                    foreach(int? i in quantities)
                    {
                        if(i.Equals(null))
                        {
                            final_quantities[index] = "1".ToString();
                        }
                        else
                        {
                            final_quantities[index] = i.ToString();
                        }
                        index++;
                    }
                    return Json(final_quantities);
                }
                else
                {
                    StringResult result = new StringResult();
                    result.Strresult = "NotLogin Or cart atleast one product";
                    return Json(result);
                }
            }catch(Exception e)
            {
                StringResult result = new StringResult();
                result.Strresult = e.Message;
                return Json(result);

            }

        }
        
        [HttpPost]
        public JsonResult SaveQuantityInCart(UserIdRecieveModelForAndroid obj)
        {
           
            // int proid = obj.proid;
            // int userid = obj.userid;
            //int quantity = obj.quantity;
            try
            {
                 Cart c = db.Cart.Where(a => a.UserId.Equals(obj.userid)).SingleOrDefault();
                
                if (c != null)
                {
                    
                    CartProdescriptionPivot cpdp = db.CartProdescriptionPivot.Where(a => a.ProductId.Equals(obj.proid) && a.CartId.Equals(c.CartId) && a.SpecificationId.Equals(obj.specificationId)).SingleOrDefault();
                   
                
                    cpdp.Quantity = obj.quantity;

                   
                     db.SaveChanges();
                    StringResult result = new StringResult();
                    result.Strresult = "SaveSuccessFully";
                    return Json(result);
                   
                }
                else
                {
                    StringResult result2 = new StringResult();
                    result2.Strresult = "LoginFirst";
                    return Json(result2);


                }
                
            }catch(Exception e)
            {
                StringResult result = new StringResult();
                result.Strresult = e.Message;
                return Json(result);
            }


          //  return Json("proid:"+proid+",userid:"+userid+",quantity:"+quantity);

        }

        /*  public JsonResult GetProductQuantity(UserIdRecieveModelForAndroid obj)
          {
              try
              {
                  Product p = db.Product.Where(a => a.ProductId.Equals(obj.proid)).SingleOrDefault();
                  int quantity=4;
                  if (p.Quantity.HasValue)
                      quantity = p.Quantity.Value;
                  //   int? quantity = p.Quantity;
                  String strquantity=quantity.ToString();
                  StringResult str = new StringResult();
                  str.Strresult = strquantity;
                  return Json(str);

              }catch(Exception e)
              {
                  StringResult str = new StringResult();
                  str.Strresult = "4";
                  str.error = e.Message;
                  return Json(str);

              }

          }


  */


        public double getprisewithproid(int proid)
        {
            double prise = db.Product.Where(a => a.ProductId.Equals(proid)).Select(b => b.Price).SingleOrDefault().Value;
            return prise;
        }

        public JsonResult justRunQuery()
        {
            try
            {
                IList<OrderItems> list = db.OrderItems.ToList();
                foreach (OrderItems i in list)
                {

                    double prise = getprisewithproid(i.ProId.Value);
                    i.unitTotal = i.Quantity * prise;
                    db.OrderItems.Update(i);
                    db.SaveChanges();

                }
                return Json("success");
            }
            catch (Exception e)
            {
                return Json(e.Message);

            }

        }

        //for android
        [HttpGet]
        public JsonResult GetOrders()
        {


            try
            {

                IList<Order> list = db.Order.Where(a => a.UserId.Equals(2)).ToList();
                return Json(list);

            }
            catch (Exception e)
            {


                return Json(e.Message);
            }

        }


       [HttpPost]
        public JsonResult  GetOrdersOfUser(UserIdRecieveModelForAndroid obj)
        {
            try
            {

                IList<Order> list = db.Order.Where(a => a.UserId.Equals(obj.userid)).ToList();
  
                return Json(list);
                // string json=JsonConvert.SerializeObject<IList<Order>>(list);
            }
            catch (Exception e)
            {


                return Json(e.Message);
            }

        }
        
        [HttpGet]
        public JsonResult GetOrderItems()
        {

           // Product product =db.Product.Where(a => a.ProductId.Equals(28)).SingleOrDefault();

               IList<Product> products = new List<Product>();

               IList<OrderItems> list = db.OrderItems.Where(a => a.OrderId.Equals(1)).ToList();

               foreach (OrderItems i in list)
               {
                   products.Add(db.Product.Where(a => a.ProductId.Equals(i.ProId)).SingleOrDefault());

               }
            list.Clear();
            return Json(products);



        }
        [HttpPost]
        public JsonResult GetOrderItems(UserIdRecieveModelForAndroid obj)
        {
            try
            {
               // IList<Product> list = db.Product.Where(a => a.Category.Equals("Trending")).ToList();
                //return Json(list);
                
                IList<Product> products = new List<Product>();

                //  IList<OrderItems> list = db.OrderItems.Where(a =>a.OrderId.Equals(obj.orderid)).ToList();
                IList<OrderItems> list = db.OrderItems.Where(a => a.OrderId.Equals(obj.orderid)).ToList();


               /* int[] proids = new int[list.Count];
                int index = 0;
                foreach(OrderItems i in list)
                {
                   proids[index]= db.Product.Where(a => a.ProductId.Equals(i.ProId)).Select(b => b.ProductId).SingleOrDefault();
                    index++;
                }*/


               foreach(OrderItems i in list )
                {
                    Product prod = db.Product.Where(a => a.ProductId.Equals(i.ProId)).SingleOrDefault();
                    prod.Quantity = i.Quantity;
                    prod.Price = i.unitTotal;
                    prod.OrderItems = null;
                    products.Add(prod);
                
                }
               
                return Json(products);
                

            }
            catch(Exception e)
            {

                return Json(e.Message);
            }
        
        }

        [HttpPost]
        public JsonResult GetProfile(UserIdRecieveModelForAndroid obj)
        {
            try
            {

                Usertable user = db.Usertable.Where(a => a.UserId.Equals(obj.userid)).SingleOrDefault();
                String usernamee = null;
                if (!user.Logo.Equals(null))
                {

                   /* int i=user.Email.IndexOf('@');
                    if(i>0)
                    {
                           usernamee   =  user.Email.Substring(0,i);
                    }*/
                    StringResult result = new StringResult();
                    result.Strresult = user.Logo;
                 //   result.username = usernamee;
                  
                    return Json(result);
                }
                else
                {
                    StringResult result = new StringResult();
                    result.Strresult = "error";
                    result.error = "Not Uploaded";
                    return Json(result);
                }

            }catch(Exception e)
            {
                StringResult result = new StringResult();
                result.Strresult = "error";
                result.error = e.Message;
                return Json(result);
            }
        }

        [HttpPost]
        public JsonResult UploadProfile(UserIdRecieveModelForAndroid obj)
        {
            try
            {
                String path = host.WebRootPath + "/img/products/" + obj.filename;

                System.IO.File.WriteAllBytes(path, Convert.FromBase64String(obj.bitmapstr));

                using (var stream = System.IO.File.OpenRead(path))
                {
                    var formFileName = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = "application/octet-stream"
                    };
                }
                Usertable usertable = db.Usertable.Where(a => a.UserId.Equals(obj.userid)).SingleOrDefault();
                usertable.Logo = "/img/products/" + obj.filename;
                db.SaveChanges();

                /*string usernamee = null; 
                int i = usertable.Email.IndexOf('@');
                if (i > 0)
                {
                  usernamee =usertable.Email.Substring(0, i);
                }*/
                StringResult result = new StringResult();
                result.Strresult ="updated";
                result.logo = usertable.Logo;
                //result.username = usernamee;

                return Json(result);
                /*StringResult result = new StringResult();
                result.Strresult = "updated";
                return Json(result);
                */
/*
                Usertable usertable = db.Usertable.Where(a => a.UserId.Equals(obj.userid)).SingleOrDefault();
                usertable.Logo = obj.bitmapstr;
                db.SaveChanges();
                StringResult result = new StringResult();
                result.Strresult = "updated";
                

                return Json(result);*/
            }catch(Exception e) { 
            
                StringResult result = new StringResult();
                result.Strresult = e.Message;
                
                return Json(result);
                

                
            }

            // IFormFile file;
            //file = obj.bitmapstr;
            /* 


             List<byte> splitBytes = new List<byte>();
             string byteString = "";

             foreach (var chr in obj.bitmapstr)
             {
                 byteString += chr;

                 if (byteString.Length == 3)
                 {
                     splitBytes.Add(Convert.ToByte(byteString));
                     byteString = "";
                 }
             }

             if (byteString != "")
                 splitBytes.AddRange(Encoding.ASCII.GetBytes(byteString));

             using (var ms = new MemoryStream(splitBytes.ToArray()))
             {

                 Image.From.(ms);

                 SixLabors.ImageSharp.Image img = SixLabors.ImageSharp.Image.FromStream()

                 //  ms.Write(splitBytes.ToArray(), 0, splitBytes.ToArray().Length);


                 //   var img = System.Drawing.Image.FromStream(ms);

                 //do something with image.
             }
             */


        }



        //for Android

        //public  JsonResult GetOrder_and_Items(UserIdRecieveModelForAndroid obj)
        //{
        //    IList<OrderItems> oneorderitems;
        //    IList<OrderItems> itemstotal = new List<OrderItems>();
        //    try
        //    {
        //        IList<Order> list = db.Order.Where(a => a.UserId.Equals(obj.userid)).ToList();

        //        foreach(Order i in list)
        //        {
        //            oneorderitems = db.OrderItems.Where(a => a.OrderId.Equals(i.OrderId)).ToList();

        //            foreach(OrderItems j in oneorderitems)
        //            {
        //                itemstotal.Add(j);

        //            }

        //        }
        //        return Json(list);

        //    }
        //    catch(Exception e)
        //    {
        //        return Json(e.Message);

        //    }

        //}
        [HttpPost]
        public  JsonResult AddToCart(AddToCartModelAndroid value)

        {
            try
            {
                int userid = int.Parse(value.userId);
                int proid = int.Parse(value.productId);
                int sellerid= int.Parse(value.sellerid);
                string color = value.color;
                string size = value.size;


                int specid = db.ProductSpecification.Where(a => a.ProductId.Equals(proid) && a.ProductColor.Equals(color) && a.ProductSize.Equals(size)).Select(b => b.SpecificationId).SingleOrDefault();

                Cart obj = db.Cart.Where(a => a.UserId.Equals(userid)).SingleOrDefault();
                if(obj!=null)
                {

                    //CartProdescriptionPivot cpdp=db.CartProdescriptionPivot.Where(a=>a.CartId.Equals(obj.CartId))
                 CartProdescriptionPivot chek_if_alreadyexists =   db.CartProdescriptionPivot.Where(a => a.ProductId.Equals(proid) && a.CartId.Equals(obj.CartId)&&a.SpecificationId.Equals(specid)).SingleOrDefault();
                    if (chek_if_alreadyexists == null)
                    {
                        CartProdescriptionPivot cpdp = new CartProdescriptionPivot();
                        cpdp.CartId = obj.CartId;
                        cpdp.ProductId = proid;
                        cpdp.Quantity = 1;
                        cpdp.SellerId = sellerid;
                        cpdp.SpecificationId = specid;
                        db.CartProdescriptionPivot.Add(cpdp);
                        db.SaveChanges();
                        StringResult result = new StringResult();
                        result.Strresult = "Added";
                        return Json(result);

                    }
                    else
                    {
                        StringResult result4 = new StringResult();
                        result4.Strresult = "AllreadyAdded";
                        return Json(result4);

                    }
                   
                    //return Json("productadded");

                }
                else
                {
                    Cart cardobj = new Cart();
                    cardobj.UserId = userid;
                    db.Cart.Add(cardobj);
                 db.SaveChanges();
                    CartProdescriptionPivot cpdp2 = new CartProdescriptionPivot();
                    cpdp2.CartId = cardobj.CartId;
                    cpdp2.ProductId = proid;
                    cpdp2.Quantity = 1;
                    cpdp2.SpecificationId = specid;
                    db.CartProdescriptionPivot.Add(cpdp2);
                    db.SaveChanges();

                    StringResult result2 = new StringResult();
                    result2.Strresult = "Added";
                    return Json(result2);

                    //  return Json("added");

                }

            }catch(Exception e)
            {
                StringResult result3 = new StringResult();
                result3.Strresult = e.Message;
                return Json(result3);

            }


        }
        public  int GetUserQuantityofProduct(int pid,int cid)
        {
            try{
                int? userquantity = db.CartProdescriptionPivot.Where(a => a.ProductId.Equals(pid) && a.CartId.Equals(cid)).Select(b => b.Quantity).SingleOrDefault();
                return (int)userquantity;
            }
            catch(Exception e)
            {
                return 1;
            }
            

        }


        //[HttpPost]
        //public JsonResult LoadUserCartProducts(UserIdRecieveModelForAndroid obj)
        //{

        //    try
        //    {
        //        int cartid;


        //        //  Cart cart=db.Cart.Where(a => a.UserId.Equals(obj.userid)).SingleOrDefault();
        //        try
        //        {
        //            cartid = db.Cart.Where(a => a.UserId.Equals(obj.userid)).Select(b => b.CartId).SingleOrDefault();



        //        }
        //        catch (NullReferenceException e)
        //        {

        //            StringResult result = new StringResult();
        //            result.Strresult = "loginfirst";
        //            return Json(result);

        //        }




        //        //int ids[]=


        //        // List<CartProdescriptionPivot> cpdp = db.CartProdescriptionPivot.Where(b => b.CartId.Equals(cartid)).ToList();


        //        int?[] ids = db.CartProdescriptionPivot.Where(a => a.CartId.Equals(cartid)).Select(c => c.ProductId).ToArray();
        //        // int?[] quantities = db.CartProdescriptionPivot.Where(b => b.CartId.Equals(cartid)).Select(d => d.Quantity).ToArray();

        //        List<Product> plist = db.Product.Where(a => ids.Contains(a.ProductId)).ToList();
        //        int[] quantities = new int[plist.Count];
        //        int i = 0;
        //        foreach (Product p in plist)
        //        {
        //            quantities[i] = GetUserQuantityofProduct(p.ProductId, cartid);

        //            i++;

        //        }
        //        List<CartModelForAndroid> cartlist = new List<CartModelForAndroid>();
        //        CartModelForAndroid temp;
        //        int index = 0;
        //        foreach (Product p in plist)
        //        {
        //            temp = new CartModelForAndroid
        //            {
        //                ProductId = p.ProductId,
        //                Price = p.Price,
        //                ProductImage = p.ProductImage,
        //                Title = p.Title,
        //                UserQuantity = quantities[index],
        //                SellerQuantity = p.Quantity,
        //                SellerId = p.UserId

        //            };
        //            index++;
        //            cartlist.Add(temp);
        //        }

        //        /*
        //        foreach (Product p in plist )
        //        {
        //            temp = new CartModelForAndroid
        //            {
        //                ProductId = p.ProductId,
        //                Price = p.Price,
        //                ProductImage = p.ProductImage,
        //                Title = p.Title,
        //                UserQuantity = quantities[index],
        //                SellerQuantity = p.Quantity
        //            };
        //            index++;
        //            cartlist.Add(temp);
        //        }
        //        */
        //        return Json(cartlist);
        //        /* foreach (int i in ids)
        //         {
        //             db.Product.Where(a=>a.)
        //         }
        //         */
        //        //db.Product.Where(ids.Contains(a))



        //    }
        //    catch (Exception e)
        //    {
        //        return Json(e.Message);
        //    }





        //}
        public int[]  getspecificationids(int pid,int cartId)
        {

         //   db.CartProdescriptionPivot.Where(a=>a.ProductId.Equals(pid) && a.)
            int[] specids = db.ProductSpecification.Where(a => a.ProductId.Equals(pid)).Select(b => b.SpecificationId).ToArray();

            return specids;
        }
        public JsonResult temp3()
        {
            int userid = 2;
            var k = (from dpdp in db.CartProdescriptionPivot
                     from sptbl in db.ProductSpecification.Where(b => dpdp.SpecificationId.Equals(b.SpecificationId))
                     from prods in db.Product.Where(a => dpdp.ProductId.Equals(a.ProductId) && dpdp.CartId.Value.Equals(userid))
                     select new
                     {
                         prods.ProductId,
                         prods.ProductImage,
                         prods.Code,
                         prods.Category,
                         prods.Description,
                         prods.Price,
                         prods.Title,
                         prods.UserId,
                         dpdp.SpecificationId,
                         userquantity = dpdp.Quantity,
                         sellerquantity = sptbl.Quantity

                     }).ToList();

            return Json(k);


        }

        [HttpPost]
        public JsonResult LoadUserCartProducts(UserIdRecieveModelForAndroid obj)
        {
           
            try
            {
                int cartid;


                //  Cart cart=db.Cart.Where(a => a.UserId.Equals(obj.userid)).SingleOrDefault();
                try
                {
                    cartid = db.Cart.Where(a => a.UserId.Equals(obj.userid)).Select(b => b.CartId).SingleOrDefault();

                    

                }
                catch(NullReferenceException e)
                {

                    StringResult result = new StringResult();
                    result.Strresult = "loginfirst";
                    return Json(result);

                }

                var t_c_list = (from dpdp in db.CartProdescriptionPivot
                                from sptbl in db.ProductSpecification.Where(b => dpdp.SpecificationId.Equals(b.SpecificationId))

                                from prods in db.Product.Where(a => dpdp.ProductId.Equals(a.ProductId) && dpdp.CartId.Equals(cartid))
                                select new
                                {
                                    prods.ProductId,
                                    prods.ProductImage,
                                    prods.Code,
                                    prods.Category,
                                    prods.Description,
                                    prods.Price,
                                    prods.Title,
                                    prods.UserId,
                                    dpdp.SpecificationId,
                                    userquantity = dpdp.Quantity,
                                    dpdp.SellerId,
                                    sellerquantity = sptbl.Quantity

                                }).ToList();


              
            List<CartModelForAndroid> cartlist = new List<CartModelForAndroid>();

                CartModelForAndroid temp;


                foreach (var p in t_c_list)
                {
                    ProductSpecification spec = db.ProductSpecification.Where(a => a.SpecificationId.Equals(p.SpecificationId)).SingleOrDefault();
                    temp = new CartModelForAndroid
                    {
                        ProductId = p.ProductId,
                        Price = p.Price,
                        ProductImage = p.ProductImage,
                        Title = p.Title,
                        UserQuantity = p.userquantity,
                        SellerQuantity = p.sellerquantity,
                        specificationid=p.SpecificationId,
                        color=spec.ProductColor,
                        size=spec.ProductSize,
                        SellerId=p.SellerId
                    };

                    cartlist.Add(temp);
                }




                
                return Json(cartlist);
                   /* foreach (int i in ids)
                    {
                        db.Product.Where(a=>a.)
                    }
                    */
                    //db.Product.Where(ids.Contains(a))
                
              

            }catch(Exception e)
            {
                return Json(e.Message);
            }


            


        }
        [HttpPost]
        public JsonResult DeleteProductFromCart(UserIdRecieveModelForAndroid obj)
        {

            try
            {
                Cart cart = db.Cart.Where(a => a.UserId.Equals(obj.userid)).SingleOrDefault();
                int cartid = cart.CartId;
               CartProdescriptionPivot cpdp= db.CartProdescriptionPivot.Where(a => a.CartId.Equals(cartid) && a.ProductId.Equals(obj.proid) &&a.SpecificationId.Equals(obj.specificationId) ).SingleOrDefault();
                db.Remove(cpdp);
                db.SaveChanges();



/* 
                Product product = db.Product.Where(a => a.ProductId.Equals(obj.proid)).SingleOrDefault();
                // return Json(product);
                db.Product.Remove(product);
                db.SaveChanges();*/

                StringResult result = new StringResult();
                result.Strresult = "Done";
                return Json(result);
            }
            catch(Exception e)
            {
                StringResult result2 = new StringResult();
                result2.Strresult = "Error:"+e.Message;
                return Json(result2);
            }
           
        }
        //for Android
        [HttpPost]
            public JsonResult getprowithsellerid(CategorymodelAndroid obj)
            {

            try {

                double? avg = 0.0;
                
               int ID = int.Parse(obj.sellerid);
                Userid_Proid_Raiting_Feedback_RecieveModel temp;

                List<Product> list = db.Product.Where(a => a.UserId.Equals(ID) && a.Category.Contains(obj.category) &&a.Quantity>0).ToList();
                try
                {
                    int[] proids = new int[list.Count];
                    int index = 0;
                    foreach (Product p in list)
                    {
                        proids[index] = p.ProductId;
                        index++;
                    }
                    foreach (int pid in proids)
                    {
                        temp = new Userid_Proid_Raiting_Feedback_RecieveModel();
                        temp.proid = pid;
                        avg = GetAvgRatingOfProduct(temp);
                        Product p = db.Product.Where(a => a.ProductId.Equals(pid)).SingleOrDefault();
                        p.AvgRating = avg;
                        // db.Product.Update(p);
                        db.SaveChanges();

                    }
                   
                }catch(Exception e)
                {
                    return Json(e.InnerException);
                }
                return Json(list);

            } catch(Exception e) {
               return Json(e.Message);
            }
           

            }

        static double toRadians(
             double angleIn10thofaDegree)
        {
            // Angle in 10th 
            // of a degree 
            return (angleIn10thofaDegree *
                           Math.PI) / 180;
        }
        static double distance(double lat1,
                               double lat2,
                               double lon1,
                               double lon2)
        {

            // The math module contains  
            // a function named toRadians  
            // which converts from degrees  
            // to radians. 
            lon1 = toRadians(lon1);
            lon2 = toRadians(lon2);
            lat1 = toRadians(lat1);
            lat2 = toRadians(lat2);

            // Haversine formula  
            double dlon = lon2 - lon1;
            double dlat = lat2 - lat1;
            double a = Math.Pow(Math.Sin(dlat / 2), 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Pow(Math.Sin(dlon / 2), 2);

            double c = 2 * Math.Asin(Math.Sqrt(a));

            // Radius of earth in  
            // kilometers. Use 3956  
            // for miles 
            double r = 6371;

            // calculate the result 
            return (c * r);
        }
        

        public  Tuple<List<Usertable>,List<Distance_User>> GetnearBysellerswithBoundry(IList<Usertable> list, LatLongModel userlocation)
        {
            List<Usertable> unsortedlist = new List<Usertable>();
           // List<Usertable> NearByUnsortedList = new List<Usertable>();
            List<Distance_User> temp = new List<Distance_User>();
           
            double d_kilometer = 0.0;
            double d_kilometerTemp = 0.0;
            try
            {


                double d_latitudeuser = double.Parse(userlocation.latitude);
                double d_longitudeuser = double.Parse(userlocation.longitude);
                // double[] kms =new  double[list.Count];
                temp = new List<Distance_User>();
                Distance_User tempobj = null;

                //int index = 0;
                double d_latitudeseller = 0.0;
                double d_longitudeseller = 0.0;
                foreach (Usertable i in list)
                {
                    tempobj = new Distance_User();
                    d_latitudeseller = decimal.ToDouble(i.Latitude.Value);

                    d_longitudeseller = decimal.ToDouble(i.Longitude.Value);

                    d_kilometerTemp = distance(d_latitudeuser, d_latitudeseller, d_longitudeuser, d_longitudeseller);
                     d_kilometer = double.Parse(String.Format("{0:0.0}", d_kilometerTemp));
                    if (d_kilometer <= 10 && d_kilometer >= -10)
                    {
                        tempobj.distance_km = d_kilometer;
                        tempobj.userid = i.UserId;


                        temp.Add(tempobj);
                    }



                }
                

                Usertable u1 = new Usertable();
                foreach (Distance_User d in temp)// shifting into usertablemodel
                {

                    u1 = list.Where(a => a.UserId.Equals(d.userid)).SingleOrDefault();
                    //System.Diagnostics.Debug.WriteLine("Userid:" + u1.UserId);
                    unsortedlist.Add(u1);

                }
                


            }
            catch (Exception e)
            {
                List<Usertable> i = new List<Usertable>();
                  Usertable obj = new Usertable();
                obj.Firstname = e.Message;
               i.Add(obj);
                 var t2 = new Tuple<List<Usertable>, List<Distance_User>>(i, temp);
                return t2;
            }

              var t = new Tuple<List<Usertable>, List<Distance_User>>(unsortedlist, temp);
            return t;



              }//Sorting into  Accending Order
            List<User_Km_Pivot> GetSortedArray(List<Usertable> tlist,List<Distance_User> userid_ds)
        {
           
            List<User_Km_Pivot> UsersWithKm = new List<User_Km_Pivot>();

            Distance_User temp = null;

           for(int i=0;i<userid_ds.Count;i++)
            {
             


                for (int j = i + 1; j < userid_ds.Count; j++)
                {
                   
                    if (userid_ds[i].distance_km > userid_ds[j].distance_km) {
                        temp = new Distance_User();
                        temp = userid_ds[i];
                        userid_ds[i] = userid_ds[j];
                        userid_ds[j] = temp;
                            }
                }



            }


            Usertable tuser = new Usertable();

            User_Km_Pivot tobj = null;
            foreach (Distance_User i in userid_ds)
            {
                tobj = new User_Km_Pivot();
                tuser =tlist.Where(a => a.UserId.Equals(i.userid)).SingleOrDefault();

                tobj.UserId = tuser.UserId;
                tobj.ShopName = tuser.ShopName;
                tobj.Logo = tuser.Logo;
                tobj.Address = tuser.Address;
                tobj.d_kilometers = i.distance_km;
                tobj.contact = tuser.PhoneNo;


                UsersWithKm.Add(tobj);
            }
            return UsersWithKm;
        }
       
       [HttpPost]///for android
        public JsonResult getsellers(LatLongModel obj)
        {
            IList<Usertable> ulist = new List<Usertable>();
            List<User_Km_Pivot> finalsellers = new List<User_Km_Pivot>();
            
            List<Usertable> unsortedlist = new List<Usertable>();
            //  List<Distance_User> userid_distance = new List<Distance_User>();
            try
            {
                ulist = db.Usertable.Where(a => a.UserType.Equals("S") && !a.SellerDetails.Equals(0) && !a.IsBlocked.Value.Equals(1)).ToList();


                   var t =GetnearBysellerswithBoundry(ulist,obj); // unsortedlist of user  with  distance kilometer

                
                 
              //  Distance_User ds2 = userid_and_Kms.ElementAt(0);
                //int ut = ds2.userid;
              // u1=ulist.Where(a => a.UserId.Equals(userid_and_Kms[0].userid)).SingleOrDefault();
                



                // List<Distance_User> ds = GetnearBysellerswithBoundry(ulist, obj);// unsortedlist of user  with  distance kilometer


                   finalsellers   =  GetSortedArray(t.Item1,t.Item2);
                
                double rating = 0.0;
                foreach(User_Km_Pivot seller in finalsellers)
                {
                rating =getratingofseller(seller.UserId);
                    seller.rating = rating;


                }
                


                return Json(finalsellers);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }


        }
        public double getratingofseller(int user_id)
        {
            try
            {
               List<Product> list = db.Product.Where(a => a.UserId.Equals(user_id)).ToList();
                 double count = 1.0;
                  count = list.Count;
                  double sumofavgrating = 0.0;
                double avg = 0.0;

                  foreach (Product p in list)
                  {
                      sumofavgrating = sumofavgrating + p.AvgRating.Value;


                  }

                avg = sumofavgrating / count;
                avg=Math.Round(avg, 2);

                if (avg >= 4.75 && avg < 5.0)
                    avg = 5.0;
                else if (avg >= 4.5 && avg < 4.75)
                    avg = 4.5;
                else if (avg >= 4.25 && avg < 4.5)
                    avg = 4.5;
                else if (avg >= 4.0 && avg < 4.25)
                    avg = 4.0;
                else if (avg >= 3.75 && avg < 4.0)
                    avg = 4.0;
                else if (avg >= 3.5 && avg < 3.75)
                    avg = 3.5;
                else if (avg >= 3.25 && avg < 3.5)
                    avg = 3.5;
                else if (avg >= 3.0 && avg < 3.25)
                    avg = 3.0;
                else if (avg >= 2.75 && avg < 3.0)
                    avg = 3.0;
                else if (avg >= 2.5 && avg < 2.75)
                    avg = 2.5;
                else if (avg >= 2.25 && avg < 2.5)
                    avg = 2.5;
                else if (avg >= 2.0 && avg < 2.25)
                    avg = 2.0;
                else if (avg >= 1.75 && avg < 2.0)
                    avg = 2.0;
                else if (avg >= 1.5 && avg < 1.75)
                    avg = 1.5;
                else if (avg >= 1.25 && avg < 1.5)
                    avg = 1.5;
                else if (avg >= 1.0 && avg < 1.25)
                    avg = 1.0;
                else if (avg >= 0.75 && avg < 1.0)
                    avg = 1.0;
                else if (avg >= 0.5 && avg < 0.75)
                    avg = 0.5;
                else if (avg >= 0.25 && avg < 0.5)
                    avg = 0.5;
                else
                    avg = 0.0;




                return avg;
              //  return Json(list);
                    



            } catch(Exception e)
            {
                return  0.0;
                  
                //return Json(0.0);

            }
        }
        public double getdistancefromseller(LatLongModel obj)
        {
           Usertable user =db.Usertable.Where(a => a.UserId.Equals(6)).SingleOrDefault();
            double d_latitudeuser = double.Parse(obj.latitude);
           double d_longitudeuser = double.Parse(obj.longitude);
            double d_latitudeseller = 0.0;

            double d_longitudeseller = 0.0;

            d_latitudeseller = decimal.ToDouble(user.Latitude.Value);

            d_longitudeseller = decimal.ToDouble(user.Longitude.Value);


           double d= distance(d_latitudeuser, d_latitudeseller, d_longitudeuser, d_longitudeseller);
            double d2=double.Parse(String.Format("{0:0.0}", d));
            return d2;
            
        }
        [HttpGet]//for android
        public JsonResult getlanlong()
        {
            try
            {
                IList<Usertable> ulist = db.Usertable.Where(a => a.UserType.Equals("S")).ToList();
                return Json(ulist);
            }
            catch(Exception e)
            {
               return Json(e.Message);
            }
         
        }
        [HttpGet]
        public JsonResult getuserwithid(String userid)
        {
            try
            {
               int UID= Int32.Parse(userid);
                Usertable obj=db.Usertable.Where(a => a.UserId == UID).SingleOrDefault();
                return Json(obj);

            }catch(Exception e)
            {
                return Json(e.Message);
                //Console.Out("error " + e.Message);
            }


        }
        // for android
        [HttpPost]
        public JsonResult getproductswithproId(String idsarray)
        {
            IList<Product> pro = new List<Product>();
            int[] ids = new int[idsarray.Length];
            int i1 = 0;
            IList<String> idList = JsonConvert.DeserializeObject<List<String>>(idsarray);
            foreach (String i in idList)
            {
                Product obj = db.Product.Where(a => a.ProductId == Int32.Parse(i)).SingleOrDefault();
                pro.Add(obj);
                //ids[i1] = Int32.Parse(i);
                //i1++;
            }
            
            return Json(pro);
            
        }
        [HttpPost]// for android 
        public JsonResult login(Usertable e)
        { 
            try
            {
                Usertable e2 = db.Usertable.Where(a => a.PhoneNo == e.PhoneNo && a.Password == e.Password).SingleOrDefault();
                if (e2 != null)
                {
                  
                       
                        loginModelAndroid obj = new loginModelAndroid();
                        obj.userid = e2.UserId;
                        obj.result = "success";
                        return Json(obj);
                    
                    
                   
                       //IList<Product> items= db.Product.Where(a => a.UserId == e2.UserId).ToList();

                        //return Json(items);
                    
                        }
                       else
                            {
                    loginModelAndroid obj3 = new loginModelAndroid();
                    obj3.result = "unauthorized";
                    obj3.userid = 0;
                    return Json(obj3);
                }
            }
            catch (NullReferenceException ex)
            {
                Console.Write(ex.Message);
                loginModelAndroid obj4 = new loginModelAndroid();
                obj4.result = ex.Message;
                obj4.userid = 0;
                return Json(obj4);
            }

        }
        //for android
        [HttpPost]
        public  JsonResult getproducts(Usertable u)
        {
            
            try
            { //Usertable Seller = db.Usertable.Where(a => a.Email ==u.Email).SingleOrDefault();
              //int userid = Seller.UserId;
              // db.SaveChanges();
               int userid= db.Usertable.Where(a => a.Email == u.Email).Select(a => a.UserId).SingleOrDefault();
                IList<Product> list = db.Product.Where(a => a.UserId == userid).ToList();
                 // list = db.Product.ToList();

                return Json(list);
            } catch(Exception e)
            {
                return Json("error:"+e.Message);
            }
           
            //  IList<Product> list = db.Product.Where(a => a.UserId == userid).ToList();
          

        }
        [HttpGet]
        public JsonResult getrecommendedproduct()
        {
            try
            {

                List<Product> list = db.Product.Where(a => a.AvgRating >= 2.5).OrderByDescending(b => b.AvgRating).Take(6).ToList();


                //    List<Product> list = db.Product.Where(a => a.Category.Equals("Recommend")).ToList();
                return Json(list);
            }
            catch (Exception e)
            {
                return Json(e.Message);

            }
        }
        [HttpPost]
        public JsonResult getrecommendedproduct(UserIdRecieveModelForAndroid obj)
        {
            try
            {





                List<Product> finallist = new List<Product>();

                if(obj.modeldata.Equals("")||obj.modeldata.Equals(" "))
                {
                    List<Product> templist= db.Product.Take(8).ToList();
                    finallist.AddRange(templist);
                    return Json(finallist);
                }


                List<ModelData> list = JsonConvert.DeserializeObject<List<ModelData>>(obj.modeldata);

                if (list.Count == 1)
                {
                   ModelData oneitem= list.OrderByDescending(b => b.NOV).Take(1).SingleOrDefault();
                   List<Product> templist= db.Product.Where(a => a.Category.Equals(oneitem.category)).Take(7).ToList();
                    finallist.AddRange(templist);
                    return Json(finallist);
                }
                else if (list.Count == 2)
                {
                    List<ModelData> modeltemplist = list.OrderByDescending(a => a.NOV).Take(2).ToList();
                    List<Product> templist = null;
                    foreach (ModelData m in modeltemplist)
                    {
                        templist = new List<Product>();

                        templist = db.Product.Where(a => a.Category.Equals(m.category)).Take(3).ToList();
                        finallist.AddRange(templist);
                       

                    }
                    return Json(finallist);

                }
                else {
                    List<ModelData> sortedlist = list.OrderByDescending(a => a.NOV).Take(3).ToList();

                    ModelData firstlargest = sortedlist.OrderByDescending(b => b.NOV).First();
                    ModelData MIDDLE = sortedlist.ElementAt(1);
                    ModelData smallest = sortedlist.OrderByDescending(b => b.NOV).Last();

                    List<Product> templist= db.Product.Where(a => a.Category.Equals(firstlargest.category)).Take(7).ToList();
                    List<Product> templist2 = db.Product.Where(a => a.Category.Equals(smallest.category)).Take(4).ToList();
                    List<Product> templist3 = db.Product.Where(a => a.Category.Equals(MIDDLE.category)).Take(2).ToList();




                    finallist.AddRange(templist);
                    finallist.AddRange(templist2);
                    finallist.AddRange(templist3);
                    return Json(finallist);




                        }

            }
            catch (Exception e)
            {
                return Json(e.Message);

            }
        }
        //for android
        [HttpGet]
        public JsonResult getrecommendedpro()
        {
            IList<Product> list;
            try
            {
                 list = db.Product.Where(a => a.Category == "Recommend").ToList();
                return Json(list);

            }
            catch(Exception e)
            {
                return Json(e.Message);
            }
           
        }
       // for android
        [HttpGet]
        public JsonResult gettrendingpro()
        {
          //  IList<Product> list;
            try
            {
               // list = db.Product.Where(a => a.Category == "Trending").ToList();


                var query =
       (from item in  db.OrderItems
        group item.Quantity by item.Pro into g
        orderby g.Sum() descending
        select g.Key).Take(5);
                return Json(query);


            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
          //  return Json(list);
        }
        [HttpPost]
        public JsonResult Checkuser(Usertable user)
        {
            try
            {
                Usertable u = db.Usertable.Where(a => a.PhoneNo.Equals(user.PhoneNo) && a.UserType.Equals("c")).SingleOrDefault();
                if(u!=null)
                {

                    StringResult result = new StringResult();
                    result.Strresult = "allreadyregistered";
                    return Json(result);
                }
                else
                {
                    StringResult result = new StringResult();
                    result.Strresult = "NewUser";
                    return Json(result);

                }

            }
            catch(Exception e)
            {

                StringResult result = new StringResult();
                result.Strresult = "NULL";
                result.error = e.Message;
                return Json(result);

            }


        }[HttpPost]
        public JsonResult UpdatePassword(ForgetPassword obj)
        {
            try
            {
              Usertable user=  db.Usertable.Where(a => a.PhoneNo.Equals(obj.phoneNo) && a.UserType.Equals("C")).SingleOrDefault();
                user.Password = obj.newpassword;
                db.SaveChanges();
                StringResult result = new StringResult();
                result.Strresult = "success";
                return Json(result);

            }catch(Exception e)
            {
                StringResult result = new StringResult();
                result.Strresult = "null";
                result.error = e.Message;
                return Json(result);
            }
        }
        [HttpPost]
        public JsonResult forgetpassword(Usertable user)
        {
            try
            {
                Usertable u = db.Usertable.Where(a => a.PhoneNo.Equals(user.PhoneNo) && a.UserType.Equals("c")).SingleOrDefault();
                if(u!=null)
                {
                    StringResult result = new StringResult();
                    result.Strresult = u.Password;
                    return Json(result);


                }
                else
                {
                    StringResult result = new StringResult();
                    result.Strresult = "NotRegistered";
                    return Json(result);
                }
            }catch(Exception e)
            {

                StringResult result = new StringResult();
                result.error = e.Message;
                result.Strresult = "NULL";
                return Json(result);
            }


        }



        [HttpPost]// for Android 
        public JsonResult signup(Usertable e)
        {
            try
            {
                Usertable e2 = db.Usertable.Where(a => a.PhoneNo.Equals(e.PhoneNo)&& a.UserType.Equals("C") ).SingleOrDefault();
                if (e2 != null)
                {
                    StringResult temp = new StringResult();
                    temp.Strresult = "alreadyregistered";
                    temp.userid = 0;
                    return Json(temp);

                    
                }
               else
                {
                   
                    
                        db.Usertable.Add(e);
                        db.SaveChanges();
                       
                        StringResult temp = new StringResult();
                        temp.Strresult = "registered";
                        temp.userid = e.UserId;
                        return Json(temp);


                    
                }
            }
            catch (NullReferenceException ex)
            {
                Console.Write(ex.Message);
                StringResult temp = new StringResult();
                temp.Strresult = ex.Message;
                temp.userid = 0;
                return Json(temp);
            }

        }
   /// //////////////////////////////////////////////////////////////////////////
   


        
        


          
        
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

        public IActionResult Error()
        {
            return View();
        }
        public IActionResult testlist()
        {
            TempData["data"] = "testlist";
            return View();
        }
        public IActionResult about()
        {
          //  System.Net.HttpWebRequest httpWebRequest;
            TempData["data"] = "about";
            return View();
        }
        [HttpPost]
        public  async Task<IActionResult> about(string n)
        {
            //  System.Net.HttpWebRequest httpWebRequest;
            TempData["data"] = "about";
            return Json("ahmed");
        }
        public PartialViewResult getloginpartial()
        {
            return PartialView("Loginn");
        }
        public PartialViewResult getsignuppartial()
        {
            return PartialView("SignUpp");
        }
        [HttpPost]
        public IActionResult code(Modelforajax number)
        {
            Random n = new Random();
            String randomnumber = (n.Next(100000, 999999)).ToString();
            return Json(randomnumber);
        }

        public IActionResult justtry()
        {
            return View();
        }
        [HttpPost]
        public IActionResult justtry(Usertable ooj)
        {
            return View();
        }

    }
}
