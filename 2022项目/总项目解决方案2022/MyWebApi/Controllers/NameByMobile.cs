using Microsoft.AspNetCore.Mvc;

namespace MyWebApi.Controllers
{
   

    [ApiController]
    [Route("[controller]")]
    public class NameByMobile : Controller
    {
      


        [HttpGet(Name = "NameByMobile")]
      

        public string Get(int mobile)
        {
            return mobile.ToString();

        }
    }
}
