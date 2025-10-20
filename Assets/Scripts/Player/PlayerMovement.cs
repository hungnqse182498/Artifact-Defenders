using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component responsible of the player movement and managing the <see cref="SpriteRenderer.flipX"/> property
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;

    new Rigidbody2D rigidbody;
    Vector2 normVector;
    SpriteRenderer sprite;

    float timer;
    bool harvesting;

    bool attacking;
    float attackTimer;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (harvesting && Time.time > timer)
        harvesting = false;

    if (attacking && Time.time > attackTimer)
        attacking = false;
        FlipSprite();
    }

    private void FlipSprite()
    {
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            sprite.flipX = false;
        }
        else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            sprite.flipX = true;
        }
    }

    void FixedUpdate()
    {
        if (harvesting || attacking)
        {
            rigidbody.linearVelocity = Vector2.zero;
        }
        else
        {
            normVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if(normVector.sqrMagnitude > 1)
            {
                normVector = normVector.normalized;
            }
            rigidbody.linearVelocity = new Vector2(normVector.x * movementSpeed,normVector.y * movementSpeed);
        }
    }

    public void HarvestStopMovement(float time)
    {
        harvesting = true;
        timer = Time.time + time;
    }

    public bool IsHarvesting()
    {
        return harvesting;
    }

    public bool IsAttacking()
    {
        return attacking;
    }

    public Vector2 GetVelocity()
    {
        return rigidbody.linearVelocity;
    }

    public void StopMovementForAttack(float time)
    {
        attacking = true;
        attackTimer = Time.time + time;
    }

}
