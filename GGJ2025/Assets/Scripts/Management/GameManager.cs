using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TheCleanerTM ObjectCleaner;
	public Player_Movement PlayerMovementScript;
    public GameState GameState;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Buscar componentes en la escena una vez cargada
        PlayerMovementScript = FindObjectInSceneAndShowError<Player_Movement>();
		ObjectCleaner = FindObjectInSceneAndShowError<TheCleanerTM>();
		GameState = FindObjectInSceneAndShowError<GameState>();
	}

    public GameObject player_instance => PlayerMovementScript.gameObject;

    public static T FindObjectInSceneAndShowError<T>() where T : MonoBehaviour
    {
        T found = FindFirstObjectByType<T>();
		if (found == null) Debug.LogError($"{typeof(T)} no encontrado en la escena.");
        return found;
	}
}
