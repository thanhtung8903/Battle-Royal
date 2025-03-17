using System;
using UnityEngine;

public class KitsuneAttack : MonoBehaviour
{

    public Animator animator; // Referencia al Animator para reproducir animaciones de ataque
    public Transform weaponHitBox; // Posici�n donde se verificar� el impacto de las armas
    public float attackRange; // Rango en el que se pueden detectar jugadores enemigos

    // Valores de da�o para diferentes ataques
    public float hitDamage;
    public float kickDamage;
    public float specialPowerDamage;

    public float hitDamageToShield;
    public float kickDamageToShield;

    public float attackRate = 1f; // Tasa de ataque: n�mero de ataques por segundo permitidos
    public float waitingTimeHit; // Tiempo de espera entre golpes
    public float waitingTimeKick; // Tiempo de espera entre patadas
    private float nexAttackTime = 0f; // Acumulador del tiempo de espera para el pr�ximo ataque

    //public KeyCode hitKey;
    //public KeyCode kickKey;
    //public KeyCode specialPowerKey;

    private KitsuneSpecialAttack specialAttack;
    private UserConfiguration userConfiguration;

    // Atributos para sonidos
    [SerializeField] private AudioClip soundAttack1;

    string ownTag;

    void Start()
    {
        specialAttack = GetComponent<KitsuneSpecialAttack>(); //CAMBIAR por Kitsune
        animator = GetComponent<Animator>();
        userConfiguration = GetComponent<UserConfiguration>();
        ownTag = gameObject.tag;
        //otherPlayer = LayerMask.GetMask("BaseFighter");
    }

    // Update se llama una vez por cuadro
    void Update()
    {
        // Solo permite ataques si ha pasado suficiente tiempo desde el �ltimo ataque
        if (Time.time >= nexAttackTime)
        {
            // Si se presiona la tecla correspondiente, realiza un golpe
            if (Input.GetKeyDown(userConfiguration.getHitKey()))
            {
                hit();
                SoundsController.Instance.RunSound(soundAttack1);
                nexAttackTime = Time.time + waitingTimeHit / attackRate;
            }
            // Si se presiona la tecla correspondiente, realiza una patada
            else if (Input.GetKeyDown(userConfiguration.getKickKey()))
            {
                kick();
                nexAttackTime = Time.time + waitingTimeKick / attackRate;
            }
            // Si se presiona la tecla correspondiente, activa el poder especial
            else if (Input.GetKeyDown(userConfiguration.getSpecialPowerKey()))
            {
                specialAttack.useSpecialAttack();
            }
        }
    }

    // M�todo para realizar el golpe
    void hit()
    {
        animator.SetTrigger("attack1"); // Activa la animaci�n de ataque
        applyDamageToEnemies(hitDamage, hitDamageToShield); // Aplica da�o a los enemigos detectados
    }

    // M�todo para realizar la patada
    private void kick()
    {
        // Activa la animaci�n de ataque
        animator.SetTrigger("attack2"); // DEBER�A SER DIFRENTE PARA LA ANIMACI�N DE KICK
        applyDamageToEnemies(kickDamage, kickDamageToShield);
    }



    // M�todo que aplica da�o a los enemigos detectados
    private void applyDamageToEnemies(float damage, float damageToShield)
    {
        // Detecta jugadores enemigos dentro del �rea del "weaponHitBox"
        //Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange, otherPlayer);
        //Collider2D[] hitOtherPlayers = Physics2D.OverlapCapsuleAll(weaponHitBox.position, attackRange, )
        Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange);


        // Aplica da�o a cada enemigo detectado
        foreach (Collider2D playerEnemy in hitOtherPlayers)
        {

            //var health = playerEnemy.GetComponent<Health>();
            //var shield = playerEnemy.GetComponent<Shield>();
            Damageable damageable = playerEnemy.GetComponent<Damageable>();
            Shieldable shieldable = playerEnemy.GetComponent<Shieldable>();

            if (damageable != null && gameObject.tag != playerEnemy.tag)
            {
                if (shieldable == null || !shieldable.IsShieldActive())
                {
                    damageable.decreaseLife(damage);
                    Debug.Log("We hit " + playerEnemy.name);
                    // Cargar barra de ataque especial con cada golpe acertado
                    specialAttack.increaseCharge(damage);
                }
                else
                {
                    shieldable.decreaseShieldCapacity(damageToShield);
                }
            }

        }
    }


    // M�todo necesario para usar hijos del GameObject en el editor
    private void OnValidate()
    {
        if (weaponHitBox == null)
        {
            weaponHitBox = transform.Find("WeaponHitBox");
            if (weaponHitBox == null)
            {
                Debug.LogWarning("WeaponHitBox not found. Ensure there is a child GameObject named 'WeaponHitBox'.");
            }
        }
    }

    // Dibuja un Gizmo para visualizar el �rea de ataque en la escena
    private void OnDrawGizmosSelected()
    {
        if (weaponHitBox == null)
        {
            return;
        }
        Gizmos.color = Color.red; // Color del Gizmo
        Gizmos.DrawWireSphere(weaponHitBox.position, attackRange); // �rea circular del rango de ataque
    }

}
