using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.AppService.Domain.Models;
using AccessControl.AppService.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.AppService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        private AccessControlContext db;

        public ItemsController(AccessControlContext db)
        {
            this.db = db;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Item>> Get()
        {
            return db.Items.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Item> Get(int id)
        {
            return db.Items.Find(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] Item value)
        {
            db.Items.Add(value);
            db.SaveChanges();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Item value)
        {
            db.Items.Attach(value);
            db.SaveChanges();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var item = db.Items.Find(id);
            db.Items.Remove(item);
            db.SaveChanges();
        }
    }
}
