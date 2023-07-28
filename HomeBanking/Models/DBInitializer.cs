using System;
using System.Linq;

namespace HomeBanking.Models
{
    public class DBInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            Random rnd = new Random();

            if (!context.Clients.Any())
            {
                //creamos datos de prueba

                Client[] clients = new Client[]
                {
                    new Client { Email = "vcoronado@gmail.com", FirstName="Victor", LastName="Coronado", Password="123456"},
                    new Client { Email = "jgutierrez@gmail.com", FirstName="Juliana", LastName="Gutierrez", Password="123456"},
                    new Client { Email = "ltoledo@gmail.com", FirstName="Lorena", LastName="Toledo", Password="123456"},
                    new Client { Email = "frobledo@gmail.com", FirstName="Fernando", LastName="Robledo", Password="123456"},
                    new Client { Email = "Jdunos@gmail.com", FirstName="Julian", LastName="Dunos", Password="123456"},

                };

                foreach (Client client in clients)
                {
                    context.Clients.Add(client);
                }

                context.SaveChanges();
            }

            if (!context.Accounts.Any())
            {
                Account newAccount;

                foreach (Client cliente in context.Clients)
                {
                    if (cliente != null)
                    {
                        newAccount = new Account { ClientId = cliente.Id, CreationDate = DateTime.Now, Number = String.Empty, Balance = rnd.Next(0, 1500000) };
                        context.Accounts.Add(newAccount);
                    }
                }
                context.SaveChanges();

                //Client accountOwner = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                //if (accountOwner != null)
                //{
                //    var accounts = new Account[]
                //    {
                //        new Account {ClientId = accountOwner.Id, CreationDate = DateTime.Now, Number = string.Empty, Balance = 0 }
                //    };
                //    foreach (Account account in accounts)
                //    {
                //        context.Accounts.Add(account);
                //    }
                //    context.SaveChanges();
                //}
            }

            if (!context.Transactions.Any())
            {
                string[] numerosCuenta = { "V001", "V002", "V003", "V004", "V005", "V006", "V007", "V008", "V009", "V0010" };
                Random random = new Random();

                foreach (String numeroCuenta in numerosCuenta)
                {
                    Account account = context.Accounts.FirstOrDefault(c => c.Number == numeroCuenta);
                    if (account != null)
                    {
                        Transaction[] transactions = new Transaction[]
                        {
                            new Transaction
                            {
                                AccountId = account.Id,
                                Amount = 1000,
                                Date = DateTime.Now.AddHours(random.Next(-8, -1)),
                                Description = "Transferencia recibida",
                                Type = TransactionType.CREDIT.ToString()
                            },

                            new Transaction
                            {
                                AccountId = account.Id,
                                Amount = -1530,
                                Date = DateTime.Now.AddHours(random.Next(-8, -1)),
                                Description = "Compra en tienda virtual",
                                Type = TransactionType.DEBIT.ToString(),
                            },

                            new Transaction
                            {
                                AccountId = account.Id,
                                Amount = -2015,
                                Date = DateTime.Now.AddHours(random.Next(-8, -1)),
                                Description = "Compra en Carrefour Express",
                                Type = TransactionType.DEBIT.ToString()
                            }
                        };

                        foreach (Transaction transaction in transactions)
                        {
                            context.Transactions.Add(transaction);
                        }
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
