using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponsiveLayoutManager : MonoBehaviour
{
    [SerializeField] private GameObject smallScreenLayout;
    [SerializeField] private GameObject mediumScreenLayout;
    [SerializeField] private GameObject largeScreenLayout;
    void Start()
    {
        UpdateLayout();
    }

    // Update is called once per frame
    private void UpdateLayout()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        smallScreenLayout.SetActive(screenWidth == 1080 && screenHeight == 1920);
        mediumScreenLayout.SetActive(screenWidth >= 1280 && screenHeight > 1920);
        largeScreenLayout.SetActive(screenWidth == 1200 && screenHeight == 1920);
        //smallScreenLayout.SetActive(screenWidth < 1280);
        //mediumScreenLayout.SetActive(screenWidth >= 1280 && screenWidth < 1920);
        //largeScreenLayout.SetActive(screenWidth >= 1920);
    }
}
