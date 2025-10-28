using UnityEngine;

public class UpgradeItem : MonoBehaviour
{
    public float pickupRadius = 2f;     // khoảng cách bắt đầu hút
    public float moveSpeed = 5f;        // tốc độ hút item về player
    private Transform player;           // lưu vị trí player
    private bool isBeingPulled = false; // trạng thái đang bị hút

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Nếu player lại gần trong bán kính
        if (distance < pickupRadius)
        {
            isBeingPulled = true;
        }

        // Khi đang bị hút thì bay dần về phía player
        if (isBeingPulled)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerUpgradeInventory inv = other.GetComponent<PlayerUpgradeInventory>();
            if (inv != null)
            {
                inv.AddStone(1);
                Destroy(gameObject); // nhặt xong biến mất
            }
        }
    }
}
