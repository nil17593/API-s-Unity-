using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

namespace SunBase.API
{
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
        [Header("Serialized fields")]
        [SerializeField] private GameObject clientButtonPrefab;
        [SerializeField] private Transform buttonParent;
        [SerializeField] private ClientInfo clientInfo;
        [SerializeField] private GameObject dataNotAvailablePanel;
        [SerializeField] private Ease ease;

        #region private fields
        private List<GameObject> clientButtons = new List<GameObject>();
        private List<Client> allClients = new List<Client>();
        private FilterType currentFilter = FilterType.AllClients;
        #endregion

        void Start()
        {
            if (dataNotAvailablePanel.activeSelf)
            {
                dataNotAvailablePanel.SetActive(false);
            }
            // Subscribe to the data received event
            ApiClient.Instance.OnClientDataReceived += OnClientDataReceived;
        }

        private void OnDisable()
        {
            // Unsubscribe from the data received event to avoid memory leaks
            ApiClient.Instance.OnClientDataReceived -= OnClientDataReceived;
        }

        public void OnCloseButtonClicked(RectTransform panel)
        {
            panel.DOScale(Vector2.zero, 0.5f).SetEase(ease);
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
                // Depending on the filter, control visibility here
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
            ClientDetails clientDetails = ApiClient.Instance.GetClientDetails(client.id);
            if (clientDetails != null)
            {

                clientInfo.transform.DOScale(Vector2.one, 0.5f).SetEase(ease);
                clientInfo.UpdateData(clientDetails.address, clientDetails.name, clientDetails.points.ToString(), client.label);
            }
            else
            {
                StartCoroutine(ActivateDeactivatePanel(dataNotAvailablePanel));
            }
        }

        private IEnumerator ActivateDeactivatePanel(GameObject panel)
        {
            panel.SetActive(true);
            yield return new WaitForSeconds(1f);
            panel.SetActive(false);
        }
    }
}