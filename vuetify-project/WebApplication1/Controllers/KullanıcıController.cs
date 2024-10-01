using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KullanıcıController : ControllerBase
    {
        private IConfiguration _configuration;
        public KullanıcıController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetKullanıcı")]
        public JsonResult GetKullanıcı()
        {
         
            string query = "select * from dbo.Kullanicilar";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("rentacarDB");
            SqlDataReader myReader;

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn)) // 'new' keyword eklendi
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                myConn.Close(); // Bu satırı `using` yapısı dışına taşıdık.
            }

            return new JsonResult(table); // Geri dönüş eklendi.
        }
    }
}
