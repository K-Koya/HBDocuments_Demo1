using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>
/// 照準による動作コンポーネント
/// </summary>
[RequireComponent(typeof(Collider))]
public class AimSystemForPlayer : AimSystem
{
    /// <summary>
    /// プレイヤー向けの照準位置
    /// </summary>
    AimMovement playersAim = default;
    

    void Start()
    {
        TimelineInit();
        area = GetComponent<Collider>();
        area.enabled = false;
        playersAim = FindObjectOfType<AimMovement>();
        status = GetComponentInParent<Status>();
        hitableAttackLayer = (byte)LayerMask.NameToLayer(hitableAttackLayerName);
        searchableItemLayer = (byte)LayerMask.NameToLayer(searchableItemLayerName);
    }

    void Update()
    {
        if (IsPausing) return;
        Vector3 direction = playersAim.transform.position - status.transform.position;
        if(direction.sqrMagnitude > Mathf.Pow(status.ComboCommonProximityRange, 2f))
            direction = direction.normalized * status.ComboCommonProximityRange;
        transform.position = status.transform.position + direction;
    }


    void OnTriggerStay(Collider other)
    {
        //調べたり使用したりできるアイテムに絞る
        if (other.gameObject.layer == searchableItemLayer)
        {

        }

        //自身の攻撃が当たるものに絞る
        if (other.gameObject.layer == hitableAttackLayer)
        {

        }
    }

    void OnTriggerEnter(Collider other)
    {
        
    }
}

