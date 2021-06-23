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
    public class AdsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [HttpGet]
        // GET: api/AdsData/ListAds
        public IEnumerable <AdDto> ListAds()
        { //TODO: incorporate images in the database
            List < Ad > Ads = db.Ads.ToList();
            List<AdDto> AdDtos = new List<AdDto>();
            Ads.ForEach(a => AdDtos.Add(new AdDto()
            {
                AdId = a.AdId,
                Description = a.Description,
                Images = a.Images,
                CarYear = a.Car.Year,
                CarMake = a.Car.Make,
                CarModel = a.Car.Model,
                Price = a.Price,
                Km = a.Km
            }));
            return AdDtos;
        }
        [HttpGet]
        // GET: api/AdsData/ListAds
        public IEnumerable<AdDto> ListAdsforUsers(int id)
        { //TODO: incorporate images in the database
            List<Ad> Ads = db.Ads.ToList();
            List<AdDto> AdDtos = new List<AdDto>();
            Ads.ForEach(a => AdDtos.Add(new AdDto()
            {
                AdId = a.AdId,
                Description = a.Description,
                Images = a.Images,
                CarYear = a.Car.Year,
                CarMake = a.Car.Make,
                CarModel = a.Car.Model,
                Price = a.Price,
                Km = a.Km
            }));
            return AdDtos;
        }
        [HttpGet]
        // GET: api/AdsData/FindAd/5
        [ResponseType(typeof(Ad))]
        public IHttpActionResult FindAd(int id)
        {
            Ad ad = db.Ads.Find(id);
            AdDto AdDto = new AdDto()
            {
                AdId = ad.AdId,
                Description = ad.Description,
                Images = ad.Images,
                CarYear = ad.Car.Year,
                CarMake = ad.Car.Make,
                CarModel = ad.Car.Model,
                Price = ad.Price,
                Km = ad.Km
            };
            if (ad == null)
            {
                return NotFound();
            }

            return Ok(AdDto);
        }

        // POST: api/AdsData/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAd(int id, Ad ad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ad.AdId)
            {
                return BadRequest();
            }

            db.Entry(ad).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdExists(id))
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

        // POST: api/AdsData/PostAd
        [ResponseType(typeof(Ad))]
        [HttpPost]
        public IHttpActionResult PostAd(Ad ad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Ads.Add(ad);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = ad.AdId }, ad);
        }

        // POST: api/AdsData/DeleteAd/5
        [ResponseType(typeof(Ad))]
        [HttpPost]
        public IHttpActionResult DeleteAd(int id)
        {
            Ad ad = db.Ads.Find(id);
            if (ad == null)
            {
                return NotFound();
            }

            db.Ads.Remove(ad);
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

        private bool AdExists(int id)
        {
            return db.Ads.Count(e => e.AdId == id) > 0;
        }
    }
}