using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using EFCoreBankAccounts.Models;

namespace EFCoreBankAccounts.Controllers
{   
    public class AccountsController : Controller
    {
        private BasicDbContext Context;
        public AccountsController(BasicDbContext Context)
        {
            this.Context = Context;
        }

        private void LogUserOut()
        {
            this.HttpContext.Session.Clear();            
        }

        [HttpGet]
        [Route("accounts/{AccountId}")]
        [LoginRequired]
        [ImportModelState]
        public IActionResult Index(int AccountId)
        {
            var accountModel = this.Context
                .Accounts
                .Where(account => account.Id == AccountId)
                .Include(account => account.User)
                .Include(account => account.Transactions)
                .SingleOrDefault();
            accountModel.Transactions = accountModel
                .Transactions
                .OrderByDescending(transaction => transaction.CreatedAt)
                .ToList();
            return View("Index", new AccountModelBundle(accountModel));
        }

        [HttpPost]
        [Route("accounts/{AccountId}")]
        [LoginRequired]
        [ExportModelState]
        public IActionResult ProcessNewTransaction(int AccountId, TransactionViewModel ViewModel)
        {       
            ViewModel.PopulateViewModel(AccountId, this.Context);
            TryValidateModel(ViewModel);
            if(ModelState.IsValid)
            {
                Account currentAccount = this.Context.Accounts.Where(account => account.Id == AccountId).SingleOrDefault();       
                Transaction newTransaction = new Transaction(ViewModel, AccountId);
                this.Context.Add(newTransaction);
                currentAccount.CurrentBalance = newTransaction.BalanceAfter;
                this.Context.SaveChanges();
            }
            return RedirectToAction("Index", new {AccountId = AccountId});
        }
        
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            this.LogUserOut();
            return RedirectToAction("Index", "Home");
        }
    }
}