using System.Collections;
using UnityEngine;

public class TestJSONLoader : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== INICIANDO PRUEBA JSON ===");
        StartCoroutine(TestJSONLoading());
    }

    IEnumerator TestJSONLoading()
    {
        // Esperar que todo se inicialice
        yield return new WaitForSeconds(1f);

        JSONManager jsonManager = FindObjectOfType<JSONManager>();

        if (jsonManager == null)
        {
            Debug.LogError("No se encontró JSONManager en la escena!");
            yield break;
        }

        // Probar carga de kahoots
        var kahoots = jsonManager.LoadAllKahoots();

        // Mostrar resultados
        Debug.Log("=== PRUEBA COMPLETADA ===");
        Debug.Log($"Kahoots cargados correctamente: {kahoots.Count}");

        foreach (var kahoot in kahoots)
        {
            Debug.Log($"- {kahoot.title}: {kahoot.questions.Length} preguntas");
        }
    }
}