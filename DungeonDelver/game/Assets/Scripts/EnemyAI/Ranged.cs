using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : MonoBehaviour
{
    public float distance = 50.0f;
    public int health = 5;
    private float deathTimer = 5f;
    public float moveSpeed = 3f;
    public GameObject fireball;
    private float attackTimer;

    private Transform playerLoc;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        playerLoc = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();
        attackTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer -= Time.deltaTime;
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
            anim.SetBool("isIdle", false);
        }
        else
        {
            float distanceTo = Vector3.Distance(transform.position, playerLoc.position);
            if (distanceTo < 3)
            {
                // Run from the player
                anim.SetBool("isAttacking", false);
                anim.SetBool("isIdle", false);
                anim.SetBool("isWalking", true);
                transform.LookAt(playerLoc);
                transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y + 180, transform.rotation.z));
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
            else if (distanceTo >= 3 && distanceTo < 10)
            {
                transform.LookAt(playerLoc);
                // Throw a fireball at the player 
                transform.LookAt(playerLoc);
                anim.SetBool("isIdle", false);
                anim.SetBool("isWalking", false);
                anim.SetBool("isAttacking", true);
                if (attackTimer <= 0)
                {
                    GameObject projectile = Instantiate(fireball, transform);
                    Rigidbody rb = projectile.GetComponent<Rigidbody>();
                    rb.velocity = transform.forward * 10;
                    attackTimer = 1.5f;
                }
            }
            else if (distanceTo >= 10 && distanceTo < 20)
            {
                transform.LookAt(playerLoc);
                // Approach the player
                anim.SetBool("isIdle", false);
                anim.SetBool("isAttacking", false);
                anim.SetBool("isWalking", true);
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
            else
            {
                anim.SetBool("isIdle", true);
                anim.SetBool("isAttacking", false);
                anim.SetBool("isWalking", false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
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
