using UnityEngine;
using UnityEngine.UI;

public class ButtonAutoConfig : MonoBehaviour
{
    [Header("Qué debe hacer este botón")]
    public string action = "MainMenu";

    void Start()
    {
        ConfigureButton();
    }

    void ConfigureButton()
    {
        Button button = GetComponent<Button>();
        if (button == null) return;

        // Buscar SceneLoader
        SceneLoader loader = FindObjectOfType<SceneLoader>();
        if (loader == null)
        {
            Debug.LogError($"No SceneLoader encontrado para botón {name}");
            return;
        }

        // Configurar según acción
        button.onClick.RemoveAllListeners();

        switch (action)
        {
            case "MainMenu": button.onClick.AddListener(loader.LoadMainMenu); break;
            case "KahootSelection": button.onClick.AddListener(loader.LoadKahootSelection); break;
            case "GameScene": button.onClick.AddListener(loader.LoadGameScene); break;
            case "Leaderboards": button.onClick.AddListener(loader.LoadLeaderboards); break;
            case "Reports": button.onClick.AddListener(loader.LoadReports); break;
            case "About": button.onClick.AddListener(loader.LoadAbout); break;
            case "Quit": button.onClick.AddListener(loader.QuitGame); break;
            default: Debug.LogWarning($"Acción desconocida: {action}"); break;
        }

        Debug.Log($"Botón {name} configurado para: {action}");
    }
}