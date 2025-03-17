using System.Collections;
using UnityEngine;

public class ZoeSpecialAttack : MonoBehaviour
{
    public float specialCharge = 0f; // Carga actual de la barra (inicia en 0).
    public float maxCharge; // Carga m�xima para activar el ataque especial.

    private bool isReady = false; // Indica si el ataque especial est� listo.
    private UIController UIController; // Referencia al controlador de la UI.

    public float specialPowerDamage;
    public float specialPowerDamageToShield;
    public Animator animator; // Referencia al Animator para reproducir animaciones de ataque
    private ZoeAttack attack;

    // Configuraci�n para el poder especial con las bolas
    public GameObject ballPrefab; // Prefab de la bola
    public int ballCount = 20; // N�mero de bolas en el c�rculo
    public float expansionSpeed = 5f; // Velocidad de expansi�n del c�rculo
    public int bursts = 3; // N�mero de r�fagas
    public float timeBetweenBursts = 0.5f; // Tiempo entre r�fagas

    [SerializeField] private AudioClip soundSpecialAttack1;

    private void Start()
    {
        UIController = GetComponent<UIController>();
        attack = GetComponent<ZoeAttack>(); // Inicializar attack.
        updateUI();
    }

    // M�todo que aumenta la barra de carga.
    public void increaseCharge(float amount)
    {
        if (!isReady) // Si el ataque especial no est� listo, cargar la barra.
        {
            specialCharge += amount;
            specialCharge = Mathf.Clamp(specialCharge, 0, maxCharge); // Asegurarse de que no pase de 100.

            if (specialCharge >= maxCharge) // Si la barra est� llena, marcar como listo.
            {
                isReady = true;
                Debug.Log("Special Attack Ready!");
            }

            updateUI();
        }
    }

    // M�todo para usar el ataque especial.
    public void useSpecialAttack()
    {
        if (isReady) // Solo se puede usar si est� completamente cargada.
        {
            Debug.Log("Special Attack Activated!");
            performSpecialAttack(); // Aqu� colocas la l�gica del ataque especial.
            specialCharge = 0f; // Reiniciar la barra.
            isReady = false;
            updateUI();
        }
    }

    private void performSpecialAttack()
    {
        special();
        Debug.Log("Performing the special attack!");
        // La r�faga ser� activada por un Animation Event al final de la animaci�n especial.
    }

    private void special()
    {
        // Activa la animaci�n de ataque
        SoundsController.Instance.RunSound(soundSpecialAttack1);
        animator.SetTrigger("special");
        attack.applyDamageToEnemies(specialPowerDamage, specialPowerDamageToShield);
    }

    // Este m�todo se llamar� al finalizar la animaci�n "special" usando un Animation Event.
    private void OnSpecialAnimationEnd()
    {
        StartCoroutine(GenerateBursts());
    }

    private IEnumerator GenerateBursts()
    {
        animator.SetTrigger("balls");
        for (int i = 0; i < bursts; i++)
        {
            GenerateCircle();
            yield return new WaitForSeconds(timeBetweenBursts);
        }
    }

    private void GenerateCircle()
    {
        float angleIncrement = 360f / ballCount;

        for (int i = 0; i < ballCount; i++)
        {
            // Calcula el �ngulo de cada bola
            float angle = i * angleIncrement * Mathf.Deg2Rad;

            // Genera la posici�n inicial en el centro del personaje
            Vector2 initialPosition = transform.position;

            // Crea la direcci�n radial basada en el �ngulo
            Vector2 radialDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            // Instancia la bola en el centro del personaje
            GameObject ball = Instantiate(ballPrefab, initialPosition, Quaternion.identity);

            // Inicializa la bola para que se mueva radialmente hacia afuera
            ball.GetComponent<BallMovement>().setUserTag(gameObject.tag);
            ball.GetComponent<BallMovement>().Initialize(radialDirection, expansionSpeed);
        }
    }

    private void updateUI()
    {
        UIController.updateSpecialBar(specialCharge, maxCharge);
    }

    public void setMaxCharge(float maxChargeFromCharacter)
    {
        this.maxCharge = maxChargeFromCharacter;
    }
}
