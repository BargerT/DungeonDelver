using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private void Start()
    {
        // Although this object persists across scenes, we don't want mutliple copies of it,
        // so destroy this object if one already exists.
        if (GameObject.FindGameObjectsWithTag(this.tag).Length > 1)
        {
            Destroy(this.gameObject);
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
