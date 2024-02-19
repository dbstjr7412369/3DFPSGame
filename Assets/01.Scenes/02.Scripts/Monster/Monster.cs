using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour, IHitable
{
    [Range(0, 100)]
    public int Health;
    public int MaxHealth = 100;
    public Slider HealthSliderUI;

    public void Hit(int damage)
    {
        Health -= damage;
    }

    private void Update()
    {
        HealthSliderUI.value = (float)Health / (float)MaxHealth;  // 0 ~ 1
    }

    private void Start()
    {
        Init();
    }



    public void Init()
    {
        Health = MaxHealth;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (Health <= 0)
        {
            Destroy(this.gameObject);
        }
    }




}