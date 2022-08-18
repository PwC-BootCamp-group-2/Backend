using System.ComponentModel.DataAnnotations.Schema;

namespace spacemeet.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        private string? Balance { get; set; }
        private string? PendingBalance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        //To Be a to fund your wallet
        public void FundWallet(int amount) 
        {
            int balance = Convert.ToInt32(Balance) + amount;
            Balance = balance.ToString();
        }

        //For Merchants to be able to withdraw
        public void WithDrawFunds(int amount)
        {
            int balance = Convert.ToInt32(Balance);
            if (amount > 0 && balance > amount)
                Balance = (balance - amount).ToString();
        }

        //To get the Balance of a User
        public string GetBalance()
        {
            string? balance = Balance;
            return balance;
        }

        //To Get The Pending Balance For Merchant 
        public string GetPendingBalance()
        {
            return PendingBalance;
        }

        //To move funds from pending Balance to balance
        public void FulfilBalance(int amount)
        {
            int newBalance = Convert.ToInt32(Balance) + amount;
            int newPendingBalance = Convert.ToInt32(PendingBalance) - amount;
            Balance = newBalance.ToString();
            PendingBalance = newPendingBalance.ToString();
        }

    }
}
