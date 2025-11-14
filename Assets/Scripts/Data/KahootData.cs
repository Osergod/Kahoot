using System;

[Serializable]
public class KahootQuestion
{
    public string question;
    public string[] answers;
    public int correctAnswer;
    public float timeLimit;
}

[Serializable]
public class KahootQuiz
{
    public string quizId;
    public string title;
    public string description;
    public KahootQuestion[] questions;
}