using UnityEngine;

public class AbsorbedBubblerBehaiviour : MonoBehaviour
{
    Transform target;
    public float vel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        Vector2 dir = (target.position - transform.position).normalized;
        transform.Translate(dir * vel * Time.fixedDeltaTime);
    }
}
