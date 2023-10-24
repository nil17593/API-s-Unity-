using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class APIClient : MonoBehaviour
{
    [SerializeField] private string apiUrl = "https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";

    private void Start()
    {
        StartCoroutine(GetAPIData());
    }

    IEnumerator GetAPIData()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("API request failed: " + request.error);
        }
        else
        {
            // Process the JSON response
            string jsonResult = request.downloadHandler.text;
            Debug.Log(jsonResult);
            // Add code to parse and handle the JSON data.
        }
    }
}
