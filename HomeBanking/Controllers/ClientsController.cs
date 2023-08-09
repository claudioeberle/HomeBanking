using HomeBanking.Models;
using HomeBanking.Models.DTOs;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ICardRepository _cardRepository;

        public ClientsController(IClientRepository clientRepository, IAccountRepository accountRepository, ICardRepository cardRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _cardRepository = cardRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                IEnumerable<Client> clients = _clientRepository.GetAllClients();
                List<ClientDTO> clientsDTO = new List<ClientDTO>();

                foreach (Client client in clients)
                {
                    ClientDTO newClientDTO = new ClientDTO()
                    {
                        Id = client.Id,
                        Email = client.Email,
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        Accounts = client.Accounts.Select(ac => new AccountDTO
                        {
                            Id = ac.Id,
                            Balance = ac.Balance,
                            CreationDate = ac.CreationDate,
                            Number = ac.Number,
                            
                        }).ToList(),
                        Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                        {
                            Id = cl.Id,
                            LoanId = cl.LoanId,
                            Name = cl.Loan.Name,
                            Amount = cl.Amount,
                            Payments = Int32.Parse(cl.Payments)

                        }).ToList(),
                        Cards = client.Cards.Select(cl => new CardDTO
                        {
                            Id = cl.Id,
                            CardHolder = cl.CardHolder,
                            Color = cl.Color,
                            Cvv = cl.Cvv,
                            FromDate = cl.FromDate,
                            ThruDate = cl.ThruDate,
                            Number = cl.Number,
                            Type = cl.Type,

                        }).ToList()
                    };

                    clientsDTO.Add(newClientDTO);
                }
                return Ok(clientsDTO);
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
                Client client = _clientRepository.FindById(id);

                if(client == null)
                {
                    return NotFound();
                }

                ClientDTO clientDTO = new ClientDTO
                {
                    Id = client.Id,

                    Email = client.Email,

                    FirstName = client.FirstName,

                    LastName = client.LastName,

                    Accounts = client.Accounts.Select(ac => new AccountDTO

                    {

                        Id = ac.Id,

                        Balance = ac.Balance,

                        CreationDate = ac.CreationDate,

                        Number = ac.Number

                    }).ToList(),

                    Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = Int32.Parse(cl.Payments)

                    }).ToList(),
                    Cards = client.Cards.Select(cl => new CardDTO
                    {
                        Id = cl.Id,
                        CardHolder = cl.CardHolder,
                        Color = cl.Color,
                        Cvv = cl.Cvv,
                        FromDate = cl.FromDate,
                        ThruDate = cl.ThruDate,
                        Number = cl.Number,
                        Type = cl.Type,

                    }).ToList()
                };

                return Ok(clientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("current")]
        public IActionResult GetCurrent()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : String.Empty;
                if(email == String.Empty) 
                {
                    return Forbid();
                }

                Client client = _clientRepository.FindByEmail(email);

                if(client == null)
                {
                    return Forbid();
                }

                var clientDTO = new ClientDTO
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Accounts = client.Accounts.Select(ac => new AccountDTO
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number
                    }).ToList(),
                    Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payments)
                    }).ToList(),
                    Cards = client.Cards.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        CardHolder = c.CardHolder,
                        Color = c.Color,
                        Cvv = c.Cvv,
                        FromDate = c.FromDate,
                        Number = c.Number,
                        ThruDate = c.ThruDate,
                        Type = c.Type
                    }).ToList()
                };

                return Ok(clientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Client client) 
        {
            Random rnd = new Random();
            Account account;
            string newAccountNumber;

            try
            {
                //validate data
                if(String.IsNullOrEmpty(client.Email) ||
                    String.IsNullOrEmpty(client.Password) ||
                    String.IsNullOrEmpty(client.FirstName) ||
                    String.IsNullOrEmpty(client.LastName))
                {
                    return StatusCode(403, "Datos Inválidos");
                }

                //look for existing user
                Client user = _clientRepository.FindByEmail(client.Email);

                if (user != null)
                {
                    return StatusCode(403, "El Email ya está en uso");
                }

                Client newClient = new Client
                {
                    Email = client.Email,
                    Password = client.Password,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                };

                //look for existing account number
                do
                {
                    newAccountNumber = "VIN-" + rnd.Next(1, 99999999);
                    account = _accountRepository.FindByNumber(newAccountNumber);
                }
                while (account != null);

                _clientRepository.Save(newClient);

                Account newAccount = new Account
                {
                    Number = newAccountNumber,
                    CreationDate = DateTime.Now,
                    Balance = 0.0,
                    ClientId = newClient.Id,
                };

                _accountRepository.Save(newAccount);
                return Created("", newClient);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("current/accounts")]
        public IActionResult PostAccount()
        {
            Random rnd = new Random();
            Account account;
            string newAccountNumber;

            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : String.Empty;
                if (email == String.Empty)
                {
                    return Forbid();
                }

                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid("No existe el cliente");
                }

                //validate max accounts
                if (client.Accounts.Count >= 3)
                {
                    return Forbid("Supera el máximo de cuentas permitidas");
                }

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
                    ClientId = client.Id,
                };

                _accountRepository.Save(newAccount);

                return Created("", newAccount);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpPost("current/cards")]
        public IActionResult PostCard([FromBody] Card card)
        {
            Random rnd = new Random();
            string newCardNumber;
            Card cardAux;

            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : String.Empty;
                if (email == String.Empty)
                {
                    return Forbid("Don't have authorization");
                }

                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid("Client doesn't exist");
                }

                //validate data
                if (String.IsNullOrEmpty(card.Type) ||
                    String.IsNullOrEmpty(card.Color) ||
                    !Card.IsCardType(card.Type) ||
                    !Card.IsCardColor(card.Color))
                {
                    return StatusCode(403, "Datos Inválidos");
                }

                //look for existing card number
                do
                {
                    newCardNumber = $"{rnd.Next(1111, 9999)}-{rnd.Next(1111, 9999)}-{rnd.Next(1111, 9999)}-{rnd.Next(1111, 9999)}";
                    cardAux = _cardRepository.FindByNumber(newCardNumber);
                }
                while (cardAux != null);

                Card newCard = new Card()
                {
                    CardHolder = $"{client.FirstName} {client.LastName}",
                    Type = card.Type,
                    Color = card.Color,
                    Number = newCardNumber,
                    Cvv = rnd.Next(111, 999),
                    FromDate = DateTime.Now,
                    ThruDate = DateTime.Now.AddYears(5),
                    ClientId = client.Id,
                };

                _cardRepository.Save(newCard);

                return Created("", newCard);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
