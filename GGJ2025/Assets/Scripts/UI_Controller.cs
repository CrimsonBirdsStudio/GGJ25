using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class UIAnimator : MonoBehaviour
{
    public float animationDuration = 0.25f; // Duración de la animación de cada elemento
    public float delayBetweenElements = 0.1f; // Retraso entre elementos
    CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(ShowBlubbers());
    }

    IEnumerator ShowBlubbers()
    {
        yield return new WaitForSeconds(1f);
        foreach (Transform blubber in transform)
        {
            Vector2 startPosition = blubber.position;
            blubber.transform.position = new Vector2(startPosition.x, startPosition.y - 100); // Offset 
            if(canvasGroup.alpha!=1)
            {
                canvasGroup.DOFade(1f, 2f).SetEase(Ease.InOutQuad).SetEase(Ease.OutBack);
                yield return new WaitForSeconds(2);
            }
            yield return new WaitForSeconds(delayBetweenElements);
            blubber.DOMoveY(startPosition.y, animationDuration).SetEase(Ease.InOutBounce);

        }
    }
}
