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
{ /*Base CRUD complete, next step is tie all the views together for a better User experience*/
    public class AdController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static AdController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44387/api/");
        }
        // GET: ad/List
        // curl https://localhost:44387/api/adsdata/listads
        public ActionResult List()
        {
            string url = "adsdata/listads";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<AdDto> ads = response.Content.ReadAsAsync<IEnumerable<AdDto>>().Result;
            return View(ads);
        }

        // GET: Ad/Details/5
        public ActionResult Details(int id)
        {
            //DetailsAd ViewModel = new DetailsAd();

            string url = "adsdata/findad/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AdDto SelectedAd = response.Content.ReadAsAsync<AdDto>().Result;
            //ViewModel.SelectedAd = SelectedAd;
            return View(SelectedAd);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Ad/Create
        public ActionResult New()
        {
            string url = "carsdata/listcars";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Car> carsoptions = response.Content.ReadAsAsync<IEnumerable<Car>>().Result;
            //figure out how to return 2 lists to a view
            string url2 = "usersdata/listusers";
            HttpResponseMessage response2 = client.GetAsync(url2).Result;
            IEnumerable<User> useroptions = response2.Content.ReadAsAsync<IEnumerable<User>>().Result;

            return View(carsoptions);
        }

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

        [HttpGet]
        // GET: Ad/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateAd ViewModel = new UpdateAd();
            
            string url = "adsdata/findad/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Ad selectedad = response.Content.ReadAsAsync<Ad>().Result;
            ViewModel.SelectedAd = selectedad;
            
            //list of all cars to choose from
            //this is giving me a headache. keeps giving an error of foreign key constraint when the db is setup properly.
            url = "carsdata/listcars";
            response = client.GetAsync(url).Result;
            IEnumerable<Car> CarOptions = response.Content.ReadAsAsync<IEnumerable<Car>>().Result;
            ViewModel.CarOptions = CarOptions;

            return View(ViewModel);
        }

        // POST: Ad/Update/5
        [HttpPost]
        public ActionResult Update(int id, Ad Ad)
        {
            string url = "adsdata/UpdateAd/" + id;
            string jsonpayload = jss.Serialize(Ad);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            Debug.WriteLine(response);
            Debug.WriteLine(jsonpayload);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Ad/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "adsdata/findad/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AdDto selectedad = response.Content.ReadAsAsync<AdDto>().Result;
            return View(selectedad);
        }

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
