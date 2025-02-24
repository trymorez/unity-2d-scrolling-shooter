using UnityEngine;
using UnityEngine.Pool;

public class ShellPoolManager : MonoBehaviour
{
    [SerializeField] TankShell tankShell;
    [SerializeField] int defaultSize = 30;
    [SerializeField] int maxSize = 90;
    public static ObjectPool<TankShell> Pool;

    void Start()
    {
        Pool = new ObjectPool<TankShell>(() =>
        {
            return Instantiate(tankShell, transform);
        },
        tankShell =>
        {
            tankShell.gameObject.SetActive(true);
        },
        tankShell =>
        {
            tankShell.gameObject.SetActive(false);
        },
        tankShell =>
        {
            Destroy(tankShell.gameObject);
        }, false, defaultSize, maxSize);
    }
}
