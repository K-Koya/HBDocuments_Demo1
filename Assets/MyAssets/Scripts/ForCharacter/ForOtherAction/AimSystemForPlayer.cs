using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;



/// <summary>
/// 照準による動作コンポーネント
/// </summary>
[RequireComponent(typeof(Collider))]
public class AimSystemForPlayer : MyMonoBehaviour
{
    /// <summary>
    /// 自身の攻撃が当たるレイヤー名
    /// </summary>
    static string hitableAttackLayerName = "Enemy";
    /// <summary>
    /// 自身の攻撃が当たるレイヤー番号
    /// </summary>
    byte hitableAttackLayer = 0;

    /// <summary>
    /// 自身が調べたり使用したりできるアイテムのレイヤー名
    /// </summary>
    static string searchableItemLayerName = "Item";
    /// <summary>
    /// 自身が調べたり使用したりできるアイテムのレイヤー番号
    /// </summary>
    byte searchableItemLayer = 0;

    /// <summary>
    /// プレイヤーのステータス
    /// </summary>
    Status status = default;
    /// <summary>
    /// プレイヤー向けの照準位置
    /// </summary>
    AimMovement playersAim = default;

    /// <summary>
    /// 範囲コライダー
    /// </summary>
    Collider area = default;

    /// <summary>
    /// 攻撃情報
    /// </summary>
    AttackInfo info = default;


    /* プロパティ */
    public Collider Area { get => area; set => area = value; }
    public AttackInfo Info { get => info; set => info = value; }
    

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

        Vector3 direction = Vector3.Normalize(playersAim.transform.position - status.transform.position);
        transform.position = direction * 3.0f;
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

