using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayClassicMode()
    {
        SceneManager.LoadScene("ClassicMode");
    }

    public void PlayArcadeMode()
    {
        SceneManager.LoadScene("ArcadeMode");
    }

    public void OpenLeaderboards()
    {
        // Implement Leaderboards UI later
        Debug.Log("Opening Leaderboards...");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
