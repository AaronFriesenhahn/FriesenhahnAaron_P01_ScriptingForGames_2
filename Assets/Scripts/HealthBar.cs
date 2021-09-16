using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider _healthSlider = null;

    public Health Health { get; private set; }
    private Color originalColor;

    bool ColorSwapping = false;

    private void Awake()
    {
        Health = GetComponent<Health>();

        _healthSlider.maxValue = Health._maxHealth;
        _healthSlider.value = Health._maxHealth;
        originalColor = _healthSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color;
    }

    private void OnEnable()
    {
        Health.Damaged += OnTakeDamage;
        Health.Healed += OnHealed;
        Health.HealthAt50Percent += HalfHealth;
    }

    private void OnDisable()
    {
        Health.Damaged -= OnTakeDamage;
        Health.Healed -= OnHealed;
    }

    void OnTakeDamage(int damage)
    {
        _healthSlider.value = Health._currentHealth;
    }

    void OnHealed(int value)
    {
        _healthSlider.value = Health._currentHealth;
    }

    void HalfHealth()
    {
        if (ColorSwapping == false)
        {
            ColorSwapping = true;
            StartCoroutine(ColorSwap());
        }
        
    }

    IEnumerator ColorSwap()
    {
        Color color = new Color(255, 255, 255);
        _healthSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = color;
        yield return new WaitForSeconds(.5f);
        _healthSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = originalColor;
        yield return new WaitForSeconds(.5f);
        ColorSwapping = false;
    }
}
