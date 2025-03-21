using UnityEngine;

public class StageEventController : MonoBehaviour
{
    public GameObject shenlongPrefab; // Prefab của Shenlong

    [Header("Giới hạn của map")]
    public Vector2 spawnPositionMin; // Giới hạn dưới cho vị trí ban đầu của Shenlong
    public Vector2 spawnPositionMax; // Giới hạn trên cho vị trí ban đầu của Shenlong
    public Vector2 targetPositionMin; // Giới hạn dưới cho vị trí cuối cùng của Shenlong
    public Vector2 targetPositionMax; // Giới hạn trên cho vị trí cuối cùng của Shenlong

    [Header("Khoảng thời gian xuất hiện")]
    public float minSpawnInterval = 10f; // Thời gian tối thiểu giữa các lần xuất hiện
    public float maxSpawnInterval = 20f; // Thời gian tối đa giữa các lần xuất hiện

    private float nextSpawnTime; // Thời gian còn lại cho lần xuất hiện tiếp theo

    void Start()
    {
        // Tính toán thời gian ban đầu cho lần xuất hiện đầu tiên
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        nextSpawnTime -= Time.deltaTime;

        if (nextSpawnTime <= 0f)
        {
            SpawnShenlong();
            // Tính toán thời gian ngẫu nhiên cho lần xuất hiện tiếp theo
            nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    // Tạo Shenlong
    void SpawnShenlong()
    {
        // Tạo vị trí ban đầu và cuối cùng ngẫu nhiên trong giới hạn
        Vector2 startPosition = new Vector2(
            Random.Range(spawnPositionMin.x, spawnPositionMax.x),
            Random.Range(spawnPositionMin.y, spawnPositionMax.y)
        );

        Vector2 endPosition = new Vector2(
            Random.Range(targetPositionMin.x, targetPositionMax.x),
            Random.Range(targetPositionMin.y, targetPositionMax.y)
        );

        // Khởi tạo Shenlong
        GameObject shenlong = Instantiate(shenlongPrefab);
        ShenlongController controller = shenlong.GetComponent<ShenlongController>();

        if (controller != null)
        {
            controller.startPosition = startPosition;
            controller.endPosition = endPosition;
        }

        Debug.Log("Shenlong đã xuất hiện!");
    }
}
