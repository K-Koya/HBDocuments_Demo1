using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>
/// 接触した攻撃情報を受け取り、自身に反映させる
/// </summary>
[RequireComponent(typeof(Status))]
[RequireComponent(typeof(Collider))]
public class Damager : MyMonoBehaviour
{
    /// <summary>
    /// キャラクターのステータス
    /// </summary>
    Status status = default;


    /// <summary>
    /// 攻撃を受けた時のエフェクト:一般
    /// </summary>
    [SerializeField, Tooltip("攻撃を受けた時のパーティクルを乗せたオブジェクトを設定:一般")]
    GameObject hitEffectNormal = default;



    // Start is called before the first frame update
    void Start()
    {
        TimelineInit();
        status = GetComponent<Status>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPausing) return;
    }

    void OnTriggerEnter(Collider other)
    {
        //こちらがプレイヤー側で相手が敵側であるか、こちらが敵側で相手がプレイヤー側であった場合に実行
        if ((other.CompareTag("Enemy") && (CompareTag("Player") || CompareTag("Party")))
            ||((other.CompareTag("Player") || other.CompareTag("Party")) && CompareTag("Enemy")))
        {
            //攻撃してきた相手の照準情報を読み出し(照準情報が読みだせなければ即抜ける)
            AimSystem enemysAim = other.GetComponent<AimSystem>();
            if (!enemysAim) return;
            AttackInfo attackInfo = enemysAim.Info;

            //直接攻撃なら「敵のAttack - 自身のDefense」
            //間接攻撃なら「敵のMagic - 自身のShield」を求める
            short subtraction = (short)(attackInfo.isAttackingByMagic ?
                                        enemysAim.Status.Magic - status.Shield
                                        : enemysAim.Status.Attack - status.Defense);

            //HPを減らす(0を下回らないようにする)
            status.NowHP = (short)Mathf.Max(status.NowHP - (calculateDamage(subtraction) * attackInfo.powerRatio), 0.0f);

            //攻撃が当たった時のエフェクトを、攻撃を受けたポイントに表示
            if (hitEffectNormal) 
            {
                GameObject obj = Instantiate(hitEffectNormal);
                obj.transform.position = other.ClosestPoint(other.transform.position);
            }

            //ダメージを受けた状態にする、HPが0になっていたら倒された状態にする
            if (!status.IsDefeated)
            {
                status.IsDamaging = true;
                if (status.NowHP <= 0) status.IsDefeated = true;
                else status.IsFlirting = true;
            }
        }
    }

    /// <summary>
    /// (攻撃側の攻撃能力値 - 防御側の防御能力値)をした時の差から、威力補正1.0の時のベースとなるダメージ値を算出
    /// -1500の時に10、1500の時に210とする
    /// </summary>
    /// <param name="subtraction">攻撃側の攻撃能力値 - 防御側の防御能力値</param>
    /// <returns>威力補正1.0の時のベースとなるダメージ値</returns>
    float calculateDamage(short subtraction)
    {
        //傾き
        float tilt = 200f / 3000f;  // (210 - 10) / (1500 - (-1500))
        //切片
        float segment = 110f;       // 210 - (1500 * tilt)

        //傾き、切片、能力値差よりベースのダメージ値を算出
        return (subtraction * tilt) + segment;
    }

    /// <summary>
    /// animatorイベントより呼び出して、怯みが終了したことを伝える
    /// </summary>
    public void DamageEnd()
    {
        status.IsFlirting = false;
    }
}
