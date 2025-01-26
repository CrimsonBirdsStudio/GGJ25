using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mantiene un registro de qué bubbles hay spawneadas en cada momento y proporciona métodos para consultarlo.
/// </summary>
public class BubblerAccounting : MonoBehaviour
{
    public Dictionary<BubblerEnums.SpawnType, int> _SpawnedBubblersBySpawnType = new Dictionary<BubblerEnums.SpawnType, int>();

	private void Start()
	{
		GameManager.Instance.GameEvents.OnBubbleSpawnedEvent += EventBubbleCreatedOrDestroyed;
		GameManager.Instance.GameEvents.OnBubbleDestroyedEvent += EventBubbleCreatedOrDestroyed;
	}

	public int GetSpawnedBlubbersOfType(BubblerEnums.SpawnType type)
    {
        return _SpawnedBubblersBySpawnType.TryGetValue(type, out var amount) ? amount : 0;
    }

	private void EventBubbleCreatedOrDestroyed(BubblerObject blubber)
	{
		BubblerConfig config = blubber.BubblerConfig;
		if (_SpawnedBubblersBySpawnType.TryGetValue(config.SpawnerType, out var amount))
		{
			_SpawnedBubblersBySpawnType[config.SpawnerType] = amount + (blubber.IsDestroyed ? -1 : 1);
		}
		else
		{
			_SpawnedBubblersBySpawnType.Add(config.SpawnerType, (blubber.IsDestroyed ? 0 : 1));
		}
	}
}
