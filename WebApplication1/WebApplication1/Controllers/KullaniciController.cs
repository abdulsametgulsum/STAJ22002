using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KullaniciController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public KullaniciController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        [Route("GetKullanicilar")]
        public async Task<ActionResult<List<KullanıcıModel>>> GetKullanicilar()
        {
            string query = "SELECT * FROM dbo.Kullanicilar";
            List<KullanıcıModel> kullanicilarList = new List<KullanıcıModel>();
            string sqlDataSource = _configuration.GetConnectionString("rentacarDB");

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                await myConn.OpenAsync();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    using (SqlDataReader myReader = await myCommand.ExecuteReaderAsync())
                    {
                        while (await myReader.ReadAsync())
                        {
                            kullanicilarList.Add(new KullanıcıModel
                            {
                                
                                Ad = myReader["Ad"].ToString(),
                                Soyad = myReader["Soyad"].ToString(),
                                Eposta = myReader["Eposta"].ToString(),
                                TcKimlikNo = myReader["TcKimlikNo"].ToString(),
                                DogumTarihi = Convert.ToDateTime(myReader["DogumTarihi"]),
                            });
                        }
                    }
                }
            }
            return Ok(kullanicilarList);
        }
        [HttpGet]
        [Route("GetKullanıcı")]
        public JsonResult GetKullanıcı()
        {
            string query = "SELECT * FROM dbo.Kullanicilar";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("rentacarDB");
            SqlDataReader myReader;

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                myConn.Close();
            }

            return new JsonResult(table);
        }

        [HttpPost]
        [Route("AddKullanıcı")]
        public IActionResult AddKullanıcı([FromBody] KullanıcıModel kullanıcı)
        {
            try
            {
                string query = @"
                    INSERT INTO dbo.Kullanicilar (Ad, Soyad, TcKimlikNo, Telefon, DogumTarihi, Eposta, Sifre)
                    VALUES (@Ad, @Soyad, @TcKimlikNo, @Telefon, @DogumTarihi, @Eposta, @Sifre)";

                string sqlDataSource = _configuration.GetConnectionString("rentacarDB");

                using (SqlConnection myConn = new SqlConnection(sqlDataSource))
                {
                    myConn.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@Ad", kullanıcı.Ad);
                        myCommand.Parameters.AddWithValue("@Soyad", kullanıcı.Soyad);
                        myCommand.Parameters.AddWithValue("@TcKimlikNo", kullanıcı.TcKimlikNo);
                        myCommand.Parameters.AddWithValue("@Telefon", kullanıcı.Telefon);
                        myCommand.Parameters.AddWithValue("@DogumTarihi", kullanıcı.DogumTarihi);
                        myCommand.Parameters.AddWithValue("@Eposta", kullanıcı.Eposta);
                        myCommand.Parameters.AddWithValue("@Sifre", kullanıcı.Sifre);

                        myCommand.ExecuteNonQuery();
                    }
                    myConn.Close();
                }

                return Ok("Kullanıcı başarıyla eklendi.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Veritabanı hatası: " + ex.Message);
            }
       
        
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            string query = "SELECT * FROM dbo.Kullanicilar WHERE Eposta = @Eposta AND Sifre = @Sifre";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("rentacarDB");
            SqlDataReader myReader;

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Eposta", login.Eposta);
                    myCommand.Parameters.AddWithValue("@Sifre", login.Sifre);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                myConn.Close();
            }

            if (table.Rows.Count > 0)
            {
                var user = new
                {
                    KullaniciId = table.Rows[0]["KullaniciId"],
                    Ad = table.Rows[0]["Ad"].ToString(),
                    Soyad = table.Rows[0]["Soyad"].ToString(),
                    Eposta = table.Rows[0]["Eposta"].ToString(),
                
                };
                return Ok(user);
            }
            else
            {
                return Unauthorized("E-posta veya şifre hatalı!");
            }
        }

        public class LoginModel
        {
            public string Eposta { get; set; }
            public string Sifre { get; set; }
        }

    }


    public class KullanıcıModel
    {
        public string KullaniciId { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string TcKimlikNo { get; set; }
        public string Telefon { get; set; }
        public DateTime DogumTarihi { get; set; }
        public string Eposta { get; set; }
        public string Sifre { get; set; }
    }
}
