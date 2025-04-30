using UnityEngine;
using UnityEngine.UI;

public class LetterButton : MonoBehaviour
{
    private Button button;
    private WordGameManager gameManager;
    private char letter;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void Init(WordGameManager manager, char character)
    {
        gameManager = manager;
        letter = character;
        GetComponentInChildren<Text>().text = letter.ToString();
    }

    private void OnClick()
    {
        gameManager.ProcessLetterClick(letter, this);
        button.interactable = false; // Делаем кнопку неактивной после нажатия
    }
}