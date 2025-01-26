using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Añadir este script a los objetos que deban eliminarse automáticamente una vez fuera de pantalla.
/// </summary>
public class TheCleanerTM : MonoBehaviour
{
	public int maxCleanChecksPerUpdate;

	private List<ToBeCleared> _toBeCleanedList = new List<ToBeCleared>();
	private bool _isCleaning;

	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (!_isCleaning)
		{
			StartCoroutine(Clean());
		}
	}

	IEnumerator Clean()
	{
		_isCleaning = true;
		int cleanChecks = 0;
		for (int i = 0; i < _toBeCleanedList.Count; i++)
		{
			if (cleanChecks >= maxCleanChecksPerUpdate)
			{
				cleanChecks = 0;
				yield return null;
				continue;
			}
			cleanChecks++;
			Vector2 cleanFromPos = GameManager.Instance.player_instance.transform.position;
			TryClearObject(_toBeCleanedList[i], cleanFromPos);
		}
		_isCleaning = false;
	}

	private void TryClearObject(ToBeCleared objectToClear, Vector2 cleanFromPos)
	{
		float gameTime = GameManager.Instance.GameState.LevelTimer;
		float distance = Vector2.Distance(cleanFromPos, objectToClear.transform.position);
		if (distance > objectToClear.DistanceToDelete)
		{
			if(objectToClear.DistanceToDeleteMax > objectToClear.DistanceToDelete &&
				distance > objectToClear.DistanceToDeleteMax)
			{
				RemoveToBeCleared(objectToClear);
				Destroy(objectToClear.gameObject);
			}else if (objectToClear.TimeAtWhenGotOut.HasValue)
			{
				if(gameTime - objectToClear.TimeAtWhenGotOut > objectToClear.TimeAtDistanceToDelete)
				{
					RemoveToBeCleared(objectToClear);
					Destroy(objectToClear.gameObject);
				}
			}
			else
			{
				objectToClear.TimeAtWhenGotOut = gameTime;
			}
		}
		else
		{
			objectToClear.TimeAtWhenGotOut = null;

		}
	}

	public void AddToBeCleared(ToBeCleared toBeCleared)
	{
		_toBeCleanedList.Add(toBeCleared);
	}
	public void RemoveToBeCleared(ToBeCleared toBeCleared)
	{
		_toBeCleanedList.Remove(toBeCleared);
	}




}
