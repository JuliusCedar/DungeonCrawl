using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
};

public abstract class EntityStats : MonoBehaviour
{
    public AudioSource hitSound;

    public Stat maxHealth;
    public int currentHealth;

    public Stat defense;
    public Stat physicalAttack;

    public Direction direction;

    public bool stunned;

    protected Coroutine kbRoutine;

    private void Awake()
    {
        currentHealth = maxHealth.GetValue();
        stunned = false;
    }

    public void TakeDamage(int damage, GameObject attacker)
    {
        hitSound.Play();

        damage -= defense.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;

        MarkerGenerator.instance.GenerateTextMarker(gameObject,new Vector3(0,1,0), (-damage).ToString(), Color.red);

        if (currentHealth <= 0)
        {
            Die();
        }

        OnDamageTaken(attacker);

    }

    public abstract void Die();

    public abstract void OnDamageTaken(GameObject source);
}
