using System.Linq;
using UnityEngine;

public class BubblerRepository : MonoBehaviour
{
    public Bubler_Scriptable[] AllBubblerSprites;

    public Bubler_Scriptable GetRandomBubblerExcluding(params string[] excluded)
    {
        var listWithourExluded = AllBubblerSprites
            .Where(x => !excluded.Contains(x.name)).ToArray();
		return listWithourExluded[Random.Range(0, listWithourExluded.Length)];
	}

    public Bubler_Scriptable[] GetBubblesForShoppingList(int amount)
    {
        // TODO: Lógica para obtener una lista de la compra elaborada.
        Bubler_Scriptable[] selected = new Bubler_Scriptable[amount];

		for (int i = 0; i < amount; i++)
        {
            selected[i] = AllBubblerSprites[Random.Range(0,AllBubblerSprites.Count())];
		}

        return selected.ToArray();
    }

	public Bubler_Scriptable[] GetAllBubblersExcluding(params string[] excluded)
	{
        return AllBubblerSprites.Where(x => !excluded.Contains(x.name)).ToArray();
	}
}
