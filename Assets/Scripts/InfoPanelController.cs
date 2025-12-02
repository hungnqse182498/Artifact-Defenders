using UnityEngine;

public class InfoPanelController : MonoBehaviour
{
    public GameObject panel; // kéo thả panel trong Inspector

    public void TogglePanel()
    {
        bool isCurrentlyActive = panel.activeSelf;

        // Tắt tất cả panel khác
        PanelManager.Instance.CloseAllPanels();

        // Nếu trước đó panel đang tắt → bật panel
        if (!isCurrentlyActive)
        {
            panel.SetActive(true);

            // Ẩn guide khi bật panel
            PanelManager.Instance.HideGuides();
        }
        // Nếu panel đang bật → giữ tắt, Guide sẽ tự hiện vì CloseAllPanels() đã gọi ShowGuides()
    }
}
