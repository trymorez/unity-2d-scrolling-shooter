using UnityEngine;
using UnityEngine.Pool;

public class BulletPoolManager : MonoBehaviour
{
    [SerializeField] Bullet bullet;
    public ObjectPool<Bullet> Pool;

    void Awake()
    {
        Bullet.OnRelease += Release;
    }

    void OnDestroy()
    {
        Bullet.OnRelease -= Release;
    }

    void Start()
    {
        Pool = new ObjectPool<Bullet>(() =>
        {
            return Instantiate(bullet);
        }, 
        bullet =>
        {
            bullet.gameObject.SetActive(true);
        }, 
        bullet =>
        {
            bullet.gameObject.SetActive(false);
        }, 
        bullet =>
        {
            Destroy(bullet.gameObject);
        }, false, 30, 90);
    }

    void Release(Bullet bullet)
    {
        Pool.Release(bullet);
    }

    void Update()
    {
        
    }
}
