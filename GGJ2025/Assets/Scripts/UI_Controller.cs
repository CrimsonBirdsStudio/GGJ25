using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class UI_Controller : MonoBehaviour
{
    public List<Sprite> bublersSpritesOff =new();
    public List<Sprite> bublersSpritesOn = new();
    public float timeBetweenBublers;
    public GameObject bublersContainer;
    public GameObject myFriendz;

    List<Sprite> bublersChosen = new();
    void Start()
    {

    }

    public void SetIcons(List<Sprite> offIcons, List<Sprite> onIcons)
    {
        for(int i = 0; i < 4; i++)
        {
           bublersContainer.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = offIcons[i];
           bublersContainer.transform.GetChild(i+4).gameObject.GetComponent<Image>().sprite = onIcons[i];
        }
        StartCoroutine(ShowBubbler());
    }

    IEnumerator ShowBubbler()
    {
        foreach (RectTransform bubbler in bublersContainer.transform)
        {
            // Ajusta la posición de las imágenes desplazándolas 150 unidades hacia abajo
            bubbler.anchoredPosition = new Vector2(
                bubbler.anchoredPosition.x,
                bubbler.anchoredPosition.y - 150
            );
        }
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
