using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component responsible of creating and managing the Player's melee attack
/// </summary>
public class PlayerSlash : MonoBehaviour
{
    public GameObject slashPrefab;
    public float cooldown;
    public Transform pivot;
    public int damage = 35;

    float timer;
    new Collider2D collider2D;
    public LayerMask enemyMask;
    AudioSource audioSource;

    PlayerMovement playerMovement;
    PlayerSpriteAnim playerAnim;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerAnim = GetComponentInParent<PlayerSpriteAnim>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && Time.time > timer)
        {
            Slash();
            audioSource.Play();
            timer = Time.time + cooldown;
        }
    }
    void Slash()
    {
        Instantiate(slashPrefab, transform.position, transform.rotation);
        if (playerAnim != null) playerAnim.PlayAttack(0.3f);
        if (playerMovement != null) playerMovement.StopMovementForAttack(0.3f);
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, new Vector2(2,1.5f), pivot.rotation.z, enemyMask);
        if(hits.Length != 0)
        {
            foreach (Collider2D hit in hits)
            {
                EnemyAI enemy = hit.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage); // hoặc damage nếu bạn muốn tính theo Player
                }
            }

        }
    }
}
