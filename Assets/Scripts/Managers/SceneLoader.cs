using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("Scene Names")]
    public string mainMenuScene = "MainMenu";
    public string kahootSelectionScene = "KahootSelection";
    public string gameScene = "GameScene";
    public string leaderboardScene = "LeaderboardView";
    public string reportsScene = "ReportsView";
    public string aboutScene = "About";

    void Awake()
    {
        // Implementar patrón Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("SceneLoader inicializado y marcado como DontDestroyOnLoad");
        }
        else
        {
            Debug.LogWarning("Ya existe un SceneLoader en la escena. Destruyendo duplicado.");
            Destroy(gameObject);
            return;
        }
    }

    // Método genérico para cargar cualquier escena
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Nombre de escena vacío o nulo");
            return;
        }

        Debug.Log($"Cargando escena: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    // Métodos específicos para cada escena
    public void LoadMainMenu()
    {
        LoadScene(mainMenuScene);
    }

    public void LoadKahootSelection()
    {
        LoadScene(kahootSelectionScene);
    }

    public void LoadGameScene()
    {
        LoadScene(gameScene);
    }

    public void LoadLeaderboards()
    {
        LoadScene(leaderboardScene);
    }

    public void LoadReports()
    {
        LoadScene(reportsScene);
    }

    public void LoadAbout()
    {
        LoadScene(aboutScene);
    }

    // Método para salir del juego
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");

#if UNITY_EDITOR
        // Si estamos en el Editor, detener la reproducción
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // Si estamos en una build, salir de la aplicación
            Application.Quit();
#endif
    }

    // Método para recargar la escena actual
    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        LoadScene(currentSceneName);
    }

    // Método para verificar si una escena existe
    public bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameInBuild = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            if (sceneNameInBuild == sceneName)
            {
                return true;
            }
        }
        return false;
    }

    // Método para obtener el nombre de la escena actual
    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    // Método para cargar escena de forma asíncrona (útil para pantallas de carga)
    public void LoadSceneAsync(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Nombre de escena vacío o nulo");
            return;
        }

        if (!SceneExists(sceneName))
        {
            Debug.LogError($"La escena '{sceneName}' no existe en Build Settings");
            return;
        }

        Debug.Log($"Cargando escena asíncrona: {sceneName}");
        SceneManager.LoadSceneAsync(sceneName);
    }
}