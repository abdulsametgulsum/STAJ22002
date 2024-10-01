using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EkHizmetlerController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EkHizmetlerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EkHizmet>>> GetEkHizmetler()
        {
            string query = "SELECT * FROM dbo.EkHizmetler";
            string sqlDataSource = _configuration.GetConnectionString("rentacarDB");

            var ekHizmetler = new List<EkHizmet>();

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                await myConn.OpenAsync();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    using (SqlDataReader myReader = await myCommand.ExecuteReaderAsync())
                    {
                        while (await myReader.ReadAsync())
                        {
                            var ekHizmet = new EkHizmet
                            {
                                Id = Convert.ToInt32(myReader["EkHizmetId"]),
                                HizmetAdi = myReader["HizmetAdi"].ToString(),
                                Fiyat = Convert.ToDecimal(myReader["Fiyat"]),
                                Aciklama = myReader["Aciklama"].ToString()
                            };
                            ekHizmetler.Add(ekHizmet);
                        }
                    }
                }
            }

            return Ok(ekHizmetler);
        }
    }

    public class EkHizmet
    {
        public int Id { get; set; }
        public string HizmetAdi { get; set; }
        public decimal Fiyat { get; set; }
        public string Aciklama { get; set; }
    }
}
