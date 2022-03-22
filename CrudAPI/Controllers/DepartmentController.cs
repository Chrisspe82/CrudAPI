using CrudAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Linq;

namespace CrudAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _iconfiguration;

        public DepartmentController(IConfiguration configuration)
        {
            _iconfiguration = configuration;
        }

        [HttpGet]

        public JsonResult Get()
        {
            MongoClient dbclient = new MongoClient(_iconfiguration.GetConnectionString("EmployeeAppcon"));

            var dblist = dbclient.GetDatabase("testedb").GetCollection<Department>("Department").AsQueryable();
                return new JsonResult(dblist); 
        }

        [HttpPost]

        public JsonResult Post(Department dep)
        {
            MongoClient dbclient = new MongoClient(_iconfiguration.GetConnectionString("EmployeeAppcon"));

            int LastDepartmentId = dbclient.GetDatabase("testedb").GetCollection<Department>("Department").AsQueryable().Count();
            dep.DepartmentId = LastDepartmentId+1;

            dbclient.GetDatabase("testedb").GetCollection<Department>("Department").InsertOne(dep);

            return new JsonResult("Add Sucess");

        }

        [HttpPut]

        public JsonResult Put(Department dep)
        {
            MongoClient dbclient = new MongoClient(_iconfiguration.GetConnectionString("EmployeeAppcon"));

            var filter = Builders<Department>.Filter.Eq("DepartamentId", dep.DepartmentId);

            var update = Builders<Department>.Update.Set("DepartmentName", dep.DepartmentName);
            
            dbclient.GetDatabase("testedb").GetCollection<Department>("Department").UpdateOne(filter,update);

            return new JsonResult("Update Sucess");

        }

        [HttpDelete("Id")]

        public JsonResult Delete(int id)
        {
            MongoClient dbclient = new MongoClient(_iconfiguration.GetConnectionString("EmployeeAppcon"));

            var filter = Builders<Department>.Filter.Eq("DepartamentId", id);

            dbclient.GetDatabase("testedb").GetCollection<Department>("Department").DeleteOne(filter);

            return new JsonResult("Deleted Sucess");

        }
    }
}
