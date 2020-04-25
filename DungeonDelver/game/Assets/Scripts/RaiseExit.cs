using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseExit : MonoBehaviour
{
    public Stalker boss;

    private int timer = 200;
    private Animation anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <= 0)
        {
            Destroy(gameObject);
        }
        if(boss.health <= 0)
        {
            timer--;
            anim.Play("Lower Fence");
        }
    }
}
