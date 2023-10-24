using System.Collections.Generic;
/// <summary>
/// Interface to apply client filter
/// </summary>
public interface IClientFilter
{
    List<Client> FilterClients(List<Client> clients);
}
