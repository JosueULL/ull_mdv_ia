using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float MaxHealth;
    public Image ImageFill;
    
    [HideInInspector]
    public float CurrentHealth;

    public void Start()
    {
        CurrentHealth = MaxHealth;
        Change(0);
    }

    public void Damage(float amount)
    {
        Change(-amount);
    }

    public void Regenerate(float amount)
    {
        Change(amount);
    }

    private void Change(float amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
        ImageFill.fillAmount = CurrentHealth / MaxHealth;
    }
}
