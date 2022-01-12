using EnergyManagementMvc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EnergyManagementMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IHostingEnvironment Environment;
        public HomeController(ILogger<HomeController> logger, IHostingEnvironment _environment)
        {
            _logger = logger;
            Environment = _environment;
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormFile postedFile)
        {
                if (postedFile != null)
                   {
        string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
                        if (!Directory.Exists(path))
                            {
            Directory.CreateDirectory(path);
                           }
        
        string fileName = Path.GetFileName(postedFile.FileName);
        string filePath = Path.Combine(path, fileName);
                        using (FileStream stream = new FileStream(filePath, FileMode.Create))
                            {
            postedFile.CopyTo(stream);
                            }
        string csvData = System.IO.File.ReadAllText(filePath);
        DataTable dt = new DataTable();
        bool firstRow = true;
                        foreach (string row in csvData.Split('\n'))
                            {
                                if (!string.IsNullOrEmpty(row))
                                    {
                                        if (!string.IsNullOrEmpty(row))
                                            {
                                                if (firstRow)
                                                    {
                                                        foreach (string cell in row.Split(','))
                                                            {
                            dt.Columns.Add(cell.Trim());
                                                            }
                        firstRow = false;
                                                    }
                                                else
                                                    {
                        dt.Rows.Add();
                        int i = 0;
                                                        foreach (string cell in row.Split(','))
                                                            {
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Trim();
                            i++;
                                                            }
                                                    }
                                            }
                                    }
                            }
        var EnergyList = new List<Energy>();
                        foreach (DataRow row in dt.Rows)
                            {
            var values = row.ItemArray;
            var EnergyObj = new Energy()
                    {
                accountId = Convert.ToInt32(values[0]),
                meterReadingDateTime = Convert.ToString(values[1]),
                meterReadValue = Convert.ToString(values[2])
                    };
            EnergyList.Add(EnergyObj);
                            }
        var client = new HttpClient();
        
        var bodyJson = JsonSerializer.Serialize(EnergyList);
        
        var stringContent = new StringContent(bodyJson, Encoding.UTF8, "application/json");
        var a = client.PostAsync("http://localhost:31888/EnergyApi/Post", stringContent).GetAwaiter().GetResult();
                var getresult = client.GetAsync("http://localhost:31888/EnergyApi/Get").GetAwaiter().GetResult();
        var result = JsonSerializer.Deserialize<List<Energy>>(getresult.Content.ReadAsStringAsync().Result);
                        return View(result);
                    }
    var empty = new List<Energy>();
                return View(empty);
    
            }

        public IActionResult Privacy()
        {
                return View();
            }

        [HttpGet]
        public IActionResult Get()
        {
    
                return View();
            }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
    }
}
