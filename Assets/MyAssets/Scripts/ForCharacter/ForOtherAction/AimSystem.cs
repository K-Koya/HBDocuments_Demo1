using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;



/// <summary>
/// 照準による動作コンポーネント
/// </summary>
[RequireComponent(typeof(Collider))]
public class AimSystem : MyMonoBehaviour
{
    /// <summary>
    /// 自身の攻撃が当たるレイヤー名
    /// </summary>
    static protected string hitableAttackLayerName = "Enemy";
    /// <summary>
    /// 自身の攻撃が当たるレイヤー番号
    /// </summary>
    protected byte hitableAttackLayer = 0;

    /// <summary>
    /// 自身が調べたり使用したりできるアイテムのレイヤー名
    /// </summary>
    static protected string searchableItemLayerName = "Item";
    /// <summary>
    /// 自身が調べたり使用したりできるアイテムのレイヤー番号
    /// </summary>
    protected byte searchableItemLayer = 0;

    /// <summary>
    /// プレイヤーのステータス
    /// </summary>
    protected Status status = default;

    /// <summary>
    /// 範囲コライダー
    /// </summary>
    protected Collider area = default;

    /// <summary>
    /// 攻撃情報
    /// </summary>
    protected AttackInfo info = default;


    /* プロパティ */
    public Collider Area { get => area; set => area = value; }
    public AttackInfo Info { get => info; set => info = value; }
    public Status Status { get => status; set => status = value; }

    void Start()
    {
        TimelineInit();
        area = GetComponent<Collider>();
        area.enabled = false;
        status = GetComponentInParent<Status>();
        hitableAttackLayer = (byte)LayerMask.NameToLayer(hitableAttackLayerName);
        searchableItemLayer = (byte)LayerMask.NameToLayer(searchableItemLayerName);
    }

    void Update()
    {
        if (IsPausing) return;
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

