using TMPro;
using UnityEngine;
using DG.Tweening;

public class BlinkingText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] int blinkRepeat = 3;
    [SerializeField] float waitAfterFadeInTime = 1f;
    [SerializeField] float fadeInDuration = 1f;
    [SerializeField] float fadeOutDuration = 0.5f;

    void OnEnable()
    {
        StartTextBlinkEffect();
    }

    void StartTextBlinkEffect()
    {
        Sequence sequence = DOTween.Sequence();

        SetTextAlpha(0f);
        for (int i = 0; i < blinkRepeat; i++)
        {
            sequence.Append(textMesh.DOFade(1f, fadeInDuration))
                .AppendInterval(waitAfterFadeInTime)
                .Append(textMesh.DOFade(0f, fadeOutDuration));
        }
        sequence.OnComplete(() => {
            SetTextAlpha(1f);
            gameObject.SetActive(false); });
    }

    //WaitForSeconds waitAfterFadeIn;
    //void Start()
    //{
    //    waitAfterFadeIn = new WaitForSeconds(fadeInDuration);
    //}

    //void OnEnable()
    //{
    //    StartCoroutine(StartTextBlinkEffect());
    //}

    //IEnumerator StartTextBlinkEffect()
    //{
    //    for (int i = 0; i < blinkRepeat; i++)
    //    {
    //        yield return TextFade(0f, 1f, fadeInDuration);
    //        yield return waitAfterFadeIn;

    //        yield return TextFade(1f, 0f, fadeOutDuration);
    //    }
    //}

    //IEnumerator TextFade(float startAlpha, float endAlpha, float duration)
    //{
    //    float elapsed = 0;
    //    float newAlpha;

    //    while (elapsed < duration)
    //    {
    //        elapsed += Time.deltaTime;
    //        newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
    //        SetTextAlpha(newAlpha);
    //        yield return null;
    //    }
    //}

    void SetTextAlpha(float newAlpha)
    {
        Color oldColor = textMesh.color;
        oldColor.a = newAlpha;
        textMesh.color = oldColor;
    }
}
