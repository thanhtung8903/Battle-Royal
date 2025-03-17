using UnityEngine;

public class ShenlongController : MonoBehaviour
{
    public float speed = 5f; // Velocidad de vuelo
    public float damage = 25f; // Da�o infligido al jugador
    public Vector2 startPosition; // Posici�n inicial aleatoria
    public Vector2 endPosition; // Posici�n final aleatoria
    public float destroyTime = 10f; // Tiempo para destruir a Shenlong si no llega al final

    private bool isFlying = false;

    void Start()
    {
        // Configurar posici�n inicial
        transform.position = startPosition;
        isFlying = true;

        // Destruir autom�ticamente despu�s de un tiempo
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        if (isFlying)
        {
            // Mover a Shenlong hacia la posici�n final
            transform.position = Vector2.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);

           
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar si golpea a un jugador
        Damageable playerHealth = collision.GetComponent<Damageable>();
        if (playerHealth != null)
        {
            playerHealth.decreaseLife(damage); // Infligir da�o al jugador
        }
    }
}
