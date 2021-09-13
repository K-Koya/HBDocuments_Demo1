using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>
/// 各キャラクターのAnimatorにパラメータを渡す、
/// あるいはAnimatorのイベントを受け取るコンポーネント
/// </summary>
[RequireComponent(typeof(Timeline))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Status))]
public class AnimatorForCharacter : MyMonoBehaviour
{
    /// <summary>
    /// animatorに渡すパラメーター名:Speed
    /// </summary>
    static string animParamNameSpeed = "Speed";
    /// <summary>
    /// animatorに渡すパラメーター名:IsGrounded
    /// </summary>
    static string animParamNameIsGrounded = "IsGrounded";
    /// <summary>
    /// animatorに渡すパラメーター名:IsJumping
    /// </summary>
    static string animParamNameIsJumping = "IsJumping";
    /// <summary>
    /// animatorに渡すパラメーター名:DoEyeBlink
    /// </summary>
    static string animParamNameDoEyeBlink = "DoEyeBlink";



    /// <summary>
    /// キャラクターにアタッチされているAnimator
    /// </summary>
    Animator animator = default;

    /// <summary>
    /// キャラクターのステータス
    /// </summary>
    Status status = default;


    // Start is called before the first frame update
    void Start()
    {
        TimelineInit();
        animator = time.animator.component;
        status = GetComponent<Status>();

        StartCoroutine(EyeBlink());
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPausing) return;

        animator.SetFloat(animParamNameSpeed, status.ResultSpeed);
        animator.SetBool(animParamNameIsGrounded, status.IsGrounded);
        animator.SetBool(animParamNameIsJumping, status.IsJumping);
    }

    /// <summary>
    /// 瞬きを一定間隔で要求
    /// </summary>
    /// <returns></returns>
    IEnumerator EyeBlink()
    {
        while (gameObject)
        {
            //瞬きを制御する
            if (Random.Range(0, 5) <= 1) animator.SetTrigger(animParamNameDoEyeBlink);
            yield return time.WaitForSeconds(0.5f);
        }
    }
}
