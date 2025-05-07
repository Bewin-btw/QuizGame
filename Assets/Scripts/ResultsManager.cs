using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultsManager : MonoBehaviour
{
    public Text correctText;
    public Text wrongText;
    public Text encouragementText;

    void Start()
    {
        int correct = PlayerPrefs.GetInt("CorrectAnswers", 0);
        int wrong = PlayerPrefs.GetInt("WrongAnswers", 0);
        int score = PlayerPrefs.GetInt("LastScore", 0);

        correctText.text = "Correct: " + correct;
        wrongText.text = "Wrong: " + wrong;

        if (score >= 300)
            encouragementText.text = "Excellent work!";
        else if (score >= 100)
            encouragementText.text = "Good job, keep practicing!";
        else
            encouragementText.text = "Don't give up, you can do it!";

        PlayerPrefs.DeleteKey("CorrectAnswers");
        PlayerPrefs.DeleteKey("WrongAnswers");
        PlayerPrefs.DeleteKey("LastScore");
    }

    public void BackToMenu() => SceneManager.LoadScene("MainMenu");
    public void ReplayLevel() => SceneManager.LoadScene(PlayerPrefs.GetInt("LastLevel", 1));

    public void NextLevel()
    {
        int next = PlayerPrefs.GetInt("LastLevel", 1) + 1;
        // Если есть сцена с таким именем
        SceneManager.LoadScene("Level" + next + "Scene");
    }
}
