using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            string regionName = string.Empty;
            string dbHost = string.Empty;
            string dbName = string.Empty;

            try
            {
                regionName = System.Environment.GetEnvironmentVariable("REGION_NAME").ToString();
            }
            catch
            {
                regionName = "VARIABLE [REGION_NAME] NOT FOUND";
            }

            try
            {
                dbHost = System.Environment.GetEnvironmentVariable("DBHOST").ToString();
            }
            catch
            {
                dbHost = "VARIABLE [DBHOST] NOT FOUND";
            }

            try
            {
                dbName = System.Environment.GetEnvironmentVariable("DBNAME").ToString();
            }
            catch
            {
                dbName = "VARIABLE [DBNAME] NOT FOUND";
            }

            return new string[] { "value1", "value2", regionName, dbHost, dbName };
        }
    }
}