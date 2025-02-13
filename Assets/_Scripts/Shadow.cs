using UnityEngine;
using UnityEngine.Timeline;

public class Shadow : MonoBehaviour
{
    [SerializeField] Transform body;
    [SerializeField] float offset;

    void Awake()
    {
        GameManager.OnPlayingGame += ChangeOffset;
    }

    void OnDestroy()
    {
        GameManager.OnPlayingGame -= ChangeOffset;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void ChangeOffset()
    {
        Vector2 bodyPos = body.transform.position;
        transform.position = bodyPos + new Vector2(offset, offset);
    }
}
