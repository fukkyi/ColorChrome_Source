using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackCollider : MonoBehaviour
{
    public bool awakeEnable = true;
    public int damageValue = 10;

    protected Collider attackCollider = null;
    protected List<GameObject> damagedObjectList = new List<GameObject>();

    protected void Awake()
    {
        attackCollider = GetComponent<Collider>();
        attackCollider.enabled = awakeEnable;
    }

    protected void OnTriggerEnter(Collider collider)
    {
        CollisionManage(collider);
    }

    protected virtual void CollisionManage(Collider collider) {}

    public void EnableCollider()
    {
        if (attackCollider.enabled) return;

        attackCollider.enabled = true;
    }

    public void DisableCollider()
    {
        if (!attackCollider.enabled) return;

        attackCollider.enabled = false;
        damagedObjectList.Clear();
    }
}
