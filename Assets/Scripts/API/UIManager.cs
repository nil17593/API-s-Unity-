using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// Ui class will handle all the UI interactions

/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject clientButtonPrefab;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private ApiClient apiClient;

    private List<GameObject> clientButtons = new List<GameObject>();//list to hold the client button list track

    void Start()
    {
        apiClient.OnClientDataReceived += UpdateClientButtons;
    }
    private void OnDisable()
    {
        apiClient.OnClientDataReceived -= UpdateClientButtons;
    }

    // Event handler for when client data is received
    void UpdateClientButtons(List<Client> clients)
    {
        // Deactivate all client buttons
        foreach (var button in clientButtons)
        {
            button.SetActive(false);
        }

        // Reuse or create buttons for each client
        for (int i = 0; i < clients.Count; i++)
        {
            GameObject clientButton;
            if (i < clientButtons.Count)
            {
                clientButton = clientButtons[i];
            }
            else
            {
                clientButton = Instantiate(clientButtonPrefab, buttonParent);
                clientButtons.Add(clientButton);
            }

            // Set the client label
            TextMeshProUGUI buttonText = clientButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = clients[i].label;

            // Activate the button
            clientButton.SetActive(true);

            // Add a click event to the button
            Button buttonComponent = clientButton.GetComponent<Button>();
            int index = i; // Capture the index variable
            buttonComponent.onClick.RemoveAllListeners();
            buttonComponent.onClick.AddListener(() => OnClientButtonClicked(clients[index]));
        }
    }

    // Event handler for when a client button is clicked
    void OnClientButtonClicked(Client client)
    {
        // Handle the click event, e.g., display client details
        Debug.Log("Clicked on client: " + client.label);
    }
}
