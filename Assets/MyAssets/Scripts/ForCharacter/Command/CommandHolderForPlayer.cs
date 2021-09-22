using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>
/// プレイヤーキャラクターの子オブジェクトにあるコマンドをまとめ、使用する
/// </summary>
public class CommandHolderForPlayer : CommandHolderForMobs
{
    /// <summary>
    /// 入力情報を持つコンポーネント
    /// </summary>
    MyInputManager input = default;

    /// <summary>
    /// 実行中のコマンドの対応入力
    /// </summary>
    InputKind forRunning = default;


    // Start is called before the first frame update
    protected override void Start()
    {
        input = FindObjectOfType<MyInputManager>();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPausing) return;

        //何かしらコマンドが出せる状況でなければ、即抜ける
        if (status.IsDamaging && status.IsJumping) return;


        //実行中のコマンドがあれば、追加入力等受付処理
        if (running)
        {
            running.Acception = forRunning.NowPushType;
            status.IsCommandRunning = true;
        }
        //実行中のコマンドがなければ、新規受付
        else
        {
            //通常コンボ受付
            if (input.Combo.NowPushDown != PushType.noPush)
            {
                running = combo;
                running.LookTarget = aim.transform.position;
                running.Run();
                forRunning = input.Combo;
            }
            else
            {
                status.IsCommandRunning = false;
            }
        }
    }
}
