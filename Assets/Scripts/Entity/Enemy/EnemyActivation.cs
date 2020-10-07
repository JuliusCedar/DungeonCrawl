using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivation : MonoBehaviour
{
    public EnemyStats stats;
    public EnemySight sight;

    public void Activate()
    {
        SetActive(true);
    }

    public void Deactivate()
    {
        SetActive(false);
    }

    private void SetActive(bool b)
    {
        stats.active = b;
        sight.gameObject.SetActive(stats.active);
    }
}
