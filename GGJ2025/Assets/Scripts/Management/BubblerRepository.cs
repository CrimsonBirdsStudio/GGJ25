using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BubblerRepository : MonoBehaviour
{
    public Bubler_Scriptable[] AllBubblerSprites;
    public Bubler_Scriptable BubbleStopperSprite;

    public Bubler_Scriptable GetRandomBubblerExcluding(params Bubler_Scriptable[] excluded)
    {
        var listWithourExluded = AllBubblerSprites
            .Where(x => !excluded.Contains(x)).ToArray();
		return listWithourExluded[Random.Range(0, listWithourExluded.Length)];
	}

    public Bubler_Scriptable[] GetBubblesForShoppingList(int amount, Bubler_Scriptable playerToExclude)
    {
        List<Bubler_Scriptable> selected = new List<Bubler_Scriptable>(amount);

		while(selected.Count < amount)
        {
            var choosen = AllBubblerSprites[Random.Range(0, AllBubblerSprites.Count())];
            if(choosen != playerToExclude && !selected.Contains(choosen))
                selected.Add(choosen);
		}

        return selected.ToArray();
    }

	public Bubler_Scriptable[] GetAllBubblersExcluding(params string[] excluded)
	{
        return AllBubblerSprites.Where(x => !excluded.Contains(x.name)).ToArray();
	}
}
