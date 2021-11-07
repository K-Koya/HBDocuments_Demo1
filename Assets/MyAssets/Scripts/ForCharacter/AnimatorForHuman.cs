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

    [Header("IK用パラメータ")]
    /// <summary>
    /// 見るターゲット
    /// </summary>
    [SerializeField] Transform lookTarget = default;
    /// <summary>
    /// どれくらい見るか
    /// </summary>
    [SerializeField, Range(0f, 1f)] float lookTargetWeight = 0;
    /// <summary>
    /// 身体をどれくらい向けるか
    /// </summary>
    [SerializeField, Range(0f, 1f)] float lookTargetBodyWeight = 0;
    /// <summary>
    /// 頭をどれくらい向けるか
    /// </summary>
    [SerializeField, Range(0f, 1f)] float lookTargetHeadWeight = 0;
    /// <summary>
    /// 目をどれくらい向けるか
    /// </summary>
    [SerializeField, Range(0f, 1f)] float lookTargetEyesWeight = 0;
    /// <summary>
    /// 関節の動きをどれくらい制限するか
    /// </summary>
    [SerializeField, Range(0f, 1f)] float lookTargetClampWeight = 0;

    /* プロパティ */
    public Transform LookTarget { set => lookTarget = value; }


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

    
    void OnAnimatorIK(int layerIndex)
    {
        //キャラクターの注視方向に関するIKを設定
        animator.SetLookAtWeight(lookTargetWeight, lookTargetBodyWeight, lookTargetHeadWeight, lookTargetEyesWeight, lookTargetClampWeight);
        //キャラクターを注視方向へ注目
        if (lookTarget) animator.SetLookAtPosition(lookTarget.position);
    }
}
