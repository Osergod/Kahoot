using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestionController : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public Slider timerSlider;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    [Header("Configuración")]
    public float timeRemaining;
    private bool questionActive = false;
    private KahootQuestion currentQuestion;

    void Start()
    {
        // Asegurarse de que tenemos un juego activo
        if (GameManager.Instance.currentQuiz == null)
        {
            Debug.LogError("No hay quiz seleccionado!");
            return;
        }

        StartQuestion();
    }

    void Update()
    {
        if (questionActive)
        {
            UpdateTimer();
        }
    }

    void StartQuestion()
    {
        // Obtener pregunta actual
        currentQuestion = GameManager.Instance.GetCurrentQuestion();

        if (currentQuestion == null)
        {
            Debug.Log("No hay más preguntas");
            return;
        }

        // Mostrar pregunta
        questionText.text = currentQuestion.question;

        // Configurar tiempo
        timeRemaining = currentQuestion.timeLimit;
        timerSlider.maxValue = currentQuestion.timeLimit;
        timerSlider.value = timeRemaining;

        // Configurar botones de respuesta
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < currentQuestion.answers.Length)
            {
                // Cambiar texto del botón
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.answers[i];

                // Reiniciar color (normal)
                ColorBlock colors = answerButtons[i].colors;
                colors.normalColor = GetButtonColor(i);
                answerButtons[i].colors = colors;

                // Hacer interactuable
                answerButtons[i].interactable = true;

                // Asignar evento (forma simple)
                int buttonIndex = i; // Importante: crear copia local
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => OnAnswerSelected(buttonIndex));
            }
            else
            {
                // Ocultar botones extras
                answerButtons[i].gameObject.SetActive(false);
            }
        }

        // Actualizar puntuación
        scoreText.text = $"Puntuación: {GameManager.Instance.playerScore}";

        questionActive = true;
    }

    void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;
        timerSlider.value = timeRemaining;
        timerText.text = $"{Mathf.Ceil(timeRemaining)}s";

        if (timeRemaining <= 0)
        {
            OnTimeUp();
        }
    }

    void OnAnswerSelected(int selectedIndex)
    {
        if (!questionActive) return;

        questionActive = false;

        // Desactivar todos los botones
        foreach (Button btn in answerButtons)
        {
            btn.interactable = false;
        }

        // Verificar respuesta
        bool isCorrect = (selectedIndex == currentQuestion.correctAnswer);

        // Mostrar feedback visual
        ShowAnswerFeedback(isCorrect, selectedIndex);

        // Calcular puntos
        if (isCorrect)
        {
            int points = Mathf.RoundToInt(timeRemaining * 10);
            GameManager.Instance.AddScore(points);
            scoreText.text = $"Puntuación: {GameManager.Instance.playerScore} (+{points})";
        }

        // Siguiente pregunta después de 2 segundos
        Invoke("LoadNextQuestion", 2f);
    }

    void OnTimeUp()
    {
        if (!questionActive) return;

        questionActive = false;

        // Desactivar botones
        foreach (Button btn in answerButtons)
        {
            btn.interactable = false;
        }

        // Mostrar respuesta correcta
        ShowAnswerFeedback(false, currentQuestion.correctAnswer);

        // Siguiente pregunta después de 2 segundos
        Invoke("LoadNextQuestion", 2f);
    }

    void ShowAnswerFeedback(bool isCorrect, int selectedIndex)
    {
        // Cambiar color del botón seleccionado
        ColorBlock colors = answerButtons[selectedIndex].colors;

        if (isCorrect)
        {
            colors.normalColor = Color.green;
            colors.disabledColor = Color.green;
        }
        else
        {
            // Si es incorrecto, mostrar en rojo
            colors.normalColor = Color.red;
            colors.disabledColor = Color.red;

            // Mostrar la correcta en verde
            if (selectedIndex != currentQuestion.correctAnswer)
            {
                ColorBlock correctColors = answerButtons[currentQuestion.correctAnswer].colors;
                correctColors.normalColor = Color.green;
                correctColors.disabledColor = Color.green;
                answerButtons[currentQuestion.correctAnswer].colors = correctColors;
            }
        }

        answerButtons[selectedIndex].colors = colors;
    }

    void LoadNextQuestion()
    {
        GameManager.Instance.NextQuestion();

        if (GameManager.Instance.HasMoreQuestions())
        {
            // Recargar escena para nueva pregunta (forma simple)
            SceneLoader.Instance.ReloadCurrentScene();
        }
        else
        {
            // Ir a resultados
            Debug.Log("Juego terminado");
            // Aquí luego iremos a la escena de resultados
        }
    }

    Color GetButtonColor(int index)
    {
        switch (index)
        {
            case 0: return new Color(0.86f, 0.15f, 0.15f); // Rojo
            case 1: return new Color(0.09f, 0.64f, 0.29f); // Verde
            case 2: return new Color(0.15f, 0.39f, 0.92f); // Azul
            case 3: return new Color(0.79f, 0.53f, 0.02f); // Amarillo
            default: return Color.white;
        }
    }
}