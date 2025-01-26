using UnityEngine;

public class BlubberCatcher : MonoBehaviour
{
    public int foundBlubbersCount;
    public BLUBBER[] targetBlubbers;
    public bool[] foundBlubbers;
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foundBlubbers = new bool[targetBlubbers.Length];
        foundBlubbersCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        //GameManager.Instance.GameEvents.OnBubbleCollideWithPlayer(collision.GetComponent<BubblerObject>());

        /*
        if(collision)
        {
            
            //print(collision.GetComponentInParent<BubbleProperties>());
            if(collision.GetComponentInParent<BubbleProperties>() != null)
            {
                //print(collision.GetComponentInParent<BubbleProperties>().blubber);
                for (int i = 0; i < targetBlubbers.Length; i++)
                {
                    if(collision.GetComponentInParent<BubbleProperties>().blubber == targetBlubbers[i])
                    {
                        foundBlubbers[i] = true;
                        foundBlubbersCount++;
                        //print("PILLASTE UN BLUBBER DE LOS BUENOS. LLEVAS: " + foundBlubbersCount);
                        break;
                    }
                    else
                    {
                        if(collision.GetComponentInParent<BubbleProperties>().freezing)
                        {
                            print("FREEZING");
                            rb.linearVelocity = Vector2.zero;
                        }
                        for (int j = 0; j < foundBlubbers.Length; j++)
                        {
                            
                            if (foundBlubbers[i])
                            {
                                foundBlubbers[i] = false;
                                foundBlubbersCount--;
                                //print("LA CAGASTE. PERDISTE UN BLUBBER. TE QUEDAN: " + foundBlubbersCount);
                                break;
                            }
                        }

                    }
                }
                GameObject.Destroy(collision.transform.parent.gameObject);
            }
        }
        */
    }
}
