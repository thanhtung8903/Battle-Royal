using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Vector2 direction; // Direcci�n en la que la bola se expandir�
    private float expansionSpeed;
    private string userTag;

    public float specialPowerDamage;
    public float specialPowerDamageToShield;

    public void Initialize(Vector2 dir, float speed)
    {
        direction = dir.normalized; // Asegura que la direcci�n est� normalizada
        expansionSpeed = speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(userTag))
        {
            Debug.Log(userTag);
            Debug.Log(other.gameObject.tag);
            return;
        }

        Damageable damageable = other.gameObject.GetComponent<Damageable>();
        Shieldable shield = other.gameObject.GetComponent<Shieldable>();

        if (damageable != null)
        {
            if (shield == null || !shield.IsShieldActive())
            {
                damageable.decreaseLife(specialPowerDamage);
            }
            else
            {
                shield.decreaseShieldCapacity(specialPowerDamageToShield);
            }
        }
    }

    public void setUserTag(string userTag)
    {
        this.userTag = userTag;
    }

    void Update()
    {
        // Mueve la bola en la direcci�n calculada
        transform.position += (Vector3)direction * expansionSpeed * Time.deltaTime;

        // Destruye la bola si est� fuera de los l�mites de la pantalla
        if (IsOutOfScreen())
        {
            Destroy(gameObject);
        }
    }

    private bool IsOutOfScreen()
    {
        Vector3 screenPosition = Camera.main.WorldToViewportPoint(transform.position);

        // Comprueba si la bola est� fuera del rango visible
        return screenPosition.x < 0 || screenPosition.x > 1 ||
               screenPosition.y < 0 || screenPosition.y > 1;
    }
}