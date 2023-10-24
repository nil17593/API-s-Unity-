using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Manager class manage the state of game
/// </summary>
public class EraseCircleManager : MonoBehaviour
{
    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
