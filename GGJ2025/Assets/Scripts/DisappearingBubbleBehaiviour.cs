using UnityEngine;

public class DisappearingBubbleBehaiviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = transform.localScale - new Vector3(1f,1f,1f) * Time.deltaTime;
        if(transform.localScale.x <=0)
        {
            Destroy(gameObject);
        }
    }
}
