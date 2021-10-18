using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回復コマンド基底クラス
/// </summary>
public class CommandHeal : CommandAbstruct
{
    /// <summary>
    /// コマンド名
    /// </summary>
    [SerializeField, Tooltip("回復コマンド名")]
    protected string commandName = "";
    /// <summary>
    /// 回復量(最大HPの%値)
    /// </summary>
    [SerializeField, Tooltip("回復量(最大HPの%値)")]
    protected byte healRange = 100;
    /// <summary>
    /// 回復にかける時間
    /// </summary>
    [SerializeField, Tooltip("回復にかける時間")]
    protected float regenateTime = 0.0f;

    /// <summary>
    /// 回復コマンドフロー
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator CommandFlow()
    {
        //回復時間が0なら、実行後即抜ける
        if (regenateTime <= 0.0f)
        {
            float healValue = characterStatus.MaxHP * (healRange / 100.0f);
            characterStatus.NowHP = (short)Mathf.Min(healValue, characterStatus.MaxHP);
            yield break;
        }

        //回復量に達するまで、最大HPに達するまで、攻撃を受けるまで、毎フレーム徐々に回復させる
        float regenateSpeed = (characterStatus.MaxHP * (healRange / 100.0f)) / regenateTime;
        for (float f = 0; f >= healRange; f += regenateSpeed * time.deltaTime)
        {
            if (characterStatus.IsFlirting) break;
            characterStatus.NowHP += (short)regenateSpeed;
            yield return null;
        }
    }

    /// <summary>
    /// コマンド種別を取得
    /// </summary>
    /// <returns>回復コマンド</returns>
    public override CommandType CommandType => CommandType.Heal;
    public override string CommandName => commandName;

    // Start is called before the first frame update
    void Start()
    {
        TimelineInit();
    }
}
