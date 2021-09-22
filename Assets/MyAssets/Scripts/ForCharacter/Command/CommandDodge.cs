using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDodge : CommandAbstruct
{
    // Start is called before the first frame update
    void Start()
    {
        TimelineInit();
        Init();
    }

    /// <summary>
    /// 回避行動フロー
    /// </summary>
    protected override IEnumerator CommandFlow()
    {
        //対応するアニメーションを指定
        InitAttackInfosOrder();
        animator.SetInteger(ANIM_PARAM_NAME_ACTION_NUMBER, animNumber);
        isEndOfAction = false;
        isAcceptable = false;

        /* 回避アクション */
        //アニメーションを開始
        animator.SetTrigger(ANIM_PARAM_NAME_DO_NEXT_ACTION);
        //アクション終了まで待つ
        while (!isAcceptable) yield return null;

        acception = PushType.noPush;
        isAcceptable = false;
    }

    /* プロパティ */
    public override string CommandName => "Dodge";
    public override CommandType CommandType => CommandType.Slide;
}
