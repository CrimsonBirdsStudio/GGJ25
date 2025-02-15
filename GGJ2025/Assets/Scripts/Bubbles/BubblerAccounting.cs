using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mantiene un registro de qu� bubbles hay spawneadas en cada momento y proporciona m�todos para consultarlo.
/// </summary>
public class BubblerAccounting : MonoBehaviour
{
    public Dictionary<BubblerEnums.SpawnType, int> _SpawnedBubblersBySpawnType = new Dictionary<BubblerEnums.SpawnType, int>();
    public Dictionary<BubblerEnums.SpawnType, float> _NextSpawnTimeBySpawnType = new Dictionary<BubblerEnums.SpawnType, float>();

	private void Start()
	{
		GameManager.Instance.GameEvents.OnBubbleSpawnedEvent += EventBubbleCreatedOrDestroyed;
		GameManager.Instance.GameEvents.OnBubbleDestroyedEvent += EventBubbleCreatedOrDestroyed;
		GameManager.Instance.GameEvents.OnBubbleTriggeredWithPlayerEvent += EventBubbleTouchedPlayer;
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

		if (!blubber.IsDestroyed && blubber.BubblerConfig.SpawnerType != BubblerEnums.SpawnType.GoodBubbler)
		{
			SetNextSpawnTimer(blubber.BubblerConfig);
		}
	}

	public float GetNextSpawnTimeForBlubbersOfType(BubblerEnums.SpawnType type)
	{
		return _NextSpawnTimeBySpawnType.TryGetValue(type, out var lastTime) ? lastTime : 0f;
	}

	public void EventBubbleTouchedPlayer(BubblerObject blubber)
	{
		if (blubber.BubblerConfig.SpawnerType == BubblerEnums.SpawnType.GoodBubbler)
			SetNextSpawnTimer(blubber.BubblerConfig);
	}

	private void SetNextSpawnTimer(BubblerConfig config)
	{
		_NextSpawnTimeBySpawnType[config.SpawnerType] =
				GameManager.Instance.GameState.LevelTimer +
				Random.Range(config.SpawnTimeFrame.x, config.SpawnTimeFrame.y);
	}
}
