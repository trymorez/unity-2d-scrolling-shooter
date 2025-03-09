using UnityEngine;
using UnityEngine.Timeline;

public class Shadow : MonoBehaviour
{
    [SerializeField] Transform body;
    [SerializeField] float offset;

    void Awake()
    {
        GameManager.OnPlaying += ChangeOffset;
    }

    void OnDestroy()
    {
        GameManager.OnPlaying -= ChangeOffset;
    }

    void ChangeOffset()
    {
        Vector2 bodyPos = body.transform.position;
        transform.position = bodyPos + new Vector2(offset, offset);
    }
}
