using System.Collections.Generic;
using UnityEngine;

public class BubblerSpawningLogic
{
    public static List<BubblerConfig> GetBubblersThatCanSpawn(BubblerConfig[] bubblerConfigs)
    {
        List<BubblerConfig> result = new List<BubblerConfig> ();

        BubblerAccounting bubblerAccounting = GameManager.Instance.BubblerAccounting;
		foreach (var bubbler in bubblerConfigs)
        {

		}

        return result;
    }

    public static GameObject InstantiateBubbler(BubblerConfig config, Vector2 position, Transform parent)
	{
        return GameObject.Instantiate(config.prefabBase, position, Quaternion.identity, parent); ;
    }
}
