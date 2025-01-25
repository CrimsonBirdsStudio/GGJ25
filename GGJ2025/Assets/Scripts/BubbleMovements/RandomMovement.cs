using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float crono;
    public float start_interval;
    public float interval;
    public float randomInterval;
    public float vel;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        interval = start_interval;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        crono += Time.fixedDeltaTime;
        if(crono > interval)
        {
            rb.linearVelocity = new Vector2( Random.Range(-1,1) , Random.Range(-1,1) ).normalized * vel;
            interval = start_interval + Random.Range(-1,1) * randomInterval ;
            crono = 0;
        }
    }
}
