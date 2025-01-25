using UnityEngine;

public class ToBeCleared : MonoBehaviour
{
	[Tooltip("A partir de esta distancia, el objeto podrá ser eliminado.")]
	public float DistanceToDelete;
	[Tooltip("Tras pasar esta cantidad de tiempo a DistanceToDelete, el objeto será eliminado")]
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
