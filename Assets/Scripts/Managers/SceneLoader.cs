using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("Nombres de Escenas")]
    public string mainMenuScene = "MainMenu";
    public string kahootSelectionScene = "KahootSelection";
    public string gameScene = "GameScene";
    public string leaderboardScene = "LeaderboardView";
    public string reportsScene = "ReportsView";
    public string aboutScene = "About";

    void Awake()
    {
        // Sistema Singleton robusto
        if (Instance == null)
        {
            // Primera vez - configurar como instancia única
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("SceneLoader inicializado como Singleton");
        }
        else if (Instance != this)
        {
            // Ya existe otra instancia - destruir esta duplicada
            Debug.LogWarning($"Destruyendo SceneLoader duplicado en escena: {SceneManager.GetActiveScene().name}");
            Destroy(gameObject);
            return; // Importante: salir para no ejecutar más código
        }

        // Opcional: renombrar para identificar mejor
        gameObject.name = "SceneLoader (Singleton)";
    }

    void Start()
    {
        Debug.Log($"SceneLoader listo. Escena actual: {SceneManager.GetActiveScene().name}");
    }

    // ========== MÉTODOS PÚBLICOS PARA BOTONES ==========

    // Método genérico para cargar cualquier escena
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("No se puede cargar escena: nombre vacío");
            return;
        }

        Debug.Log($"Intentando cargar escena: {sceneName}");

        // Verificar si la escena existe en Build Settings
        if (!DoesSceneExist(sceneName))
        {
            Debug.LogError($"La escena '{sceneName}' no existe en Build Settings");
            return;
        }

        // Cargar la escena
        SceneManager.LoadScene(sceneName);
    }

    // Métodos específicos (para configurar en botones del Editor)
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

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    // ========== MÉTODOS UTILES ==========

    // Verificar si una escena existe en Build Settings
    private bool DoesSceneExist(string sceneName)
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

    // Recargar escena actual
    public void ReloadCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        LoadScene(currentScene);
    }

    // Obtener nombre de escena actual
    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    // Cargar escena por índice (útil para pruebas)
    public void LoadSceneByIndex(int buildIndex)
    {
        if (buildIndex >= 0 && buildIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(buildIndex);
        }
        else
        {
            Debug.LogError($"Índice de escena inválido: {buildIndex}");
        }
    }

    // ========== MÉTODOS PARA DEBUG ==========

    // Listar todas las escenas disponibles
    public void ListAllScenes()
    {
        Debug.Log("=== ESCENAS EN BUILD SETTINGS ===");
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            Debug.Log($"[{i}] {sceneName}");
        }
    }
}