using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TheCleanerTM ObjectCleaner;
	public Player_Movement PlayerMovementScript;
    public GameState GameState;
    public BubblerRepository BubblerRepository;
    public BubblerAccounting BubblerAccounting;
	public GameEvents GameEvents;
    public UI_Controller UIController;

    public Vector2 CurrentViewSize {
        get {
            Camera cam = Camera.main;
            return new Vector2(cam.orthographicSize * 2 * cam.aspect,  cam.orthographicSize * 2);
        } }
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
        ResetPerGameScripts();
        PlayerMovementScript = FindObjectInSceneAndShowError<Player_Movement>();
		ObjectCleaner = FindObjectInSceneAndShowError<TheCleanerTM>();
		GameState = FindObjectInSceneAndShowError<GameState>();
		BubblerRepository = FindObjectInSceneAndShowError<BubblerRepository>();
		BubblerAccounting = FindObjectInSceneAndShowError<BubblerAccounting>();
        UIController = FindObjectInSceneAndShowError<UI_Controller>();
    }

	private void ResetPerGameScripts()
    {
		GameEvents = FindFirstObjectByType<GameEvents>();
        if (GameEvents == null)
            GameEvents = gameObject.AddComponent<GameEvents>();

		BubblerAccounting = FindFirstObjectByType<BubblerAccounting>();
		if (BubblerAccounting == null)
			BubblerAccounting = gameObject.AddComponent<BubblerAccounting>();
	}

    public GameObject player_instance => PlayerMovementScript.gameObject;

    public static T FindObjectInSceneAndShowError<T>() where T : MonoBehaviour
    {
        T found = FindFirstObjectByType<T>();
		if (found == null) Debug.LogError($"{typeof(T)} no encontrado en la escena.");
        return found;
	}
}
