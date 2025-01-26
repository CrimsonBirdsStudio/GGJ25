using DG.Tweening;
using UnityEngine;
using static BubblerEnums;

public class BubblerRepeler : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter theSound;
    GameObject bubblerGo;
    public GameObject repelledPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.GameEvents.OnBubbleTriggeredWithPlayerEvent += StartRepelation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartRepelation(BubblerObject bubbler)
    {
        theSound.Play();
        bubblerGo = bubbler.gameObject;
        if (bubbler.BubblerConfig.SpawnerType != SpawnType.GoodBubbler)
        {
            //Vector2 pos = bubbler.transform.position;
            //GameObject go = GameObject.Instantiate(repelledPrefab);
            //go.transform.position = pos;
  
            Destroy(bubbler.gameObject);
            /*
            GameObject bu = GameObject.Instantiate(DecreasingBubble);
            bu.transform.position = pos;
            Destroy(bubbler.gameObject);
            Transform myBubble = transform.Find("Bubble");
            Vector3 newscale = myBubble.transform.localScale * 1.25f;
            growBubbleTween = myBubble.transform.DOScale(newscale, 2f);
            growBubbleTween.onComplete += CompleteGrowAnim;
            */

        }
    }
}
