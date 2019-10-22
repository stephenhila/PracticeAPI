using Newtonsoft.Json;
using PracticeAPI.Web.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Mvc;

namespace PracticeAPI.Web.Controllers
{
    public class CarsController : Controller
    {
        // GET: Car
        public ActionResult Index()
        {
            AuthResponse authResponse = null;
            using (var client = new HttpClient())
            {
                AuthRequest authRequest = new AuthRequest("stephen.hila@gmail.com", "M0nsterMatthew");
                var authRequestJson = JsonConvert.SerializeObject(authRequest);

                var builder = new UriBuilder(new Uri("https://ef7a50ef-9022-460d-92ec-779d72cbf328.app.jexia.com/auth/"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, builder.Uri);
                request.Content = new StringContent(authRequestJson, Encoding.UTF8, "application/json");
                authResponse = client.SendAsync(request).Result.Content.ReadAsAsync<AuthResponse>().Result;

                if (authResponse != null && !string.IsNullOrEmpty(authResponse.AccessToken))
                {
                    //using (var client = new HttpClient())
                    //{
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResponse.AccessToken);

                        builder = new UriBuilder(new Uri("https://ef7a50ef-9022-460d-92ec-779d72cbf328.app.jexia.com/ds/cars/"));
                        request = new HttpRequestMessage(HttpMethod.Get, builder.Uri);
                        request.Headers.Authorization = client.DefaultRequestHeaders.Authorization;

                        var cars = client.SendAsync(request).Result.Content.ReadAsAsync<object>().Result;
                    //}
                }
            }
            return View();
        }

        // GET: Car/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Car/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Car/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Car/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Car/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Car/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Car/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
