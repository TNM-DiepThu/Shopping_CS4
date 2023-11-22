using Newtonsoft.Json;
using Shopping_Application.Models;

namespace Shopping_Application.Services
{
    public static class SessionServices
    {
        public static List<Product> GetObjFromSession(ISession session, string key)
        {
            // Lấy string Json từ Session
            var jsonData = session.GetString(key);
            if (jsonData == null) return new List<Product>();
            // Chuyển đổi dữ liệu vừa lấy được sang dạng mong muốn
            var products = JsonConvert.DeserializeObject<List<Product>>(jsonData);
             // Nếu null thì trả về 1 list rỗng
            return products;
        }
        public static void SetObjToSession(ISession session, string key, object values)
        {
            var jsonData = JsonConvert.SerializeObject(values);
            session.SetString(key, jsonData); 
        }
        public static bool CheckObjInList(Guid id, List<Product> products)
        {
            return products.Any(x => x.Id == id);
        }
    }
}
