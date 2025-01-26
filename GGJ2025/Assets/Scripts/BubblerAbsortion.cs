using UnityEditorInternal;
using UnityEngine;
using static BubblerEnums;
using DG.Tweening;

public class BubblerAbsortion : MonoBehaviour
{
    Tweener growBubbleTween;
    //Transform freeBubbler;
    public GameObject AbsorbedPrefab;
    public GameObject DecreasingBubble;
    GameObject bubblerGo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.GameEvents.OnBubbleTriggeredWithPlayerEvent += StartAbsortion;
    }
    void StartAbsortion(BubblerObject bubbler)
    {
        bubblerGo = bubbler.gameObject; 
        if (bubbler.BubblerConfig.SpawnerType == SpawnType.GoodBubbler)
        {
            Vector2 pos = bubbler.transform.position;
            GameObject go = GameObject.Instantiate(AbsorbedPrefab);
            go.transform.position = pos;
            go.GetComponent<AbsorbedBubblerBehaiviour>().SetTarget(transform);
            GameObject bu = GameObject.Instantiate(DecreasingBubble);
            bu.transform.position = pos;
            Destroy(bubbler.gameObject);
            Transform myBubble = transform.Find("Bubble");
            Vector3 newscale = myBubble.transform.localScale * 1.25f;
            growBubbleTween = myBubble.transform.DOScale(newscale, 2f);
            growBubbleTween.onComplete += CompleteGrowAnim;


        }
    }

    void CompleteGrowAnim()
    {
        GetComponentInChildren<CircleCollider2D>().radius *= 1.25f;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
