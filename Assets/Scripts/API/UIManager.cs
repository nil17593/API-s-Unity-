using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum FilterType
{
    AllClients,
    ManagersOnly,
    NonManagers
}
/// <summary>
/// Ui Manager class to handle all the Ui functions
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject clientButtonPrefab;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private ApiClient apiClient;

    private List<GameObject> clientButtons = new List<GameObject>();
    private List<Client> allClients = new List<Client>();
    private FilterType currentFilter = FilterType.AllClients;

    void Start()
    {
        // Subscribe to the data received event
        apiClient.OnClientDataReceived += OnClientDataReceived;
    }

    private void OnDisable()
    {
        // Unsubscribe from the data received event to avoid memory leaks
        apiClient.OnClientDataReceived -= OnClientDataReceived;
    }

    // Event handler for when client data is received
    void OnClientDataReceived(List<Client> clients)
    {
        // Store the list of clients
        allClients = clients;
        // Update the displayed clients based on the current filter
        ShowClients(currentFilter);
    }

    // Show clients based on the selected filter
    void ShowClients(FilterType filter)
    {
        // Deactivate all buttons
        foreach (var button in clientButtons)
        {
            button.SetActive(false);
        }

        foreach (Client client in allClients)
        {
            // Depending on the filter, you can control visibility here
            bool shouldShow = IsClientVisible(client, filter);

            GameObject clientButton;
            if (clientButtons.Count < allClients.Count)
            {
                clientButton = Instantiate(clientButtonPrefab, buttonParent);
                clientButtons.Add(clientButton);
            }
            else
            {
                clientButton = clientButtons[allClients.IndexOf(client)];
            }

            TextMeshProUGUI buttonText = clientButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = client.label;
            clientButton.SetActive(shouldShow);

            // Add a click event to the button
            Button buttonComponent = clientButton.GetComponent<Button>();
            int index = allClients.IndexOf(client);
            buttonComponent.onClick.RemoveAllListeners();
            buttonComponent.onClick.AddListener(() => OnClientButtonClicked(allClients[index]));
        }

        // Update the current filter
        currentFilter = filter;
    }

    // Check if a client should be visible based on the filter
    bool IsClientVisible(Client client, FilterType filter)
    {
        switch (filter)
        {
            case FilterType.AllClients:
                return true;
            case FilterType.ManagersOnly:
                return client.isManager;
            case FilterType.NonManagers:
                return !client.isManager;
            default:
                return true;
        }
    }

    // Event handler for when a client button is clicked
    void OnClientButtonClicked(Client client)
    {
        // Handle the click event, e.g., display client details
        Debug.Log("Clicked on client: " + client.label);
    }
}
