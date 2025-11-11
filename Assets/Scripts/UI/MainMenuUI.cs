using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Popup How To Play")]
    public GameObject howToPlayPopup;     // Gán trong Inspector
    public HowToPlayTabs howToPlayTabs;   // Gán trong Inspector

    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("level");  // Scene gameplay
    }

    // Mở popup + luôn về tab đầu tiên
    public void ShowHowToPlay()
    {
        if (howToPlayPopup != null)
            howToPlayPopup.SetActive(true);

        if (howToPlayTabs != null)
            howToPlayTabs.ShowPage(0);    // Trang "Hướng dẫn chơi"
    }

    // Đóng popup
    public void HideHowToPlay()
    {
        if (howToPlayPopup != null)
            howToPlayPopup.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
