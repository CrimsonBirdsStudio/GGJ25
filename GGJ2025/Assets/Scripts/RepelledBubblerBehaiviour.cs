using UnityEngine;

public class RepelledBubblerBehaiviour : MonoBehaviour
{

    public float vely;
    public float grav;
    float crono;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        crono = 0;
    }

    // Update is called once per frame
    void Update()
    {
        crono += Time.deltaTime;
        if(crono > 5)
        {
            Destroy(gameObject);
            return;
        }
        vely -=  Time.deltaTime * grav;
        transform.Translate(Vector2.up * vely * Time.deltaTime);

    }
}
