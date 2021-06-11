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
{//Base CRUD complete. Next steps are to incorporate which users are associated with which cars and ads.
    public class CarController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CarController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44387/api/");
        }
        // GET: Car/list
        public ActionResult List()
        {
            string url = "carsdata/listcars";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Car> cars = response.Content.ReadAsAsync<IEnumerable<Car>>().Result;
            return View(cars);
        }

        // GET: Car/Details/5
        public ActionResult Details(int id)
        {
            string url = "carsdata/findcar/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Car selectedcar = response.Content.ReadAsAsync<Car>().Result;
            
            Debug.WriteLine(response.StatusCode);
            Debug.WriteLine(selectedcar);
            return View(selectedcar);
        }
        public ActionResult Error()
        {
            return View();
        }

        // GET: Car/Create
        public ActionResult New()
        {
            return View();
        }

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

        // GET: Car/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "carsdata/findcar/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Car selectedcar = response.Content.ReadAsAsync<Car>().Result;
            return View(selectedcar);
        }

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
