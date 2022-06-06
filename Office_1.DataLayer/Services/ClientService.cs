using Office_1.DataLayer.Models;

namespace Office_1.DataLayer.Services;

public static class ClientService
{

    public static IList<Client> GetClientsByPrefixOfName(string prefixOfName)
    {
        using var context = new ApplicationContext();

        return context.Clients.Where(c => c.Name.StartsWith(prefixOfName)).ToList();
    }

    public static Client GetOrCreateClientByNameAndAddress(string name, string address)
    {
        using var context = new ApplicationContext();

        var clients = context.Clients.Where(c => c.Name.Equals(name) && c.Address.Equals(address));

        if (clients.Any()) // клиент уже есть в базе
        {
            return clients.First();
        }

        // клиента еще нет в базе
        var c = new Client
        {
            Name = name,
            Address = address
        };

        InsertClient(c);

        return c;
    }

    public static IList<Client> GetAllClients()
    {
        using var context = new ApplicationContext();

        return context.Clients.ToList();
    }

    public static void InsertClient(Client client)
    {
        using var context = new ApplicationContext();

        context.Clients.Add(client);
        context.SaveChanges();
    }

}