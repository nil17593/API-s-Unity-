using System.Collections.Generic;
namespace SunBase.API
{
    /// <summary>
    /// Interface to apply client filter
    /// </summary>
    public interface IClientFilter
    {
        List<Client> FilterClients(List<Client> clients);
    }
}