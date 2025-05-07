using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WordGameManager : MonoBehaviour
{
    public List<WordQuestion> wordQuestions;
    public GameObject letterButtonPrefab;
    public GameObject slotPrefab;
    public Transform lettersContainer;
    public Transform slotsContainer;
    public Text hintText;
    public Image hintImage;
    public Text scoreText;
    public int levelNumber;

    private List<LetterButton> usedLetterButtons = new List<LetterButton>();
    private List<GameObject> slotObjects = new List<GameObject>();

    private string currentWord;
    private List<Text> slotTexts = new List<Text>();
    private int currentSlotIndex = 0;
    private int currentQuestionIndex = 0;
    private int score = 0;
    private int correctCount = 0;
    private int wrongCount = 0;

    void Start()
    {
        LoadQuestion();
        UpdateScore();
    }

    void LoadQuestion()
    {
        ClearOldLetters();
        currentSlotIndex = 0;

        WordQuestion question = wordQuestions[currentQuestionIndex];
        currentWord = question.word.ToUpper();
        hintText.text = question.hint;
        hintImage.sprite = question.hintImage;

        for (int i = 0; i < currentWord.Length; i++)
        {
            GameObject slot = Instantiate(slotPrefab, slotsContainer);
            Text text = slot.GetComponentInChildren<Text>();
            text.text = "";
            slotTexts.Add(text);

            // Text text = slot.AddComponent<Text>();
            // text.fontSize = 40;
            // text.color = Color.black;
            // text.alignment = TextAnchor.MiddleCenter;
            // text.text = "";

            // slotTexts.Add(text);
            // slotObjects.Add(slot);

            // Добавим EventTrigger для возвращения буквы по нажатию
            Button slotButton = slot.GetComponent<Button>();
            int index = i;
            slotButton.onClick.AddListener(() => OnSlotClicked(index));

        }

        void OnSlotClicked(int index)
        {
            if (slotTexts[index].text != "")
            {
                char letter = slotTexts[index].text[0];
                GameObject letterObj = Instantiate(letterButtonPrefab, lettersContainer);
                letterObj.GetComponent<LetterButton>().Init(this, letter);
                slotTexts[index].text = "";

                currentSlotIndex = Mathf.Max(0, currentSlotIndex - 1);
            }
        }


        char[] shuffled = ShuffleLetters(currentWord);
        foreach (char c in shuffled)
        {
            GameObject letterObj = Instantiate(letterButtonPrefab, lettersContainer);
            letterObj.GetComponent<LetterButton>().Init(this, c);
        }
    }

    char[] ShuffleLetters(string word)
    {
        char[] letters = word.ToCharArray();
        System.Random rand = new System.Random();

        for (int i = letters.Length - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            (letters[i], letters[j]) = (letters[j], letters[i]);
        }
        return letters;
    }

    public void ProcessLetterClick(char letter, LetterButton button)
    {
        if (currentSlotIndex < slotTexts.Count)
        {
            slotTexts[currentSlotIndex].text = letter.ToString();
            usedLetterButtons.Add(button);
            button.gameObject.SetActive(false);
            currentSlotIndex++;
        }
    }

    public void RemoveLetterFromSlot(int index)
    {
        if (index < usedLetterButtons.Count && slotTexts[index].text != "")
        {
            slotTexts[index].text = "";
            LetterButton button = usedLetterButtons[index];
            button.gameObject.SetActive(true);
            usedLetterButtons[index] = null;
            currentSlotIndex = index;
        }
    }

    public void CheckAnswer()
    {
        string assembled = "";
        foreach (Text slotText in slotTexts)
            assembled += slotText.text;

        bool isCorrect = assembled.ToUpper() == currentWord.ToUpper();
        if (isCorrect)
        {
            score += 100;
            correctCount++;
            UpdateScore();
            ShowFeedback("Correct!", Color.green);
        }
        else
        {
            wrongCount++;
            ShowFeedback("Wrong!", Color.red);
        }

        Invoke(nameof(AdvanceOrFinish), 1f);
    }

    private void AdvanceOrFinish()
    {
        if (currentQuestionIndex < wordQuestions.Count - 1)
        {
            currentQuestionIndex++;
            LoadQuestion();
        }
        else
        {
            PlayerPrefs.SetInt("LastLevel", levelNumber);
            PlayerPrefs.SetInt("CorrectAnswers", correctCount);
            PlayerPrefs.SetInt("WrongAnswers", wrongCount);
            PlayerPrefs.SetInt("LastScore", score);

            SceneManager.LoadScene("ResultsScene");
        }
    }

    public GameObject feedbackCanvas;
    public Text feedbackText;

    void ShowFeedback(string text, Color color)
    {
        feedbackText.text = text;
        feedbackText.color = color;
        feedbackCanvas.SetActive(true);
        Invoke("HideFeedback", 1f);
    }

    public void HideFeedback()
    {
        feedbackCanvas.SetActive(false);
    }




    // void CheckSolution()
    // {
    //     string assembled = "";
    //     foreach (Text slotText in slotTexts)
    //     {
    //         assembled += slotText.text;
    //     }

    //     if (assembled.ToUpper() == currentWord.ToUpper())
    //     {
    //         score += 100;
    //         UpdateScore();
    //         NextQuestion();
    //     }
    //     else
    //     {
    //         Debug.Log("Incorrect! The right one: " + currentWord);
    //         Invoke("ResetLetters", 2f);
    //     }
    // }

    void ResetLetters()
    {
        currentSlotIndex = 0;
        foreach (Text slotText in slotTexts)
        {
            slotText.text = "";
        }

        foreach (Transform child in lettersContainer)
        {
            child.GetComponent<Button>().interactable = true;
        }
    }

    void UpdateScore() => scoreText.text = "Score: " + score;

    void ClearOldLetters()
    {
        foreach (Transform child in lettersContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in slotsContainer)
        {
            Destroy(child.gameObject);
        }

        slotTexts.Clear();
    }

    // void NextQuestion()
    // {
    //     currentQuestionIndex++;
    //     if (currentQuestionIndex < wordQuestions.Count)
    //     {
    //         LoadQuestion();
    //     }
    //     else
    //     {
    //         Debug.Log("Game over!");
    //     }
    // }
}