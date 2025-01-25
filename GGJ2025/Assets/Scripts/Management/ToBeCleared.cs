using UnityEngine;

public class ToBeCleared : MonoBehaviour
{
	[Tooltip("A partir de esta distancia, el objeto podr� ser eliminado.")]
	public float DistanceToDelete;
	[Tooltip("Tras pasar esta cantidad de tiempo a DistanceToDelete, el objeto ser� eliminado")]
	public float TimeAtDistanceToDelete;

	[HideInInspector]
	public float? TimeAtWhenGotOut;

	void Start()
    {
		GameManager.Instance.ObjectCleaner.AddToBeCleared(this);
	}

	void OnDestroy()
	{
		GameManager.Instance.ObjectCleaner.RemoveToBeCleared(this);
	}
}
