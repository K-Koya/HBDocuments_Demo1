using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>
/// プレイヤーキャラクターのうちミナヒトが対象、子オブジェクトにあるコマンドをまとめ、使用する
/// </summary>
public class CommandHolderForMinahitoPlayer : CommandHolderForMinahito
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
        TimelineInit();
        status = GetComponent<Status>();
        characterAnimator = GetComponent<Animator>();
        aim = GetComponentInChildren<AimSystem>();

        //子オブジェクトからコマンドコンポーネントを取得し、種類によって分配する
        CommandAbstruct[] commands = GetComponentsInChildren<CommandAbstruct>();
        foreach (CommandAbstruct command in commands)
        {
            //各アニメーターのパスを提供
            command.Animator = characterAnimator;
            command.AccessoriesAnimators.Add(whipAnimator);

            switch (command.CommandType)
            {
                case CommandType.Attack:
                case CommandType.Support:
                case CommandType.Heal:
                    {
                        commandList.Add(command);
                        break;
                    }
                case CommandType.Combo:
                    {
                        combo = (ComboCommand)command;
                        break;
                    }
                case CommandType.Slide:
                    {
                        dodge = command;
                        break;
                    }
                default: break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPausing) return;

        //何かしらコマンドが出せる状況でなければ、即抜ける
        if (status.IsJumping) return;
        if (status.IsFlirting || status.IsDefeated)
        {
            if (running)
            {
                running.Stop();
                HitAreaColliderInactivate();
                EndOfAction();
            }
            return;
        }


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
