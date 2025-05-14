using UnityEngine;

public class ShurikenData : MonoBehaviour
{
    public static ShurikenData Instance;
    public int selectedShurikenIndex = 0;

    void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}

