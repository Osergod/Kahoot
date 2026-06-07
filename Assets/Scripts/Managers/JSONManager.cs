using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSONManager : MonoBehaviour
{
    private string userKahootsPath;

    void Awake()
    {
        SetupPaths();
        Debug.Log("JSONManager inicializado");
    }

    private void SetupPaths()
    {
        userKahootsPath = Path.Combine(Application.persistentDataPath, "UserKahoots");

        if (!Directory.Exists(userKahootsPath))
        {
            Directory.CreateDirectory(userKahootsPath);
            Debug.Log("Directorio UserKahoots creado: " + userKahootsPath);
        }
    }

    public List<KahootQuiz> LoadAllKahoots()
    {
        List<KahootQuiz> allKahoots = new List<KahootQuiz>();

        // 1. Cargar de Resources (kahoots por defecto)
        LoadFromResources(allKahoots);

        // 2. Cargar de usuario (persistentDataPath)
        LoadFromUserFolder(allKahoots);

        Debug.Log($"Total kahoots cargados: {allKahoots.Count}");
        return allKahoots;
    }

    private void LoadFromResources(List<KahootQuiz> kahootsList)
    {
        try
        {
            // Cargar todos los JSON de Resources/DefaultKahoots
            TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("DefaultKahoots");
            Debug.Log($"Encontrados {jsonFiles.Length} archivos en Resources");

            foreach (TextAsset jsonFile in jsonFiles)
            {
                KahootQuiz quiz = ParseJSON(jsonFile.text);
                if (quiz != null)
                {
                    quiz.quizId = "default_" + jsonFile.name;
                    kahootsList.Add(quiz);
                    Debug.Log($"✓ Cargado: {quiz.title}");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error cargando de Resources: " + e.Message);
        }
    }

    private void LoadFromUserFolder(List<KahootQuiz> kahootsList)
    {
        try
        {
            if (!Directory.Exists(userKahootsPath))
            {
                Debug.Log("No existe carpeta de usuario");
                return;
            }

            string[] jsonFiles = Directory.GetFiles(userKahootsPath, "*.json");
            Debug.Log($"Encontrados {jsonFiles.Length} archivos de usuario");

            foreach (string filePath in jsonFiles)
            {
                try
                {
                    string jsonContent = File.ReadAllText(filePath);
                    KahootQuiz quiz = ParseJSON(jsonContent);
                    if (quiz != null)
                    {
                        quiz.quizId = "user_" + Path.GetFileNameWithoutExtension(filePath);
                        kahootsList.Add(quiz);
                        Debug.Log($"✓ Cargado de usuario: {quiz.title}");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error leyendo {Path.GetFileName(filePath)}: {e.Message}");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error accediendo a carpeta de usuario: " + e.Message);
        }
    }

    private KahootQuiz ParseJSON(string jsonContent)
    {
        try
        {
            KahootQuiz quiz = JsonUtility.FromJson<KahootQuiz>(jsonContent);

            // Validaciones básicas
            if (quiz == null)
            {
                Debug.LogError("JSON vacío o mal formado");
                return null;
            }

            if (string.IsNullOrEmpty(quiz.title))
            {
                quiz.title = "Kahoot sin título";
            }

            if (quiz.questions == null || quiz.questions.Length == 0)
            {
                Debug.LogWarning($"Kahoot '{quiz.title}' no tiene preguntas");
                quiz.questions = new KahootQuestion[0];
            }

            return quiz;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error parseando JSON: " + e.Message);
            return null;
        }
    }

    public KahootQuiz GetKahootById(string quizId)
    {
        List<KahootQuiz> allKahoots = LoadAllKahoots();
        foreach (KahootQuiz quiz in allKahoots)
        {
            if (quiz.quizId == quizId)
                return quiz;
        }
        return null;
    }
}