using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    /// 範囲コライダー
    /// </summary>
    protected Collider area = default;

    /// <summary>
    /// 攻撃情報
    /// </summary>
    protected AttackInfo info = default;


    /// <summary>
    /// 照準しているオブジェクトのコライダ集
    /// </summary>
    protected List<Collider> aimings = new List<Collider>();
    /// <summary>
    /// 照準しているオブジェクトが持っている照準コマンド情報
    /// </summary>
    protected AimCommandReceiver aimCommand = default;


    /* プロパティ */
    public Collider Area { get => area; set => area = value; }
    public AttackInfo Info { get => info; set => info = value; }

    void Start()
    {
        TimelineInit();
        area = GetComponent<Collider>();
        hitableAttackLayer = (byte)LayerMask.NameToLayer(hitableAttackLayerName);
        searchableItemLayer = (byte)LayerMask.NameToLayer(searchableItemLayerName);
    }

    void Update()
    {
        if (IsPausing) return;
    }

    /// <summary>
    /// 照準を合わせた時のコマンドを、照準したコライダのうち最も近いオブジェクトから取得する
    /// </summary>
    void PickAimCommand()
    {
        if (aimings.Count > 0)
        {
            //近い順に並び替えをして、先頭よりGetComponent
            aimings = aimings.OrderBy(a => Vector3.SqrMagnitude(a.transform.position - transform.position)).ToList();
            aimCommand = aimings.First().GetComponent<AimCommandReceiver>();
        }
        //照準内に該当のコライダがなければ離脱
        else aimCommand = null;
    }


    protected void OnTriggerEnter(Collider other)
    {
        //コライダ範囲内のものを受け取る
        if(!aimings.Contains(other))
        {
            aimings.Add(other);
            PickAimCommand();
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        //コライダ範囲外に出たものを取り除く
        if (aimings.Contains(other))
        {
            aimings.Remove(other);
            PickAimCommand();
        }
    }
}

