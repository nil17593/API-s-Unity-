using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// filter class will activate filter with given list
/// </summary>
public class ClientFilter : IClientFilter
{
    private Func<Client, bool> filterFunc;

    //Constructor to set the filter
    public ClientFilter(Func<Client, bool> filterFunction)
    {
        filterFunc = filterFunction;
    }

    //will filter the clients list
    public List<Client> FilterClients(List<Client> clients)
    {
        return clients.Where(filterFunc).ToList();
    }
}
