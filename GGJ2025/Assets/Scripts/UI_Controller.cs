using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class UIAnimator : MonoBehaviour
{
    public List<Sprite> bublersSpritesOff =new();
    public List<Sprite> bublersSpritesOn = new();
    public float timeBetweenBublers;
    public GameObject bublersContainer;
    public GameObject myFriendz;

    List<Sprite> bublersChosen = new();
    void Start()
    {
        ChoseBublers();
    }

    void ChoseBublers()
    {
        foreach (Transform bubbler in bublersContainer.transform)
        {
            Sprite sprite = bublersSpritesOn[Random.Range(0, bublersSpritesOn.Count)];

            // Obtener el RectTransform del elemento
            RectTransform rectTransform = bubbler.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // Modificar la posición local inicial
                rectTransform.localPosition = new Vector3(
                    rectTransform.localPosition.x,
                    rectTransform.localPosition.y - 150,
                    rectTransform.localPosition.z
                );
            }
            bublersChosen.Add(sprite);
        }
            StartCoroutine(ShowBubbler());
    }

    IEnumerator ShowBubbler()
    {
        yield return new WaitForSeconds(1);
        myFriendz.transform.DOScale(1,1).SetEase(Ease.OutElastic);
        yield return new WaitForSeconds(1);
        foreach (Transform bubbler in bublersContainer.transform)
        {
            bubbler.DOMoveY(bubbler.transform.position.y + 150, 0.5f).SetEase(Ease.OutBounce); // Animar con DOTween
            yield return new WaitForSeconds(timeBetweenBublers); // Esperar el tiempo correspondiente
        }
    }


}
