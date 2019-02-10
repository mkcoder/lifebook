using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.EntityFramework.Entities;
using lifebook.authentication.core.dbcontext;

namespace lifebook.authentication.core.repository
{
    public class ClientRepository 
    {
        protected ClientDbContext ClientDbContext { get; set; }

        public ClientRepository(ClientDbContext clientDbContext)
        {
            ClientDbContext = clientDbContext;
        }

        public List<Client> GetAllClients() => ClientDbContext.Clients.ToList();

        public void Add(Client client)
        {
            ClientDbContext.Add(client);
            ClientDbContext.SaveChanges();
        }
    }
}
