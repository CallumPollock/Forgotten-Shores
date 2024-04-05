using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Entity activeEntity;
    [SerializeField] float xOffset, yOffset;

    [SerializeField] TextMeshProUGUI entityInfoText;
    [SerializeField] Image healthBar;
    [SerializeField] TextMeshProUGUI healthText;

    float activeTime = 0;


    public void UpdateEntityInfo(Entity entity)
    {
        gameObject.SetActive(true);

        entityInfoText.text = String.Format("<b>{0}</b>\nLevel {1} {2}", entity.data.name, entity.data.level, entity.GetType().ToString());
        activeEntity = entity;

        transform.position = activeEntity.transform.position;

        activeTime = 10f;

        activeEntity.OnHealthModified += UpdateHealthBar;
        activeEntity.OnEntityDied += DeactivateHealthBar;
        UpdateHealthBar(entity.data.health, entity.data.maxHealth);
    }

    void UpdateHealthBar(int health, int maxHealth)
    {
        if (health <= 0) return;

        activeTime = 10f;
        healthBar.fillAmount = (float)health/(float)maxHealth;
        healthText.text = String.Format("{0}/{1}", health, maxHealth);
    }

    void DeactivateHealthBar(Entity _entity)
    {
        activeEntity.OnHealthModified -= UpdateHealthBar;
        activeEntity.OnEntityDied -= DeactivateHealthBar;
        activeEntity = null;

        gameObject.SetActive(false);
    }

    void Update()
    {
        if (activeEntity == null) return;

        Vector3 targetPos= new Vector3(activeEntity.transform.position.x + xOffset, activeEntity.transform.position.y + yOffset, 10f);

        transform.position = Vector3.Lerp(transform.position, targetPos, 7f * Time.deltaTime);

        activeTime -= Time.deltaTime;

        if (activeTime <= 0)
        {
            DeactivateHealthBar(activeEntity);
        }
    }
}
