using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppService.Domain.Models;
using AppService.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "AdministratorsOnly")]
    public class IdentitiesController : ControllerBase
    {
        private AccessControlContext db;

        public IdentitiesController(AccessControlContext db)
        {
            this.db = db;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Identity>> Get()
        {
            return db.Identitiets.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Identity> Get(Guid id)
        {
            return db.Identitiets.Find(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] Identity value)
        {
            db.Identitiets.Add(value);
            db.SaveChanges();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] Identity value)
        {
            db.Identitiets.Update(value);
            db.SaveChanges();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            var item = db.Identitiets.Find(id);
            db.Identitiets.Remove(item);
            db.SaveChanges();
        }

        // GET api/values
        [HttpGet("{identityId}/credentials")]
        public IEnumerable<CardCredential> GetCredentials(Guid identityId)
        {
            var identity = db.Identitiets
                .Include(x => x.Credentials)
                .First(x => x.IdentityId == identityId);

            return identity.Credentials?.OfType<CardCredential>();
        }

        // GET api/values
        [HttpPost("{identityId}/credentials")]
        public async Task AddCredential(Guid identityId, CardCredential cardCredential)
        {
            var identity = db.Identitiets.Find(identityId);
            if (identity.Credentials == null)
            {
                identity.Credentials = new List<Credential>();
            }
            identity.Credentials.Add(cardCredential);
            await db.SaveChangesAsync();
        }
    }
}
