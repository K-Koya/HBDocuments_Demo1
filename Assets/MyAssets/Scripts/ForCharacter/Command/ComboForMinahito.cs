using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// "ミナヒト"の通常コンボ
/// </summary>
public class ComboForMinahito : ComboCommand
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
        animator.transform.LookAt(lookTarget.YEqual());
        //アクション終了まで待つ
        while (!isAcceptable) yield return null;

        //一定時間待ち、追加入力がなければ、このコマンドを終了する
        float timer = 0.0f;
        acception = PushType.noPush;
        while (timer < 0.2f)
        {
            if (acception != PushType.noPush)
            {
                isEndOfAction = true;
                break;
            }
            timer += time.deltaTime;
            yield return null;
        }
        if (acception == PushType.noPush) yield break;


        /* コンボ2段目 */
        isAcceptable = false;
        //アニメーションを開始
        animator.SetTrigger(ANIM_PARAM_NAME_DO_NEXT_ACTION);
        animator.transform.LookAt(lookTarget.YEqual());
        //アクション終了まで待つ
        while (!isAcceptable) yield return null;

        //一定時間待ち、追加入力がなければ、このコマンドを終了する
        timer = 0.0f;
        acception = PushType.noPush;
        while (timer < 0.2f)
        {
            if (acception != PushType.noPush)
            {
                isEndOfAction = true;
                break;
            }
            timer += time.deltaTime;
            yield return null;
        }
        if (acception == PushType.noPush) yield break;


        /* コンボ3段目 */
        isAcceptable = false;
        //アニメーションを開始
        animator.SetTrigger(ANIM_PARAM_NAME_DO_NEXT_ACTION);
        animator.transform.LookAt(lookTarget.YEqual());
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
