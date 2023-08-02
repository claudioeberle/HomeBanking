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
    }
}
