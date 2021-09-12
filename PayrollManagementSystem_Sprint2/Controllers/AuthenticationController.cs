using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PayrollManagementSystem_Sprint2.Models;
using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;

using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace PayrollManagementSystem_Sprint2.Controllers
{

    public class AuthenticationController : Controller
    {
        static string localHostLink = "https://localhost:44314/";   //Common localhost link variable
        private HttpContent content;
        public UserManager<IdentityUser> UserManager { get; }
        public SignInManager<IdentityUser> SignInManager { get; }
        public AuthenticationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public async Task<EmployeeMaster> IsValidEmployee(AuthorizationResponse authResponse)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{localHostLink}api/");
                client.DefaultRequestHeaders.Clear();

                //Defining Request Data Format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string tmp = JsonConvert.SerializeObject(authResponse).ToString();
                var content = new StringContent(tmp, Encoding.UTF8, "application/json");
                HttpResponseMessage Result = await client.PostAsync($"EmployeeMasters/Login", content);
                if (Result.IsSuccessStatusCode)
                {
                    //storing the response details recieved from web api
                    var employeeResponse = Result.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web Api and storing into the mployee list
                    var resultValue = JsonConvert.DeserializeObject<LoginGet>(employeeResponse);
                    HttpContext.Session.SetString("Token", resultValue._applicationSideToken);
                    return resultValue.EmpMasterObj;
                }
                else
                {
                    return null;
                }
            }
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> Register(EmployeeMaster empObj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new IdentityUser() { Id =empObj.EmployeeId };
        //        var result = await UserManager.CreateAsync(user, empObj.EmployeePassword);
        //        if (result.Succeeded)
        //        {
        //            await SignInManager.SignInAsync(user, isPersistent: false);
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            foreach (var error in result.Errors)
        //            {
        //                ModelState.AddModelError("", error.Description);
        //            }
        //        }
        //    }
        //    return View(empObj);
        //}


        [HttpPost]
        public ActionResult Login(AuthorizationResponse model)
        {
            if (ModelState.IsValid)
            {
                //validation of the Employee weather the employee is valid or Not
                var isValidUser = IsValidEmployee(model).Result;

                //If the employee Is Valid and present in Db we redirect them to their respective Pages that is weather
                //admin page or Employee Page
                if (isValidUser != null)
                {
                    string isAdminString = null;
                    if (isValidUser.AdminPrivilege == true)
                    {
                        isAdminString = "True";
                    }
                    else
                    {
                        isAdminString = "False";
                    }
                    HttpContext.Session.SetString("EmployeeID", isValidUser.EmployeeId);
                    HttpContext.Session.SetString("EmpFirstName", isValidUser.EmployeeFirstname);
                    HttpContext.Session.SetString("AdminPrivelege", isAdminString);
                    return HttpContext.Session.GetString("AdminPrivelege") == "True" ? RedirectToAction("Index", "Admin") : RedirectToAction("Index","Employee");
                    //return View();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "You entered wrong username and password Combination");
                    return View();
                }
            }
            else
            {
                return View(model);
            }
        }

        //public ActionResult Login(AuthorizationResponse model)
    }




}
