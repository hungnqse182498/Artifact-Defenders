using UnityEngine;

public class UpgradeItem : MonoBehaviour
{
    public enum StoneType { Tower, Attack }
    public StoneType stoneType; // loại đá này là gì
    public float pickupRadius = 2f;
    public float moveSpeed = 5f;
    private Transform player;
    private bool isBeingPulled = false;
    [SerializeField] private float attackIncrease = 2f;  // mức tăng damage khi nhặt đá Attack

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < pickupRadius)
            isBeingPulled = true;

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
        if (!other.CompareTag("Player")) return;

        switch (stoneType)
        {
            case StoneType.Tower:
                PlayerUpgradeInventory inv = other.GetComponent<PlayerUpgradeInventory>();
                if (inv != null)
                    inv.AddStone(1); // cộng đá trụ
                break;

            case StoneType.Attack:
                PlayerSlash slash = other.GetComponentInChildren<PlayerSlash>();
                if (slash != null)
                {
                    slash.damage += 2; // mỗi viên +2 damage
                    Debug.Log($"+2 Damage! New Damage: {slash.damage}");
                }
                break;
        }

        Destroy(gameObject); // nhặt xong biến mất
    }
}
