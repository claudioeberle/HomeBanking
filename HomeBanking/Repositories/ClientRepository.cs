using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace HomeBanking.Repositories
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {

        public ClientRepository(HomeBankingContext repositoryContext) : base(repositoryContext) { }

        public Client FindById(long id)
        {
            throw new NotImplementedException();
            //return FindByCondition(Client =>  Client.Id == id).Include(client => client.Accounts);
        }

        public IEnumerable<Client> GetAllClients()
        {
            throw new System.NotImplementedException();
        }

        public void Save(Client client)
        {
            throw new System.NotImplementedException();
        }
    }
}
