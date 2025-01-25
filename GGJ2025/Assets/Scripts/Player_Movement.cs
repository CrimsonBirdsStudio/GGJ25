using UnityEngine;
using UnityEngine.UI;

public class Player_Movement : MonoBehaviour
{
    public float velocityMultiplier = 10f; // Multiplicador de fuerza
    public float staminaRefillRate = 10f; // Velocidad de recarga de estamina
    public float maxStamina = 100f; // Valor máximo de estamina
    public Image staminaIndicator; // Indicador de estamina
    public float cooldown = 1f; // Cooldown entre impulsos
    public AnimationCurve impulseCurve = new AnimationCurve(new Keyframe(0f, 1f, 2, 2), new Keyframe(1f, 5f, 4, 4));

    private float stamina; // Estamina actual
    private float currentCooldown = 0f; // Cooldown restante
    private Vector2 targetDirection; // Dirección del impulso
    private float impulseStrength = 0f; // Fuerza acumulada
    private Rigidbody2D _rb;
    private bool isCharging = false; // Indicador de carga
    private bool isCooldownActive = false; // Indicador del cooldown

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        stamina = maxStamina;
	}

    void Update()
    {
        // Actualizar posición del indicador
        staminaIndicator.transform.position = Input.mousePosition;

        // Cargar impulso mientras mantienes el clic y no estás en cooldown
        if (Input.GetMouseButton(0) && CanImpulse())
        {
            isCharging = true;
            targetDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

            float currentImpulse = impulseStrength / maxStamina;

            // Incrementar la fuerza acumulada limitada por la estamina disponible
            float maxIncrement = Time.deltaTime * impulseCurve.Evaluate(currentImpulse) * velocityMultiplier;
            float actualIncrement = Mathf.Min(maxIncrement, stamina); // Asegura que no consuma más de lo disponible
            impulseStrength += actualIncrement;
            stamina -= actualIncrement; // Consumir estamina gradualmente
            stamina = Mathf.Clamp(stamina, 0, maxStamina);
        }

        // Soltar el clic para aplicar el impulso
        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            ApplyImpulse();
        }

        // Gestionar cooldown y regeneración de estamina
        if (!isCharging)
        {
            HandleCooldownAndStamina();
        }

        // Actualizar el indicador visual
        UpdateStaminaIndicator();
    }

    private void ApplyImpulse()
    {
        float randomZ = Random.Range(0f, 15f); // Ángulo aleatorio entre 0 y 360 grados
        transform.rotation = Quaternion.Euler(0f, 0f, randomZ); // Solo rota en el eje Y
        if (impulseStrength > 0) // Solo aplica impulso si hay fuerza acumulada
        {
            _rb.AddForce(targetDirection * impulseStrength, ForceMode2D.Impulse);
            currentCooldown = cooldown; // Iniciar cooldown
            isCooldownActive = true;
        }

        impulseStrength = 0f; // Reiniciar fuerza acumulada
        isCharging = false;
    }
    private bool CanImpulse()
    {
        // Solo puede cargar si hay estamina y no está en cooldown
        return stamina > 0 && !isCooldownActive;
    }

    private void HandleCooldownAndStamina()
    {
        // Reducir cooldown
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0)
        {
            isCooldownActive = false; // Cooldown terminado
        }
        // Regenerar estamina si no está en cooldown
        stamina += staminaRefillRate*10 * Time.deltaTime;
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        
    }

    private void UpdateStaminaIndicator()
    {
        // Actualizar el fillAmount del indicador
        staminaIndicator.fillAmount = stamina / maxStamina;

        // Cambiar el color según el estado
        if (isCharging)
        {
            staminaIndicator.color = Color.yellow; // Mientras está cargando
        }
        else if (!isCooldownActive && stamina >= maxStamina)
        {
            staminaIndicator.color = Color.green; // Lista para usar
        }
        else if (isCooldownActive)
        {
            staminaIndicator.color = Color.red; // En cooldown
        }
        else
        {
            staminaIndicator.color = Color.red; // Mientras recarga
        }
    }
}
