using System.Net;
using System;
using ConfigurationManager = System.Configuration.ConfigurationManager;
using PayStack.Net;

namespace spacemeet.Services
{
    public class WalletService
    {
       

        
        public static TransactionVerifyResponse verifyPayment(string reference, string skey)
        {
            PayStackApi payStack = new PayStackApi(skey);
            var response = payStack.Transactions.Verify(reference);
            
            return response;
        }
        public static TransactionInitializeResponse WithdrawFunds(int amount, string skey)
        {
            PayStackApi payStack = new PayStackApi(skey);
            var response = payStack.Transactions.Initialize("faniogor@gmail.com", amount*100);

            return response;
        }
    }

}
