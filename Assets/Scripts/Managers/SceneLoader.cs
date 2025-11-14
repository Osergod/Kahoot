using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadMainMenu() => LoadScene("MainMenu");
    public void LoadKahootSelection() => LoadScene("KahootSelection");
    public void LoadGameScene() => LoadScene("GameScene");
    public void LoadLeaderboards() => LoadScene("LeaderboardView");
    public void LoadReports() => LoadScene("ReportsView");
    public void LoadAbout() => LoadScene("About");
}