using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watcher : MonoBehaviour
{
    public GameObject player;
    public float moveSpeed = 4f;
    public int aggroRange = 7;
    public int attackRange = 3;
    private float deathTimer = 5f;
    public int health;

    private Vector3 spawn;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        spawn = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) // enemy is dead
        {
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0)
            {
                Destroy(gameObject);
            }
            anim.SetBool("isDead", true);
            anim.SetBool("isAttacking", false);
            anim.SetBool("isWalking", false);
        }
        else
        {
            if (transform.position.y > 1)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }

            if (transform.position.y < -2)
            {
                transform.position = spawn;
                health -= 2;
            }

            transform.LookAt(player.transform);
            float distanceTo = Vector3.Distance(transform.position, player.transform.position);
            if (distanceTo >= attackRange && distanceTo <= aggroRange)
            {
                // Stalk the player
                anim.SetBool("isIdle", false);
                anim.SetBool("isAttacking", false);
                anim.SetBool("isWalking", true);
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
            else if (distanceTo <= attackRange)
            {
                // Attack the player
                anim.SetBool("isIdle", false);
                anim.SetBool("isWalking", false);
                anim.SetBool("isAttacking", true);
            }
            else
            {
                // idle
                anim.SetBool("isWalking", false);
                anim.SetBool("isAttacking", false);
                anim.SetBool("isIdle", true);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Normal Sword"))
        {
            health -= 5;
        }

        if (other.CompareTag("Legendary Sword"))
        {
            health -= 10;
        }
    }
}
