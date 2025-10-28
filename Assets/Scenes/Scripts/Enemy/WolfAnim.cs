using System.Collections;
using UnityEngine;

public class WolfAnim : MonoBehaviour
{
    [Header("Animation Sprites")]
    [SerializeField] private Sprite[] idleSprites;
    [SerializeField] private Sprite[] walkSprites;
    [SerializeField] private Sprite[] attackSprites;
    [SerializeField] private Sprite[] hurtSprites;
    [SerializeField] private Sprite[] deathSprites;

    [Header("Animation Settings")]
    [SerializeField] private float frameTime = 0.15f;

    private SpriteRenderer spriteRenderer;
    private EnemyAI enemyAI;
    private int frameIndex = 0;
    private float timer;
    private bool isDead = false;

    // New fields
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
        if (isDead) return; // stop updating after death animation finishes

        // Choose current animation
        Sprite[] currentAnim = GetCurrentAnim();

        // Guard: if no sprites provided, do nothing
        if (currentAnim == null || currentAnim.Length == 0) return;

        // Reset frame when animation changes
        if (currentAnim != lastAnim)
        {
            lastAnim = currentAnim;
            frameIndex = 0;
            timer = Time.time + frameTime;
            spriteRenderer.sprite = currentAnim[frameIndex];
        }

        // Run animation unless a PlayOnce coroutine is active
        if (!isPlayingOnce)
        {
            if (Time.time >= timer)
            {
                frameIndex = (frameIndex + 1) % currentAnim.Length;
                spriteRenderer.sprite = currentAnim[frameIndex];
                timer = Time.time + frameTime;
            }
        }

        // Flip sprite according to AI
        spriteRenderer.flipX = enemyAI.left;
    }

    private Sprite[] GetCurrentAnim()
    {
        // Death has highest priority; play once
        if (enemyAI.isDead)
        {
            if (playOnceCoroutine == null)
                playOnceCoroutine = StartCoroutine(PlayOnce(deathSprites, true));
            return deathSprites;
        }

        // Hurt plays once, then clears isHurt on EnemyAI
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

    private IEnumerator PlayOnce(Sprite[] anim, bool dieAfter)
    {
        if (anim == null || anim.Length == 0)
        {
            // Nothing to play; ensure flags are cleaned up
            if (dieAfter) isDead = true;
            else enemyAI.isHurt = false;
            playOnceCoroutine = null;
            yield break;
        }

        isPlayingOnce = true;

        // Play all frames once
        for (int i = 0; i < anim.Length; i++)
        {               
            spriteRenderer.sprite = anim[i];
            yield return new WaitForSeconds(frameTime);
        }

        // After playing
        isPlayingOnce = false;
        playOnceCoroutine = null;

        // If this was the death animation, stop future updates
        if (dieAfter)
        {
            isDead = true;
            // ensure final death frame stays (last frame already set)
        }
        else
        {
            // Clear hurt flag on AI so animation can return to normal state
            enemyAI.isHurt = false;
        }

        if (dieAfter)
        {
            isDead = true;
            spriteRenderer.sprite = anim[anim.Length - 1];
        }

        // Reset timers so next animation begins cleanly
        frameIndex = 0;
        timer = Time.time + frameTime;
    }
}
