using System.ComponentModel.DataAnnotations.Schema;

namespace spacemeet.Models
{
    public class Wallet
    {
        

        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public double Balance { get ; set ; }
        public double PendingBalance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        //To Be a to fund your wallet
        public void FundWallet(int amount)
        {
            double balance = Balance + amount;
            Balance = balance;
        }

        //For Merchants to be able to withdraw
        public void WithDrawFunds(int amount)
        {
            double balance = Balance;
            if (amount > 0 && balance > amount)
                Balance = balance - amount;
        }

        //To get the Balance of a User
        public double GetBalance()
        {
            double balance = Balance;
            return balance;
        }

        //To Get The Pending Balance For Merchant 
        public double GetPendingBalance()
        {
            return PendingBalance;
        }

        //To move funds from pending Balance to balance
        public void FulfilBalance(int amount)
        {
            double newBalance = Balance + amount;
            double newPendingBalance = PendingBalance - amount;
            Balance = newBalance;
            PendingBalance = newPendingBalance;
        }

    }
}
