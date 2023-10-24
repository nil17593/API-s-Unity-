using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ApiClient : MonoBehaviour
{
    private string apiUrl = "https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";

    [System.Serializable]
    private class ClientData
    {
        public List<Client> clients;
        public Data data;
        public string label;
    }

    [System.Serializable]
    private class Client
    {
        public bool isManager;
        public int id;
        public string label;
    }

    [System.Serializable]
    private class Data
    {
        public ClientDetails[] details;
    }

    [System.Serializable]
    private class ClientDetails
    {
        public string address;
        public string name;
        public int points;
    }

    void Start()
    {
        StartCoroutine(FetchDataFromAPI());
    }

    IEnumerator FetchDataFromAPI()
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
                ClientData clientData = JsonUtility.FromJson<ClientData>(jsonData);
                // Handle the data as needed.
                HandleApiResponse(clientData);
            }
        }
    }

    void HandleApiResponse(ClientData clientData)
    {
        foreach(Client client in clientData.clients)
        {
            Debug.Log("ID= " + client.id);
            Debug.Log("ismanager= " + client.isManager);
            Debug.Log("label= " + client.label);
        }
    }
}
