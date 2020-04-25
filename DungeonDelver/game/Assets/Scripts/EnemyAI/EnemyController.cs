using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator anim;
    public float HP;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(HP <= 0)
        {
            anim.SetBool("isDead", true); 
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
