using UnityEngine;
[SelectionBase]
public class WaveMovement : MonoBehaviour
{

    Rigidbody2D rb;
    public Vector2 dir;
    public float vel;
    public float amp;
    public float freq;
    
    float crono;
    public Vector2 wave;
    public Transform player;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dir = (player.position - transform.position).normalized * vel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        

        crono += Time.fixedDeltaTime;
        wave = new Vector2(-dir.y, dir.x * Mathf.Cos(crono * freq) ) * amp;
        //dir.x = dir.x;
        //dir.y = Mathf.Cos(crono * freq);


        rb.linearVelocity = dir + wave;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("HEY");
    }



}
