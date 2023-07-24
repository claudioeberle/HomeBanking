using System.Linq;

namespace HomeBanking.Models
{
    public class DBInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            if (!context.Clients.Any())
            {
                //creamos datos de prueba

                Client[] clients = new Client[]
                {
                    new Client { Email = "vcoronado@gmail.com", FirstName="Victor", LastName="Coronado", Password="123456"}
                };

                foreach (Client client in clients)
                {
                    context.Clients.Add(client);
                }

                context.SaveChanges();
            }
        }
    }
}
