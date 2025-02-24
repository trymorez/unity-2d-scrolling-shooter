using UnityEngine;
using UnityEngine.Pool;

public class MuzzleFlashPoolManager : MonoBehaviour
{
    [SerializeField] MuzzleFlash muzzleFlash;
    [SerializeField] int defaultSize = 30;
    [SerializeField] int maxSize = 90;
    public static ObjectPool<MuzzleFlash> Pool;

    void Start()
    {
        Pool = new ObjectPool<MuzzleFlash>(() =>
        {
            return Instantiate(muzzleFlash, transform);
        },
        muzzleFlash =>
        {
            muzzleFlash.gameObject.SetActive(true);
        },
        muzzleFlash =>
        {
            muzzleFlash.gameObject.SetActive(false);
            muzzleFlash.transform.SetParent(transform);
        },
        muzzleFlash =>
        {
            Destroy(muzzleFlash.gameObject);
        }, false, defaultSize, maxSize);
    }
}
