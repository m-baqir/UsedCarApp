using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Diagnostics;
using UsedCarApp.Models;
using UsedCarApp.Models.ViewModels;
using System.Net.Http;

namespace UsedCarApp.Controllers
{
    public class CarController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CarController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44387/api/");
        }
        /// <summary>
        /// lists all cars in the db
        /// </summary>
        /// <returns></returns>
        // GET: Car/list
        public ActionResult List()
        {
            string url = "carsdata/listcars";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Car> cars = response.Content.ReadAsAsync<IEnumerable<Car>>().Result;
            return View(cars);
        }
        /// <summary>
        /// presents the details of a particular ad given its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Car/Details/5
        public ActionResult Details(int id)
        {
            DetailsCar ViewModel = new DetailsCar();

            string url = "carsdata/findcar/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Car selectedcar = response.Content.ReadAsAsync<Car>().Result;
            ViewModel.SelectedCar = selectedcar;
            Debug.WriteLine(response.StatusCode);
            Debug.WriteLine(selectedcar);

            url = "adsdata/listadsforcar/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<AdDto> RelatedAd = response.Content.ReadAsAsync<IEnumerable<AdDto>>().Result;
            ViewModel.RelatedAd = RelatedAd;


            return View(ViewModel);
        }
        /// <summary>
        /// a blank error method that works in case of code malfunction
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }
        /// <summary>
        /// presents blank form elements to create a new car
        /// </summary>
        /// <returns></returns>
        // GET: Car/Create
        public ActionResult New()
        {
            return View();
        }
        /// <summary>
        /// creates a new car in the db, takes a Car object
        /// </summary>
        /// <param name="Car"></param>
        /// <returns></returns>
        // POST: Car/Create
        [HttpPost]
        public ActionResult Create(Car Car)
        {
            string url = "carsdata/createcar";

            string jsonpaylod = jss.Serialize(Car);

            HttpContent content = new StringContent(jsonpaylod);
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
        /// presents car information in form elements to update car information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Car/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateCar ViewModel = new UpdateCar();

            string url = "carsdata/findcar/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Car selectedcar = response.Content.ReadAsAsync<Car>().Result;
            ViewModel.SelectedCar = selectedcar;
            return View(ViewModel);
        }
        /// <summary>
        /// updates a particular car given an id and car object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="car"></param>
        /// <returns>updated car information</returns>
        // POST: Car/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Car car)
        {
            string url = "carsdata/updatecar/" + id;
            string jsonpayload = jss.Serialize(car);
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
        /// <summary>
        /// a warning page before deletion. presents the details of the particular car
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Car/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "carsdata/findcar/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Car selectedcar = response.Content.ReadAsAsync<Car>().Result;
            return View(selectedcar);
        }
        /// <summary>
        /// deletes a particular car given its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: Car/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "carsdata/deletecar/" + id;
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
