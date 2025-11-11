using UnityEngine;
using UnityEngine.UI;

public class HowToPlayTabs : MonoBehaviour
{
    [System.Serializable]
    public class Page
    {
        public string title;
        [TextArea(5, 20)]
        public string content;
    }

    public Text headerText;
    public Text contentText;
    public ScrollRect scrollRect;
    public Page[] pages;

    public void ShowPage(int index)
    {
        if (pages == null || pages.Length == 0) return;
        if (index < 0 || index >= pages.Length) return;

        var page = pages[index];
        if (headerText != null && !string.IsNullOrEmpty(page.title))
            headerText.text = page.title;

        if (contentText != null)
            contentText.text = page.content;

        if (scrollRect != null)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }
}
