using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BubbleGeneration : MonoBehaviour
{
	private Transform PlayerObject;
	private Transform CameraObject;

	public BubblerConfig[] bubblerConfigs;
	public Vector2 SpawnDistance; // A partir de qué distancia pueden aparecer las burbujas, desde el borde de la pantalla.
	public float SpawnRadius; // En qué radio aparecen las burbujas, respecto al SpawnDistance.
	public float DesiredBubbleDensity; // Densidad de burbujas deseada.
	public float DensityCheckRadius = 1f; // En qué radio se comprueba la densidad antes de spawnear.
	public float SpawnCooldown = 0.2f; // Tiempos entre spawneos, en segundos.
	public float BubbleMargin;
	public float BubbleRadius; // Radio de cada burbuja por defecto, para calcular la densidad de burbujas.

	private float _lastSpawnTime = 0f;
	private float _currentAreaForDensityCheck;
	private List<Vector2> _nextPositionsToSpawn = new List<Vector2>();
	private Vector2 lastPlayerPos;

	// Debugs
	private Vector2 _debugSpawnPos;
	private float _debugBubbleRadius;
	private float _debugSpawnRadius;

	void Start()
	{
		transform.position = Vector2.zero;
		if (CameraObject == null)
			CameraObject = Camera.main.transform;
		if (PlayerObject == null)
			PlayerObject = GameManager.Instance.player_instance.transform;
	}

	// Update is called once per frame
	void Update()
	{
		if (Time.time > _lastSpawnTime + SpawnCooldown)
		{
			_lastSpawnTime = Time.time;
			//SpawnBubbles();
			SpawnBubbles2();
			lastPlayerPos = PlayerObject.position;
		}
	}

	void SpawnBubbles2()
	{
		_debugBubbleRadius = 0f;
		_debugSpawnRadius = 0f;

		// Obtener dirección del jugador.
		Vector2 playerDir = ((Vector2)PlayerObject.position - lastPlayerPos).normalized;
		if (Mathf.Abs(playerDir.magnitude) < 0.01f)
			return;


		// Comprobar qué bubblers tienen posibilidad de spawnear y obtener una lista filtrada.
		var configsToSpawn = BubblerSpawningLogic.GetBubblersThatCanSpawn(bubblerConfigs);
		if (configsToSpawn.Count == 0)
			return;

		_debugSpawnRadius = configsToSpawn.Max(x => x.SpawnRadius);
		Vector2[][] nextPositionsListToSpawn = new Vector2[configsToSpawn.Count][];

		int configIndex = 0;
		foreach (var configToSpawn in configsToSpawn)
		{
			// Elegir posición de spawn en base a la dirección en la que va el jugador.
			Vector2 borderDistance = GameManager.Instance.CurrentViewSize / 2f;
			Vector2 spawnCenterPos = (Vector2)CameraObject.position + new Vector2(playerDir.x * (borderDistance.x + SpawnDistance.x + configToSpawn.SpawnRadius), playerDir.y * (borderDistance.y + SpawnDistance.y + configToSpawn.SpawnRadius));
			_debugSpawnPos = spawnCenterPos;

			// Obtener densidad de burbujas en la zona.
			_currentAreaForDensityCheck = Mathf.PI * configToSpawn.SpawnRadius * configToSpawn.SpawnRadius;
			Collider2D[] collidersInArea = Physics2D.OverlapCircleAll(spawnCenterPos, configToSpawn.SpawnRadius);

			float areaUsedByAllBubbles = 0f;
			float areaUsedBySameBubbles = 0f;
			float maxAllBubbleRadius = configsToSpawn.Max(x => x.Radius);
			float maxSameBubbleRadius = configToSpawn.Radius;
			float maxAllBubbleArea = Mathf.PI * maxAllBubbleRadius * maxAllBubbleRadius;
			float maxSameBubbleArea = Mathf.PI * configToSpawn.Radius * configToSpawn.Radius;

			for (int i = 0; i < collidersInArea.Length; i++)
			{
				var foundBubbleObject = collidersInArea[i]?.transform.parent?.GetComponent<BubblerObject>();
				if (foundBubbleObject == null)
				{
					print($"collidersInArea[i]: {collidersInArea[i].name}, parent = {collidersInArea[i]?.transform.parent.name}");
				}
				if (foundBubbleObject != null)
				{
					BubblerConfig existingBubble = foundBubbleObject.BubblerConfig;
					areaUsedByAllBubbles += Mathf.PI * existingBubble.Radius * existingBubble.Radius;
					float nextAllBubbleRadius = existingBubble.Radius;
					float nextAllBubbleArea = Mathf.PI * nextAllBubbleRadius * nextAllBubbleRadius;

					maxAllBubbleRadius = Mathf.Max(nextAllBubbleRadius, maxAllBubbleRadius);
					maxAllBubbleArea = Mathf.Max(nextAllBubbleArea, maxAllBubbleArea);

					if (existingBubble.SpawnerType == configToSpawn.SpawnerType)
					{
						float nextSameBubbleArea = Mathf.PI * existingBubble.Radius * existingBubble.Radius;
						areaUsedBySameBubbles += nextSameBubbleArea;
					}
				}
			}
			float maxRadiusAllBoth = Mathf.Max(maxAllBubbleRadius, maxSameBubbleRadius);
			_debugBubbleRadius = Mathf.Max( maxRadiusAllBoth, _debugBubbleRadius);

			float currentAllDensity = areaUsedByAllBubbles == 0 ? 0 : areaUsedByAllBubbles / _currentAreaForDensityCheck;
			float currentSameDensity = areaUsedBySameBubbles == 0 ? 0 : areaUsedBySameBubbles / _currentAreaForDensityCheck;

			//print($"type: {configToSpawn.SpawnerType}, currentAllDensity: {currentAllDensity}, currentSameDensity: {currentSameDensity}, maxRadiusAllBoth: {maxRadiusAllBoth}");

			// En base a la densidad, calcular cuantas burbujas hay que spawnear de cada tipo.
			if (currentAllDensity >= configToSpawn.MaxDensityOverall ||
				currentSameDensity >= configToSpawn.MaxDensityForSameType)
				return;

			int bubbleAmountToSpawn = Mathf.FloorToInt(Mathf.Clamp(
				(configToSpawn.MaxDensityForSameType - currentSameDensity) * (_currentAreaForDensityCheck / maxSameBubbleArea),
				0f,
				(configToSpawn.MaxDensityOverall - currentAllDensity) * (_currentAreaForDensityCheck / maxAllBubbleArea)
				));
			bubbleAmountToSpawn = Mathf.Clamp(bubbleAmountToSpawn, 0, configToSpawn.MaxTotalSpawned);

			//print(((configToSpawn.MaxDensityForSameType - currentSameDensity) * (_currentAreaForDensityCheck / maxSameBubbleArea))+" "+ (configToSpawn.MaxDensityOverall - currentAllDensity) * (_currentAreaForDensityCheck / maxAllBubbleArea));

			// Elegir posiciones para cada burbuja sin que se sobrepongan.
			_nextPositionsToSpawn.Clear();
			for (int i = 0; i < bubbleAmountToSpawn; i++)
			{
				if (_nextPositionsToSpawn.Count < i)
					break;

				int attemptsLeft = 10;
				while (attemptsLeft > 0)
				{
					attemptsLeft--;
					Vector2 nextPosition = spawnCenterPos + new Vector2(UnityEngine.Random.Range(-configToSpawn.SpawnRadius, configToSpawn.SpawnRadius), UnityEngine.Random.Range(-configToSpawn.SpawnRadius, configToSpawn.SpawnRadius));

					if (Physics2D.OverlapCircle(nextPosition, maxRadiusAllBoth + BubbleMargin) == null &&
						!_nextPositionsToSpawn.Any(pos => Vector2.Distance(nextPosition, pos) < maxRadiusAllBoth + BubbleMargin))
					{
						_nextPositionsToSpawn.Add(nextPosition);
						break;
					}
				}
			}
			nextPositionsListToSpawn[configIndex] = _nextPositionsToSpawn.ToArray();
			configIndex++;
		}

		// Spawnear burbujas en las posiciones determinadas.
		for (int i = 0; i < nextPositionsListToSpawn.Length; i++)
		{
			if (nextPositionsListToSpawn[i] == null)
				continue;
			for (int j = 0; j < nextPositionsListToSpawn[i].Length; j++)
			{
				BubblerSpawningLogic.InstantiateBubbler(configsToSpawn[i], nextPositionsListToSpawn[i][j], transform);
			}
		}
	}

	void SpawnBubbles()
	{
		// Obtener dirección del jugador.
		Vector2 playerDir = ((Vector2)PlayerObject.position - lastPlayerPos).normalized;
		if (Mathf.Abs(playerDir.magnitude) < 0.01f)
			return;

		// Elegir posición de spawn en base a la dirección en la que va el jugador.
		Vector2 borderDistance = GameManager.Instance.CurrentViewSize / 2f;
		Vector2 spawnCenterPos = (Vector2)CameraObject.position + new Vector2(playerDir.x * (borderDistance.x + SpawnDistance.x + SpawnRadius), playerDir.y * (borderDistance.y + SpawnDistance.y + SpawnRadius));
		_debugSpawnPos = spawnCenterPos;

		// Obtener densidad de burbujas en la zona.
		_currentAreaForDensityCheck = Mathf.PI * SpawnRadius * SpawnRadius;
		Collider2D[] collidersInArea = Physics2D.OverlapCircleAll(spawnCenterPos, SpawnRadius);
		float areaUsedByBubbles = 0f;
		float maxBubbleRadius = BubbleRadius;
		float maxBubbleArea = Mathf.PI * BubbleRadius * BubbleRadius;

		for (int i = 0; i < collidersInArea.Length; i++)
		{
			areaUsedByBubbles += Mathf.PI * BubbleRadius * BubbleRadius; // TODO: Obtener el BubbleRadius de la configuración de cada burbuja.
			float nextBubbleRadius = BubbleRadius;
			float nextBubbleArea = Mathf.PI * BubbleRadius * BubbleRadius;
			maxBubbleRadius = Mathf.Max(nextBubbleRadius, maxBubbleRadius);
			maxBubbleArea = Mathf.Max(nextBubbleArea, maxBubbleArea);
		}
		_debugBubbleRadius = maxBubbleRadius;

		float currentDensity = areaUsedByBubbles == 0 ? 0 : areaUsedByBubbles / _currentAreaForDensityCheck;

		// En base a la densidad, calcular cuantas burbujas hay que spawnear.
		if (currentDensity >= DesiredBubbleDensity)
			return;
		int bubbleAmountToSpawn = Mathf.FloorToInt((DesiredBubbleDensity - currentDensity) * (_currentAreaForDensityCheck / maxBubbleArea));

		// Elegir posiciones para cada burbuja sin que se sobrepongan.
		_nextPositionsToSpawn.Clear();
		for (int i = 0; i < bubbleAmountToSpawn; i++)
		{
			if (_nextPositionsToSpawn.Count < i)
				break;

			int attemptsLeft = 10;
			while (attemptsLeft > 0)
			{
				attemptsLeft--;
				Vector2 nextPosition = spawnCenterPos + new Vector2(UnityEngine.Random.Range(-SpawnRadius, SpawnRadius), UnityEngine.Random.Range(-SpawnRadius, SpawnRadius));

				if (Physics2D.OverlapCircle(nextPosition, maxBubbleRadius + BubbleMargin) == null &&
					!_nextPositionsToSpawn.Any(pos => Vector2.Distance(nextPosition, pos) < maxBubbleRadius + BubbleMargin))
				{
					_nextPositionsToSpawn.Add(nextPosition);
					break;
				}
			}
		}

		// Elegir burbujas a spawnear.
		// TODO: Implementar lógica.
		var possibleBubblers = BubblerSpawningLogic.GetBubblersThatCanSpawn(bubblerConfigs);
		if (possibleBubblers.Count > 0)
		{
			BubblerConfig[] bubblesChoosen = _nextPositionsToSpawn.Select(pos => possibleBubblers[UnityEngine.Random.Range(0, possibleBubblers.Count)]).ToArray();

			// Spawnear burbujas.
			for (int i = 0; i < bubblesChoosen.Length; i++)
			{
				BubblerSpawningLogic.InstantiateBubbler(bubblesChoosen[i], _nextPositionsToSpawn[i], transform);
			}
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(_debugSpawnPos, _debugSpawnRadius);

		Gizmos.color = Color.red;
		for (int i = 0; i < _nextPositionsToSpawn.Count; i++)
		{
			Gizmos.DrawSphere(_nextPositionsToSpawn[i], _debugBubbleRadius);
		}
	}
}
