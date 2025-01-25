using System;
using UnityEngine;

/// <summary>
/// Eventos del juego. Necesita una nueva instancia por partida.
/// </summary>
public class GameEvents : MonoBehaviour
{
#region Event definition

	public Action<GameObject> OnBubbleSpawnedEvent;
	public Action<GameObject> OnBubbleCollideWithPlayerEvent;
	public Action<GameObject> OnBubbleDestroyedEvent;

	public Action OnGameStartedEvent;
	public Action OnGameOverEvent;


	#endregion Event difinition

	#region Event triggering

	public void OnBubbleSpawned(GameObject bubble) => OnBubbleSpawnedEvent?.Invoke(bubble); // TODO: Recibir el script de bubbles.
	public void OnBubbleCollideWithPlayer(GameObject bubble) => OnBubbleCollideWithPlayerEvent?.Invoke(bubble); // TODO: Recibir el script de bubbles.
	public void OnBubbleDestroyed(GameObject bubble) => OnBubbleDestroyedEvent?.Invoke(bubble); // TODO: Recibir el script de bubbles.
	public void OnGameStarted() => OnGameStartedEvent?.Invoke();
	public void OnGameOver() => OnGameOverEvent?.Invoke();

	#endregion
}
