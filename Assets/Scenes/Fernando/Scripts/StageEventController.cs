using UnityEngine;

public class StageEventController : MonoBehaviour
{
    public GameObject shenlongPrefab; // Prefab de Shenlong

    [Header("L�mites del Escenario")]
    public Vector2 spawnPositionMin; // L�mite inferior para la posici�n inicial de Shenlong
    public Vector2 spawnPositionMax; // L�mite superior para la posici�n inicial de Shenlong
    public Vector2 targetPositionMin; // L�mite inferior para la posici�n final de Shenlong
    public Vector2 targetPositionMax; // L�mite superior para la posici�n final de Shenlong

    [Header("Intervalo de Aparici�n")]
    public float minSpawnInterval = 10f; // Tiempo m�nimo entre apariciones
    public float maxSpawnInterval = 20f; // Tiempo m�ximo entre apariciones

    private float nextSpawnTime; // Tiempo restante para la pr�xima aparici�n

    void Start()
    {
        // Calcular el tiempo inicial para la primera aparici�n
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        nextSpawnTime -= Time.deltaTime;

        if (nextSpawnTime <= 0f)
        {
            SpawnShenlong();
            // Calcular el tiempo aleatorio para la siguiente aparici�n
            nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    void SpawnShenlong()
    {
        // Crear una posici�n inicial y final aleatoria dentro de los l�mites
        Vector2 startPosition = new Vector2(
            Random.Range(spawnPositionMin.x, spawnPositionMax.x),
            Random.Range(spawnPositionMin.y, spawnPositionMax.y)
        );

        Vector2 endPosition = new Vector2(
            Random.Range(targetPositionMin.x, targetPositionMax.x),
            Random.Range(targetPositionMin.y, targetPositionMax.y)
        );

        // Instanciar a Shenlong
        GameObject shenlong = Instantiate(shenlongPrefab);
        ShenlongController controller = shenlong.GetComponent<ShenlongController>();

        if (controller != null)
        {
            controller.startPosition = startPosition;
            controller.endPosition = endPosition;
        }

        Debug.Log("�Shenlong ha aparecido!");
    }
}
