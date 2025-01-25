using UnityEngine;

public class BGParallax : MonoBehaviour
{
    public Vector2 cameraStartpos;
    public SpriteRenderer bg;
    public Vector2 offset;
    public float ParallaxMultiplier;
    SpriteRenderer bgSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraStartpos = Camera.main.transform.position;
        bg = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        transform.position = (Vector2)Camera.main.transform.position;
        offset = cameraStartpos - (Vector2)Camera.main.transform.position;
        
        bg.material.SetVector("_Offset", offset * ParallaxMultiplier);
        //bg.material.mainTextureOffset = mierda;
    }
}
