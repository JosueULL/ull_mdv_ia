using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float MaxHealth;
    public Image ImageFill;
    public float AutoHealInterval = 1f;
    public float AutoHealAmount = 0.05f;

    [HideInInspector]
    public float CurrentHealth;

    public void Start()
    {
        CurrentHealth = MaxHealth;
        Change(0);

        if (AutoHealAmount > 0f)
            InvokeRepeating("AutoHeal", 0f, AutoHealInterval);
    }

    private void AutoHeal()
    {
        Regenerate(AutoHealAmount);
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
