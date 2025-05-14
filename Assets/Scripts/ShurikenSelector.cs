using UnityEngine;
using UnityEngine.UI;

public class ShurikenSelector : MonoBehaviour
{
    public void SelectShuriken(int index) 
    {
        if (ShurikenData.Instance != null) ShurikenData.Instance.selectedShurikenIndex = index;
    }
}

