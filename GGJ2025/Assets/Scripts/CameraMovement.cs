using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float smooth;
    public Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector2 newpos = Vector2.Lerp(transform.position, target.position, smooth);
        transform.position = new Vector3(newpos.x, newpos.y, transform.position.z);
    }
}
