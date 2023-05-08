using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffManager : MonoBehaviour
{
    public string weaponName = "staff";
    [SerializeField] float damage;
    public bool isHittingEnemy = false;

    public ChomperManager enemyHit;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            //Deal damage to enemy hit by staff
            enemyHit = other.gameObject.GetComponent<ChomperManager>();
            GameManager.instance.player.SetCurrentTarget(other.gameObject.name);
            isHittingEnemy = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isHittingEnemy)
        {
            isHittingEnemy = false;
        }
    }

    public ChomperManager GetEnemyHit()
    {
        return enemyHit;
    }
}
