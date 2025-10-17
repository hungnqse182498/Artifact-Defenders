using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ArtifactHealthUI : MonoBehaviour
{
    Slider slider;
    [SerializeField] Artifact artifact;
    [SerializeField] private TextMeshProUGUI healthText;

    void Start()
    {
        slider = GetComponent<Slider>();

        if (artifact != null)
        {
            // Thiết lập MaxValue chỉ cần làm một lần
            slider.maxValue = artifact.maxHealth;

            // 🌟 ĐĂNG KÝ VÀO EVENT
            artifact.OnHealthChanged += UpdateHealthUI;

            // Cập nhật lần đầu
            UpdateHealthUI(artifact.health, artifact.maxHealth);
        }
    }

    // ⛔ LOẠI BỎ HÀM UPDATE() HOẶC ĐỂ TRỐNG
    // void Update()
    // {
    //     // Không cần thiết nữa!
    // }

    // ✨ HÀM CHỈ CHẠY KHI EVENT ĐƯỢC GỌI
    private void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        // 1. Cập nhật thanh Slider
        slider.value = currentHealth;

        // 2. Cập nhật Text HP
        if (healthText != null)
        {
            int currentHP = Mathf.RoundToInt(currentHealth);
            int maxHP = Mathf.RoundToInt(maxHealth);

            healthText.text = currentHP.ToString() + " / " + maxHP.ToString();
        }
    }

    // 🗑 QUAN TRỌNG: Hủy đăng ký khi đối tượng bị hủy (để tránh lỗi)
    private void OnDestroy()
    {
        if (artifact != null)
        {
            artifact.OnHealthChanged -= UpdateHealthUI;
        }
    }
}