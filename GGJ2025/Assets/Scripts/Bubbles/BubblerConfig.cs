using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BubblerBase", menuName = "Scriptable Objects/BubblerBase")]
public class BubblerConfig : ScriptableObject
{
	// public BubblerEnums.Shape Shape;
	// public BubblerEnums.Family Family;
	public GameObject prefabBase; // Prefab with rigidbody.
	public GameObject prefabBubble; // Prefab with spriterenderer and collider2d.
	public GameObject prefabBubbler; // Prefab with spriterenderer.

	public BubblerEnums.SpawnType SpawnerType;
	public BubblerEnums.SpawnAreaType SpawnAreaType;
	public BubblerEnums.RespawnMechanic RespawnMechanic;
	public BubblerEnums.DespawnMechanic DespawnMechanic;

	public Vector2 SpawnTimeFrame;
	public Vector2 RespawnTimeFrame;

	public int MaxTotalSpawned;
	public float MaxDensityForSameType;
	public float MaxDensityOverall;
	public float Radius;

	public float DespawnTime;
	public float DespawnDistance;

	public float MovementInterval;
	public float MovementForce;

}
