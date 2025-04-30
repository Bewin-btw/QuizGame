using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartLevel1() => SceneManager.LoadScene("Level1Scene");
    public void StartLevel2() => SceneManager.LoadScene("Level2Scene");
    public void StartLevel3() => SceneManager.LoadScene("Level3Scene");
    public void QuitGame() => Application.Quit();
}
