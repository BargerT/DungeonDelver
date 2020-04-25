using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IndicatorText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private float textTimer;  // Seconds.  Goes negative during fading.
    public float textFadeTime = 1;

    public void Tricked(uint count)
    {
        switch (count)
        {
            case 1:
                ShowText(5, "This looks familiar...");
                break;
            case 2:
                ShowText(10, "I think this is the same floor again... I should try looking for a less obvious exit");
                break;
            case 3:
                ShowText(5, "The room before the stairs up looked suspicious...");
                break;
            case 4:
                ShowText(10, "It must be that doorway in the floor that is in the room before the final stairs!");
                break;
            default:
                ShowText(15, "You're a funny one. If you want to progress jump through the doorway in the floor surrounded by the four skeletons");
                break;
        }
    }

    public void LevelCleared(uint count)
    {
        ShowText(5, Color.gray, "Levels cleared: " + count);
    }

    /// <summary>
    /// Renders text for the given duration, excluding fade time.
    /// </summary>
    public void ShowText(float time, string text)
    {
        ShowText(time, Color.white, text);
    }

    /// <summary>
    /// Renders text in the given color for the given duration, excluding fade time.
    /// </summary>
    public void ShowText(float time, Color color, string text)
    {
        textMesh.text = text;
        textMesh.color = color;
        textTimer = time;
    }

    void TestText()
    {
        ShowText(4, Color.magenta, "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.");
    }

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        //TestText();
    }

    // Update is called once per frame
    void Update()
    {
        if (textTimer > -textFadeTime)
        {
            textTimer -= Time.deltaTime;
            if (textTimer <= -textFadeTime)
            {
                textMesh.text = "";
            }
            else if (textTimer <= 0)
            {
                float timeSpentFading = -textTimer;
                float fadeProgress = timeSpentFading / textFadeTime;
                Color color = textMesh.color;
                color.a = 1 - fadeProgress;
                textMesh.color = color;
            }
        }
    }
}
