using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField] GameObject getReady;
    [SerializeField] GameObject lifeIconPrefab;
    [SerializeField] Transform lifePanel;
    [SerializeField] List<GameObject> lifeIcons = new();
    static GUIManager instance;

    void Awake()
    {
        instance = this;
    }

    public static void ShowGetReady()
    {
        instance.getReady.SetActive(true);
    }

    public static void ChangeLifeIcon(ref int life, int change)
    {
        foreach(var lifeIcon in instance.lifeIcons)
        {
            Destroy(lifeIcon);
        }
        instance.lifeIcons.Clear();

        life += change;

        for (int i = 0; i < life; i++)
        {
            instance.lifeIcons.Add( Instantiate(instance.lifeIconPrefab, instance.lifePanel) );
        }
    }
}
