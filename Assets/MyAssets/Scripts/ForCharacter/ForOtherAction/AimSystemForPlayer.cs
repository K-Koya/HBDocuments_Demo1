using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 照準による動作コンポーネント
/// </summary>
public class AimSystemForPlayer : AimSystem
{
    /// <summary>
    /// プレイヤー向けの照準位置
    /// </summary>
    AimMovement playersAim = default;

    /// <summary>
    /// 入力情報を持つコンポーネント
    /// </summary>
    MyInputManager input = default;

    void Start()
    {
        TimelineInit();
        area = GetComponent<Collider>();
        playersAim = FindObjectOfType<AimMovement>();
        input = FindObjectOfType<MyInputManager>();
        hitableAttackLayer = (byte)LayerMask.NameToLayer(hitableAttackLayerName);
        searchableItemLayer = (byte)LayerMask.NameToLayer(searchableItemLayerName);
    }

    void Update()
    {
        if (IsPausing) return;
        Status status = playersAim.Status;
        Vector3 direction = playersAim.transform.position - status.transform.position;
        if(direction.sqrMagnitude > Mathf.Pow(status.ComboCommonProximityRange, 2f))
            direction = direction.normalized * status.ComboCommonProximityRange;
        transform.position = status.transform.position + direction;

        playersAim.CommandName = aimCommand?.CommandName;

        if(aimCommand && input.Guard.NowPushDown != PushType.noPush)
        {
            aimCommand.RunReaction();
        }
    }
}

