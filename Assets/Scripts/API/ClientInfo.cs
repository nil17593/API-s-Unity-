using UnityEngine;
using TMPro;

namespace SunBase.API
{
    /// <summary>
    /// Client info class attached on client popup prefab
    /// updates clients data into texts and show to user
    /// </summary>
    public class ClientInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI addressText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI clientLabeltext;

        public void UpdateData(string _addressText, string _nameText, string _scoreText, string _label)
        {
            addressText.text = "Address: " + _addressText;
            nameText.text = "Name: " + _nameText;
            scoreText.text = "Score: " + _scoreText;
            clientLabeltext.text = _label + " Information";
        }

    }
}