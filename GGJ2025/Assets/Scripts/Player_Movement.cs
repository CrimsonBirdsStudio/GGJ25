using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class Player_Movement : MonoBehaviour
{

    public Bubler_Scriptable scriptableData;
    public float velocityMultiplier = 10f; // Multiplicador de fuerza
    public float staminaRefillRate = 10f; // Velocidad de recarga de estamina
    public float maxStamina = 100f; // Valor máximo de estamina
    public Image staminaIndicator; // Indicador de estamina
    public float cooldown = 1f; // Cooldown entre impulsos
    public AnimationCurve impulseCurve = new AnimationCurve(new Keyframe(0f, 1f, 2, 2), new Keyframe(1f, 5f, 4, 4));
    public GameObject arrow;


    private Animator animator;
    private float stamina; // Estamina actual
    private float currentCooldown = 0f; // Cooldown restante
    private Vector2 targetDirection; // Dirección del impulso
    private float impulseStrength = 0f; // Fuerza acumulada
    private Rigidbody2D _rb;
    private bool isCharging = false; // Indicador de carga
    private bool isCooldownActive = false; // Indicador del cooldown

    void Start()
    {
        animator = transform.GetChild(1).GetComponent<Animator>();
        animator.runtimeAnimatorController = scriptableData.animatorController;
        _rb = GetComponent<Rigidbody2D>();
        stamina = maxStamina;
        Cursor.visible=false;
	}

    void Update()
    {
        // Actualizar posición del indicador de estamina
        staminaIndicator.transform.position = Input.mousePosition;

        // Obtener la posición del ratón en el mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Mantener en el plano 2D

        if (Input.GetMouseButton(0) && CanImpulse())
        {
            arrow.GetComponent<SpriteRenderer>().color = new Color(
                arrow.GetComponent<SpriteRenderer>().color.r,
                arrow.GetComponent<SpriteRenderer>().color.g,
                arrow.GetComponent<SpriteRenderer>().color.b,
                1); // Asegúrate de que el alpha sea 1

            isCharging = true;

            // Calcular la dirección hacia el ratón
            targetDirection = (mousePosition - transform.position).normalized;

            // Colocar la flecha en la posición del ratón
            arrow.transform.position = mousePosition;

            // Escalar la flecha en Y según la fuerza acumulada
            float currentImpulse = impulseStrength / maxStamina;
            float maxIncrement = Time.deltaTime * impulseCurve.Evaluate(currentImpulse) * velocityMultiplier;
            float actualIncrement = Mathf.Min(maxIncrement, stamina);
            impulseStrength += actualIncrement;
            stamina -= actualIncrement;
            stamina = Mathf.Clamp(stamina, 0, maxStamina);

            float scaleX = Mathf.Clamp01(impulseStrength / maxStamina);
            arrow.transform.localScale = new Vector3(
                scaleX, // Cambiar la escala X
                arrow.transform.localScale.y, // Mantener la escala Y
                arrow.transform.localScale.z  // Mantener la escala Z
            );

            // Rotar la flecha para que apunte hacia la dirección del disparo
            Vector3 direction = targetDirection;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            ApplyImpulse();
        }

        if (!isCharging)
        {
            HandleCooldownAndStamina();
        }

        UpdateStaminaIndicator();
    }
    private void ApplyImpulse()
    {
        GameObject bubblerSprite = transform.GetChild(1).gameObject;
        animator.Play("Push");
        bubblerSprite.transform
            .DOPunchScale(new Vector3(0, .25f, 0), 0.5f, 10, 0.5f)
            .OnComplete(() => {
                // Restablecer la escala original
                bubblerSprite.transform.localScale = new Vector3(.25f, .25f, .25f);
            });

        arrow.GetComponent<SpriteRenderer>().color = new Color
                (arrow.GetComponent<SpriteRenderer>().color.r, arrow.GetComponent<SpriteRenderer>().color.g, arrow.GetComponent<SpriteRenderer>().material.color.b, 0);

        // Obtener la posición del ratón en coordenadas del mundo
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldMousePosition.z = transform.position.z; // Mantener la profundidad del objeto

        // Calcular la dirección del ratón respecto al objeto
        Vector3 direction = (worldMousePosition - transform.position).normalized;

        // Calcular el ángulo que se necesita para alinear el sprite
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotar el sprite para que el ratón esté hacia arriba
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

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
        return !isCooldownActive;
    }

    private void HandleCooldownAndStamina()
    {
        // Reducir cooldown
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0)
        {
            if(isCooldownActive)
            {
                GameObject bubblerSprite = transform.GetChild(1).gameObject;
                bubblerSprite.transform
                .DOPunchScale(new Vector3(0, .25f, 0), 0.5f, 10, 0.5f)
                .OnComplete(() => {
                    // Restablecer la escala original
                    bubblerSprite.transform.localScale = new Vector3(.25f, .25f, .25f);
                });
                ;
                animator.Play("Idle");
            }
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.transform.parent.gameObject);   
    }
}
