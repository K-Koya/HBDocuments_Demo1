﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>
/// 全コマンドの基底クラス
/// </summary>
[RequireComponent(typeof(Timeline))]
public abstract class CommandAbstruct : MyMonoBehaviour
{
    /// <summary>
    /// animatorに渡すパラメーター名:ActionNumber
    /// </summary>
    protected const string ANIM_PARAM_NAME_ACTION_NUMBER = "ActionNumber";

    /// <summary>
    /// animatorに渡すパラメーター名:DoNextAction
    /// </summary>
    protected const string ANIM_PARAM_NAME_DO_NEXT_ACTION = "DoNextAction";

    /// <summary>
    /// Animatorで再生するアニメーション番号
    /// </summary>
    [SerializeField]
    protected byte animNumber = 0;

    /// <summary>
    /// キャラクターのアニメーター
    /// </summary>
    protected Animator animator = default;

    /// <summary>
    /// コマンド追加入力受付
    /// </summary>
    protected PushType acception = default;
    /// <summary>
    /// true:コマンドの追加入力を受け付けている
    /// </summary>
    protected bool isAcceptable = default;
    /// <summary>
    /// true:コマンドのアクションが終了した
    /// </summary>
    protected bool isEndOfAction = true;


    /// <summary>
    /// 初期化
    /// </summary>
    protected void Init()
    {
        animator = GetComponentInParent<Animator>();
    }


    /// <summary>
    /// コマンドを、入力があれば走らせる
    /// コマンド実行中なら、追加入力を受け付ける
    /// </summary>
    public void Run()
    {
        //コマンドフロー用のコルーチンを開始
        StartCoroutine(CommandFlow());
    }



    /// <summary>
    /// コマンドを流れに沿って実行
    /// </summary>
    protected abstract IEnumerator CommandFlow();

    /// <summary>
    /// 使用する攻撃情報の番号を初期化(攻撃系クラス内でoverrideして使用)
    /// </summary>
    public virtual void InitAttackInfosOrder() { }


    /* プロパティ */
    /// <summary>
    /// コマンド種別を取得
    /// </summary>
    public abstract CommandType CommandType { get; }
    /// <summary>
    /// コマンド名を取得
    /// </summary>
    public abstract string CommandName { get; }
    /// <summary>
    /// 攻撃情報を取得
    /// </summary>
    public virtual AttackInfo Info { get => default; }
    public PushType Acception { get => acception; set => acception = value; }
    public bool IsEndOfAction { get => isEndOfAction; set => isEndOfAction = value; }
    public bool IsAcceptable { get => isAcceptable; set => isAcceptable = value; }
}

/// <summary>
/// 攻撃コマンド基底クラス
/// </summary>
public abstract class AttackCommand : CommandAbstruct
{
    /// <summary>
    /// 攻撃情報配列
    /// </summary>
    [SerializeField]
    protected AttackInfo[] attackInfos = default;
    /// <summary>
    /// 現在の攻撃情報の番号
    /// </summary>
    protected byte attackInfosOrder = 0;


    /// <summary>
    /// コマンド種別を取得
    /// </summary>
    /// <returns>アタックコマンド</returns>
    public override CommandType CommandType => CommandType.Attack;

    /// <summary>
    /// 使用する攻撃情報の番号を初期化
    /// </summary>
    public override void InitAttackInfosOrder() { attackInfosOrder = 0; }

    /* プロパティ */
    /// <summary>
    /// 攻撃情報を取得
    /// </summary>
    public override AttackInfo Info { get => attackInfos[attackInfosOrder++]; }
}


/// <summary>
/// 通常コンボ攻撃基底クラス
/// </summary>
public abstract class ComboCommand : AttackCommand
{

    /// <summary>
    /// コマンド種別を取得
    /// </summary>
    /// <returns>通常コンボ</returns>
    public override CommandType CommandType => CommandType.Combo;

    /// <summary>
    /// コマンド名を取得
    /// </summary>
    public override string CommandName => "Combo";
}


/// <summary>
/// 攻撃情報の構造体
/// </summary>
[System.Serializable]
public struct AttackInfo
{
    /// <summary>
    /// true:Magic値による攻撃である
    /// false:Attack値による攻撃である
    /// </summary>
    [Tooltip("true:Magic値による攻撃である\nfalse:Attack値による攻撃である")]
    public bool isAttackingByMagic;

    /// <summary>
    /// 技の威力補正
    /// </summary>
    [Tooltip("技の威力補正")]
    public float powerRatio;
}

/// <summary>
/// コマンド種別
/// </summary>
public enum CommandType : byte
{
    /// <summary>
    /// 通常コンボ
    /// </summary>
    Combo,
    /// <summary>
    /// ガード
    /// </summary>
    Guard,
    /// <summary>
    /// 短距離回避
    /// </summary>
    Slide,
    /// <summary>
    /// アタックコマンド
    /// </summary>
    Attack,
    /// <summary>
    /// サポートコマンド
    /// </summary>
    Support,
    /// <summary>
    /// アイテムコマンド
    /// </summary>
    Item
}