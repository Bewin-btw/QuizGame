using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WordGameManager : MonoBehaviour
{
    public List<WordQuestion> wordQuestions;
    public GameObject letterButtonPrefab;
    public Transform lettersContainer;
    public Transform slotsContainer;
    public Text hintText;
    public Image hintImage;
    public Text scoreText;

    private string currentWord;
    private List<Text> slotTexts = new List<Text>();
    private int currentSlotIndex = 0;
    private int currentQuestionIndex = 0;
    private int score = 0;

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
            GameObject slot = new GameObject("Slot");
            slot.transform.SetParent(slotsContainer);

            // Добавляем RectTransform
            RectTransform rt = slot.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(80, 80); // Размер слота

            // Настраиваем Text
            Text text = slot.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial");
            text.fontSize = 40;
            text.color = Color.black;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = ""; // Начальное значение

            slotTexts.Add(text);
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
            currentSlotIndex++;

            if (currentSlotIndex == slotTexts.Count)
            {
                CheckSolution();
            }
        }
    }

    void CheckSolution()
    {
        string assembled = "";
        foreach (Text slotText in slotTexts)
        {
            assembled += slotText.text;
        }

        if (assembled.ToUpper() == currentWord.ToUpper())
        {
            score += 100;
            UpdateScore();
            NextQuestion();
        }
        else
        {
            Debug.Log("Неверно! Правильный ответ: " + currentWord);
            Invoke("ResetLetters", 2f);
        }
    }

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

    void UpdateScore() => scoreText.text = "Счет: " + score;

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

    void NextQuestion()
    {
        currentQuestionIndex++;
        if (currentQuestionIndex < wordQuestions.Count)
        {
            LoadQuestion();
        }
        else
        {
            Debug.Log("Игра завершена!");
        }
    }
}