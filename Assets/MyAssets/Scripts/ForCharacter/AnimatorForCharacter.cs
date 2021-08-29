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
    /// キャラクターにアタッチされているTimeline
    /// </summary>
    Timeline timeline = default;

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
        timeline = GetComponent<Timeline>();
        animator = timeline.animator.component;
        status = GetComponent<Status>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", status.ResultSpeed);
        animator.SetBool("IsGrounded", status.IsGrounded);
        animator.SetBool("IsJumping", status.IsJumping);
    }

    void FixedUpdate()
    {
        //瞬きを制御する
        if (Random.Range(0, 500) <= 2) animator.SetTrigger("DoEyeBlink");
    }
}
