using UnityEngine;

public class BoomerangSkill : MonoBehaviour
{
    public BoomerangProjectile projectilePrefab;
    public Transform firePoint;      // nếu null sẽ bắn từ player
    public float speed = 10f;
    public float maxDistance = 5f;
    public int damage = 2;
    public float knockback = 2f;
    public float cooldown = 0.6f;
    public LayerMask enemyMask;      // nếu = 0 mình sẽ set "Enemy"
    public bool aimAtMouse = true;

    float lastUse = -999f;

    void Awake()
    {
        // nếu quên chọn mask thì tự bắt "Enemy" theo tên layer
        if (enemyMask.value == 0)
        {
            int enemyLayer = LayerMask.NameToLayer("Enemy");
            if (enemyLayer >= 0) enemyMask = 1 << enemyLayer;
        }
    }

    public void TryUse()
    {
        if (Time.time < lastUse + cooldown) return;
        lastUse = Time.time;

        if (projectilePrefab == null)
        {
            Debug.LogError("BoomerangSkill: projectilePrefab is NULL");
            return;
        }

        var spawn = firePoint ? firePoint.position : transform.position;

        Vector2 dir;

        if (aimAtMouse)
        {
            var cam = Camera.main;
            if (cam == null)
            {
                Debug.LogError("BoomerangSkill: Camera.main is NULL (thiếu tag MainCamera). Dùng firePoint.right thay thế.");
                dir = (firePoint ? (Vector2)firePoint.right : Vector2.right);
            }
            else
            {
                Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
                mouse.z = 0f;
                Vector3 from = firePoint ? firePoint.position : transform.position;
                dir = ((Vector2)(mouse - from));
            }
        }
        else
        {
            dir = (firePoint ? (Vector2)firePoint.right : Vector2.right);
        }

        if (dir.sqrMagnitude < 0.0001f) dir = Vector2.right; // phòng khi trùng vị trí
        dir.Normalize();

        var proj = Instantiate(projectilePrefab, spawn, Quaternion.identity);
        proj.Launch(this, (Vector2)transform.position, dir, speed, maxDistance, damage, knockback, enemyMask);
    }
}