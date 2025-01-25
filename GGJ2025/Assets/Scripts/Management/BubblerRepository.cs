using System.Linq;
using UnityEngine;

public class BubblerRepository : MonoBehaviour
{
    public GameObject[] AllSprites;

    public GameObject GetRandomBubblerExcluding(string[] excluded)
    {
        GameObject selected;
        do
        {
            selected = AllSprites[Random.Range(0, AllSprites.Length)];

        } while (excluded.Contains(selected.name));
        return selected;

	}

    public GameObject[] GetBubblesForShoppingList(int amount)
    {
		// TODO: Lógica para obtener una lista de la compra elaborada.
		GameObject[] selected = new GameObject[amount];

		for (int i = 0; i < amount; i++)
        {
            selected[i] = AllSprites[i];
		}

        return selected;
    }
}
