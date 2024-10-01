using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ReservationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        [Route("GetAllKiralamalar")]
        public async Task<ActionResult<List<KiralamaModel>>> GetAllKiralamalar()
        {
            string query = "SELECT * FROM dbo.Kiralamalar";
            List<KiralamaModel> kiralamalarList = new List<KiralamaModel>();
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
                            kiralamalarList.Add(new KiralamaModel
                            {
                                KiralamaId = Convert.ToInt32(myReader["KiralamaId"]),
                                KullaniciId = Convert.ToInt32(myReader["KullaniciId"]),
                                AracId = Convert.ToInt32(myReader["AracId"]),
                                KiralamaTarihi = Convert.ToDateTime(myReader["KiralamaTarihi"]),
                                DonusTarihi = Convert.ToDateTime(myReader["DonusTarihi"]),
                                GuvencePaketiId = myReader["GuvencePaketiId"] == DBNull.Value ? (int?)null : Convert.ToInt32(myReader["GuvencePaketiId"]),
                                EkHizmetler = myReader["EkHizmetler"] == DBNull.Value ? null : myReader["EkHizmetler"].ToString(),
                                ToplamFiyat = Convert.ToDecimal(myReader["ToplamFiyat"]),
                            });
                        }
                    }
                }
            }

            return Ok(kiralamalarList);
        }

        [HttpGet]
        [Route("GetAllOdemeler")]
        public async Task<ActionResult<List<OdemeModel>>> GetAllOdemeler()
        {
            string query = "SELECT * FROM dbo.Odemeler";
            List<OdemeModel> odemelerList = new List<OdemeModel>();
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
                            odemelerList.Add(new OdemeModel
                            {
                                OdemeId = Convert.ToInt32(myReader["OdemeId"]),
                                KiralamaId = Convert.ToInt32(myReader["KiralamaId"]),
                                OdemeTarihi = Convert.ToDateTime(myReader["OdemeTarihi"]),
                                OdemeTutari = Convert.ToDecimal(myReader["OdemeTutari"]),
                                
                            });
                        }
                    }
                }
            }
            return Ok(odemelerList);
        }
        [HttpGet("GetAracDetaylari")]
        public JsonResult GetAracDetaylari([FromQuery] int aracId, [FromQuery] DateTime pickupDate, [FromQuery] DateTime dropoffDate)
        {
            pickupDate = pickupDate.Date;
            dropoffDate = dropoffDate.Date;
            string query = @"
                SELECT a.Marka, a.Model, a.Yil, a.Renk, a.GunlukFiyat, a.YakitTuru, a.VitesTuru, a.AracSinifi, a.YolcuKapasitesi, a.GunlukKM, a.EhliyetYasi
                FROM dbo.Araclar a
                WHERE a.AracId = @AracId;
                
                -- Rezervasyonun yapılıp yapılmadığını kontrol et
                SELECT COUNT(*) AS RezervasyonVarMi
                FROM dbo.Kiralamalar k
                WHERE k.AracId = @AracId
                AND k.KiralamaTarihi <= @PickupDate
                AND k.DonusTarihi >= @DropoffDate;
            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("rentacarDB");
            SqlDataReader myReader;

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@AracId", aracId);
                    myCommand.Parameters.AddWithValue("@PickupDate", pickupDate);
                    myCommand.Parameters.AddWithValue("@DropoffDate", dropoffDate);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                myConn.Close();
            }

            return new JsonResult(table);
        }

        [HttpPost("CompleteReservation")]
        public IActionResult CompleteReservation([FromBody] ReservationModel reservation)
        {
            // Tarihlerin geçerli bir aralıkta olup olmadığını kontrol edin
            if (reservation.KiralamaTarihi < new DateTime(1753, 1, 1) || reservation.KiralamaTarihi > new DateTime(9999, 12, 31))
            {
                return BadRequest("Kiralama tarihi geçersiz.");
            }

            if (reservation.DonusTarihi < new DateTime(1753, 1, 1) || reservation.DonusTarihi > new DateTime(9999, 12, 31))
            {
                return BadRequest("Dönüş tarihi geçersiz.");
            }

            string query = @"
        INSERT INTO dbo.Kiralamalar (KullaniciId, AracId, KiralamaTarihi, DonusTarihi, GuvencePaketiId, EkHizmetler, ToplamFiyat)
        VALUES (@KullaniciId, @AracId, @KiralamaTarihi, @DonusTarihi, @GuvencePaketiId, @EkHizmetler, @ToplamFiyat);
        SELECT SCOPE_IDENTITY(); -- Son eklenen ID'yi döndürür
    ";

            int kiralamaId;

            try
            {
                using (SqlConnection myConn = new SqlConnection(_configuration.GetConnectionString("rentacarDB")))
                {
                    myConn.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@KullaniciId", reservation.KullaniciId);
                        myCommand.Parameters.AddWithValue("@AracId", reservation.AracId);
                        myCommand.Parameters.AddWithValue("@KiralamaTarihi", reservation.KiralamaTarihi);
                        myCommand.Parameters.AddWithValue("@DonusTarihi", reservation.DonusTarihi);
                        myCommand.Parameters.AddWithValue("@GuvencePaketiId", reservation.GuvencePaketiId);
                        myCommand.Parameters.AddWithValue("@EkHizmetler", reservation.EkHizmetler); // JSON formatında veriyi kontrol edin
                        myCommand.Parameters.AddWithValue("@ToplamFiyat", reservation.ToplamFiyat);

                        kiralamaId = Convert.ToInt32(myCommand.ExecuteScalar()); // Yeni kiralama ID'sini al
                    }
                }

                if (kiralamaId <= 0)
                {
                    return StatusCode(500, "Kiralama işlemi yapılamadı.");
                }

                // Şimdi ödeme bilgilerini ekleyelim
                string paymentQuery = @"
            INSERT INTO dbo.Odemeler (KiralamaId, OdemeTarihi, OdemeTutari)
            VALUES (@KiralamaId, @OdemeTarihi, @OdemeTutari);
        ";

                using (SqlConnection myConn = new SqlConnection(_configuration.GetConnectionString("rentacarDB")))
                {
                    myConn.Open();
                    using (SqlCommand myCommand = new SqlCommand(paymentQuery, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@KiralamaId", kiralamaId);
                        myCommand.Parameters.AddWithValue("@OdemeTarihi", DateTime.Now); // Şu anki ödeme tarihi
                        myCommand.Parameters.AddWithValue("@OdemeTutari", reservation.ToplamFiyat);

                        myCommand.ExecuteNonQuery(); // Ödeme kaydını ekle
                    }
                }

                return Ok(new { Message = "Kiralama ve ödeme başarılı." });
            }
            catch (Exception ex)
            {
                // Hata ayrıntılarını loglayın veya döndürün
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        public class OdemeModel
        {
            public int OdemeId { get; set; }
            public int KiralamaId { get; set; }
            public DateTime OdemeTarihi { get; set; }
            public decimal OdemeTutari { get; set; }
            
        }
        public class KiralamaModel
        {
            public int KiralamaId { get; set; }
            public int KullaniciId { get; set; }
            public int AracId { get; set; }
            public DateTime KiralamaTarihi { get; set; }
            public DateTime DonusTarihi { get; set; }
            public int? GuvencePaketiId { get; set; }
            public string EkHizmetler { get; set; }
            public decimal ToplamFiyat { get; set; }
        }


        public class ReservationModel
        {
            public int KullaniciId { get; set; }
            public int AracId { get; set; }
            public DateTime KiralamaTarihi { get; set; }
            public DateTime DonusTarihi { get; set; }
            public int GuvencePaketiId { get; set; }
            public string EkHizmetler { get; set; }
            public decimal ToplamFiyat { get; set; }
        }
    }
}
