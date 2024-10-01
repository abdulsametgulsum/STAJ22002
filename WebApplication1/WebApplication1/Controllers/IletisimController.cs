using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class IletisimController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public IletisimController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> PostIletisim([FromBody] Mesaj mesaj)
    {
        if (ModelState.IsValid)
        {
            mesaj.GonderilmeTarihi = DateTime.Now;

            string query = "INSERT INTO dbo.Mesajlar (GonderenAd, GonderenSoyad, Konu, GonderenTelefon, MesajMetni, GonderilmeTarihi) " +
                           "VALUES (@GonderenAd, @GonderenSoyad, @Konu, @GonderenTelefon, @MesajMetni, @GonderilmeTarihi)";

            string sqlDataSource = _configuration.GetConnectionString("rentacarDB");

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                await myConn.OpenAsync();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@GonderenAd", mesaj.GonderenAd);
                    myCommand.Parameters.AddWithValue("@GonderenSoyad", mesaj.GonderenSoyad);
                    myCommand.Parameters.AddWithValue("@Konu", mesaj.Konu);
                    myCommand.Parameters.AddWithValue("@GonderenTelefon", mesaj.GonderenTelefon);
                    myCommand.Parameters.AddWithValue("@MesajMetni", mesaj.MesajMetni);
                    myCommand.Parameters.AddWithValue("@GonderilmeTarihi", mesaj.GonderilmeTarihi);

                    await myCommand.ExecuteNonQueryAsync();
                }
            }

            return Ok(new { message = "Mesaj başarıyla kaydedildi." });
        }

        return BadRequest(ModelState);
    }

    public class Mesaj
    {
        public int MesajId { get; set; }
        public string GonderenAd { get; set; }
        public string GonderenSoyad { get; set; }
        public string Konu { get; set; }
        public string GonderenTelefon { get; set; }
        public string MesajMetni { get; set; }
        public DateTime GonderilmeTarihi { get; set; }
    }
}
