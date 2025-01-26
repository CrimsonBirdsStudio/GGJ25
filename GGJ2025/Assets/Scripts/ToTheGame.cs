using UnityEngine;
using UnityEngine.SceneManagement;

public class ToTheGame : MonoBehaviour
{


    [FMODUnity.BankRef]
    public string musicBank;
    [FMODUnity.BankRef]
    public string fxBank;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            print("MIERDA");
            if (FMODUnity.RuntimeManager.HasBankLoaded(musicBank))
            {
                print("DEL");
                if (FMODUnity.RuntimeManager.HasBankLoaded(fxBank))
                {
                    print("CULO");
                    SceneManager.LoadScene("MainSceneFMOD");
                }
            }

        }
    }
}
