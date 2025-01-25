using System;
using UnityEngine;

/// <summary>
/// Eventos del juego. Necesita una nueva instancia por partida.
/// </summary>
public class GameEvents : MonoBehaviour
{
#region Event definition

	public Action<BubblerObject> OnBubbleSpawnedEvent;
	public Action<BubblerObject> OnBubbleCollideWithPlayerEvent;
	public Action<BubblerObject> OnBubbleTriggeredWithPlayerEvent;
	public Action<BubblerObject> OnBubbleDestroyedEvent;

	public Action OnGameStartedEvent;
	public Action OnGameOverEvent;


	#endregion Event difinition

	#region Event triggering

	public void OnBubbleSpawned(BubblerObject bubble) => OnBubbleSpawnedEvent?.Invoke(bubble);
	public void OnBubbleCollideWithPlayer(BubblerObject bubble) => OnBubbleCollideWithPlayerEvent?.Invoke(bubble);
	public void OnBubbleTriggeredWithPlayer(BubblerObject bubble) => OnBubbleTriggeredWithPlayerEvent?.Invoke(bubble);
	public void OnBubbleDestroyed(BubblerObject bubble) => OnBubbleDestroyedEvent?.Invoke(bubble);
	public void OnGameStarted() => OnGameStartedEvent?.Invoke();
	public void OnGameOver() => OnGameOverEvent?.Invoke();

	#endregion
}
