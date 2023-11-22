using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Shopping_Application.IServices;
using Shopping_Application.Models;
using Shopping_Application.Services;
using System.Diagnostics;

namespace Shopping_Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductServices productServices; // Interface
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            productServices = new ProductServices(); // Class Service
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            string content = HttpContext.Session.GetString("Message");
            ViewData["SessionData"] = content;
            return View();
        }

        public IActionResult Test()
        {
            return View();
        }
        public IActionResult Redirect()
        {
            return RedirectToAction("Test"); // Thực hiện điều hướng đến 1 action nào đó
        }

        public IActionResult Show()
        {
            Product product = new Product() { 
                Id = Guid.NewGuid(),
                Name = "Hàng Tồn",
                Price = -10000, 
                AvailableQuantity = 1, 
                Supplier = "Bome", 
                Description = "Ko ai cần", 
                Status = 0 };
            return View(product);
        }

        public IActionResult ShowListProduct()
        {
            List<Product> products = productServices.GetAllProducts();
            // Thêm list này vào Session
            HttpContext.Session.SetString("Product", JsonConvert.SerializeObject(products));
            // JsonConvert.SerializeObject(products) => Chuyển từ List sang Json string
            return View(products);
        }
        public IActionResult Create() // Khi ấn vào Create thì hiển thị View
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product, [Bind] IFormFile imageFile) // Thực hiện chức năng thêm
        {
            var x = imageFile.FileName; // only for debug
            if (imageFile != null && imageFile.Length > 0) // Không null và không trống
            {
                //Trỏ tới thư mục wwwroot để lát nữa thực hiện việc Copy sang
                var path = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot", "images", imageFile.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    // Thực hiện copy ảnh vừa chọn sang thư mục mới (wwwroot)
                    imageFile.CopyTo(stream);
                }
                // Gán lại giá trị cho Description của đối tượng bằng tên file ảnh đã được sao chép
                product.Description = imageFile.FileName;
            }
            if (productServices.CreateProduct(product)) // Nếu thêm thành công
            {
                return RedirectToAction("Redirect");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(Guid id) // Khi ấn vào Create thì hiển thị View
        {
            // Lấy Product từ database dựa theo id truyền vào từ route
            Product p = productServices.GetProductById(id);
            return View(p);
        }
       
        public IActionResult Edit(Product p) // Thực hiện việc Tạo mới
        {
            if (productServices.UpdateProduct(p))
            {
                return RedirectToAction("ShowListProduct");
            }
            else return BadRequest();
        }
        public IActionResult Delete(Guid id)
        {
            if (productServices.DeleteProduct(id))
            {           
                return RedirectToAction("ShowListProduct");
            }
            else return View("Index");
        }
        public IActionResult Details(Guid id)
        {
            ShopDbContext shopDbContext = new ShopDbContext();
            var product = shopDbContext.Products.Find(id);
            return View(product);
        }
        public IActionResult ShowListFromSession()
        {
            string JsonData = HttpContext.Session.GetString("Product"); // Lấy data từ Session
            if(JsonData == null) {
                return Content("Session đã bị xóa, trang web đã bị chiếm quyền kiểm soát");
            }
            var products = JsonConvert.DeserializeObject<List<Product>>(JsonData); 
            return View(products);  
        }

        public IActionResult DeleteSession()
        {
            HttpContext.Session.Remove("Product"); // Xóa 1 Session cụ thể nào đó
            return RedirectToAction("ShowListFromSession");
        }
        public IActionResult TransferData() // Đẩy dữ liệu qua các View
        {
            // Để truyền được dữ liệu sang View thì ngoài cách truyền trực tiếp
            // 1 object model ta có thể sử dụng các cách sau:
            /*
             * Sử dụng ViewBag: Dữ liệu trong ViewBag là dữ liệu dynamic
             * Không cần khởi tạo thành phần Viewbag mà đặt tên luôn
             */
            int[] marks = { 1, 2, 3, 4, 5, 6 };
            List<string> characterNames = new List<string>() { 
                "Nobita", "Shizuka", "Chaiko", "Jaien", "Doraemon", "Deki", "Xeko"
            };
            /*
             * Sử dụng ViewData: Data sẽ được truyền tải dưới dạng Key-Value nhưng
             * dữ liệu lại ở dạng Generic 
             */
            ViewBag.Marks = marks; // Gán dữ liệu vào ViewBag
            ViewData["name"] = characterNames; // Gán dữ liệu vào ViewData
            // Dữ liệu này dùng được ở chỉ 1 View
            /*
             * Sử dụng Session (Phiên làm việc), cơ chế key - value
             */
            string messages = "Đói, buồn ngủ, mệt mỏi";
            // Truyền data vào Session (Không chuẩn)
            HttpContext.Session.SetString("Message", messages);
            // Lấy data từ Session
            string content = HttpContext.Session.GetString("Message");
            ViewData["SessionData"] = content;
            return View();
        }

        public IActionResult AddToCart(Guid id)
        {
            // Lấy được dữ liệu sản phẩm
            var product = productServices.GetProductById(id);
            // Lấy dữ liệu từ Cart (Trong Session)
            var products = SessionServices.GetObjFromSession(HttpContext.Session, "Cart");
            // Kiểm tra xem list dữ liệu đó có phần tử nào chưa...
            if(products.Count == 0)
            {
                products.Add(product);  // Nếu không có sp nào thì add nó vào
                                        // Sau đó gán lại giá trị vào trong Session
                SessionServices.SetObjToSession(HttpContext.Session, "Cart", products);
            }
            else
            {
                if(SessionServices.CheckObjInList(id, products))
                {
                    return Content("List đã chứa sản phẩm này? Bạn định ăn cắp ư");
                } else
                {
                    products.Add(product);  // Nếu chưa có sản phẩm đó trong list thì thêm vào
                                            // Sau đó gán lại giá trị vào trong Session
                    SessionServices.SetObjToSession(HttpContext.Session, "Cart", products);
                }
            }
            return RedirectToAction("ShowCart");
        }
        public IActionResult ShowCart()
        {
            var products = SessionServices.GetObjFromSession(HttpContext.Session, "Cart");
            return View(products);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}