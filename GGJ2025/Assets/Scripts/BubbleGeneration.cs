using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BubbleGeneration : MonoBehaviour
{
	public Transform PlayerObject; // Referencia a objeto de jugador. TODO: Hacerlo private y obtenerlo en el start.
	public Transform CameraObject;

	public GameObject[] BubblesToSpawn;
	public Vector2 SpawnDistance; // A partir de qué distancia pueden aparecer las burbujas.
	public float SpawnRadius; // En qué radio aparecen las burbujas, respencto al SpawnDistance.
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
	private float _debugBubbleArea;

	void Start()
	{
		if (CameraObject == null)
			CameraObject = Camera.main.transform;
		if (PlayerObject == null)
			PlayerObject = GameObject.Find("Player").transform;
	}

	// Update is called once per frame
	void Update()
	{
		if (Time.time > _lastSpawnTime + SpawnCooldown)
		{
			_lastSpawnTime = Time.time;
			SpawnBubbles();
			lastPlayerPos = PlayerObject.position;
		}
	}

	void SpawnBubbles()
	{
		// Obtener dirección del jugador.
		Vector2 playerDir = ((Vector2)PlayerObject.position - lastPlayerPos).normalized;
		if (playerDir.magnitude == 0)
			return;

		// Elegir posición de spawn en base a la dirección en la que va el jugador.
		Vector2 spawnCenterPos = (Vector2)CameraObject.position + new Vector2(playerDir.x * (SpawnDistance.x + SpawnRadius), playerDir.y * (SpawnDistance.y + SpawnRadius));
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
		_debugBubbleArea = maxBubbleArea;
		_debugBubbleRadius = maxBubbleRadius;

		float currentDensity = areaUsedByBubbles == 0 ? 0 : areaUsedByBubbles / _currentAreaForDensityCheck;

		// En base a la densidad, calcular cuantas burbujas hay que spawnear.
		if (currentDensity >= DesiredBubbleDensity)
			return;
		int bubbleAmountToSpawn = Mathf.FloorToInt((DesiredBubbleDensity - currentDensity) * (_currentAreaForDensityCheck / maxBubbleArea));
		Debug.Log($"currentDensity: {currentDensity}, bubbleAmountToSpawn: {bubbleAmountToSpawn}, _currentAreaForDensityCheck: {_currentAreaForDensityCheck}, areaUsedByBubbles: {areaUsedByBubbles}, maxBubbleArea: {maxBubbleArea}, maxBubbleRadius: {maxBubbleRadius}");

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
		GameObject[] bubblesChoosen = _nextPositionsToSpawn.Select(pos => BubblesToSpawn[0]).ToArray();

		// Spawnear burbujas.
		for(int i = 0;i < bubblesChoosen.Length; i++)
		{
			GameObject.Instantiate(bubblesChoosen[i], _nextPositionsToSpawn[i], Quaternion.identity);
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(_debugSpawnPos, SpawnRadius);

		Gizmos.color = Color.red;
		for (int i = 0; i < _nextPositionsToSpawn.Count; i++)
		{
			Gizmos.DrawSphere(_nextPositionsToSpawn[i], _debugBubbleRadius);
		}
	}
}
