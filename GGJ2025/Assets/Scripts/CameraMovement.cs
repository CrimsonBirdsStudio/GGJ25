using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float smooth;
    public Transform target;
    public float distanceAdvance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 targetDir = target.GetComponent<Rigidbody2D>().linearVelocity.normalized;
        Vector2 tragetPos = (Vector2)target.position + (targetDir * distanceAdvance);
        Vector2 newpos = Vector2.Lerp(transform.position, tragetPos, smooth);
        transform.position = new Vector3(newpos.x, newpos.y, transform.position.z);
    }
}
