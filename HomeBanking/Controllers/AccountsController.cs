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

        
    }
}
