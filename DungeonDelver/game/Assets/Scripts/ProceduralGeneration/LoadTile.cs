using System.Collections.Generic;
using UnityEngine;

public class LoadTile : MonoBehaviour
{
    private Renderer[] listOfChildren;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        listOfChildren = this.GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 80)
        {
            foreach(Renderer child in listOfChildren)
            {
                child.enabled = true;
            }
        }
        else
        {
            foreach (Renderer child in listOfChildren)
            {
                child.enabled = false;
            }
        }
    }
}
