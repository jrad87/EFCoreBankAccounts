using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EFCoreBankAccounts.Models
{
    public class Account : BasicEntity
    {
        public int Id {get; set;}
        public int CurrentBalance {get; set;}
        public User User {get; set;}
        public List<Transaction> Transactions {get; set;}

        public Account() : base()
        {
            this.Transactions = new List<Transaction>();
        }
    }
    public class AccountModelBundle
    {
        public Account Account {get; set;}
        public TransactionViewModel ViewModel {get; set;}
        public AccountModelBundle() {}
        public AccountModelBundle(Account Account)
        {
            this.Account = Account;
            this.ViewModel = new TransactionViewModel();
        }
    }
}