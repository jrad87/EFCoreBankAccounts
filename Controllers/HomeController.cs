using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using EFCoreBankAccounts.Models;

namespace EFCoreBankAccounts.Controllers
{
    public class HomeController : Controller
    {
        private BasicDbContext Context;
        public HomeController(BasicDbContext Context)
        {
            this.Context = Context;
        }


        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("login")]
        [ImportModelState]
        public IActionResult ShowLogin()
        {
            var ViewModel = new LoginViewModel();
            return View("Login", ViewModel);
        }

        [HttpPost]
        [Route("login")]
        [ExportModelState]
        public IActionResult ProcessLoginForm(LoginViewModel data)
        {
            TryValidateModel(data);
            if(ModelState.IsValid)
            {
                User VisitingUser = this.Context.Users
                    .Where(user => user.Email == data.Email)
                    .SingleOrDefault();
                if(VisitingUser == null)
                {
                    TempData["LoginError"] = "Bad Credentials";
                    return RedirectToAction("ShowLogin");
                }
                else
                {
                    PasswordHasher<User> pwHasher = new PasswordHasher<User>();
                    PasswordVerificationResult result = pwHasher.VerifyHashedPassword(VisitingUser, VisitingUser.Password, data.Password);
                    if(result is PasswordVerificationResult.Success)
                    {
                        LogUserIn(VisitingUser);
                        return RedirectToAction("Index", "Accounts", new {AccountId = VisitingUser.AccountId});                        
                    }
                    else
                    {
                        TempData["LoginError"] = "Bad Credentials";
                        return RedirectToAction("ShowLogin");
                    }
                }
            }
            return RedirectToAction("ShowLogin");    
        }

        [HttpGet]
        [Route("register")]
        [ImportModelState]
        public IActionResult ShowRegistration()
        {
            var ViewModel = new RegistrationViewModel();
            return View("Register", ViewModel);
        }
        [HttpPost]
        [Route("register")]
        [ExportModelState]
        public IActionResult ProcessRegistrationForm(RegistrationViewModel data)
        {
            TryValidateModel(data);
            if(!ModelState.IsValid)
            {
                return RedirectToAction("ShowRegistration");
            }
            User ExistingUser = this.Context.Users
                .Where(User => User.Email == data.Email)
                .SingleOrDefault();
            if(ExistingUser != null)
            {
                TempData["RegistrationError"] = "User already exists, please login";
                return RedirectToAction("ShowRegistration");
            }
            User NewUser = RegisterUser(data); 
            LogUserIn(NewUser);
            
            return RedirectToAction("Index", "Accounts", new {AccountId = NewUser.AccountId});
        }
        private void LogUserIn(User user)
        {
            this.HttpContext.Session.SetInt32("UserId", user.Id);
            this.HttpContext.Session.SetInt32("AccountId", user.AccountId);
        }
        private User RegisterUser(RegistrationViewModel data)
        {
            PasswordHasher<User> pwHasher = new PasswordHasher<User>();
            User NewUser = new User(data);
            NewUser.Account = new Account();
            NewUser.Password = pwHasher.HashPassword(NewUser, NewUser.Password);
            this.Context.Add(NewUser);
            this.Context.SaveChanges();
            return NewUser;
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
