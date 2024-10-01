using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuvencePaketleriController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public GuvencePaketleriController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GuvencePaketi>>> GetGuvencePaketleri()
        {
            string query = "SELECT * FROM dbo.GuvencePaketleri";
            string sqlDataSource = _configuration.GetConnectionString("rentacarDB");

            var guvencePaketleri = new List<GuvencePaketi>();

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                await myConn.OpenAsync();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    using (SqlDataReader myReader = await myCommand.ExecuteReaderAsync())
                    {
                        while (await myReader.ReadAsync())
                        {
                            var guvencePaketi = new GuvencePaketi
                            {
                                GuvencePaketiId = Convert.ToInt32(myReader["GuvencePaketiId"]),
                                PaketAdi = myReader["PaketAdi"].ToString(),
                                Fiyat = Convert.ToDecimal(myReader["Fiyat"]),
                                Aciklama = myReader["Aciklama"].ToString()
                            };
                            guvencePaketleri.Add(guvencePaketi);
                        }
                    }
                }
            }

            return Ok(guvencePaketleri);
        }
    }

    public class GuvencePaketi
    {
        public int GuvencePaketiId { get; set; }
        public string PaketAdi { get; set; }
        public decimal Fiyat { get; set; }
        public string Aciklama { get; set; }
    }
}
