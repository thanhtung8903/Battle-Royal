using Unity.VisualScripting;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    public float specialCharge = 0f; // Carga actual de la barra (inicia en 0).
    public float maxCharge; // Carga m�xima para activar el ataque especial.

    private bool isReady = false; // Indica si el ataque especial est� listo.
    private UIController UIController; // Referencia al controlador de la UI.

    private void Start()
    {
        UIController = GetComponent<UIController>();
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
       
        Debug.Log("Performing the special attack!");
    }

    private void updateUI()
    {
        UIController.updateSpecialBar(specialCharge, maxCharge);
    }

    public void setMaxCharge(float maxChargeFromPersonaje)
    {
        this.maxCharge = maxChargeFromPersonaje;
    }
}
