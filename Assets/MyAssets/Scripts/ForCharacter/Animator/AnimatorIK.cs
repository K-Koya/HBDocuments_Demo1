using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アニメーションで指定した方向を見させるためのIKスクリプト
/// </summary>
[RequireComponent(typeof(Animator))]
public class AnimatorIK : MonoBehaviour
{
    /// <summary>
    /// キャラクターにアタッチされているAnimator
    /// </summary>
    Animator animator = default;

    [Header("IK用パラメータ")]
    [SerializeField, Tooltip("見るターゲット")]
    Transform lookTarget = default;

    [SerializeField, Tooltip("どれくらい見るか"), Range(0f, 1f)]
    float lookTargetWeight = 0;

    [SerializeField, Tooltip("身体をどれくらい向けるか"), Range(0f, 1f)]
    float lookTargetBodyWeight = 0;

    [SerializeField, Tooltip("頭をどれくらい向けるか"), Range(0f, 1f)]
    float lookTargetHeadWeight = 0;

    [SerializeField, Tooltip("目をどれくらい向けるか"), Range(0f, 1f)]
    float lookTargetEyesWeight = 0;

    [SerializeField, Tooltip("関節の動きをどれくらい制限するか"), Range(0f, 1f)]
    float lookTargetClampWeight = 0;

    /* プロパティ */
    public Transform LookTarget { set => lookTarget = value; }
    public float LookTargetWeight { set => lookTargetWeight = value; }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        //キャラクターの注視方向に関するIKを設定
        animator.SetLookAtWeight(lookTargetWeight, lookTargetBodyWeight, lookTargetHeadWeight, lookTargetEyesWeight, lookTargetClampWeight);
        //キャラクターを注視方向へ注目
        if (lookTarget) animator.SetLookAtPosition(lookTarget.position);
    }
}
