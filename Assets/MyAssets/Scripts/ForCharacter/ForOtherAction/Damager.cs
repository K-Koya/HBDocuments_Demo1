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
            //攻撃してきた相手の照準情報を読み出し
            AimSystem enemysAim = other.GetComponent<AimSystem>();
            AttackInfo attackInfo = enemysAim.Info;

            //直接攻撃なら「敵のAttack - 自身のDefense」
            //間接攻撃なら「敵のMagic - 自身のShield」を求める
            short subtraction = (short)(attackInfo.isAttackingByMagic ?
                                        enemysAim.Status.Magic - status.Shield
                                        : enemysAim.Status.Attack - status.Defense);

            Debug.Log("Hit! : " + calculateDamage(subtraction) * attackInfo.powerRatio);


        }
    }



    /// <summary>
    /// (攻撃側の攻撃能力値 - 防御側の防御能力値)をした時の差から、威力補正1.0の時のベースとなるダメージ値を算出
    /// -1500の時に10、1500の時に210とする
    /// </summary>
    /// <param name="subtraction">攻撃側の攻撃能力値 - 防御側の防御能力値</param>
    /// <returns>補助技能数値</returns>
    float calculateDamage(short subtraction)
    {
        //傾き
        float tilt = 200f / 3000f;
        //切片
        float segment = 100f;

        //傾き、切片、能力値差よりベースのダメージ値を算出
        return (subtraction * tilt) + segment;
    }
}
