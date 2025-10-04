using TMPro; 
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public delegate void DeathEventHandler();
public class AttributesComponent : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] TMP_Text healthText;
    public int maxHealth = 3;
    public bool bCanDie = true;
    public bool bCanTakeDamage = true; // for future use such as imortality or respawn safe time 
    private int currentHealth = 0;
    public bool bIsDead = false;
    private Animator anim;

    public event DeathEventHandler OnDeath;




    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        anim = GetComponent<Animator>();
    }

    public void ApplyDamage(int damageAmount)
    {
        if (bCanTakeDamage)
        {
            currentHealth = Mathf.Clamp(currentHealth - damageAmount, 0, maxHealth);
            UpdateHealthUI();

            if (!gameObject.CompareTag("Player")) Debug.Log("enemy hit");

            anim.SetTrigger("Attacked");

            if (currentHealth <= 0)
            {
                Death();
            }
        }
    }

    // TODO Move death logic to another place
    public void Death()
    {
        if (bCanDie)
        {
            currentHealth = 0;
            Debug.Log("Death!");
            bCanTakeDamage = false;
            bIsDead = true;
            OnDeath?.Invoke();

            if (anim != null)
            {
                anim.SetBool("IsDead", true);
                StartCoroutine(DestroyObjAfterAnimCoroutine());
            }
        }
    }

    IEnumerator DestroyObjAfterAnimCoroutine()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + 2f);

        if (!gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText == null) return;
        healthText.text = currentHealth.ToString();
    }

    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth);
        UpdateHealthUI(); 
    }

}
