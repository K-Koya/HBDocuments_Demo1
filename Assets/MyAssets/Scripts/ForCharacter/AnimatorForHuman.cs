using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>
/// 各キャラクターのAnimatorにパラメータを渡す、
/// あるいはAnimatorのイベントを受け取るコンポーネント
/// 瞬き等の特殊なアニメーションも含む
/// </summary>
public class AnimatorForHuman : AnimatorForCharacter
{
    /// <summary>
    /// animatorに渡すパラメーター名:DoEyeBlink
    /// </summary>
    protected static string animParamNameDoEyeBlink = "DoEyeBlink";


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        StartCoroutine(EyeBlink());
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (IsPausing) return;

        base.Update();
    }

    /// <summary>
    /// 瞬きを一定間隔で要求
    /// </summary>
    /// <returns></returns>
    protected IEnumerator EyeBlink()
    {
        while (gameObject)
        {
            //瞬きを制御する
            if (Random.Range(0, 5) <= 1) animator.SetTrigger(animParamNameDoEyeBlink);
            yield return time.WaitForSeconds(0.5f);
        }
    }
}
