using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AracController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AracController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Diğer metodlar...

        [HttpPost]
        [Route("AddArac")]
        public async Task<IActionResult> AddArac([FromBody] AracModel arac)
        {
            if (arac == null)
            {
                return BadRequest("Araç bilgileri eksik.");
            }

            string query = @"
            INSERT INTO dbo.Araclar
            (Marka, Model, Yil, Renk, GunlukFiyat, Durum, MotorGucu, YakitTuru, YolcuKapasitesi, GunlukKM, AracSinifi, EhliyetYasi, VitesTuru, FotoUrl)
            VALUES
            (@Marka, @Model, @Yil, @Renk, @GunlukFiyat, @Durum, @MotorGucu, @YakitTuru, @YolcuKapasitesi, @GunlukKM, @AracSinifi, @EhliyetYasi, @VitesTuru, @FotoUrl);
        ";

            string sqlDataSource = _configuration.GetConnectionString("rentacarDB");

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                try
                {
                    await myConn.OpenAsync();
                    using (SqlCommand myCommand = new SqlCommand(query, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@Marka", arac.Marka);
                        myCommand.Parameters.AddWithValue("@Model", arac.Model);
                        myCommand.Parameters.AddWithValue("@Yil", arac.Yil);
                        myCommand.Parameters.AddWithValue("@Renk", arac.Renk);
                        myCommand.Parameters.AddWithValue("@GunlukFiyat", arac.GunlukFiyat);
                        myCommand.Parameters.AddWithValue("@Durum", arac.Durum);
                        myCommand.Parameters.AddWithValue("@MotorGucu", arac.MotorGucu);
                        myCommand.Parameters.AddWithValue("@YakitTuru", arac.YakitTuru);
                        myCommand.Parameters.AddWithValue("@YolcuKapasitesi", arac.YolcuKapasitesi);
                        myCommand.Parameters.AddWithValue("@GunlukKM", arac.GunlukKM);
                        myCommand.Parameters.AddWithValue("@AracSinifi", arac.AracSinifi);
                        myCommand.Parameters.AddWithValue("@EhliyetYasi", arac.EhliyetYasi);
                        myCommand.Parameters.AddWithValue("@VitesTuru", arac.VitesTuru);
                        myCommand.Parameters.AddWithValue("@FotoUrl", arac.FotoUrl);

                        await myCommand.ExecuteNonQueryAsync();
                    }
                    return Ok("Araç başarıyla eklendi.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"İçsel sunucu hatası: {ex.Message}");
                }
            }
        }
    
    [HttpGet]
        [Route("GetAllAraclar")]
        public async Task<ActionResult<List<AracModel>>> GetAllAraclar()
        {
            string query = "SELECT* FROM dbo.Araclar";
            List<AracModel> araclarList = new List<AracModel>();
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
                            araclarList.Add(new AracModel
                            {
                                AracId = Convert.ToInt32(myReader["AracId"]),
                                Marka = myReader["Marka"].ToString(),
                                Model = myReader["Model"].ToString(),
                                Yil = Convert.ToInt32(myReader["Yil"]),
                                Renk = myReader["Renk"].ToString(),
                                GunlukFiyat = Convert.ToDecimal(myReader["GunlukFiyat"]),
                                Durum = myReader["Durum"].ToString(),
                                MotorGucu = Convert.ToInt32(myReader["MotorGucu"]),
                                YakitTuru = myReader["YakitTuru"].ToString(),
                                YolcuKapasitesi = Convert.ToInt32(myReader["YolcuKapasitesi"]),
                                GunlukKM = Convert.ToInt32(myReader["GunlukKM"]),
                                AracSinifi = myReader["AracSinifi"].ToString(),
                                EhliyetYasi = Convert.ToInt32(myReader["EhliyetYasi"]),
                                VitesTuru = myReader["VitesTuru"].ToString(),
                                FotoUrl = myReader["FotoUrl"].ToString()

                            });
                        }
                    }
                }
            }
            return Ok(araclarList);
        }

        // Her araç için detay sayfası
        [HttpGet("{id}")]
        public async Task<ActionResult<AracModel>> GetAracById(int id)
        {
            string query = "SELECT * FROM dbo.Araclar WHERE AracId = @AracId";
            string sqlDataSource = _configuration.GetConnectionString("rentacarDB");

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                await myConn.OpenAsync();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@AracId", id);

                    using (SqlDataReader myReader = await myCommand.ExecuteReaderAsync())
                    {
                        if (await myReader.ReadAsync())
                        {
                            var arac = new AracModel
                            {
                                AracId = Convert.ToInt32(myReader["AracId"]),
                                Marka = myReader["Marka"].ToString(),
                                Model = myReader["Model"].ToString(),
                                Yil = Convert.ToInt32(myReader["Yil"]),
                                Renk = myReader["Renk"].ToString(),
                                GunlukFiyat = Convert.ToDecimal(myReader["GunlukFiyat"]),
                                Durum = myReader["Durum"].ToString(),
                                MotorGucu = Convert.ToInt32(myReader["MotorGucu"]),
                                YakitTuru = myReader["YakitTuru"].ToString(),
                                YolcuKapasitesi = Convert.ToInt32(myReader["YolcuKapasitesi"]),
                                GunlukKM = Convert.ToInt32(myReader["GunlukKM"]),
                                AracSinifi = myReader["AracSinifi"].ToString(),
                                EhliyetYasi = Convert.ToInt32(myReader["EhliyetYasi"]),
                                VitesTuru = myReader["VitesTuru"].ToString(),
                                FotoUrl = myReader["FotoUrl"].ToString()
                            };

                            return Ok(arac);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
        }
        [HttpGet("searchCars")]
        public async Task<ActionResult<IEnumerable<AracModel>>> SearchAvailableCars(
     [FromQuery] DateTime pickupDate,
     [FromQuery] DateTime dropoffDate)
        {
            // Tarihlerin saat kısmını sıfırla
            pickupDate = pickupDate.Date;
            dropoffDate = dropoffDate.Date;

            string query = @"
    SELECT * 
    FROM dbo.Araclar a
    WHERE NOT EXISTS (
        SELECT 1 
        FROM dbo.Kiralamalar k
        WHERE k.AracId = a.AracId
        AND @pickupDate < k.DonusTarihi
        AND @dropoffDate > k.KiralamaTarihi
    )";

            string sqlDataSource = _configuration.GetConnectionString("rentacarDB");

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                await myConn.OpenAsync();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    // Parametreleri ekleyelim
                    myCommand.Parameters.AddWithValue("@pickupDate", pickupDate);
                    myCommand.Parameters.AddWithValue("@dropoffDate", dropoffDate);

                    using (SqlDataReader myReader = await myCommand.ExecuteReaderAsync())
                    {
                        var availableCars = new List<AracModel>();
                        while (await myReader.ReadAsync())
                        {
                            var arac = new AracModel
                            {
                                AracId = Convert.ToInt32(myReader["AracId"]),
                                Marka = myReader["Marka"].ToString(),
                                Model = myReader["Model"].ToString(),
                                Yil = Convert.ToInt32(myReader["Yil"]),
                                Renk = myReader["Renk"].ToString(),
                                GunlukFiyat = Convert.ToDecimal(myReader["GunlukFiyat"]),
                                Durum = myReader["Durum"].ToString(),
                                MotorGucu = Convert.ToInt32(myReader["MotorGucu"]),
                                YakitTuru = myReader["YakitTuru"].ToString(),
                                YolcuKapasitesi = Convert.ToInt32(myReader["YolcuKapasitesi"]),
                                GunlukKM = Convert.ToInt32(myReader["GunlukKM"]),
                                AracSinifi = myReader["AracSinifi"].ToString(),
                                EhliyetYasi = Convert.ToInt32(myReader["EhliyetYasi"]),
                                VitesTuru = myReader["VitesTuru"].ToString(),
                                FotoUrl = myReader["FotoUrl"].ToString()
                            };
                            availableCars.Add(arac);
                        }

                        if (availableCars.Count == 0)
                        {
                            return NotFound("Müsait araç bulunamadı.");
                        }

                        return Ok(availableCars);
                    }
                }
            }
        }


        [HttpGet("BenzerAraclar/{id}")]
        public async Task<ActionResult<IEnumerable<AracModel>>> GetSimilarAraclar(int id)
        {
            string query = @"
        SELECT TOP 5 * 
        FROM dbo.Araclar
        WHERE AracSinifi = (SELECT AracSinifi FROM dbo.Araclar WHERE AracId = @AracId)
          AND AracId != @AracId";
            string sqlDataSource = _configuration.GetConnectionString("rentacarDB");

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                await myConn.OpenAsync();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@AracId", id);

                    using (SqlDataReader myReader = await myCommand.ExecuteReaderAsync())
                    {
                        var araclar = new List<AracModel>();
                        while (await myReader.ReadAsync())
                        {
                            var arac = new AracModel
                            {
                                AracId = Convert.ToInt32(myReader["AracId"]),
                                Marka = myReader["Marka"].ToString(),
                                Model = myReader["Model"].ToString(),
                                Yil = Convert.ToInt32(myReader["Yil"]),
                                Renk = myReader["Renk"].ToString(),
                                GunlukFiyat = Convert.ToDecimal(myReader["GunlukFiyat"]),
                                Durum = myReader["Durum"].ToString(),
                                MotorGucu = Convert.ToInt32(myReader["MotorGucu"]),
                                YakitTuru = myReader["YakitTuru"].ToString(),
                                YolcuKapasitesi = Convert.ToInt32(myReader["YolcuKapasitesi"]),
                                GunlukKM = Convert.ToInt32(myReader["GunlukKM"]),
                                AracSinifi = myReader["AracSinifi"].ToString(),
                                EhliyetYasi = Convert.ToInt32(myReader["EhliyetYasi"]),
                                VitesTuru = myReader["VitesTuru"].ToString(),
                                FotoUrl = myReader["FotoUrl"].ToString()
                            };
                            araclar.Add(arac);
                        }

                        if (araclar.Count == 0)
                        {
                            return NotFound("Benzer araçlar bulunamadı.");
                        }

                        return Ok(araclar);
                    }
                }
            }
        }
        // Araç güncelleme endpoint'i
[HttpPut("UpdateArac/{id}")]
public async Task<IActionResult> UpdateArac(int id, [FromBody] AracModel arac)
{
    string query = @"
        UPDATE dbo.Araclar
        SET Marka = @Marka,
            Model = @Model,
            Yil = @Yil,
            Renk = @Renk,
            GunlukFiyat = @GunlukFiyat,
            Durum = @Durum,
            MotorGucu = @MotorGucu,
            YakitTuru = @YakitTuru,
            YolcuKapasitesi = @YolcuKapasitesi,
            GunlukKM = @GunlukKM,
            AracSinifi = @AracSinifi,
            EhliyetYasi = @EhliyetYasi,
            VitesTuru = @VitesTuru,
            FotoUrl = @FotoUrl
        WHERE AracId = @AracId";
    
    string sqlDataSource = _configuration.GetConnectionString("rentacarDB");

    using (SqlConnection myConn = new SqlConnection(sqlDataSource))
    {
        await myConn.OpenAsync();
        using (SqlCommand myCommand = new SqlCommand(query, myConn))
        {
            myCommand.Parameters.AddWithValue("@AracId", id);
            myCommand.Parameters.AddWithValue("@Marka", arac.Marka);
            myCommand.Parameters.AddWithValue("@Model", arac.Model);
            myCommand.Parameters.AddWithValue("@Yil", arac.Yil);
            myCommand.Parameters.AddWithValue("@Renk", arac.Renk);
            myCommand.Parameters.AddWithValue("@GunlukFiyat", arac.GunlukFiyat);
            myCommand.Parameters.AddWithValue("@Durum", arac.Durum);
            myCommand.Parameters.AddWithValue("@MotorGucu", arac.MotorGucu);
            myCommand.Parameters.AddWithValue("@YakitTuru", arac.YakitTuru);
            myCommand.Parameters.AddWithValue("@YolcuKapasitesi", arac.YolcuKapasitesi);
            myCommand.Parameters.AddWithValue("@GunlukKM", arac.GunlukKM);
            myCommand.Parameters.AddWithValue("@AracSinifi", arac.AracSinifi);
            myCommand.Parameters.AddWithValue("@EhliyetYasi", arac.EhliyetYasi);
            myCommand.Parameters.AddWithValue("@VitesTuru", arac.VitesTuru);
            myCommand.Parameters.AddWithValue("@FotoUrl", arac.FotoUrl);

            int rowsAffected = await myCommand.ExecuteNonQueryAsync();
            if (rowsAffected > 0)
            {
                return Ok("Araç başarıyla güncellendi.");
            }
            else
            {
                return NotFound("Güncellenecek araç bulunamadı.");
            }
        }
    }
}

// Araç silme endpoint'i
[HttpDelete("DeleteArac/{id}")]
public async Task<IActionResult> DeleteArac(int id)
{
    string query = "DELETE FROM dbo.Araclar WHERE AracId = @AracId";
    string sqlDataSource = _configuration.GetConnectionString("rentacarDB");

    using (SqlConnection myConn = new SqlConnection(sqlDataSource))
    {
        await myConn.OpenAsync();
        using (SqlCommand myCommand = new SqlCommand(query, myConn))
        {
            myCommand.Parameters.AddWithValue("@AracId", id);

            int rowsAffected = await myCommand.ExecuteNonQueryAsync();
            if (rowsAffected > 0)
            {
                return Ok("Araç başarıyla silindi.");
            }
            else
            {
                return NotFound("Silinecek araç bulunamadı.");
            }
        }
    }
}

        // Araç detaylarını getiren endpoint
        [HttpGet("details/{id}")]
        public async Task<ActionResult<AracModel>> GetAracDetails(int id)
        {
            string query = "SELECT * FROM dbo.Araclar WHERE AracId = @AracId";
            string sqlDataSource = _configuration.GetConnectionString("rentacarDB");

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                await myConn.OpenAsync();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@AracId", id);

                    using (SqlDataReader myReader = await myCommand.ExecuteReaderAsync())
                    {
                        if (await myReader.ReadAsync())
                        {
                            var arac = new AracModel
                            {
                                AracId = Convert.ToInt32(myReader["AracId"]),
                                Marka = myReader["Marka"].ToString(),
                                Model = myReader["Model"].ToString(),
                                Yil = Convert.ToInt32(myReader["Yil"]),
                                Renk = myReader["Renk"].ToString(),
                                GunlukFiyat = Convert.ToDecimal(myReader["GunlukFiyat"]),
                                Durum = myReader["Durum"].ToString(),
                                MotorGucu = Convert.ToInt32(myReader["MotorGucu"]),
                                YakitTuru = myReader["YakitTuru"].ToString(),
                                YolcuKapasitesi = Convert.ToInt32(myReader["YolcuKapasitesi"]),
                                GunlukKM = Convert.ToInt32(myReader["GunlukKM"]),
                                AracSinifi = myReader["AracSinifi"].ToString(),
                                EhliyetYasi = Convert.ToInt32(myReader["EhliyetYasi"]),
                                VitesTuru = myReader["VitesTuru"].ToString(),
                                FotoUrl = myReader["FotoUrl"].ToString()
                            };

                            return Ok(arac);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }

            }

        }




        public class AracModel
        {
            public int AracId { get; set; }
            public string Marka { get; set; }
            public string Model { get; set; }
            public int Yil { get; set; }
            public string Renk { get; set; }
            public decimal GunlukFiyat { get; set; }
            public string Durum { get; set; }
            public int MotorGucu { get; set; }
            public string YakitTuru { get; set; }
            public int YolcuKapasitesi { get; set; }
            public int GunlukKM { get; set; }
            public string AracSinifi { get; set; }
            public int EhliyetYasi { get; set; }
            public string VitesTuru { get; set; }
            public string FotoUrl { get; set; }
        }

    }
}
