using HomeBanking.Models;
using HomeBanking.Models.DTOs;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountRepository _accountRepository;

        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                IEnumerable<Account> accounts = _accountRepository.GetAllAccounts();
                List<AccountDTO> accountsDTO = new List<AccountDTO>();

                foreach (Account account in accounts)
                {
                    AccountDTO accountDTO = new AccountDTO()
                    {
                        Id = account.Id,
                        Number = account.Number,
                        CreationDate = account.CreationDate,
                        Balance = account.Balance,
                        Transactions = account.Transactions.Select(tr => new TransactionDTO
                        {
                            Id = tr.Id,
                            Date = tr.Date,
                            Type = tr.Type,
                            Amount = tr.Amount,
                            Description = tr.Description
                        }).ToList(),
                    };
                    accountsDTO.Add(accountDTO);
                }
                return Ok(accountsDTO);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                Account account = _accountRepository.FindById(id);

                if (account == null)
                {
                    return NotFound();
                }

                AccountDTO accountDTO = new AccountDTO
                {
                    Id = account.Id,

                    Number = account.Number,

                    CreationDate = account.CreationDate,

                    Balance = account.Balance,

                    Transactions = account.Transactions.Select(tr => new TransactionDTO
                    {

                        Id = tr.Id,
                        
                        Date = tr.Date,

                        Type = tr.Type, 

                        Amount = tr.Amount,

                        Description = tr.Description

                    }).ToList()
                };

                return Ok(accountDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetByClient(long clientId)
        {
            try
            {
                IEnumerable<Account> accounts = _accountRepository.GetAccountsByClient(clientId);

                if(accounts == null)
                {
                    return Forbid();
                }

                List<AccountDTO> accountsDTO = new List<AccountDTO>();

                foreach (Account account in accounts)
                {
                    AccountDTO accountDTO = new AccountDTO()
                    {
                        Id = account.Id,
                        Number = account.Number,
                        CreationDate = account.CreationDate,
                        Balance = account.Balance,
                        Transactions = account.Transactions.Select(tr => new TransactionDTO
                        {
                            Id = tr.Id,
                            Date = tr.Date,
                            Type = tr.Type,
                            Amount = tr.Amount,
                            Description = tr.Description
                        }).ToList(),
                    };
                    accountsDTO.Add(accountDTO);
                }
                return Ok(accountsDTO);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public AccountDTO Post(long clientId)
        {
            Random rnd = new Random();
            Account account;
            string newAccountNumber;

            try
            {

                //look for existing account number
                do
                {
                    newAccountNumber = "VIN-" + rnd.Next(1, 99999999);
                    account = _accountRepository.FindByNumber(newAccountNumber);
                }
                while (account != null);


                Account newAccount = new Account
                {
                    Number = newAccountNumber,
                    CreationDate = DateTime.Now,
                    Balance = 0.0,
                    ClientId = clientId
                };

                _accountRepository.Save(newAccount);

                AccountDTO accountDTO = new AccountDTO
                {
                    Id = newAccount.Id,
                    Number = newAccount.Number,
                    CreationDate = newAccount.CreationDate,
                    Balance = newAccount.Balance,
                };
                return accountDTO;

            }
            catch
            {
                return null;
            }
        }
    }
}
