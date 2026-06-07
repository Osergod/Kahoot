using UnityEngine;

public class ExceptionManager : MonoBehaviour
{
    public static ExceptionManager Instance;

    void Awake()
    {
        // Patrón Singleton simple
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método básico que no hace nada (por ahora)
    public void ReportException(System.Exception exception, string context)
    {
        // Solo mostrar en consola por ahora
        Debug.LogWarning($"Excepción en {context}: {exception.Message}");

        // En otra rama implementaremos el sistema de informes completo
    }
}