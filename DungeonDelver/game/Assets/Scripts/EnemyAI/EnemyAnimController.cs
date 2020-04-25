using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimController : MonoBehaviour
{
    private GameObject main;
    private Transform player;
    private NavMeshAgent agent;
    public float HP;
    private float deathTime = 5f;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        main = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        player = main.transform;
        if (HP <= 0)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", false);
            anim.SetBool("isDead", true);
            deathTime -= Time.deltaTime;
            if(deathTime <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Vector3 direction = player.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.forward);
            if (Vector3.Distance(player.position, this.transform.position) < 10 && angle < 30)
            {
                agent.isStopped = true;
                direction.y = 0;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                if (direction.magnitude > 5)
                {
                    this.transform.Translate(0, 0, 0.05f);
                    anim.SetBool("isWalking", true);
                    anim.SetBool("isAttacking", false);
                }
                else
                {
                    anim.SetBool("isAttacking", true);
                    anim.SetBool("isWalking", false);
                }
            }
            else
            {
                agent.isStopped = false;
                anim.SetBool("isAttacking", false);
                anim.SetBool("isWalking", true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Normal Sword"))
        {
            HP -= 10;
        }
    }
}
