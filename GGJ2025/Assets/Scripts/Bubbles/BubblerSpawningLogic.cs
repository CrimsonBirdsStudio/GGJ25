using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BubblerSpawningLogic
{
    public static List<BubblerConfig> GetBubblersThatCanSpawn(BubblerConfig[] bubblerConfigs)
    {
        List<BubblerConfig> result = new List<BubblerConfig> ();

        BubblerAccounting bubblerAccounting = GameManager.Instance.BubblerAccounting;
		foreach (var bubbler in bubblerConfigs)
        {
			int spawnedAmount = bubblerAccounting.GetSpawnedBlubbersOfType(bubbler.SpawnerType);
			if (spawnedAmount >= bubbler.MaxTotalSpawned) continue;

			float nextSpawnTime = bubblerAccounting.GetNextSpawnTimeForBlubbersOfType(bubbler.SpawnerType);
			if (GameManager.Instance.GameState.LevelTimer < nextSpawnTime) continue;

			result.Add(bubbler);
		}

        return result;
    }

    public static GameObject InstantiateBubbler(BubblerConfig config, Vector2 position, Transform parent)
	{
		GameObject result = GameObject.Instantiate(config.prefabBase, position, Quaternion.identity, parent);
        BubblerObject bubbler = result.AddComponent<BubblerObject>();
		bubbler.BubblerConfig = config;


		ConfigBubbleDespawn(bubbler);
		ConfigBubbleMovement(bubbler);
		ConfigBubbleBubble(bubbler);
		ConfigBubbleBubbler(bubbler);

		return result;
	}
	#region Config spawned bubble
	private static void ConfigBubbleDespawn(BubblerObject bubbler)
    {
        if (bubbler.BubblerConfig.DespawnMechanic == BubblerEnums.DespawnMechanic.None)
            return;

        var despawner = bubbler.gameObject.AddComponent<ToBeCleared>();

        switch(bubbler.BubblerConfig.DespawnMechanic){
            case BubblerEnums.DespawnMechanic.TimeAndDistance:
                despawner.DistanceToDelete = bubbler.BubblerConfig.DespawnDistance;
                despawner.TimeAtDistanceToDelete = bubbler.BubblerConfig.DespawnTime;

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


		switch (bubbler.BubblerConfig.SpawnerType)
		{
			case BubblerEnums.SpawnType.BadFollower:
			case BubblerEnums.SpawnType.BadOscillingGroup:
			case BubblerEnums.SpawnType.GoodBubbler:
				break;
			default:
				break;
		}
	}

	private static void ConfigBubbleBubbler(BubblerObject bubbler)
	{
		if (bubbler.BubblerConfig.prefabBubble == null)
			return;

		var bubbleSprite = GameObject.Instantiate(bubbler.BubblerConfig.prefabBubbler, bubbler.transform);

		// TODO: Obtener sprites con nuevo scriptableobject de sprites.

		switch (bubbler.BubblerConfig.SpawnerType)
		{
			case BubblerEnums.SpawnType.BadFollower:
			case BubblerEnums.SpawnType.BadOscillingGroup:
				{
					var spriteRendererSelected = GameManager.Instance.BubblerRepository.GetRandomBubblerExcluding();
					var spriteRenderer = bubbleSprite.GetComponent<SpriteRenderer>();
					spriteRenderer.sprite = spriteRendererSelected.GetComponent<SpriteRenderer>().sprite;
				}
				break;
			case BubblerEnums.SpawnType.GoodBubbler:
				{
					var targets = GameManager.Instance.GameState.BubblesTarget;
					var spriteRendererSelected = targets[Random.Range(0, targets.Length)];
					var spriteRenderer = bubbleSprite.GetComponent<SpriteRenderer>();
					spriteRenderer.sprite = spriteRendererSelected.GetComponent<SpriteRenderer>().sprite;
				}
				break;
			default:
				break;
		}
	}

	#endregion Config spawned bubble
}
