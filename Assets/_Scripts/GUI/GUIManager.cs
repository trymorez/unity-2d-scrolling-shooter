using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField] GameObject getReady;
    static GUIManager instance;

    void Awake()
    {
        instance = this;
    }

    public static void ShowGetReady()
    {
        instance.getReady.SetActive(true);
    }
}
