using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayStack.Net;
using spacemeet.Data;
using spacemeet.Models;
using spacemeet.Services;

namespace spacemeet.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly spacemeetContext _context;
        private readonly IConfiguration _config;


        public WalletsController(spacemeetContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }



        // GET: api/Wallets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Wallet>> GetWallet(int id)
        {
          if (_context.Wallets == null)
          {
              return NotFound();
          }
            var wallet = await _context.Wallets.FindAsync(id);

            if (wallet == null)
            {
                return NotFound();
            }

            return wallet;
        }

        // Patch: api/Wallets/FundWallet/5
        // To Fund Wallet
        
        [HttpPatch("{id}/{reference}")]
        public async Task<IActionResult> FundWallet(int id, string reference)
        {
            string sKey = _config["AppSettings:PstackSecretKey"];
            Wallet? wallet = await _context.Wallets.FindAsync(id);
           
            TransactionVerifyResponse response = WalletService.verifyPayment(reference, sKey);
            if (WalletExists(id) && !TransactionExists(reference))
                if (response.Status)
                {
                    int amount = response.Data.Amount / 100;
                    //fund user account with amount sent
                    wallet?.FundWallet(amount);
                    wallet.UpdatedAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                    
                    //Add transaction with transaction with transaction and save authcode
                    Transaction transaction = new Transaction()
                    {
                        Id = reference,
                        Type = "Credit",
                        Purpose = "Fund Wallet",
                        UserId = wallet.UserId,
                        Amount = amount,
                        CreatedDate = DateTime.Now
                    };
                    await _context.Transactions.AddAsync(transaction);
                    await _context.SaveChangesAsync();
                    return Ok(transaction);
                }
            return BadRequest("Bad Request");
        }

        // Patch: api/Wallets/WithdrawFunds/5
        // To Withdraw Payment
        [HttpPatch("{id}")]
        public async Task<IActionResult> WithdrawFunds(int id, [FromBody]int amount)
        {
            string sKey = _config["AppSettings:PstackSecretKey"];

            TransactionInitializeResponse response = WalletService.WithdrawFunds(amount, sKey);
            if (WalletExists(id))
                //Check if user is a merchant
                if (response.Status)
                    //save transactions with transaction model
                    //Withdraw Account
                    return Ok(response);
            return BadRequest(response);
        }





        private bool WalletExists(int id)
        {
            return (_context.Wallets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool TransactionExists(string id)
        {
            return (_context.Transactions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
