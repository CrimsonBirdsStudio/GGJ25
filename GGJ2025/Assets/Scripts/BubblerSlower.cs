using UnityEngine;
using static BubblerEnums;

public class BubblerSlower : MonoBehaviour
{
	public FMODUnity.StudioEventEmitter theSound;
	void Start()
	{
		GameManager.Instance.GameEvents.OnBubbleTriggeredWithPlayerEvent += StartRepelation;
	}

	void StartRepelation(BubblerObject bubbler)
	{
		if (bubbler.BubblerConfig.SpawnerType == SpawnType.ObstacleStopper)
		{
			theSound.Play();
			var playerRb = GameManager.Instance.player_instance.GetComponent<Rigidbody2D>();
			playerRb.angularVelocity *= bubbler.BubblerConfig.MovementForce;
			playerRb.linearVelocity *= bubbler.BubblerConfig.MovementForce;
			Destroy(bubbler.gameObject);
		}
	}
}
