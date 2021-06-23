using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using UsedCarApp.Models.ViewModels;
using UsedCarApp.Models;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace UsedCarApp.Controllers
{//Base CRUD complete. Next step is to incorporate which cars and ads are associcated with which users and display them in the views.
    public class UserController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static UserController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44387/api/");
        }
        // GET: User/List
        public ActionResult List()
        {
            string url = "usersdata/listusers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<UserDto> users = response.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;
            return View(users);
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            DetailsUser ViewModel = new DetailsUser();

            string url = "usersdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            UserDto selecteduser = response.Content.ReadAsAsync<UserDto>().Result;

            ViewModel.SelectedUser = selecteduser;
            url = "adsdata/listadsforusers/" + id;
            IEnumerable<AdDto> RelatedAds = ;
            ViewModel.RelatedAds = RelatedAds;

            return View(selecteduser);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: User/Create
        public ActionResult New()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(User user)
        {
            string url = "usersdata/adduser";


            string jsonpayload = jss.Serialize(user);

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

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateUser ViewModel = new UpdateUser();

            string url = "usersdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            User selecteduser = response.Content.ReadAsAsync<User>().Result;
            ViewModel.SelectedUser = selecteduser;
            return View(ViewModel);
        }

        // POST: User/Update/5
        [HttpPost]
        public ActionResult Update(int id, User user)
        {
            string url = "usersdata/Updateuser/" + id;
            string jsonpayload = jss.Serialize(user);
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

        // GET: User/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "usersdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            UserDto selecteduser = response.Content.ReadAsAsync<UserDto>().Result;
            return View(selecteduser);
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "usersdata/deleteuser/" + id;
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
