using UnityEngine;
using DG.Tweening;

public class Shield : MonoBehaviour
{
    [SerializeField] Transform shield;
    [SerializeField] float scale = 0.24f;
    [SerializeField] float duration = 0.5f;

    void Start()
    {
        ModulateShield();
    }

    void ModulateShield()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(shield.DOScaleX(-scale, duration));
        sequence.Append(shield.DOScaleX(scale, duration));
        sequence.SetLoops(-1);
    }
}
