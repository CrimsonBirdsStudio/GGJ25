using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class UI_Controller : MonoBehaviour
{
    public List<Image> bublersSpritesOff =new();
    public List<Image> bublersSpritesOn = new();
    public float timeBetweenBublers;
    public GameObject bublersContainer;
    public GameObject myFriendz;
    public GameObject tittleAnim;
    public GameObject initialBubblers;
    public GameObject playerBubble;
    public GameObject backGorundFader;
    public GameObject mouseIcon;
    public GameObject endGood;
    public GameObject endBad;

    List<Sprite> bublersChosen = new();
    void Start()
    {
        StartCoroutine(StartAnimation());
        foreach (RectTransform bubbler in bublersContainer.transform)
        {
            // Ajusta la posici�n de las im�genes desplaz�ndolas 150 unidades hacia abajo
            bubbler.anchoredPosition = new Vector2(
                bubbler.anchoredPosition.x,
                bubbler.anchoredPosition.y - 150
            );
        }
		GameManager.Instance.GameEvents.OnGameStateBubblersObtainedEvent += OnBubblerObtained;
		GameManager.Instance.GameEvents.OnGameStateBubblersLostEvent += OnBubblerLost;
        GameManager.Instance.GameEvents.OnGameOverEvent += OnEndGame;

    }
    private void OnEndGame(bool win)
    {
        Debug.Log("EndGame");
        StartCoroutine(EndAnimation(win));
    }
    public void SetIcons(List<Sprite> offIcons, List<Sprite> onIcons)
    {
        for(int i = 0; i < 4; i++)
        {
           bublersContainer.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = offIcons[i];

           bublersContainer.transform.GetChild(i+4).gameObject.GetComponent<Image>().sprite = onIcons[i];
        }
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

    IEnumerator StartAnimation()
    {
        Animator anim = tittleAnim.GetComponent<Animator>();
        anim.Play("tittle_idle");

        yield return new WaitForSeconds(5);

        anim.Play("tittle_death");
        Color initialColor = backGorundFader.GetComponent<SpriteRenderer>().color;
        float startAlpha = initialColor.a;
        float timeElapsed = 0;

        while (timeElapsed < .5f)
        {
            timeElapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0, timeElapsed / .5f);
            backGorundFader.GetComponent<SpriteRenderer>().color = new Color(initialColor.r, initialColor.g, initialColor.b, newAlpha);
            yield return null; // Espera hasta el siguiente frame
        }

        // Aseg�rate de que el alfa final sea exactamente el deseado
        backGorundFader.GetComponent<SpriteRenderer>().color = new Color(initialColor.r, initialColor.g, initialColor.b, 0);
        tittleAnim.transform.DOScale(0,.25f).OnComplete(() => {
            tittleAnim.transform.DOScale(1, .25f);
            anim.Play("where_message_idle");
        });
        yield return new WaitForSeconds(3);
        for(int i = 1; i < 5; i++)
        {
            if(i == 4)
            {
                initialBubblers.transform.GetChild(0).transform.DOScale(0, .05f).OnComplete(() => {
                    playerBubble.SetActive(true);
                    playerBubble.GetComponentInParent<Player_Movement>().enabled = true;
                }); ;
            }
            initialBubblers.transform.GetChild(i).transform.DOScale(0, .05f);
            yield return new WaitForSeconds(1f);
        }
        tittleAnim.transform.DOScale(0, .25f).OnComplete(() => {
            StartCoroutine(ShowBubbler());
            mouseIcon.SetActive(true);
        });
    }
    public IEnumerator EndAnimation(bool win)
    {
        playerBubble.SetActive(false);
        
        Color initialColor = backGorundFader.GetComponent<SpriteRenderer>().color;
        float startAlpha = initialColor.a;
        float timeElapsed = 0;

        while (timeElapsed < .5f)
        {
            timeElapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 1, timeElapsed / .5f);
            backGorundFader.GetComponent<SpriteRenderer>().color = new Color(initialColor.r, initialColor.g, initialColor.b, newAlpha);
            yield return null; // Espera hasta el siguiente frame
        }

        // Aseg�rate de que el alfa final sea exactamente el deseado
        backGorundFader.GetComponent<SpriteRenderer>().color = new Color(initialColor.r, initialColor.g, initialColor.b, 1);
        yield return new WaitForSeconds(2);
        SpriteRenderer endMsg = !win ? endBad.GetComponent<SpriteRenderer>() : endGood.GetComponent<SpriteRenderer>();
        initialColor = endMsg.color;
        startAlpha = initialColor.a;
        timeElapsed = 0;

        while (timeElapsed < .5f)
        {
            timeElapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 1, timeElapsed / .5f);
            endMsg.GetComponent<SpriteRenderer>().color = new Color(initialColor.r, initialColor.g, initialColor.b, newAlpha);
            yield return null; // Espera hasta el siguiente frame
        }

        // Aseg�rate de que el alfa final sea exactamente el deseado
        endMsg.GetComponent<SpriteRenderer>().color = new Color(initialColor.r, initialColor.g, initialColor.b, 1);
       
        yield return new WaitForSeconds(4);
        initialColor = endMsg.color;
        startAlpha = initialColor.a;
        timeElapsed = 0;

        while (timeElapsed < .5f)
        {
            timeElapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0, timeElapsed / .5f);
            endMsg.GetComponent<SpriteRenderer>().color = new Color(initialColor.r, initialColor.g, initialColor.b, newAlpha);
            yield return null; // Espera hasta el siguiente frame
        }

        // Aseg�rate de que el alfa final sea exactamente el deseado
        endMsg.GetComponent<SpriteRenderer>().color = new Color(initialColor.r, initialColor.g, initialColor.b, 0);
        yield return new WaitForSeconds(4);

        SceneManager.LoadScene(0);
    }
	void OnBubblerObtained(BubblerObject bubbler)
	{
		print($"Bubbler Obtained! {bubbler.name}");
        foreach (Image bublerSelected in bublersSpritesOn)
        {
            if (bublerSelected.sprite.name == bubbler.BubblerScriptableSprite.bublerSpriteUIOn.name)
            {
                bublerSelected.gameObject.transform.DOScale(0, .25f);
                break;
            }
        }
    }
	void OnBubblerLost(Bubler_Scriptable bubbler)
	{
		print($"Bubbler lost! {bubbler.name}");
        foreach(Image bublerSelected in bublersSpritesOn)
        {
            if(bublerSelected.sprite == bubbler.bublerSpriteUIOn)
            {
                Debug.Log(bublerSelected.name);
                bublerSelected.gameObject.transform.DOScale(1, .25f);
                break;
            }
        }
    }
private void OnDestroy()
	{
		GameManager.Instance.GameEvents.OnGameStateBubblersObtainedEvent += OnBubblerObtained;
		GameManager.Instance.GameEvents.OnGameStateBubblersLostEvent += OnBubblerLost;
	}
}
