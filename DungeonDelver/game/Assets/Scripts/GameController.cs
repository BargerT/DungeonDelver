using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GrabCursor();
        Screen.fullScreen = Screen.fullScreen;
        if (SceneManager.GetActiveScene().name.CompareTo("TileBuilder") == 0)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(-15, 20, 33);
            GameObject.FindGameObjectWithTag("Player").transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
        else if (SceneManager.GetActiveScene().name.CompareTo("Boss") == 0)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(52.05f, 0.1f, 13);
            GameObject.FindGameObjectWithTag("Player").transform.rotation = new Quaternion(0f, 270f, 0f, 0f);
        }
    }

    public static void GrabCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void UngrabCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
