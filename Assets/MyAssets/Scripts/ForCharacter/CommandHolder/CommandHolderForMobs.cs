using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターの子オブジェクトにあるコマンドをまとめ、使用する
/// </summary>
public class CommandHolderForMobs : MyMonoBehaviour
{
    /// <summary>
    /// 各コマンドを格納しているオブジェクト
    /// </summary>
    [SerializeField, Tooltip("子オブジェクトより、コマンドを格納しているオブジェクトをドラッグアンドドロップ")]
    protected GameObject holderObject = default;

    /// <summary>
    /// キャラクターのステータス
    /// </summary>
    protected Status status = default;

    /// <summary>
    /// キャラクター向けのアニメーター
    /// </summary>
    protected Animator characterAnimator = default;

    /// <summary>
    /// キャラクターが持っているアクセサリーのアニメーター
    /// </summary>
    [SerializeField, Tooltip("キャラクターが持っているアクセサリーのアニメーターを各種アタッチ")]
    protected List<Animator> accessoriesAnimators = new List<Animator>();

    /// <summary>
    /// モブの場合の入力コンポーネント
    /// </summary>
    MoveForEnemy ai = default;

    /// <summary>
    /// 照準システム
    /// </summary>
    protected AimSystem aim = default;

    /// <summary>
    /// 攻撃を受けるレイヤー
    /// </summary>
    [SerializeField, Tooltip("攻撃を受けるレイヤー")]
    protected LayerMask layerOfReceiveAttack = default;


    /// <summary>
    /// 実行中のコマンド
    /// </summary>
    protected CommandAbstruct running = default;

    /// <summary>
    /// 実行中のコマンドの対応入力を保管
    /// </summary>
    PushType forRunning = PushType.noPush;

    /// <summary>
    /// 通常コンボコマンド
    /// </summary>
    protected ComboCommand combo = default;

    /// <summary>
    /// 回避行動コマンド
    /// </summary>
    protected CommandAbstruct dodge = default;

    /// <summary>
    /// コマンドリスト
    /// </summary>
    protected List<CommandAbstruct> commandList = new List<CommandAbstruct>();


    // Start is called before the first frame update
    protected virtual void Start()
    {
        TimelineInit();
        status = GetComponent<Status>();
        ai = GetComponent<MoveForEnemy>();
        aim = GetComponentInChildren<AimSystem>();
        characterAnimator = GetComponent<Animator>();

        //子オブジェクトからコマンドコンポーネントを取得し、種類によって分配する
        CommandAbstruct[] commands = GetComponentsInChildren<CommandAbstruct>();
        foreach (CommandAbstruct command in commands)
        {
            //アニメーターのパスを提供
            command.Animator = characterAnimator;
            command.AccessoriesAnimators = accessoriesAnimators;

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
            running.Acception = forRunning;
        }
        //実行中のコマンドがなければ、新規受付
        else
        {
            //コマンド実行中フラグを立てておく
            status.IsCommandRunning = true;
            //通常コンボ受付
            if (ai.Combo != PushType.noPush)
            {
                running = combo;
                running.Run();
                forRunning = ai.Combo;
            }
            else if(ai.Dodge != PushType.noPush)
            {
                running = dodge;
                running.Run();
                forRunning = ai.Dodge;
            }
            else
            {
                status.IsCommandRunning = false;
            }
        }
    }


    /// <summary>
    /// animatorイベントより呼び出して、攻撃範囲と攻撃を発生
    /// </summary>
    public virtual void CreateAttackArea()
    {
        if (running)
        {
            running.LookTarget = aim.transform.position;
            aim.Info = running.Info;

            //球状の当たり判定を出し、当たる相手を限定する
            Collider[] hits = Physics.OverlapSphere(aim.transform.position, aim.Info.radius, layerOfReceiveAttack);
            if (CompareTag("Enemy")) hits = hits.Where(h => h.CompareTag("Player") || h.CompareTag("Party")).ToArray();
            else if (CompareTag("Party")) hits = hits.Where(h => h.CompareTag("Enemy")).ToArray();

            //攻撃処理
            Array.ForEach(hits, h => h.GetComponent<Damager>().Damaging(aim.Info, status));
        }
    }


    /// <summary>
    /// animatorイベントより呼び出して、コライダーの起動、攻撃情報を更新する
    /// </summary>
    public void HitAreaColliderActivate()
    {
        if (running)
        {
            running.LookTarget = aim.transform.position;
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
