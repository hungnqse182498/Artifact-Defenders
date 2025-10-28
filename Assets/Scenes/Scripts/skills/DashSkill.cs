using System.Collections;
using UnityEngine;

public class DashSkill : MonoBehaviour
{
    [Header("Tuning")]
    public float dashSpeed = 18f;
    public float dashDur = 0.15f;
    public float cooldown = 1.0f;

    [Header("I-Frame (tùy chọn)")]
    public bool grantIFrame = true;
    public string invincibleLayer = "PlayerIFrame";

    Rigidbody2D rb;
    MonoBehaviour move; // PlayerMovement hoặc script di chuyển khác
    float lastUse = -999f;
    bool dashing;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // tìm component di chuyển để tắt trong lúc dash
        move = GetComponent<MonoBehaviour>(); // placeholder nếu bạn đổi tên
        // nếu có script tên PlayerMovement thì dùng đúng nó
        var pm = GetComponent("PlayerMovement") as MonoBehaviour;
        if (pm != null) move = pm;
    }

    public void TryUse()
    {
        if (dashing) return;
        if (Time.time < lastUse + cooldown) return;
        StartCoroutine(DashRoutine());
    }

    IEnumerator DashRoutine()
    {
        lastUse = Time.time;
        dashing = true;

        // xác định hướng dash
        Vector2 dir = GetDashDir();
        if (dir.sqrMagnitude < 0.0001f) dir = Vector2.right;

        int originalLayer = gameObject.layer;

        if (move != null) move.enabled = false;            // khóa script di chuyển
        if (grantIFrame) gameObject.layer = LayerMask.NameToLayer(invincibleLayer);

        float t = 0f;
        // tắt drag để khỏi mất lực
        float origDrag = rb.linearDamping;
        rb.linearDamping = 0f;

        while (t < dashDur)
        {
            t += Time.deltaTime;
            rb.linearVelocity = dir * dashSpeed;
            yield return null;
        }

        // reset
        rb.linearVelocity = Vector2.zero;
        rb.linearDamping = origDrag;
        if (grantIFrame) gameObject.layer = originalLayer;
        if (move != null) move.enabled = true;
        dashing = false;
    }

    Vector2 GetDashDir()
    {
        // 1) ưu tiên vận tốc hiện tại
        if (rb != null && rb.linearVelocity.sqrMagnitude > 0.01f)
            return rb.linearVelocity.normalized;

        // 2) nếu đứng yên: dash theo hướng chuột
        var cam = Camera.main;
        if (cam != null)
        {
            Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = 0f;
            return ((Vector2)(mouse - transform.position)).normalized;
        }

        // 3) cuối cùng: hướng phải
        return Vector2.right;
    }
}