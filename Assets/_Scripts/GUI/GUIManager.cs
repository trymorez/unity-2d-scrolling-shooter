using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField] GameObject getReady;
    [SerializeField] GameObject lifeIcon;
    [SerializeField] GameObject lifePanel;
    static GUIManager instance;

    void Awake()
    {
        instance = this;
    }

    public static void ShowGetReady()
    {
        instance.getReady.SetActive(true);
    }

    public static void ShowLifeIcon(int life)
    {

    }
}
