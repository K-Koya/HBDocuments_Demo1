using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>
/// 敵の照準による動作コンポーネント
/// </summary>
public class AimSystemForMobs : AimSystem
{
    // Start is called before the first frame update
    void Start()
    {
        TimelineInit();
        area = GetComponent<Collider>();
        area.enabled = false;
        status = GetComponentInParent<Status>();
        hitableAttackLayer = (byte)LayerMask.NameToLayer(hitableAttackLayerName);
        searchableItemLayer = (byte)LayerMask.NameToLayer(searchableItemLayerName);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPausing) return;
    }
}
