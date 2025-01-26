using UnityEngine;

public class FMODEMITTERMIERDAS : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter mant;
    public FMODUnity.StudioEventEmitter solt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.player_instance.GetComponent<Player_Movement>().enabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mant.Play();
            }
            if (Input.GetMouseButtonUp(0))
            {
                solt.Play();
            }
        }
        
        
    }




}
