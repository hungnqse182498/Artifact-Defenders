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

    [Header("Mana Cost")] // Mana
    public int manaCost = 15;

    [Header("Auto Aim Settings")]
    public bool autoAim = true;          // có bật auto aim hay không
    public float autoAimRange = 8f;

    float lastUse = -999f;
    PlayerMana playerMana;
    PlayerMovement playerMovement;

    void Awake()
    {
        // nếu quên chọn mask thì tự bắt "Enemy" theo tên layer
        if (enemyMask.value == 0)
        {
            int enemyLayer = LayerMask.NameToLayer("Enemy");
            if (enemyLayer >= 0) enemyMask = 1 << enemyLayer;
        }
        playerMana = GetComponent<PlayerMana>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void TryUse()
    {
        if (Time.time < lastUse + cooldown) return;

        if (playerMana == null)
        {
            return;
        }
        if (!playerMana.TryUseMana(manaCost)) return;

        lastUse = Time.time;

        if (projectilePrefab == null)
        {
            Debug.LogError("BoomerangSkill: projectilePrefab is NULL");
            return;
        }

        var spawn = firePoint ? firePoint.position : transform.position;

        //boomerang theo hướng chuột
        //Vector2 dir;

        //if (aimAtMouse)
        //{
        //    var cam = Camera.main;
        //    if (cam == null)
        //    {
        //        Debug.LogError("BoomerangSkill: Camera.main is NULL (thiếu tag MainCamera). Dùng firePoint.right thay thế.");
        //        dir = (firePoint ? (Vector2)firePoint.right : Vector2.right);
        //    }
        //    else
        //    {
        //        Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
        //        mouse.z = 0f;
        //        Vector3 from = firePoint ? firePoint.position : transform.position;
        //        dir = ((Vector2)(mouse - from));
        //    }
        //}
        //else
        //{
        //    dir = (firePoint ? (Vector2)firePoint.right : Vector2.right);
        //}

        //if (dir.sqrMagnitude < 0.0001f) dir = Vector2.right; // phòng khi trùng vị trí
        //dir.Normalize();

        //var proj = Instantiate(projectilePrefab, spawn, Quaternion.identity);
        //proj.Launch(this, (Vector2)transform.position, dir, speed, maxDistance, damage, knockback, enemyMask);

        // --- XÁC ĐỊNH HƯỚNG ---
        //boomerang theo hướng di chuyển 
        //Vector2 dir = Vector2.right; // mặc định

        //if (playerMovement != null && playerMovement.MoveDirection.sqrMagnitude > 0.1f)
        //{
        //    dir = playerMovement.MoveDirection.normalized;
        //}
        //else if (aimAtMouse) // fallback PC
        //{
        //    var cam = Camera.main;
        //    if (cam != null)
        //    {
        //        Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
        //        mouse.z = 0f;
        //        Vector3 from = firePoint ? firePoint.position : transform.position;
        //        dir = ((Vector2)(mouse - from)).normalized;
        //    }
        //}
        //else
        //{
        //    dir = firePoint ? (Vector2)firePoint.right : Vector2.right;
        //}

        //// --- SPAWN PROJECTILE ---
        //var proj = Instantiate(projectilePrefab, spawn, Quaternion.identity);
        //proj.Launch(this, (Vector2)transform.position, dir, speed, maxDistance, damage, knockback, enemyMask);

        // --- XÁC ĐỊNH HƯỚNG ---
        // boomerang theo hướng có quái gần nhất trong phạm vi, nếu không có thì fallback như trên
        Vector2 dir = Vector2.right;

        // 🔹 AUTO AIM: tìm quái gần nhất trong phạm vi
        if (autoAim)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, autoAimRange, enemyMask);
            Transform closestEnemy = null;
            float closestDist = Mathf.Infinity;

            foreach (var e in enemies)
            {
                float dist = Vector2.Distance(transform.position, e.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestEnemy = e.transform;
                }
            }

            if (closestEnemy != null)
            {
                dir = (closestEnemy.position - transform.position).normalized;
            }
        }

        // 🔸 Nếu không có auto aim hoặc không tìm thấy địch → fallback
        if (!autoAim || dir == Vector2.right)
        {
            if (playerMovement != null && playerMovement.MoveDirection.sqrMagnitude > 0.1f)
            {
                dir = playerMovement.MoveDirection.normalized;
            }
            else if (aimAtMouse)
            {
                var cam = Camera.main;
                if (cam != null)
                {
                    Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
                    mouse.z = 0f;
                    Vector3 from = firePoint ? firePoint.position : transform.position;
                    dir = ((Vector2)(mouse - from)).normalized;
                }
            }
            else
            {
                dir = firePoint ? (Vector2)firePoint.right : Vector2.right;
            }
        }

        // --- SPAWN PROJECTILE ---
        var proj = Instantiate(projectilePrefab, spawn, Quaternion.identity);
        proj.Launch(this, (Vector2)transform.position, dir, speed, maxDistance, damage, knockback, enemyMask);
    }

    void OnDrawGizmosSelected()
    {
        if (autoAim)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, autoAimRange);
        }
    }
}