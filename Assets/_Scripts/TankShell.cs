using UnityEngine;

public class TankShell : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float life = 3f;
    Vector2 direction;

    void Awake()
    {
        GameManager.OnPlayingGame += ProcessProjectile;
        Destroy(this.gameObject, life);
    }

    void OnDestroy()
    {
        GameManager.OnPlayingGame -= ProcessProjectile;
    }

    public void SetDirection(Vector2 dir)
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
        //var pos = transform.position;
        //var newX = pos.x + direction.x * speed * Time.deltaTime;
        //var newY = pos.y + direction.y * speed * Time.deltaTime;
        //transform.position = new Vector2 (newX, newY);

        //transform.Translate(direction * (speed * Time.deltaTime), Space.World);
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
    }
}
