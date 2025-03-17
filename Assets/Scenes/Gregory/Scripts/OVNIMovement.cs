using UnityEngine;

public class OVNIMovement : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float progress = 0f;  // Progreso del movimiento (0 a 1)
    [SerializeField] private AudioClip movementSound;

    private void Start()
    {
        SoundsController.Instance.RunSound(movementSound);
    }

    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime); // Rotar el objeto
        progress += Time.deltaTime * speed; // Incrementar el progreso seg�n la velocidad y el tiempo
        transform.position = Vector3.Lerp(startPosition, endPosition, progress);  // Mover el objeto entre la posici�n inicial y final
        
        if (progress < 1f)
        {
            return;
        }
        // Detener el movimiento cuando llega a la posici�n final
        progress = 1f; // Asegurarse de que no exceda el rango
        Destroy(gameObject);// Destruir el objeto al llegar al final
    }

    public void setStartPosition(Vector3 startPosition)
    {
        this.startPosition = startPosition;
    }

    public void setEndPosition(Vector3 endPosition)
    {
        this.endPosition = endPosition;
    }
}
