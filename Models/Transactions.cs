using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace EFCoreBankAccounts.Models
{
    public enum TransactionType 
    {
        Deposit, Withdrawal
    }
    public class Transaction : BasicEntity
    {
        public int Id {get; set;}
        public int Amount {get; set;}
        public int BalanceBefore {get; set;}
        public int BalanceAfter {get; set;}
        public int AccountId {get; set;}
        public Account Account {get; set;}
        public Transaction() : base()
        {}
        public Transaction(TransactionViewModel ViewModel, int AccountId) : base()
        {   
            this.AccountId = AccountId;
            this.BalanceBefore = ViewModel.CurrentBalance;
            Console.WriteLine("Transaction Constructor");
            
            switch(ViewModel.Action)
            {
                case TransactionType.Deposit:
                    this.Amount = ViewModel.Amount;
                    this.BalanceAfter = ViewModel.CurrentBalance + ViewModel.Amount;
                    break;
                case TransactionType.Withdrawal:
                    this.Amount = -ViewModel.Amount;
                    this.BalanceAfter = ViewModel.CurrentBalance - ViewModel.Amount;
                    break;
                default:
                    break;
            }
        }
    }
    public class TransactionViewModel
    {
        [OverdraftPrevention(ErrorMessage = "Can not withdraw more than current balance")]
        public int Amount {get; set;}
        public TransactionType Action {get; set;}
        public int CurrentBalance {get; set;}
        public bool CanValidate {get; set;}
        public TransactionViewModel()
        {
            this.CanValidate = false;
        }
        public void PopulateViewModel(int AccountId, BasicDbContext Context)
        {
            Account currentAccount = Context.Accounts
                .Where(account => account.Id == AccountId)
                .SingleOrDefault();
            this.CurrentBalance = currentAccount.CurrentBalance;    
            this.CanValidate = true;
        }
    }

    sealed class OverdraftPreventionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext ctx)
        {
            TransactionViewModel ViewModel = (TransactionViewModel)ctx.ObjectInstance;
            if(ViewModel.CanValidate)
            {
                int adjustment = (int)value;
                switch(ViewModel.Action){
                    case TransactionType.Deposit:
                        break;
                    case TransactionType.Withdrawal:
                        if(adjustment > ViewModel.CurrentBalance){
                            return new ValidationResult(this.ErrorMessage);
                        }
                        break;
                    default : break;
                }
                return ValidationResult.Success;
            }                        
            return ValidationResult.Success;
        }
    }
}