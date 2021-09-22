using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// "ラプトル"の通常コンボ
/// </summary>
public class ComboForRaptor : ComboCommand
{
    /// <summary>
    /// 通常コンボフロー
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator CommandFlow()
    {
        //対応するアニメーションを指定
        InitAttackInfosOrder();
        animator.SetInteger(ANIM_PARAM_NAME_ACTION_NUMBER, animNumber);
        isEndOfAction = false;
        isAcceptable = false;

        /* コンボ1段目 */
        //アニメーションを開始
        animator.SetTrigger(ANIM_PARAM_NAME_DO_NEXT_ACTION);
        //アクション終了まで待つ
        while (!isAcceptable) yield return null;

        acception = PushType.noPush;
        isAcceptable = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        TimelineInit();
        Init();
    }
}
