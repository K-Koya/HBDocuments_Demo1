using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>
/// キャラクターの子オブジェクトにあるコマンドをまとめ、使用する
/// </summary>
public class CommandHolderForPlayer : MyMonoBehaviour
{
    /// <summary>
    /// 各コマンドを格納しているオブジェクト
    /// </summary>
    [SerializeField, Tooltip("子オブジェクトより、コマンドを格納しているオブジェクトをドラッグアンドドロップ")]
    GameObject holderObject = default;

    /// <summary>
    /// 入力情報を持つコンポーネント
    /// </summary>
    MyInputManager input = default;
    /// <summary>
    /// キャラクターのステータス
    /// </summary>
    Status status = default;

    /// <summary>
    /// 照準システム
    /// </summary>
    AimSystemForPlayer aim = default;


    /// <summary>
    /// 実行中のコマンド
    /// </summary>
    [SerializeField]
    CommandAbstruct running = default;

    /// <summary>
    /// 実行中のコマンドの対応入力
    /// </summary>
    [SerializeField]
    InputKind forRunning = default;


    /// <summary>
    /// 通常コンボコマンド
    /// </summary>
    ComboCommand combo = default;

    /// <summary>
    /// コマンドリスト
    /// </summary>
    List<CommandAbstruct> commandList = new List<CommandAbstruct>();





    // Start is called before the first frame update
    void Start()
    {
        TimelineInit();
        input = FindObjectOfType<MyInputManager>();
        status = GetComponent<Status>();
        aim = GetComponentInChildren<AimSystemForPlayer>();

        //子オブジェクトからコマンドコンポーネントを取得し、種類によって分配する
        CommandAbstruct[] commands = GetComponentsInChildren<CommandAbstruct>();
        foreach(CommandAbstruct command in commands)
        {
            switch (command.CommandType)
            {
                case CommandType.Attack:
                case CommandType.Support:
                case CommandType.Item:
                    {
                        commandList.Add(command);
                        break;
                    }
                case CommandType.Combo:
                    {
                        combo = (ComboCommand)command;
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
            if(input.Combo.NowPushDown != PushType.noPush)
            {
                running = combo;
                running.Run();
                forRunning = input.Combo;
            }
            else
            {
                status.IsCommandRunning = false;
            }
        }
         
    }



    /// <summary>
    /// animatorイベントより呼び出して、コライダーの起動、攻撃情報を更新する
    /// </summary>
    public void HitAreaColliderActivate()
    {
        if (running)
        {
            aim.Info = running.Info;
            aim.Area.enabled = true;
        }
    }

    /// <summary>
    /// animatorイベントより呼び出して、コライダーの停止をする
    /// </summary>
    public void HitAreaColliderInactivate()
    {
        aim.Area.enabled = false;
    }

    /// <summary>
    /// animatorイベントより呼び出して、コマンド追加入力を受け付ける
    /// </summary>
    public void Acceptable()
    {
        if (running) running.IsAcceptable = true;
    }

    /// <summary>
    /// animatorイベントより呼び出して、各アクションが終了したことを伝える
    /// </summary>
    public void EndOfAction()
    {
        running = null;
    }
}
