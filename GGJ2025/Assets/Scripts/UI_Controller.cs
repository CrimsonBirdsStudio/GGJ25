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
        ShowBlubbers();
    }

    void ShowBlubbers()
    {
        float animationDuration = 0.5f; // Duración de la animación de cada objeto
        float delayBetweenObjects = 0.2f; // Retraso entre cada objeto

        // Fade in del CanvasGroup
        canvasGroup.DOFade(1f, 1f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                // Animar los objetos del contenedor uno a uno
                int index = 0; // Índice para calcular el retraso por objeto
                foreach (Transform blubber in transform)
                {
                    Vector2 startPosition = blubber.position;
                    blubber.position = new Vector2(startPosition.x, startPosition.y - 100); // Posición inicial desplazada

                    // Animar posición con retraso individual
                    blubber.DOMoveY(startPosition.y, animationDuration)
                        .SetEase(Ease.OutBack)
                        .SetDelay(index * delayBetweenObjects);

                    index++; // Incrementar el índice para el próximo retraso
                }
            });
    }

}
