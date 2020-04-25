using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private Vector3 startingLoc;

    // Start is called before the first frame update
    void Start()
    {
        startingLoc = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(startingLoc, transform.position) > 60)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Caster"))
        {
            Destroy(gameObject);
        }
    }
}
