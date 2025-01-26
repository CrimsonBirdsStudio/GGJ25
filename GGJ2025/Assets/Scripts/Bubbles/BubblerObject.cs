using UnityEngine;

/// <summary>
/// Objecto de referencia de los blubbers spawneados.
/// </summary>
public class BubblerObject : MonoBehaviour
{
    public BubblerConfig BubblerConfig;
	public GameObject BubblerSprite;
	public GameObject BubbleSprite;
	public Bubler_Scriptable BubblerScriptableSprite;

	public bool IsDestroyed { get; private set; }
    void Start()
    {
        GameManager.Instance.GameEvents.OnBubbleSpawned(this);
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject == GameManager.Instance.player_instance)
		{
			GameManager.Instance.GameEvents.OnBubbleCollideWithPlayer(this);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject == GameManager.Instance.player_instance)
		{
			GameManager.Instance.GameEvents.OnBubbleTriggeredWithPlayer(this);
		}
	}

	private void OnDestroy()
	{
		IsDestroyed = true;
		GameManager.Instance.GameEvents.OnBubbleDestroyed(this);
	}
}
