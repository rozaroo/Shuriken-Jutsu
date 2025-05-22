using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    private ScreenOrientation previousOrientation;
    void Start()
    {
        previousOrientation = Screen.orientation;
        ShowDebugScreenInfo();
    }

    // Update is called once per frame
    void Update()
    {
        if (previousOrientation != Screen.orientation)
        {
            previousOrientation = Screen.orientation;
            ShowDebugScreenInfo();
        }
    }
    private void ShowDebugScreenInfo()
    {
        Debug.Log("Screen Width: " + Screen.width);
        Debug.Log("Screen Height: " + Screen.height);
        Debug.Log("Screen Aspect Ratio: " + ((float)Screen.width / (float)Screen.height));
    }
}
