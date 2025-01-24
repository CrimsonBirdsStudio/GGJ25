
using UnityEngine;


public class RandomDirectionalImpulses : MonoBehaviour
{

    public Transform target;
    Vector2 dir;
    float crono;
    public float interval;
    public float force;
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        crono = 0;
    }

    // Update is called once per frame
    void Update()
    {
        crono += Time.deltaTime;
        if(crono >= interval)
        {
            if(target)
            {
                //float halfDistance = Vector2.Distance(target.position, transform.position) / 2;
                //Vector2 randomTargetPos = new Vector2(target.position.x + Mathf.Sin(Random.Range(0, Mathf.PI * 2)), target.position.y + Mathf.Cos(Random.Range(0, Mathf.PI * 2)));
                dir = (target.position - transform.position).normalized;

            }

            crono = 0;
            MakeImpulse();
        }
    }

    private void MakeImpulse()
    {
        
        rb.AddForce(dir  * force , ForceMode2D.Impulse);
    }
}
