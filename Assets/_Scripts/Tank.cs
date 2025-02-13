using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform turret;

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 dir = target.position - turret.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
        turret.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
    }
}
