using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System;

/// <summary>
/// API client class handles 
/// calling of API and fetch API data
/// Set filter for the clients
/// dropdown filter for the clients
/// </summary>
public class ApiClient : MonoBehaviour
{
    public enum ClientFilterEnum { AllClients, MangerClients, NonManagerClients }
    [Header ("Serialized fields")]
    [SerializeField] private string apiUrl = "https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";
    [SerializeField] TMP_Dropdown filterDropdown;

    #region private fiels
    private ClientData clientData;
    private IClientFilter currentFilter;
    #endregion

    #region event
    public delegate void ClientDataReceivedHandler(List<Client> clients);
    public event ClientDataReceivedHandler OnClientDataReceived;
    #endregion

    private void Start()
    {
        StartCoroutine(FetchDataFromAPI());
        filterDropdown.onValueChanged.AddListener((int index) =>
        {
            // Map the index to the corresponding enum value
            ClientFilterEnum selectedFilter = (ClientFilterEnum)index;
            OnFilterChanged(selectedFilter);
        });
        SetFilter(new ClientFilter(client => true)); // Default filter (all clients)
    }

    //fetch the API data
    private IEnumerator FetchDataFromAPI()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("API Request Error: " + webRequest.error);
            }
            else
            {
                // Process the JSON response here using JsonUtility.
                string jsonData =  webRequest.downloadHandler.text;
                clientData = JsonUtility.FromJson<ClientData>(jsonData);
                ShowClientsList(clientData);
            }
        }
    }

    //event function for filter value change
    private void OnFilterChanged(ClientFilterEnum index)
    {
        Func<Client, bool> filterFunction = null;

        switch (index)
        {
            case ClientFilterEnum.AllClients:
                filterFunction = client => true; // All clients
                break;
            case ClientFilterEnum.MangerClients:
                filterFunction = client => client.isManager; // Managers only
                break;
            case ClientFilterEnum.NonManagerClients:
                filterFunction = client => !client.isManager; // Non-managers
                break;

            default:
                filterFunction = client => true; // All clients
                break;

        }

        SetFilter(new ClientFilter(filterFunction));

        // Apply the selected filter to the client list
        ShowClientsList(clientData);
    }

    //set the filter to show filtered clients
    private void SetFilter(IClientFilter filter)
    {
        currentFilter = filter;
    }

    //show the filtered clients list
    private void ShowClientsList(ClientData clientData)
    {
        List<Client> filteredClients = currentFilter.FilterClients(clientData.clients);
        OnClientDataReceived?.Invoke(filteredClients);
    }
}

#region Serializable classes
[System.Serializable]
public class ClientData
{
    public List<Client> clients;
    public Data data;
    public string label;
}

[System.Serializable]
public class Client
{
    public bool isManager;
    public int id;
    public string label;
}

[System.Serializable]
public class Data
{
    public ClientDetails[] details;
}

[System.Serializable]
public class ClientDetails
{
    public string address;
    public string name;
    public int points;
}
#endregion