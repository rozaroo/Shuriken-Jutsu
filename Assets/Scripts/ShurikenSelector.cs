using UnityEngine;
using UnityEngine.UI;

public class ShurikenSelector : MonoBehaviour
{
    public MainMenuManager menu;
    private int ShurikenOneCount = 0;
    private int ShurikenTwoCount = 0;
    private int ShurikenThreeCount = 0;
    private void Start()
    {
        menu = FindObjectOfType<MainMenuManager>();
    }
    public void SelectShuriken(int index) 
    {
        if (ShurikenData.Instance != null) ShurikenData.Instance.selectedShurikenIndex = index;
        //Enviar evento a Analytics
        string userId = menu.userId;
        if (ShurikenData.Instance.selectedShurikenIndex == 0) 
        {
            ShurikenOneCount++;
            Debug.Log($"[ShurikenSelector] Usuario: {userId} seleccionó el shuriken #{index}, {ShurikenOneCount} veces");
            AnalyticsManager.Instance.ShurikenSelected(index, userId, ShurikenOneCount);
        }
        if (ShurikenData.Instance.selectedShurikenIndex == 1)
        {
            ShurikenTwoCount++;
            Debug.Log($"[ShurikenSelector] Usuario: {userId} seleccionó el shuriken #{index}, {ShurikenTwoCount} veces");
            AnalyticsManager.Instance.ShurikenSelected(index, userId, ShurikenTwoCount);
        }
        if (ShurikenData.Instance.selectedShurikenIndex == 2)
        {
            ShurikenThreeCount++;
            Debug.Log($"[ShurikenSelector] Usuario: {userId} seleccionó el shuriken #{index}, {ShurikenThreeCount} veces");
            AnalyticsManager.Instance.ShurikenSelected(index, userId, ShurikenThreeCount);
        }
    }
}

