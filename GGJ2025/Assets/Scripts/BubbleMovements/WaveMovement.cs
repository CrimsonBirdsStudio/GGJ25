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
    //public Vector2 wave;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        crono += Time.fixedDeltaTime;
        //wave.x = Mathf.Sin(crono * freq);
        //dir.x = dir.x;
        dir.y = Mathf.Cos(crono * freq);


        rb.linearVelocity = dir * vel;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("HEY");
    }



}
