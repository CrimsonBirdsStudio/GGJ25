
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;


public class BackgroundGenerator : MonoBehaviour
{

    public List<GameObject> prefabs;
    public float worldCameraWidth;
    public float worldCameraHeight;
    //public string[] rects;
    public bool[][] rects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        worldCameraHeight = Camera.main.orthographicSize * 2;
        worldCameraWidth = worldCameraHeight * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FillRectangle(Vector2 orgigin)
    {
        

        float numX = 10f;
        float numY = 5f;
        float spacingX = worldCameraWidth / numX;
        float spacingY = worldCameraHeight / numY;



        for (int i = 0; i < numX; i++)
        {
            for (int j = 0; j < numY; j++)
            {
                GameObject ngo = GameObject.Instantiate(prefabs[Random.Range(0, prefabs.Count)]);

                float randomX = Random.Range(-spacingX / 2, spacingX / 2);
                float randomY = Random.Range(-spacingY / 2, spacingY / 2);

                //float x = PrefixOrigin.;

            }
        }

    }
}
