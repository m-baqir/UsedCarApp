using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using UsedCarApp.Models;
using UsedCarApp.Models.ViewModels;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace UsedCarApp.Controllers
{ 
    public class AdController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static AdController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44387/api/");
        }
        /// <summary>
        /// this method lists all ads in the database
        /// </summary>
        /// <returns>returns an ienumerable list of AdDtos</returns>
        // GET: ad/List
        // curl https://localhost:44387/api/adsdata/listads
        public ActionResult List()
        {
            string url = "adsdata/listads";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<AdDto> ads = response.Content.ReadAsAsync<IEnumerable<AdDto>>().Result;
            return View(ads);
        }

        /// <summary>
        /// This method presents the specific details of an ad given the ad id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>returns addto object to the view with specific details</returns>
        // GET: Ad/Details/5
        public ActionResult Details(int id)
        {
            DetailsAd ViewModel = new DetailsAd();

            string url = "adsdata/findad/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AdDto selectedad = response.Content.ReadAsAsync<AdDto>().Result;
            ViewModel.SelectedAd = selectedad;

            
            return View(ViewModel);
        }

        /// <summary>
        /// An empty method to present errors in case of malfunction in the code
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// The new method presents the form elements to the user to create a new ad in the database
        /// </summary>
        /// <returns></returns>
        // GET: Ad/Create
        public ActionResult New()
        {
            NewAd ViewModel = new NewAd();

            string url = "carsdata/listcars";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Car> carsoptions = response.Content.ReadAsAsync<IEnumerable<Car>>().Result;
            ViewModel.AllCars = carsoptions;
            
            string url2 = "usersdata/listusers";
            HttpResponseMessage response2 = client.GetAsync(url2).Result;
            IEnumerable<UserDto> useroptions = response2.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;
            ViewModel.AllUsers = useroptions;

            return View(ViewModel);
        }

        /// <summary>
        /// actually creates the Ad object and adds it to the db
        /// </summary>
        /// <param name="Ad"></param>
        /// <returns>executes the CREATE command in the db</returns>
        // POST: Ad/Create
        [HttpPost]
        public ActionResult Create(Ad Ad)
        {
            string url = "adsdata/postad";

            
            string jsonpayload = jss.Serialize(Ad);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
                
        }

        /// <summary>
        /// presents the specific ad information in form elements before updating it given the ad id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        // GET: Ad/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateAd ViewModel = new UpdateAd();
            //ad information
            string url = "adsdata/findad/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Ad SelectedAd = response.Content.ReadAsAsync<Ad>().Result;
            ViewModel.SelectedAd = SelectedAd;
            
            //list of all cars to choose from
            
            url = "carsdata/listcars";
            response = client.GetAsync(url).Result;
            IEnumerable<Car> CarOptions = response.Content.ReadAsAsync<IEnumerable<Car>>().Result;
            ViewModel.CarOptions = CarOptions;
            // list of all users to choose from
            url = "usersdata/listusers";
            response = client.GetAsync(url).Result;
            IEnumerable<User> UserOptions = response.Content.ReadAsAsync<IEnumerable<User>>().Result;
            ViewModel.UserOptions = UserOptions;

            return View(ViewModel);
        }
        /// <summary>
        /// performs the actual update commmand in the db given the input of id and Ad object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Ad"></param>
        /// <returns>executes the update command</returns>
        // POST: Ad/Update/5
        [HttpPost]
        public ActionResult Update(int id, Ad Ad, HttpPostedFileBase AdPic)
        {
            string url = "adsdata/UpdateAd/" + id;
            string jsonpayload = jss.Serialize(Ad);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);

            //update request is successful, and we have image data
            if (response.IsSuccessStatusCode && AdPic != null)
            {
                //Updating the ad picture as a separate request
                Debug.WriteLine("Calling Update Image method.");
                //Send over image data for player
                url = "adsdata/updateadpic/" + id;

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(AdPic.InputStream);
                requestcontent.Add(imagecontent, "AdPic", AdPic.FileName);
                response = client.PostAsync(url, requestcontent).Result;

                return RedirectToAction("List");
            }
            else if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// gives the user a warning before confirming deletion of an ad. presents the details of an ad given the ad id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Ad/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "adsdata/findad/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AdDto selectedad = response.Content.ReadAsAsync<AdDto>().Result;
            return View(selectedad);
        }
        /// <summary>
        /// deletes a specific ad given the ad id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>executes the delete command in the db</returns>
        // POST: Ad/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "adsdata/deletead/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
