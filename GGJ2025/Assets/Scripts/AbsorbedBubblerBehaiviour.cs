using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class AbsorbedBubblerBehaiviour : MonoBehaviour
{
    public Transform target;
    public float vel;
    public Vector2 offset;
    public Bubler_Scriptable bublerScript;
    public bool stop;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stop = false;
        
        GameManager.Instance.GameEvents.OnGameStateBubblersLostEvent += OnBubblerLost;
    }


    void OnBubblerLost(Bubler_Scriptable bscript)
    {
        if(gameObject)
        {
            if (bscript == bublerScript)
            {
                Destroy(gameObject);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position , target.position)<0.5f)
        {
            vel = 0;
            transform.SetParent(target);
        }
    }

    public void SetTarget(Transform _target , Vector2 _offset)
    {
        offset = _offset;
        target = _target;
        vel = _target.GetComponent<Rigidbody2D>().linearVelocity.magnitude;
    }

    private void FixedUpdate()
    {
        if(stop) return;
        Vector2 targetpos = (Vector2)target.position + offset;

        Vector2 dir = (targetpos - (Vector2)transform.position).normalized;
        vel = target.GetComponent<Rigidbody2D>().linearVelocity.magnitude;

        if(Vector2.Distance (transform.position , targetpos) <= 0.15f )
        {
            transform.SetParent(target);
            stop = true;
        }

        transform.Translate(dir * vel * Time.fixedDeltaTime);
    }
}
