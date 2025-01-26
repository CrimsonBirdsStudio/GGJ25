using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Información sobre el estado actual del juego.
/// </summary>
public class GameState : MonoBehaviour
{
	// TODO: Gestionar los bubbles obtenidos y los que faltan.
	public float CurrentDifficulty; // Dificultad actual que escala con el progreso. // TODO: Implementar como getter a partir del progreso del jugador.
	public int CurrentLevel; // Nivel actual, en base a los blubbers recogidos. // TODO: Implementar como getter a partir del progreso del jugador.
	public Bubler_Scriptable[] BubblesTarget;
	public Bubler_Scriptable[] BubblesEnemies;
	public bool[] BubblesObtained;
	public Bubler_Scriptable BubblerPlayer; // Sprite del player.
	public int TargetShoppingListAmount;

	// Cosas de pause
	public bool IsPaused { get { return _isPaused; } set { _isPaused = value; SetIsTimePassing(); } }
	private bool _isPaused;
	public bool IsGameEnded { get { return _isGameEnded; } set { _isGameEnded = value; SetIsTimePassing(); } }
	private bool _isGameEnded = false;


	// Tema pausas y calculo del tiempo.
	public float LevelTimer { get { return (_isTimePassing ? Time.timeSinceLevelLoad : _lastPausedAt) - _levelTimeStart - _totalTimePaused; } }
	private float _levelTimeStart;
	private float _lastPausedAt; // Tiempo en el que se pausó por última vez.
	private float _totalTimePaused;
	private bool _isTimePassing;

	private void Start()
	{
		SetNewGame();
	}
	/// <summary>
	/// Establece el estado en nuevo juego.
	/// </summary>
	public void SetNewGame()
	{
		_levelTimeStart = Time.timeSinceLevelLoad;
		IsGameEnded = false;
		IsPaused = false;

		BubblerPlayer = GameManager.Instance.BubblerRepository.GetRandomBubblerExcluding();
		BubblesTarget = GameManager.Instance.BubblerRepository.GetBubblesForShoppingList(TargetShoppingListAmount, BubblerPlayer);
		BubblesEnemies = GameManager.Instance.BubblerRepository.GetAllBubblersExcluding().Where(x => x != BubblerPlayer && !BubblesTarget.Contains(x)).ToArray();
		BubblesObtained = new bool[BubblesTarget.Length];
		UI_Controller UIcontroller = GameManager.Instance.UIController.GetComponentInChildren<UI_Controller>();


		GameManager.Instance.player_instance.GetComponentInChildren<Player_Movement>().scriptableData = BubblerPlayer;
		//Tengo que asignar aqui los sprites en la UI leyendolos de los scriptables
		UIcontroller.SetIcons(BubblesTarget.Select(x => x.bublerSpriteUIOff).ToList(), BubblesTarget.Select(x => x.bublerSpriteUIOn).ToList());

		GameManager.Instance.GameEvents.OnBubbleTriggeredWithPlayerEvent += PlayerGotBubbler;
	}

	public void SetGameOver()
	{
		IsGameEnded = true;
	}

	public bool IsGoodBubbler(Bubler_Scriptable sprite)
	{
		return BubblesTarget.Contains(sprite);
	}

	public Bubler_Scriptable[] GetRemainingTargets()
	{
		var remaining = new List<Bubler_Scriptable>();
		for (int i = 0; i < BubblesObtained.Length; i++)
		{
			if (!BubblesObtained[i])
				remaining.Add(BubblesTarget[i]);
		}

		return remaining.ToArray();
	}

	private void PlayerGotBubbler(BubblerObject bubbler)
	{
		var targets = BubblesTarget.ToList();
		var sprite = bubbler.BubblerScriptableSprite;
		bool isGood = IsGoodBubbler(bubbler.BubblerScriptableSprite);
		if (isGood)
		{
			int index = targets.IndexOf(sprite);
			BubblesObtained[index] = true;
			GameManager.Instance.GameEvents.OnGameStateBubblersObtained(bubbler);

			if (BubblesObtained.Count(x => x) == BubblesObtained.Length)
			{
				GameManager.Instance.GameEvents.OnGameOver(true);
			}
		}
		else
		{
			if (BubblesObtained.Count(x => x) == BubblesObtained.Length)
			{
				GameManager.Instance.GameEvents.OnGameOver(false);
			}
			else
			{
				for (int i = 0; i < BubblesObtained.Length; i++)
				{
					if (BubblesObtained[i])
					{
						BubblesObtained[i] = false;
						GameManager.Instance.GameEvents.OnGameStateBubblersLost(BubblesTarget[i]);
						break;
					}
				}
			}
		}
	}

	/// <summary>
	/// Reanuda o pausa el paso del tiempo en base a si está pausado o el juego ha terminado.
	/// </summary>
	private void SetIsTimePassing()
	{
		bool prevIsTimePassing = _isTimePassing;
		_isTimePassing = !IsGameEnded && !IsPaused;

		if (_isTimePassing != prevIsTimePassing)
		{
			if (_isTimePassing)
			{
				_totalTimePaused += Time.timeSinceLevelLoad - _lastPausedAt;
			}
			else
			{
				_lastPausedAt = Time.timeSinceLevelLoad;
			}
		}
	}
}
