using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Text HintText;

    private void Start()
    {
        HintText.text = "Hint: " + GameManager.Instance.levelGoalText;
    }
    public void BackToMain()
    {
        GameManager.Instance.BackToMain();
        GameManager.Instance.TogglePause();
    }

    public void ResumeLevel()
    {
        GameManager.Instance.TogglePause();
    }

    public void QuitApplication()
    {
        Debug.Log("Quitting Application...");
        Application.Quit();
    }
}

