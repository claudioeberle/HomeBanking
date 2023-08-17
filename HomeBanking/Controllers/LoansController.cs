using HomeBanking.Models;
using HomeBanking.Models.DTOs;
using HomeBanking.Models.Enum;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        public IClientRepository _clientRepository;
        public IAccountRepository _accountRepository;
        public ILoanRepository _loanRepository;
        public IClientLoanRepository _clientLoanRepository;
        public ITransactionRepository _transactionRepository;

        public LoansController(IClientRepository clientRepository, IAccountRepository accountRepository, ILoanRepository loanRepository, IClientLoanRepository clientLoanRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _loanRepository = loanRepository;
            _clientLoanRepository = clientLoanRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpPost]
        public IActionResult Post(LoanApplicationDTO loanApplicationDTO)
        {
            Loan loan = null;
            string loanTypePaymentsString = string.Empty;
            int loanPaymentsInt = 0;
            List<string> loanTypePayments = new List<string>();
            List<string> appPayments = new List<string>();

            try
            {
                //check for client credentials
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : String.Empty;
                if (email == String.Empty)
                {
                    return Forbid("client not allowed");
                }

                //check for current client
                var client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid("client not found");
                }

                //get loanType from repository
                loan = _loanRepository.FindById(loanApplicationDTO.LoanId);
                if (loan == null)
                {
                    return Forbid("Selected loan does not exists");
                }

                //check for right amount
                if (loanApplicationDTO.Amount <= 0 || loanApplicationDTO.Amount > loan.MaxAmount)
                {
                    return Forbid("Incorrect application loan amount");
                }

                //check for payments
                if (String.IsNullOrEmpty(loanApplicationDTO.Payments))
                {
                    return Forbid("No payments given in loan application");
                }

                if (!loan.Payments.Contains(loanApplicationDTO.Payments) 
                    && int.TryParse(loanApplicationDTO.Payments, out loanPaymentsInt))
                {
                    return Forbid("Inconsistency between application loan payments and type loan payments");
                }

                //get ToAccount fro repository
                var toAccount = _accountRepository.FindByNumber(loanApplicationDTO.ToAccountNumber);
                if (toAccount == null)
                {
                    return Forbid("To Account does not exist or could not be found");
                }

                //check same account client and current
                if(toAccount.ClientId != client.Id)
                {
                    return Forbid("inconsistency between applicant id and account owner id");
                }

                ClientLoan clientLoan = new ClientLoan()
                {
                    Amount = loanApplicationDTO.Amount * 1.2,
                    Payments = loanApplicationDTO.Payments,
                    ClientId = client.Id,
                    LoanId = loanApplicationDTO.LoanId,
                };

                Transaction transaction = new Transaction()
                {
                    Type = TransactionType.CREDIT.ToString(),
                    Amount = loanApplicationDTO.Amount,
                    Description = $"{loan.Name} loan approved",
                    Date = DateTime.Now,
                    AccountId = toAccount.Id,
                };

                ClientLoanDTO clientLoanDTO = new ClientLoanDTO()
                {
                    Id = clientLoan.Id,
                    LoanId = clientLoan.LoanId,
                    Name = loan.Name,
                    Amount = clientLoan.Amount,
                    Payments = loanPaymentsInt
                };

                _clientLoanRepository.Save(clientLoan);

                _transactionRepository.Save(transaction);

                toAccount.Balance += transaction.Amount;
                _accountRepository.Save(toAccount);

                return Created("", clientLoan);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<LoanDTO> dtoLoansList = new List<LoanDTO>();
            try
            {
                IEnumerable<Loan> loans = _loanRepository.GetAllLoans();
                if(loans == null)
                {
                    return Forbid("Could not get avaible loans");
                }

                foreach (Loan loanAux in loans)
                {
                    LoanDTO loanDTO = new LoanDTO()
                    {
                        Id = loanAux.Id,
                        Name = loanAux.Name,
                        MaxAmount = loanAux.MaxAmount,
                        Payments = loanAux.Payments,
                    };
                    dtoLoansList.Add(loanDTO);
                }

                return Ok(dtoLoansList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
