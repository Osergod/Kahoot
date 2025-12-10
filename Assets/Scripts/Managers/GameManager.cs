using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Datos del Juego")]
    public KahootQuiz currentQuiz;
    public int currentQuestionIndex = 0;
    public int playerScore = 0;
    public string playerName = "Jugador";
    public bool isPlaying = false;

    void Awake()
    {
        // Singleton pattern
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

    public void StartNewGame(KahootQuiz quiz, string name)
    {
        currentQuiz = quiz;
        playerName = name;
        currentQuestionIndex = 0;
        playerScore = 0;
        isPlaying = true;

        Debug.Log($"Nuevo juego: {quiz.title}, Jugador: {name}");
    }

    public void AddScore(int points)
    {
        playerScore += points;
        Debug.Log($"Puntuación: {playerScore} (+{points})");
    }

    public bool HasMoreQuestions()
    {
        if (currentQuiz == null) return false;
        return currentQuestionIndex < currentQuiz.questions.Length;
    }

    public KahootQuestion GetCurrentQuestion()
    {
        if (currentQuiz == null || !HasMoreQuestions()) return null;
        return currentQuiz.questions[currentQuestionIndex];
    }

    public void NextQuestion()
    {
        if (HasMoreQuestions())
        {
            currentQuestionIndex++;
        }
        else
        {
            EndGame();
        }
    }

    void EndGame()
    {
        isPlaying = false;
        Debug.Log($"Juego terminado. Puntuación final: {playerScore}");
        // Aquí luego guardaremos en XML
    }
}