using System.Collections;
using UnityEngine;

public class BossAnim : MonoBehaviour
{
    [Header("Animation Sprites")]
    [SerializeField] private Sprite[] idleSprites;
    [SerializeField] private Sprite[] walkSprites;
    [SerializeField] private Sprite[] attackSprites;
    [SerializeField] private Sprite[] hurtSprites;
    [SerializeField] private Sprite[] deathSprites;

    [Header("Animation Settings")]
    [SerializeField] private float frameTime = 0.15f;
    [SerializeField] private float attackFrameTime = 0.08f; // tốc độ khung hình riêng cho animation tấn công

    private SpriteRenderer spriteRenderer;
    private EnemyAI enemyAI;
    private int frameIndex = 0;
    private float timer;
    private bool isDead = false;

    // Các trường phụ trợ
    private Sprite[] lastAnim = null;
    private bool isPlayingOnce = false;
    private Coroutine playOnceCoroutine = null;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAI = GetComponent<EnemyAI>();
    }

    void Update()
    {
        if (isDead) return;

        Sprite[] currentAnim = GetCurrentAnim();
        if (currentAnim == null || currentAnim.Length == 0) return;

        // Nếu animation thay đổi, reset lại
        if (currentAnim != lastAnim)
        {
            lastAnim = currentAnim;
            frameIndex = 0;
            timer = Time.time + GetFrameTime(currentAnim);
            spriteRenderer.sprite = currentAnim[frameIndex];
        }

        if (!isPlayingOnce)
        {
            if (Time.time >= timer)
            {
                frameIndex = (frameIndex + 1) % currentAnim.Length;
                spriteRenderer.sprite = currentAnim[frameIndex];
                timer = Time.time + GetFrameTime(currentAnim);
            }
        }

        spriteRenderer.flipX = enemyAI.left;
    }

    private Sprite[] GetCurrentAnim()
    {
        if (enemyAI.isDead)
        {
            if (playOnceCoroutine == null)
                playOnceCoroutine = StartCoroutine(PlayOnce(deathSprites, true));
            return deathSprites;
        }

        if (enemyAI.isHurt)
        {
            if (playOnceCoroutine == null)
                playOnceCoroutine = StartCoroutine(PlayOnce(hurtSprites, false));
            return hurtSprites;
        }

        if (enemyAI.isAttacking)
            return attackSprites;

        if (enemyAI.isMoving)
            return walkSprites;

        return idleSprites;
    }

    // Xác định tốc độ hiển thị frame theo loại animation
    private float GetFrameTime(Sprite[] anim)
    {
        if (anim == attackSprites) return attackFrameTime; // nhanh hơn khi tấn công
        return frameTime;
    }

    private IEnumerator PlayOnce(Sprite[] anim, bool dieAfter)
    {
        if (anim == null || anim.Length == 0)
        {
            if (dieAfter) isDead = true;
            else enemyAI.isHurt = false;
            playOnceCoroutine = null;
            yield break;
        }

        isPlayingOnce = true;

        float currentFrameTime = GetFrameTime(anim);

        for (int i = 0; i < anim.Length; i++)
        {
            spriteRenderer.sprite = anim[i];
            yield return new WaitForSeconds(currentFrameTime);
        }

        isPlayingOnce = false;
        playOnceCoroutine = null;

        if (dieAfter)
        {
            isDead = true;
            spriteRenderer.sprite = anim[anim.Length - 1];
        }
        else
        {
            enemyAI.isHurt = false;
        }

        frameIndex = 0;
        timer = Time.time + frameTime;
    }
}
