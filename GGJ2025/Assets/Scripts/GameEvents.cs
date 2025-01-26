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

	public Action<BubblerObject> OnGameStateBubblersObtainedEvent;
	public Action<BubblerObject> OnGameStateBubblersLostEvent;

	public Action OnGameStartedEvent;
	public Action<bool> OnGameOverEvent;


	#endregion Event difinition

	#region Event triggering

	public void OnBubbleSpawned(BubblerObject bubble) => OnBubbleSpawnedEvent?.Invoke(bubble);
	public void OnBubbleCollideWithPlayer(BubblerObject bubble) => OnBubbleCollideWithPlayerEvent?.Invoke(bubble);
	public void OnBubbleTriggeredWithPlayer(BubblerObject bubble) => OnBubbleTriggeredWithPlayerEvent?.Invoke(bubble);
	public void OnBubbleDestroyed(BubblerObject bubble) => OnBubbleDestroyedEvent?.Invoke(bubble);
	public void OnGameStarted() => OnGameStartedEvent?.Invoke();
	public void OnGameStateBubblersObtained(BubblerObject bubble) => OnGameStateBubblersObtainedEvent?.Invoke(bubble);
	public void OnGameStateBubblersLost(BubblerObject bubble) => OnGameStateBubblersLostEvent?.Invoke(bubble);
	public void OnGameOver(bool playerWon) => OnGameOverEvent?.Invoke(playerWon);

	#endregion
}
