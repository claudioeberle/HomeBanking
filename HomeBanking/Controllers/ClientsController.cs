using HomeBanking.Models;
using HomeBanking.Models.DTOs;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private IClientRepository _clientRepository;

        public ClientsController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
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

                _clientRepository.Save(newClient);
                return Created("", newClient);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
