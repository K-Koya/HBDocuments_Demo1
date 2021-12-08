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
    static protected string animParamNameSpeed = "Speed";
    /// <summary>
    /// animatorに渡すパラメーター名:IsGrounded
    /// </summary>
    static protected string animParamNameIsGrounded = "IsGrounded";
    /// <summary>
    /// animatorに渡すパラメーター名:IsJumping
    /// </summary>
    static protected string animParamNameIsJumping = "IsJumping";
    /// <summary>
    /// animatorに渡すパラメーター名:isDamaged
    /// </summary>
    static protected string animParamNameIsDamaged = "IsDamaged";
    /// <summary>
    /// animatorに渡すパラメーター名:isDefeated
    /// </summary>
    static protected string animParamNameIsDefeated = "IsDefeated";

    /// <summary>
    /// キャラクターにアタッチされているAnimator
    /// </summary>
    protected Animator animator = default;

    /// <summary>
    /// キャラクターのステータス
    /// </summary>
    protected Status status = default;




    // Start is called before the first frame update
    protected virtual void Start()
    {
        TimelineInit();
        animator = time.animator.component;
        status = GetComponent<Status>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (IsPausing) return;

        animator.SetFloat(animParamNameSpeed, status.ResultSpeed);
        animator.SetBool(animParamNameIsGrounded, status.IsGrounded);
        animator.SetBool(animParamNameIsJumping, status.IsJumping);
        animator.SetBool(animParamNameIsDefeated, status.IsDefeated);

        if (status.IsDamaging)
        {
            animator.SetTrigger(animParamNameIsDamaged);
            status.IsDamaging = false;
        }
    }
}
