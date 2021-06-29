using System;
using System.IO;
using System.Web;
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
using System.Diagnostics;

namespace UsedCarApp.Controllers
{
    public class AdsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// lists all ads in the database
        /// </summary>
        /// <returns>returns an ienumerable list of addto objects</returns>
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
        /// <summary>
        /// lists all ads for a particular user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ienumerable of addtos</returns>
        [HttpGet]
        // GET: api/AdsData/ListAdsforusers
        public IEnumerable<AdDto> ListAdsforUsers(int id)
        { 
            List<Ad> Ads = db.Ads.Where(a=>a.UserId==id).ToList();
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
        /// <summary>
        /// lists all ads for a particular car
        /// </summary>
        /// <param name="id"></param>
        /// <returns>AdDto Object</returns>
        [HttpGet]
        // GET: api/AdsData/ListAdsforcar
        public IEnumerable<AdDto> ListAdsforCar(int id)
        {
            List<Ad> Ads = db.Ads.Where(a => a.CarId == id).ToList();
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


        /// <summary>
        /// finds a specific ad given its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>specific details of a particular ad</returns>
        [HttpGet]
        // GET: api/AdsData/FindAd/5
        [ResponseType(typeof(AdDto))]
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
                Km = ad.Km,
                UserId = ad.UserId,
                CarId = ad.CarId,
                AdHasPic = ad.AdHasPic
            };
            if (ad == null)
            {
                return NotFound();
            }

            return Ok(AdDto);
        }
        /// <summary>
        /// provides the update functionality for a specific ad given its id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ad"></param>
        /// <returns></returns>
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
            // image update is handled by another method
            db.Entry(ad).Property(a => a.AdHasPic).IsModified = false;
            db.Entry(ad).Property(a => a.Images).IsModified = false;

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
        /// <summary>
        /// creates a new ad in the db
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
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
        /// <summary>
        /// deletes a particular ad given its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
            if (ad.AdHasPic && ad.Images != "")
            {
                //also delete image from path
                string path = HttpContext.Current.Server.MapPath("/Content/Images/" + id + "." + ad.Images);
                if (System.IO.File.Exists(path))
                {
                    Debug.WriteLine("File exists... preparing to delete!");
                    System.IO.File.Delete(path);
                }
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
        /// <summary>
        /// recieves ad image data, uploads to webserver and updates the adhaspic option
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateAdPic(int id)
        {
            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("have recieved multipart form data");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    //Debug.WriteLine("step1");
                    var adPic = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (adPic.ContentLength > 0)
                    {
                        //Debug.WriteLine("step2");
                        //establish valid file types (can be changed to other file extensions if desired!)
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(adPic.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                           // Debug.WriteLine("step3");
                            try
                            {
                                //Debug.WriteLine("step4");
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/ads/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("/Content/Images/"), fn);

                                //save the file
                                adPic.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the ad haspic and picextension fields in the database
                                Ad SelectedAd = db.Ads.Find(id);
                                SelectedAd.AdHasPic = haspic;
                                SelectedAd.Images = extension;
                                db.Entry(SelectedAd).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("ad Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                                return BadRequest();
                            }
                        }
                    }
                }
                return Ok();
            }
            else
            {
                //not multipart form data
                return BadRequest();
            }
        }
    }
}