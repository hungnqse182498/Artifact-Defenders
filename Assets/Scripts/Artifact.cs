using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // 🌟 CẦN THIẾT cho Action

/// <summary>
/// Holds and manages the information about the Artifact's health and provides functions to damage and restore it's health.
/// </summary>
public class Artifact : MonoBehaviour
{
    // ✨ KHAI BÁO EVENT: Thông báo (máu hiện tại, máu tối đa) cho UI
    public event Action<float, float> OnHealthChanged;

    // Đã đổi sang private để kiểm soát việc thay đổi qua hàm
    private int _health;

    // Chúng ta vẫn giữ MaxHealth là public
    public int maxHealth;

    public int bleed;
    AudioSource audioSource;
    float timer;

    // ✨ THUỘC TÍNH PUBLIC ĐỂ CÁC SCRIPT KHÁC CÓ THỂ ĐỌC MÁU HIỆN TẠI (Nhưng không thay đổi trực tiếp)
    public int health
    {
        get { return _health; }
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        _health = maxHealth;
        timer = Time.time + 1;
        // Gọi Event lần đầu để UI hiển thị HP Max ngay lập tức
        OnHealthChanged?.Invoke(_health, maxHealth);
    }

    void Update()
    {
        if (Time.time > timer)
        {
            // 🛑 GỌI HÀM THAY THẾ CHO health -= bleed;
            ChangeHealth(-bleed);
            timer = Time.time + 1;
        }

        // Không cần health = 0; ở đây nữa, vì logic đó nằm trong ChangeHealth
    }

    // 🛑 THAY THẾ HÀM DAMAGE CŨ
    public void Damage(int amount)
    {
        // Gọi hàm kiểm soát máu mới
        ChangeHealth(-amount);
    }

    // ✨ HÀM CHÍNH ĐỂ THAY ĐỔI MÁU VÀ GỌI EVENT
    public void ChangeHealth(int amount)
    {
        // 1. Tính toán giá trị mới
        int newHealth = _health + amount;

        // 2. Giới hạn giá trị máu (từ 0 đến MaxHealth)
        _health = Mathf.Clamp(newHealth, 0, maxHealth);

        // 3. 📞 GỌI EVENT ĐỂ CẬP NHẬT UI
        OnHealthChanged?.Invoke(_health, maxHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBackpack>() != null)
        {
            int fruitAmount = collision.GetComponent<PlayerBackpack>().TakeFruits();

            if (fruitAmount != 0) // Chỉ phát âm thanh khi có trái cây được lấy
            {
                audioSource.Play();
            }

            // 🛑 GỌI HÀM THAY THẾ CHO health += ...
            ChangeHealth(fruitAmount);

            // Không cần kiểm tra if(health > maxHealth) nữa vì ChangeHealth đã xử lý
        }
    }
}