using UnityEngine;
using UnityEngine.SceneManagement;

public class ToTheGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (FMODUnity.RuntimeManager.HasBankLoaded("Music"))
            {
                if (FMODUnity.RuntimeManager.HasBankLoaded("FX"))
                {
                    SceneManager.LoadScene("Game");
                }
            }

        }
    }
}
