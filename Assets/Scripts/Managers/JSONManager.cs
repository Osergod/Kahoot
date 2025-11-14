using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSONManager : MonoBehaviour
{
    private string userKahootsPath;

    void Awake()
    {
        SetupPaths();
    }

    private void SetupPaths()
    {
        // Ruta para kahoots del usuario
        userKahootsPath = Path.Combine(Application.persistentDataPath, "UserKahoots");

        // Crear directorio si no existe
        if (!Directory.Exists(userKahootsPath))
        {
            Directory.CreateDirectory(userKahootsPath);
            Debug.Log("Directorio UserKahoots creado en: " + userKahootsPath);
        }
    }

    // Método principal para cargar todos los kahoots
    public List<KahootQuiz> LoadAllKahoots()
    {
        List<KahootQuiz> allKahoots = new List<KahootQuiz>();

        // 1. Cargar kahoots por defecto
        LoadDefaultKahoots(allKahoots);

        // 2. Cargar kahoots de usuario
        LoadUserKahoots(allKahoots);

        Debug.Log($"Total de kahoots cargados: {allKahoots.Count}");
        return allKahoots;
    }

    private void LoadDefaultKahoots(List<KahootQuiz> kahootsList)
    {
        try
        {
            // Cargar desde Resources/DefaultKahoots
            TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("DefaultKahoots");
            Debug.Log($"Encontrados {jsonFiles.Length} kahoots por defecto");

            foreach (TextAsset jsonFile in jsonFiles)
            {
                KahootQuiz quiz = ParseKahootJSON(jsonFile.text);
                if (quiz != null)
                {
                    kahootsList.Add(quiz);
                    Debug.Log($"Kahoot cargado: {quiz.title}");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error cargando kahoots por defecto: " + e.Message);
        }
    }

    private void LoadUserKahoots(List<KahootQuiz> kahootsList)
    {
        try
        {
            if (!Directory.Exists(userKahootsPath))
            {
                Debug.Log("No existe directorio de kahoots de usuario");
                return;
            }

            string[] jsonFiles = Directory.GetFiles(userKahootsPath, "*.json");
            Debug.Log($"Encontrados {jsonFiles.Length} kahoots de usuario");

            foreach (string filePath in jsonFiles)
            {
                try
                {
                    string jsonContent = File.ReadAllText(filePath);
                    KahootQuiz quiz = ParseKahootJSON(jsonContent);
                    if (quiz != null)
                    {
                        kahootsList.Add(quiz);
                        Debug.Log($"✓ Kahoot de usuario cargado: {quiz.title}");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error con archivo {Path.GetFileName(filePath)}: {e.Message}");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error accediendo a kahoots de usuario: " + e.Message);
        }
    }

    private KahootQuiz ParseKahootJSON(string jsonContent)
    {
        try
        {
            KahootQuiz quiz = JsonUtility.FromJson<KahootQuiz>(jsonContent);

            // Validaciones básicas
            if (string.IsNullOrEmpty(quiz.title))
            {
                Debug.LogError("Kahoot sin título");
                return null;
            }

            if (quiz.questions == null || quiz.questions.Length == 0)
            {
                Debug.LogError($"Kahoot '{quiz.title}' no tiene preguntas");
                return null;
            }

            return quiz;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error parseando JSON: " + e.Message);
            return null;
        }
    }
}