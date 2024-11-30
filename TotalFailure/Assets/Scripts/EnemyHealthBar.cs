using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Vector3 offset;
   

    private void Update()
    {
        healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);

    }

    public void SetHealthValue(double current_health, double max_health)
    {
        healthBar.gameObject.SetActive(current_health < max_health);
        healthBar.value = (float)current_health;
        healthBar.maxValue = (float)max_health;
    }

}
