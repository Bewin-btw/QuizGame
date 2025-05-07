// using UnityEngine;
// using UnityEngine.UI;

// public class SlotButton : MonoBehaviour
// {
//     private WordGameManager manager;
//     private Button button;
//     private Text text;
//     private LetterButton originalButton;

//     void Awake()
//     {
//         button = GetComponent<Button>();
//         text = GetComponentInChildren<Text>();
//         button.onClick.AddListener(OnClick);
//     }

//     public void SetManager(WordGameManager mgr)
//     {
//         manager = mgr;
//     }

//     public void SetLetter(string letter, LetterButton original)
//     {
//         text.text = letter;
//         originalButton = original;
//     }

//     public string GetLetter() => text.text;

//     public void ClearLetter()
//     {
//         text.text = "";
//         originalButton = null;
//     }

//     public LetterButton GetOriginalButton() => originalButton;

//     private void OnClick()
//     {
//         manager.OnSlotClick(this);
//     }
// }
