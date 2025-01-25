using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class UIAnimator : MonoBehaviour
{
    public float animationDuration = 0.25f; // Duraci�n de la animaci�n de cada elemento
    public float delayBetweenElements = 0.1f; // Retraso entre elementos
    CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        ShowBlubbers();
    }

    void ShowBlubbers()
    {
        float animationDuration = 0.5f; // Duraci�n de la animaci�n de cada objeto
        float delayBetweenObjects = 0.2f; // Retraso entre cada objeto

        // Fade in del CanvasGroup
        canvasGroup.DOFade(1f, 1f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                // Animar los objetos del contenedor uno a uno
                int index = 0; // �ndice para calcular el retraso por objeto
                foreach (Transform blubber in transform)
                {
                    Vector2 startPosition = blubber.position;
                    blubber.position = new Vector2(startPosition.x, startPosition.y - 100); // Posici�n inicial desplazada

                    // Animar posici�n con retraso individual
                    blubber.DOMoveY(startPosition.y, animationDuration)
                        .SetEase(Ease.OutBack)
                        .SetDelay(index * delayBetweenObjects);

                    index++; // Incrementar el �ndice para el pr�ximo retraso
                }
            });
    }

}
