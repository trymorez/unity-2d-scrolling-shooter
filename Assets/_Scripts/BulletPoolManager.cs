using UnityEngine;
using UnityEngine.Pool;

public class BulletPoolManager : MonoBehaviour
{
    [SerializeField] Bullet bullet;
    [SerializeField] int defaultSize = 30;
    [SerializeField] int maxSize = 90;
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
            return Instantiate(bullet, transform);
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
        }, false, defaultSize, maxSize);
    }

    void Release(Bullet bullet)
    {
        Pool.Release(bullet);
    }
}
