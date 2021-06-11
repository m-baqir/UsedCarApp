using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using UsedCarApp.Models;

namespace UsedCarApp.Controllers
{
    public class UsersDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/UsersData/ListUsers
        [HttpGet]
        public IEnumerable<UserDto> ListUsers()
        {
            List<User> Users = db.AllUsers.ToList();
            List<UserDto> UserDtos = new List<UserDto>();
            Users.ForEach(a => UserDtos.Add(new UserDto()
            {
                UserName = a.UserName,
                Phone = a.Phone,
                Email = a.Email,
                UserId = a.UserId
            }));

            return UserDtos;
        }

        // GET: api/UsersData/FindUser/5
        [ResponseType(typeof(User))]
        [HttpGet]
        public IHttpActionResult FindUser(int id)
        {
            User user = db.AllUsers.Find(id);
            UserDto userDto = new UserDto()
            {
                UserName = user.UserName,
                Phone = user.Phone,
                Email = user.Email,
                UserId = user.UserId
            };

            if (user == null)
            {
                return NotFound();
            }

            return Ok(userDto);
        }

        // PosT: api/UsersData/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserId)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/UsersData
        [ResponseType(typeof(User))]
        [HttpPost]
        public IHttpActionResult AddUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AllUsers.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.UserId }, user);
        }

        // DELETE: api/UsersData/5
        [ResponseType(typeof(User))]
        [HttpPost]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.AllUsers.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.AllUsers.Remove(user);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.AllUsers.Count(e => e.UserId == id) > 0;
        }
    }
}