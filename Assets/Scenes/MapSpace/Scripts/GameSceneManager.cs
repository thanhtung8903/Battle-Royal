using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject ovniToSpawn; // El objeto que aparecer�
    [SerializeField] private Vector2 spawnAreaMin;    // Esquina inferior izquierda del �rea de aparici�n
    [SerializeField] private Vector2 spawnAreaMax;    // Esquina superior derecha del �rea de aparici�n
    [SerializeField] private float nextAppearanceTime; // Acumulador de tiempo para la pr�xima aparici�n
    [SerializeField] private float spawnInterval; // Intervalo de tiempo entre apariciones
    [SerializeField] private bool isChangedX = false;

    [Header("Boundires Manage Settings")]
    [SerializeField] private const float totalDamage = 100;

    [Header("Audio Settings")]
    //[SerializeField] private AudioClip backgroundSound;
    [SerializeField] private AudioClip fightSound;

    // ---------------------------- Audio Background Music ----------------------------
    //private void Start()
    //{
    //    // Asegurarse de que la m�sica de fondo se inicie o se reanude
    //    SoundsController.Instance.StartBackgroundMusic(); // Esto asegura que la m�sica de fondo empiece a sonar
    //    SoundsController.Instance.RunSound(fightSound);  // Esto reproduce efectos de sonido si es necesario
    //}

    // ---------------------------- Spawn OVNI ----------------------------
    void spawnObject()
    {
        float randomX = isChangedX? Random.Range(spawnAreaMin.x, spawnAreaMax.x) : spawnAreaMin.x;
        float randomY = !isChangedX? Random.Range(spawnAreaMin.y, spawnAreaMax.y) : spawnAreaMin.y;

        Vector3 startPosition = new Vector3(randomX, randomY, -1f);
        Vector3 endPosition = new Vector3(-randomX, -randomY, -1f);

        ovniToSpawn.GetComponent<OVNIMovement>().setStartPosition(startPosition);
        ovniToSpawn.GetComponent<OVNIMovement>().setEndPosition(endPosition);

        Instantiate(ovniToSpawn, startPosition, Quaternion.identity);

        if (!isChangedX) {
            spawnAreaMax.x = -spawnAreaMax.x;
            spawnAreaMin.x = -spawnAreaMin.x;
            isChangedX = true;
            return;
        }
        spawnAreaMax.y = -spawnAreaMax.y;
        spawnAreaMin.y = -spawnAreaMin.y;
        isChangedX = false;
    }


    void Update()
    {
        if (Time.time < nextAppearanceTime) // Si no ha pasado el tiempo de aparici�n
        {
            return;
        }
        nextAppearanceTime = Time.time + spawnInterval;
        spawnObject();
    }


    // ---------------------------- Manage Boundaries Attack ----------------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("BaseFighter"))
        {
            return;
        }

        if (collision.gameObject.name != "Shield")
        {
            // Si no es un escudo, buscar Damageable en el objeto colisionado
            Damageable damageable = collision.gameObject.GetComponent<Damageable>();
            if (damageable == null)
            {
                return;
            }
            damageable.decreaseLife(totalDamage);
            return;
        }

        // Acceder al objeto padre
        Transform parentTransform = collision.gameObject.transform.parent;
        if (parentTransform == null)
        {
            return;
        }
        Shieldable shieldableParent = parentTransform.GetComponent<Shieldable>();
        
        if (shieldableParent == null)
        {
            return;
        }
        shieldableParent.decreaseShieldCapacity(totalDamage);
    }
}
