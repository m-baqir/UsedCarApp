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
        /// <summary>
        /// lists all users in the db
        /// </summary>
        /// <returns>returns ienumerable list of userdto</returns>
        // GET: User/List
        public ActionResult List()
        {
            string url = "usersdata/listusers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<UserDto> users = response.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;
            return View(users);
        }
        /// <summary>
        /// returns details for a particular user given its userid
        /// </summary>
        /// <param name="id"></param>
        /// <returns>userdto object</returns>
        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            DetailsUser ViewModel = new DetailsUser();

            string url = "usersdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            UserDto selecteduser = response.Content.ReadAsAsync<UserDto>().Result;
            ViewModel.SelectedUser = selecteduser;

            url = "adsdata/listadsforusers/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<AdDto> RelatedAds = response.Content.ReadAsAsync<IEnumerable<AdDto>>().Result;
            ViewModel.RelatedAds = RelatedAds;

            return View(ViewModel);
        }
        /// <summary>
        /// a blank error controller in case of code malfunction
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }
        /// <summary>
        /// presents empty form fields to create a new user in the db
        /// </summary>
        /// <returns></returns>
        // GET: User/Create
        public ActionResult New()
        {
            return View();
        }
        /// <summary>
        /// creates a new user in the db given a user object
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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
        /// <summary>
        /// presents form fields with user information to be edited in the db given user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// updates user information in the db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns>executes the update command in the db</returns>
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
        /// <summary>
        /// warning page to confirm deletion before going ahead with deletion
        /// </summary>
        /// <param name="id"></param>
        /// <returns>user information given user id</returns>
        // GET: User/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "usersdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            UserDto selecteduser = response.Content.ReadAsAsync<UserDto>().Result;
            return View(selecteduser);
        }
        /// <summary>
        /// deletes a user in the db given user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>executes delete command in the db</returns>
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
