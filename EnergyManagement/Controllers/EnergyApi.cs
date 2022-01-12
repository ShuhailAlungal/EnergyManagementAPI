using EnergyManagement.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Controllers
{
    [ApiController]
    public class EnergyApiController : ControllerBase
    {
        private readonly IConfiguration config;

        public EnergyApiController(IConfiguration configuration)
        {
            this.config = configuration;
        }


        [HttpPost]
        [Route("[controller]/Post")]
        public IEnumerable<Energy> Post([FromBody] List<Energy> value)
        {
            string cnString = config.GetConnectionString("EnergyCon");
            SqlConnection conn = new SqlConnection(cnString);
            conn.Open();
            SqlCommand com = new SqlCommand("insert into dbo.Meter_Reading (AccountId, MeterReadingDateTime, MeterReadValue) values(@param1, @param2, @param3)", conn);
            foreach(var val in value)
            {
                com.Parameters.Clear();
                SqlCommand test = new SqlCommand("select AccountId, MeterReadingDateTime, MeterReadValue from Meter_Reading where AccountId = '" + val.AccountId + "'", conn);
                var data = test.ExecuteReader();
                int id = -1;
                while (data.Read())
                {
                     id = (int) data["AccountId"];
                }

                if (id == -1)
                    {
    com.Parameters.Add("@param1", SqlDbType.VarChar, 50).Value = val.AccountId;
    com.Parameters.Add("@param2", SqlDbType.VarChar, 50).Value = val.MeterReadingDateTime;
    com.Parameters.Add("@param3", SqlDbType.VarChar, 50).Value = val.MeterReadValue;
    com.ExecuteNonQuery();
                    }
                else
                    {
    SqlCommand update = new SqlCommand("update dbo.Meter_Reading set MeterReadingDateTime = @param2, MeterReadValue =@param3 where AccountId = '" + val.AccountId + "'", conn);
    update.Parameters.Add("@param2", SqlDbType.VarChar, 50).Value = val.MeterReadingDateTime;
    update.Parameters.Add("@param3", SqlDbType.VarChar, 50).Value = val.MeterReadValue;
    update.ExecuteNonQuery();
    update.Parameters.Clear();
                    }

            }
conn.Close();
            return null;
        }

        [HttpGet]
        [Route("[controller]/Get")]
        public IEnumerable<Energy> Get()
        {
    string cnString = config.GetConnectionString("EnergyCon");
            SqlConnection conn = new SqlConnection(cnString);
    conn.Open();
    SqlCommand test = new SqlCommand("select * from Meter_Reading r join Test_Accounts t on t.AccountId = r.AccountId ", conn);
    var data = test.ExecuteReader();
    var energylist = new List<Energy>();
                while (data.Read())
                    {
        var energy = new Energy();
        energy.AccountId = (int)data["AccountId"];
        energy.MeterReadingDateTime = (string)data["MeterReadingDateTime"];
        energy.MeterReadValue = (string)data["MeterReadValue"];
        energy.FirstName = (string)data["FirstName"];
        energy.LastName = (string)data["LastName"];
        energylist.Add(energy);
                    }
                return energylist;
            }

    }
}

