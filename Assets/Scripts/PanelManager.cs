using UnityEngine;
using System.Collections.Generic;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance;

    [Header("All buttons that contain panels")]
    public List<GameObject> buttons = new List<GameObject>();

    [Header("Guide parent (show/hide)")]
    public GameObject guideTextsParent;

    void Awake()
    {
        Instance = this;
    }

    // Ẩn tất cả panel
    public void CloseAllPanels()
    {
        foreach (var btn in buttons)
        {
            Transform panel = btn.transform.Find("Panel");
            if (panel != null)
                panel.gameObject.SetActive(false);
        }

        // Sau khi ẩn hết panel, hiện Guide lại
        ShowGuides();
    }

    public void HideGuides()
    {
        if (guideTextsParent != null)
            guideTextsParent.SetActive(false);
    }

    public void ShowGuides()
    {
        if (guideTextsParent != null)
            guideTextsParent.SetActive(true);
    }
}
