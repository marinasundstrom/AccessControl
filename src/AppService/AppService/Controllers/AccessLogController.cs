using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppService.Domain.Entities;
using AppService.Infrastructure;
using AppService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccessLogController : ControllerBase
    {
        private AccessControlContext db;

        public AccessLogController(AccessControlContext db)
        {
            this.db = db;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<AccessLogEntry>> Get()
        {
            return db.AccessLogEntries.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<AccessLogEntry> Get(int id)
        {
            return db.AccessLogEntries.Find(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] AccessLogEntry value)
        {
            db.AccessLogEntries.Add(value);
            db.SaveChanges();
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] AccessLogEntry value)
        //{
        //    db.AccessLogEntries.Attach(value);
        //    db.SaveChanges();
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //    var item = db.AccessLogEntries.Find(id);
        //    db.AccessLogEntries.Remove(item);
        //    db.SaveChanges();
        //}
    }
}
