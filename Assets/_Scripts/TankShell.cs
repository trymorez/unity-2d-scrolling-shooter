using UnityEngine;

public class TankShell : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float life = 3f;
    Vector3 direction = Vector3.up;

    void Awake()
    {
        GameManager.OnPlayingGame += ProcessProjectile;
        Destroy(gameObject, life);
    }

    void OnDestroy()
    {
        GameManager.OnPlayingGame -= ProcessProjectile;
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }    

    void Start()
    {
    }

    void Update()
    {
    }

    void ProcessProjectile()
    {
        transform.Translate(direction.normalized * (speed * Time.deltaTime));
    }
}
