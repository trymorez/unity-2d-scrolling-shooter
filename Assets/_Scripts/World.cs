using UnityEngine;
using UnityEngine.UIElements;

public class World : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 1f;

    void Awake()
    {
        GameManager.OnUpdatingGame += ScrollMap;
    }

    void OnDestroy()
    {
        GameManager.OnUpdatingGame -= ScrollMap;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void ScrollMap()
    {
        var pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y -= scrollSpeed * Time.deltaTime, 0);
    }
}
