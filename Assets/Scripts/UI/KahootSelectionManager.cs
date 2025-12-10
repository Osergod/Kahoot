using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KahootSelectionManager : MonoBehaviour
{
    [Header("Referencias UI - IMPORTANTE")]
    public Transform kahootListContent;      // El Content del ScrollView
    public GameObject kahootItemPrefab;      // El prefab que creamos
    public TMP_InputField playerNameInput;   // InputField para el nombre

    [Header("Botón Volver")]
    public Button backButton;                // Botón para volver al menú

    private List<KahootQuiz> availableKahoots = new List<KahootQuiz>();

    void Start()
    {
        Debug.Log("=== INICIANDO DIAGNÓSTICO ===");

        // 1. Verificar referencias
        Debug.Log($"Referencia kahootListContent: {kahootListContent != null}");
        Debug.Log($"Referencia kahootItemPrefab: {kahootItemPrefab != null}");
        Debug.Log($"Referencia playerNameInput: {playerNameInput != null}");

        // 2. Verificar prefab
        if (kahootItemPrefab != null)
        {
            Debug.Log("Prefab encontrado. Buscando componentes...");

            // Buscar componentes en el prefab
            Transform titleText = kahootItemPrefab.transform.Find("TitleText");
            Transform descText = kahootItemPrefab.transform.Find("DescriptionText");
            Transform playButton = kahootItemPrefab.transform.Find("PlayButton");

            Debug.Log($"TitleText en prefab: {titleText != null}");
            Debug.Log($"DescriptionText en prefab: {descText != null}");
            Debug.Log($"PlayButton en prefab: {playButton != null}");
        }

        // Configurar botón volver
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }

        // Cargar kahoots
        LoadKahoots();

        // Llenar la lista
        PopulateKahootList();
    }

    void LoadKahoots()
    {
        // Añadir log de diagnóstico
        Debug.Log("=== CARGANDO KAHOOTS ===");

        JSONManager jsonManager = FindObjectOfType<JSONManager>();

        if (jsonManager != null)
        {
            Debug.Log("JSONManager encontrado");
            availableKahoots = jsonManager.LoadAllKahoots();
            Debug.Log($"Kahoots cargados: {availableKahoots.Count}");

            foreach (var kahoot in availableKahoots)
            {
                Debug.Log($"- {kahoot.title}: {kahoot.questions?.Length ?? 0} preguntas");
            }
        }
        else
        {
            Debug.LogWarning("JSONManager NO encontrado. Usando datos de prueba.");
            CreateTestKahoots();
        }
    }

    void CreateTestKahoots()
    {
        Debug.Log("Creando kahoots de prueba...");

        // Kahoot 1 - Matemáticas
        KahootQuiz mathQuiz = new KahootQuiz();
        mathQuiz.quizId = "test_math_001";
        mathQuiz.title = "Matemáticas Test";
        mathQuiz.description = "Preguntas básicas de matemáticas";

        KahootQuestion q1 = new KahootQuestion();
        q1.question = "¿Cuánto es 2 + 2?";
        q1.answers = new string[] { "3", "4", "5", "6" };
        q1.correctAnswer = 1;
        q1.timeLimit = 10f;

        KahootQuestion q2 = new KahootQuestion();
        q2.question = "¿Cuánto es 5 × 3?";
        q2.answers = new string[] { "10", "12", "15", "18" };
        q2.correctAnswer = 2;
        q2.timeLimit = 15f;

        mathQuiz.questions = new KahootQuestion[] { q1, q2 };
        availableKahoots.Add(mathQuiz);

        // Kahoot 2 - Geografía
        KahootQuiz geoQuiz = new KahootQuiz();
        geoQuiz.quizId = "test_geo_001";
        geoQuiz.title = "Geografía Test";
        geoQuiz.description = "Capitales del mundo";

        KahootQuestion q3 = new KahootQuestion();
        q3.question = "¿Cuál es la capital de Francia?";
        q3.answers = new string[] { "Londres", "Berlín", "París", "Madrid" };
        q3.correctAnswer = 2;
        q3.timeLimit = 12f;

        geoQuiz.questions = new KahootQuestion[] { q3 };
        availableKahoots.Add(geoQuiz);

        Debug.Log("Kahoots de prueba creados: " + availableKahoots.Count);
    }

    void PopulateKahootList()
    {
        Debug.Log("=== POBLANDO LISTA ===");

        if (kahootListContent == null)
        {
            Debug.LogError("ERROR: kahootListContent es NULL!");
            return;
        }

        if (kahootItemPrefab == null)
        {
            Debug.LogError("ERROR: kahootItemPrefab es NULL!");
            return;
        }

        // Limpiar lista
        int childCount = kahootListContent.childCount;
        Debug.Log($"Limpiando {childCount} hijos existentes");

        foreach (Transform child in kahootListContent)
        {
            Destroy(child.gameObject);
        }

        // Crear items
        Debug.Log($"Creando {availableKahoots.Count} items");

        for (int i = 0; i < availableKahoots.Count; i++)
        {
            Debug.Log($"Creando item {i}: {availableKahoots[i].title}");
            CreateKahootItem(availableKahoots[i]);
        }

        Debug.Log("=== FIN POBLACIÓN ===");
    }

    void CreateKahootItem(KahootQuiz kahoot)
    {
        // Instanciar el prefab
        GameObject item = Instantiate(kahootItemPrefab, kahootListContent);

        // Buscar los textos en el prefab
        TextMeshProUGUI titleText = item.transform.Find("TitleText")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI descText = item.transform.Find("DescriptionText")?.GetComponent<TextMeshProUGUI>();

        if (titleText != null && descText != null)
        {
            titleText.text = kahoot.title;
            descText.text = kahoot.description;
        }
        else
        {
            Debug.LogError("No se encontraron TitleText o DescriptionText en el prefab");
        }

        // Buscar el botón JUGAR
        Button playButton = item.transform.Find("PlayButton")?.GetComponent<Button>();

        if (playButton != null)
        {
            // IMPORTANTE: Crear una variable local para evitar problemas de referencia
            KahootQuiz selectedKahoot = kahoot;
            playButton.onClick.AddListener(() => OnKahootSelected(selectedKahoot));
        }
        else
        {
            Debug.LogError("No se encontró PlayButton en el prefab");
        }
    }

    void OnKahootSelected(KahootQuiz selectedKahoot)
    {
        Debug.Log($"Kahoot seleccionado: {selectedKahoot.title}");

        // Obtener nombre del jugador
        string playerName = "Jugador";
        if (playerNameInput != null && !string.IsNullOrEmpty(playerNameInput.text))
        {
            playerName = playerNameInput.text;
        }

        Debug.Log($"Jugador: {playerName}");

        // Buscar GameManager
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("No se encontró GameManager!");
            return;
        }

        // Iniciar nuevo juego
        gameManager.StartNewGame(selectedKahoot, playerName);

        // Buscar SceneLoader
        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();

        if (sceneLoader != null)
        {
            sceneLoader.LoadGameScene();
        }
        else
        {
            Debug.LogError("No se encontró SceneLoader!");
        }
    }

    void OnBackButtonClicked()
    {
        Debug.Log("Volviendo al menú principal...");

        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
        if (sceneLoader != null)
        {
            sceneLoader.LoadMainMenu();
        }
    }
}