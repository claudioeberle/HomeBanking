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

            if (!context.Loans.Any())
            {
                //crearemos 3 prestamos Hipotecario, Personal y Automotriz
                var loans = new Loan[]
                {
                    new Loan { Name = "Hipotecario", MaxAmount = 500000, Payments = "12,24,36,48,60" },
                    new Loan { Name = "Personal", MaxAmount = 100000, Payments = "6,12,24" },
                    new Loan { Name = "Automotriz", MaxAmount = 300000, Payments = "6,12,24,36" },
                };

                foreach (Loan loan in loans)
                {
                    context.Loans.Add(loan);
                }

                context.SaveChanges();
            }

            if (!context.ClientLoans.Any())
            {
                var client1 = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                if (client1 != null)
                {
                    //ahora usaremos los 3 tipos de prestamos
                    var loan1 = context.Loans.FirstOrDefault(l => l.Name == "Hipotecario");
                    if (loan1 != null)
                    {
                        var clientLoan1 = new ClientLoan
                        {
                            Amount = 400000,
                            ClientId = client1.Id,
                            LoanId = loan1.Id,
                            Payments = "60"
                        };
                        context.ClientLoans.Add(clientLoan1);
                    }

                    var loan2 = context.Loans.FirstOrDefault(l => l.Name == "Personal");
                    if (loan2 != null)
                    {
                        var clientLoan2 = new ClientLoan
                        {
                            Amount = 50000,
                            ClientId = client1.Id,
                            LoanId = loan2.Id,
                            Payments = "12"
                        };
                        context.ClientLoans.Add(clientLoan2);
                    }

                    var loan3 = context.Loans.FirstOrDefault(l => l.Name == "Automotriz");
                    if (loan3 != null)
                    {
                        var clientLoan3 = new ClientLoan
                        {
                            Amount = 100000,
                            ClientId = client1.Id,
                            LoanId = loan3.Id,
                            Payments = "24"
                        };
                        context.ClientLoans.Add(clientLoan3);
                    }

                    //guardamos todos los prestamos
                    context.SaveChanges();
                }
            }
        }
    }
}
