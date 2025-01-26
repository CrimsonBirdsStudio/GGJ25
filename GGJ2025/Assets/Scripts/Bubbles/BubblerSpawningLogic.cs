using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BubblerSpawningLogic
{
	public static List<BubblerConfig> GetBubblersThatCanSpawn(BubblerConfig[] bubblerConfigs)
	{
		List<BubblerConfig> result = new List<BubblerConfig>();

		BubblerAccounting bubblerAccounting = GameManager.Instance.BubblerAccounting;
		foreach (var bubbler in bubblerConfigs)
		{
			int spawnedAmount = bubblerAccounting.GetSpawnedBlubbersOfType(bubbler.SpawnerType);
			if (bubbler.MaxTotalSpawned > 0 && spawnedAmount >= bubbler.MaxTotalSpawned) continue;

			float nextSpawnTime = bubblerAccounting.GetNextSpawnTimeForBlubbersOfType(bubbler.SpawnerType);
			if (bubbler.SpawnTimeFrame.y > 0f && GameManager.Instance.GameState.LevelTimer < nextSpawnTime) continue;

			result.Add(bubbler);
		}

		return result;
	}

	public static GameObject InstantiateBubbler(BubblerConfig config, Vector2 position, Transform parent)
	{
		GameObject result = GameObject.Instantiate(config.prefabBase, position, Quaternion.identity, parent);
		result.name = config.SpawnerType.ToString();
		BubblerObject bubbler = result.AddComponent<BubblerObject>();
		bubbler.BubblerConfig = config;

		ConfigBubbleDespawn(bubbler);
		ConfigBubbleMovement(bubbler);
		ConfigBubbleBubble(bubbler);
		ConfigBubbleBubbler(bubbler);
		ConfigBubbleRotation(bubbler);

		return result;
	}
	#region Config spawned bubble
	private static void ConfigBubbleDespawn(BubblerObject bubbler)
	{
		if (bubbler.BubblerConfig.DespawnMechanic == BubblerEnums.DespawnMechanic.None)
			return;

		var despawner = bubbler.gameObject.AddComponent<ToBeCleared>();

		switch (bubbler.BubblerConfig.DespawnMechanic)
		{
			case BubblerEnums.DespawnMechanic.TimeAndDistance:
				despawner.DistanceToDelete = bubbler.BubblerConfig.DespawnDistance;
				despawner.TimeAtDistanceToDelete = bubbler.BubblerConfig.DespawnTime;
				despawner.DistanceToDeleteMax = bubbler.BubblerConfig.DespawnDistanceMax;

				break;
		}
	}

	private static void ConfigBubbleMovement(BubblerObject bubbler)
	{
		switch (bubbler.BubblerConfig.SpawnerType)
		{
			case BubblerEnums.SpawnType.BadFollower:
				var badFollower = bubbler.AddComponent<FollowPlayer>();
				badFollower.interval = bubbler.BubblerConfig.MovementInterval;
				badFollower.force = bubbler.BubblerConfig.MovementForce;
				break;
			case BubblerEnums.SpawnType.GoodBubbler:
				var gottaCatch = bubbler.AddComponent<RandomMovement>();
				gottaCatch.interval = bubbler.BubblerConfig.MovementInterval;
				gottaCatch.start_interval = bubbler.BubblerConfig.MovementInterval;
				gottaCatch.vel = bubbler.BubblerConfig.MovementForce;
				break;
		}
	}
	private static void ConfigBubbleBubble(BubblerObject bubbler)
	{
		if (bubbler.BubblerConfig.prefabBubble == null)
			return;

		var bubbleSprite = GameObject.Instantiate(bubbler.BubblerConfig.prefabBubble, bubbler.transform);
		bubbler.BubbleSprite = bubbleSprite;

		switch (bubbler.BubblerConfig.SpawnerType)
		{
			case BubblerEnums.SpawnType.ObstacleStopper:
				bubbleSprite.GetComponent<Animator>().runtimeAnimatorController = GameManager.Instance.BubblerRepository.BubbleStopperSprite.animatorController;
				break;
			default:
				break;
		}
	}

	private static void ConfigBubbleBubbler(BubblerObject bubbler)
	{
		if (bubbler.BubblerConfig.prefabBubbler == null)
			return;

		var bubbleSprite = GameObject.Instantiate(bubbler.BubblerConfig.prefabBubbler, bubbler.transform);
		bubbler.BubblerSprite = bubbleSprite;

		switch (bubbler.BubblerConfig.SpawnerType)
		{
			case BubblerEnums.SpawnType.BadFollower:
			case BubblerEnums.SpawnType.BadOscillingGroup:
				{
					var allEnemies = GameManager.Instance.GameState.BubblesEnemies;
					var spriteSelected = allEnemies[Random.Range(0, allEnemies.Length)];
					var spriteAnimator = bubbleSprite.GetComponent<Animator>();
					spriteAnimator.runtimeAnimatorController = spriteSelected.animatorController;
					bubbler.BubblerScriptableSprite = spriteSelected;
				}
				break;
			case BubblerEnums.SpawnType.GoodBubbler:
				{
					var targets = GameManager.Instance.GameState.BubblesTarget;
					var spriteSelected = targets[Random.Range(0, targets.Length)];
					var spriteAnimator = bubbleSprite.GetComponent<Animator>();
					spriteAnimator.runtimeAnimatorController = spriteSelected.animatorController;
					bubbler.BubblerScriptableSprite = spriteSelected;
				}
				break;
			default:
				break;
		}
	}

	private static void ConfigBubbleRotation(BubblerObject bubbler)
	{
		var rb = bubbler.GetComponent<Rigidbody2D>();
		if (bubbler.BubblerConfig.RotateOnSpawn > 0f)
		{
			rb.angularDamping = 0f;
			rb.angularVelocity = Random.Range(-bubbler.BubblerConfig.RotateOnSpawn, bubbler.BubblerConfig.RotateOnSpawn);
		}
		else
		{
			rb.freezeRotation = true;
		}
	}

	#endregion Config spawned bubble
}
