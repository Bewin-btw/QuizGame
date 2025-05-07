using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public QuestionAndAnswers[] allQuestions;
    private List<QuestionAndAnswers> questions;

    public GameObject[] options;
    public Text QuestionTxt;
    public Image questionImage;
    public Text scoreText;
    public GameObject feedbackPanel;
    public CanvasGroup feedbackCanvasGroup;
    public Text feedbackText;

    public int currentLevel = 1;
    private int currentQuestionIndex = 0;
    private int score = 0;

    void Start()
    {
        questions = allQuestions.Where(q => q.level == currentLevel).ToList();
        currentQuestionIndex = 0;
        ShowQuestion();
        UpdateScore();
    }

    void ShowQuestion()
    {
        if (currentQuestionIndex >= questions.Count)
        {
            // Сохраняем в PlayerPrefs
            PlayerPrefs.SetInt("CorrectAnswers", score / 100);           // т.к. +100 за correct
            PlayerPrefs.SetInt("WrongAnswers", questions.Count - (score / 100));
            PlayerPrefs.SetInt("LastScore", score);
            PlayerPrefs.SetInt("LastLevel", currentLevel);

            SceneManager.LoadScene("ResultsScene");
            return;
        }
        // if (currentQuestionIndex >= questions.Count)
        // {
        //     feedbackText.text = "Level Complete! Final Score: " + score;
        //     return;
        // }

        QuestionAndAnswers question = questions[currentQuestionIndex];
        QuestionTxt.text = question.Question;
        questionImage.sprite = question.questionImage;

        if (question.questionAudio != null)
            AudioSource.PlayClipAtPoint(question.questionAudio, Camera.main.transform.position);

        for (int i = 0; i < options.Length; i++)
        {
            options[i].SetActive(i < question.Answers.Length);
            options[i].GetComponentInChildren<Text>().text = question.Answers[i];

            AnswerScript answerScript = options[i].GetComponent<AnswerScript>();
            answerScript.isCorrect = (i == question.CorrectAnswer - 1);
            answerScript.quizManager = this;
        }
    }

    public void OnAnswerSelected(bool isCorrect)
    {
        if (isCorrect)
        {
            score += 100;
            ShowFeedback("Correct!", Color.green);
        }
        else
        {
            ShowFeedback("Wrong!", Color.red);
        }

        UpdateScore();
        Invoke("NextQuestion", 1.5f);
    }

    void NextQuestion()
    {
        HideFeedback();
        currentQuestionIndex++;
        ShowQuestion();
    }

    void UpdateScore() => scoreText.text = "Score: " + score;

    void ShowFeedback(string text, Color color)
    {
        feedbackText.text = text;
        feedbackText.color = color;
        feedbackCanvasGroup.alpha = 1;
        feedbackCanvasGroup.blocksRaycasts = true;
    }

    public void HideFeedback()
    {
        feedbackCanvasGroup.alpha = 0;
        feedbackCanvasGroup.blocksRaycasts = false;
    }
}
