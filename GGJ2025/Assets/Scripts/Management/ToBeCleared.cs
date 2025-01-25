using UnityEngine;

public class ToBeCleared : MonoBehaviour
{
	[Tooltip("A partir de esta distancia, el objeto podrá ser eliminado.")]
	public float DistanceToDelete;
	[Tooltip("Tras pasar esta cantidad de tiempo a DistanceToDelete, el objeto será eliminado")]
	public float TimeAtDistanceToDelete;

	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnDestroy()
	{
		// Desuscribir de TheCleaner.
	}
}
