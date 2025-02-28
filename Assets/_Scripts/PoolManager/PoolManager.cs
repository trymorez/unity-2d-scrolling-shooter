using UnityEngine;
using UnityEngine.Pool;

public class PoolManager<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] T prefab;
    [SerializeField] int defaultSize = 30;
    [SerializeField] int maxSize = 90;
    public static ObjectPool<T> Pool;

    void Awake()
    {
        Pool = new ObjectPool<T>(
            () => Instantiate(prefab, transform),
            obj =>
            {
                obj.gameObject.SetActive(true);
            },
            obj =>
            {
                obj.gameObject.SetActive(false);
            },
            obj =>
            {
                Destroy(obj.gameObject);
            },
            true,
            defaultSize,
            maxSize
        );
    }

    public static T Get() => Pool.Get();
    public static void Release(T obj) => Pool.Release(obj);
}
