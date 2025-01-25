using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    Player_Movement _player;
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
        _player = FindFirstObjectByType<Player_Movement>();

        // Verificar que los componentes se han encontrado
        if (_player == null) Debug.LogError("Player no encontrado en la escena.");
    }

    public GameObject player_instance => _player.gameObject;
}
