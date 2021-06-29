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
    public class CarsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// lists all car in the db
        /// </summary>
        /// <returns>ienumerable list of car objects</returns>
        // GET: api/CarsData/ListCars
        [HttpGet]
        public IEnumerable <Car> ListCars()
        {
            List<Car> Car = db.Cars.ToList();
            List<Car> Cars = new List<Car>();
            Car.ForEach(a => Cars.Add(new Car()
            {
                CarId = a.CarId,
                Year = a.Year,
                Make = a.Make,
                Model = a.Model
                
            }));
            return Cars;
        }
        /// <summary>
        /// presents details for a particular car given its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>car object</returns>
        // GET: api/CarsData/Findcar/5
        [ResponseType(typeof(Car))]
        [HttpGet]
        public IHttpActionResult FindCar(int id)
        {
            Car car = db.Cars.Find(id);

            if (car == null)
            {
                return NotFound();
            }

            return Ok(car);
        }
        /// <summary>
        /// updates a particular car given its id and car object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="car"></param>
        /// <returns></returns>
        // Post: api/CarsData/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCar(int id, Car car)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != car.CarId)
            {
                return BadRequest();
            }

            db.Entry(car).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
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
        /// <summary>
        /// creates a new car in the db
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        // POST: api/CarsData/AddCar
        [ResponseType(typeof(Car))]
        [HttpPost]
        public IHttpActionResult CreateCar(Car car)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cars.Add(car);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = car.CarId }, car);
        }
        /// <summary>
        /// deletes a car in the db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/CarsData/DeleteCar/5
        [ResponseType(typeof(Car))]
        [HttpPost]
        public IHttpActionResult DeleteCar(int id)
        {
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return NotFound();
            }

            db.Cars.Remove(car);
            db.SaveChanges();

            return Ok(car);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CarExists(int id)
        {
            return db.Cars.Count(e => e.CarId == id) > 0;
        }
    }
}