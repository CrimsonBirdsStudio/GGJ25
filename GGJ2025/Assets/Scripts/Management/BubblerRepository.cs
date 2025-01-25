using System.Linq;
using UnityEngine;

public class BubblerRepository : MonoBehaviour
{
    public GameObject[] AllBubblerSprites;

    public GameObject GetRandomBubblerExcluding(params string[] excluded)
    {
        var listWithourExluded = AllBubblerSprites
            .Where(x => !excluded.Contains(x.name)).ToArray();
		return listWithourExluded[Random.Range(0, listWithourExluded.Length)];
	}

    public GameObject[] GetBubblesForShoppingList(int amount)
    {
		// TODO: Lógica para obtener una lista de la compra elaborada.
		GameObject[] selected = new GameObject[amount];

		for (int i = 0; i < amount; i++)
        {
            selected[i] = AllBubblerSprites[i];
		}

        return selected.ToArray();
    }

	public GameObject[] GetAllBubblersExcluding(params string[] excluded)
	{
        return AllBubblerSprites.Where(x => !excluded.Contains(x.name)).ToArray();
	}
}
