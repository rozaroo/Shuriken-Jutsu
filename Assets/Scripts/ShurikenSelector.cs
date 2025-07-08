using UnityEngine;
using UnityEngine.UI;

public class ShurikenSelector : MonoBehaviour
{
    public void SelectShuriken(int index) 
    {
        if (ShurikenData.Instance != null) ShurikenData.Instance.selectedShurikenIndex = index;
        //Enviar evento a Analytics
        string userId = MainMenuManager.Instance.userId;
        Debug.Log($"[ShurikenSelector] Usuario: {userId} seleccionó el shuriken #{index}");
        AnalyticsManager.Instance.ShurikenSelected(index, userId);

    }
}

