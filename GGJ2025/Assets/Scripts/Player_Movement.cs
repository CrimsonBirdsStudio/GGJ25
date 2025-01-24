using UnityEngine;
using UnityEngine.UI;

public class Player_Movement : MonoBehaviour
{
    public float vel,stamina,staminaRefill,maxStamina;
    public Image staminaIndicator;

    Vector2 _targetPos;
    Rigidbody2D _rb;
    bool _charging;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        stamina = maxStamina;
    }

    void Update()
    {
    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            _targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _rb.AddForce(_targetPos * vel);
            StaminaIndicator(false);
        }
        else
        {
            StaminaIndicator(true);
        }
        staminaIndicator.transform.position = Input.mousePosition;        
    }
    void StaminaIndicator(bool up)
    {
        int direction = up ? 1 : -2;
        if (!up && stamina < maxStamina)
        {
            stamina += (staminaRefill * direction) * Time.deltaTime;
        }
        else
        {
            stamina += (staminaRefill * direction) * Time.deltaTime;
        }
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        staminaIndicator.fillAmount = stamina / maxStamina;
    }
}
