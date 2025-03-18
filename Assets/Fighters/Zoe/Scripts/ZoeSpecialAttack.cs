using System.Collections;
using UnityEngine;

public class ZoeSpecialAttack : MonoBehaviour
{
    public float specialCharge = 0f; 
    public float maxCharge; 

    private bool isReady = false; 
    private UIController UIController; 

    public float specialPowerDamage;
    public float specialPowerDamageToShield;
    public Animator animator; 
    private ZoeAttack attack;

    
    public GameObject ballPrefab; 
    public int ballCount = 20; 
    public float expansionSpeed = 5f; 
    public int bursts = 3; 
    public float timeBetweenBursts = 0.5f; 

    [SerializeField] private AudioClip soundSpecialAttack1;

    private void Start()
    {
        UIController = GetComponent<UIController>();
        attack = GetComponent<ZoeAttack>(); 
        updateUI();
    }

    
    public void increaseCharge(float amount)
    {
        if (!isReady) 
        {
            specialCharge += amount;
            specialCharge = Mathf.Clamp(specialCharge, 0, maxCharge); 

            if (specialCharge >= maxCharge) 
            {
                isReady = true;
                Debug.Log("Special Attack Ready!");
            }

            updateUI();
        }
    }

    
    public void useSpecialAttack()
    {
        if (isReady) 
        {
            Debug.Log("Special Attack Activated!");
            performSpecialAttack();
            specialCharge = 0f;
            isReady = false;
            updateUI();
        }
    }

    private void performSpecialAttack()
    {
        special();
        Debug.Log("Performing the special attack!");
        
    }

    private void special()
    {
        // Activa la animación de ataque
        SoundsController.Instance.RunSound(soundSpecialAttack1);
        animator.SetTrigger("special");
        attack.applyDamageToEnemies(specialPowerDamage, specialPowerDamageToShield);
    }

    
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
            
            float angle = i * angleIncrement * Mathf.Deg2Rad;

            
            Vector2 initialPosition = transform.position;

            
            Vector2 radialDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            
            GameObject ball = Instantiate(ballPrefab, initialPosition, Quaternion.identity);

            
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
