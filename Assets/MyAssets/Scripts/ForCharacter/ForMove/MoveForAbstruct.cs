using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>
/// 行動方針
/// </summary>
public enum CourseOfAction : byte
{
    /// <summary>
    /// 自分から動かない
    /// </summary>
    Stop,
    /// <summary>
    /// その座標を維持する
    /// </summary>
    KeepPoint,
    /// <summary>
    /// ある座標を起点に自由に動き回る
    /// </summary>
    Wander,
    /// <summary>
    /// 複数の指定座標を巡回する
    /// </summary>
    Patrol,
    /// <summary>
    /// 対象を追いかける
    /// </summary>
    Chase,
    /// <summary>
    /// 対象を探し、戦う
    /// </summary>
    Seek,
    /// <summary>
    /// 対象と戦う
    /// </summary>
    Fight,
    /// <summary>
    /// 対象から逃げる
    /// </summary>
    RunAway,
    /// <summary>
    /// 対象を補助する
    /// </summary>
    Follow
}

/// <summary>
/// 当該の行動方針における段階
/// </summary>
public enum StepOfAction : byte
{
    /// <summary>
    /// 待機中
    /// </summary>
    Stay,
    /// <summary>
    /// 周囲を見渡す
    /// </summary>
    LookAround,
    /// <summary>
    /// 目的地に前進する
    /// </summary>
    GoTowards,
    /// <summary>
    /// 対象を牽制する
    /// </summary>
    Restraint,
    /// <summary>
    /// 攻撃
    /// </summary>
    Attack,
    /// <summary>
    /// ガード態勢をとる
    /// </summary>
    Guard,
    /// <summary>
    /// 回避行動をとる
    /// </summary>
    Dodge
}


/// <summary>
/// 各キャラクターの移動用抽象スクリプト
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Status))]
public abstract class MoveForAbstruct : MyMonoBehaviour
{
    /// <summary>
    /// 崖から、接地させないように押し出す力
    /// </summary>
    public const float CLIFF_SLIDE_SPEED = 10.0f;
    /// <summary>
    /// 坂道とみなす角度
    /// </summary>
    public const float SLOPE_LIMIT_ANGLE = 60.0f;
    /// <summary>
    /// 最小移動速度
    /// </summary>
    public const float MIN_MOVE_DISTANCE = 0.1f;
    /// <summary>
    /// 落下最高速[m/s]
    /// </summary>
    public const float FALLING_MAX_SPEED = -40.0f;
    /// <summary>
    /// 急旋回が可能な速度閾値[m/s]
    /// </summary>
    public const float ABLE_QUICK_ROTATE_SPEED = 5.0f;
    /// <summary>
    /// 速度5[m/s]の時の旋回角度[deg/s]
    /// </summary>
    public const float ROTATE_SPEED_BY_LOW_SPEED = 720.0f;
    /// <summary>
    /// 走行速度5[m/s]超過の時のベースになる旋回角度[deg/s]
    /// </summary>
    public const float ROTATE_SPEED_BY_RUN = 360.0f;
    /// <summary>
    /// 滞空時のベースとなる旋回角度[deg/s]
    /// </summary>
    public const float ROTATE_SPEED_ARIAL = 40.0f;



    /// <summary>
    /// 使用するリジッドボディ
    /// </summary>
    protected RigidbodyTimeline3D rb = default;

    /// <summary>
    /// キャラクターの情報が格納されたコンポーネント
    /// </summary>
    protected Status status = default;

    /// <summary>
    /// 地面レイヤ用のレイヤマスク
    /// </summary>
    [SerializeField]
    [Tooltip("地面レイヤ用のレイヤマスク")]
    protected LayerMask layerOfGround = default;
    /// <summary>
    /// 地面レイヤーのレイヤー名
    /// </summary>
    [SerializeField]
    [Tooltip("地面レイヤーのレイヤー名")]
    protected string layerNameOfGround = "Ground";
    /// <summary>
    /// 地面レイヤーのレイヤー番号
    /// </summary>
    protected byte layerNumberOfGround = 0;
    /// <summary>
    /// 接地判定のための、足元のトリガーコライダーの地面との接触判定
    /// </summary>
    protected bool isHitFootCollider = false;
    /// <summary>
    /// 接地判定のための、当たり判定コライダーと地面との接触判定
    /// </summary>
    protected bool isHitCollider = false;


    /// <summary>
    /// キャラクターを指定スピード分向きを変えさせる
    /// </summary>
    /// <param name="targetDirection">回転先の方向</param>
    /// <param name="rotateSpeed">旋回角速度</param>
    protected void CharacterRotation(Vector3 targetDirection ,float rotateSpeed)
    {
        if (targetDirection.sqrMagnitude <= 0.0f) return;

        //180°ターンは各キャラクターのIsTurnLeftパラメータを参照して、回転方向を指定
        Vector3 trunDirection = transform.right;
        if (status.IsTurnLeft) trunDirection *= -1.0f;
        Quaternion charDirectionQuaternion = Quaternion.LookRotation(targetDirection + (trunDirection * 0.001f));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, charDirectionQuaternion, rotateSpeed * Time.deltaTime);
    }


    /// <summary>
    /// 接地判定処理
    /// </summary>
    protected void GroundedCheck()
    {
        status.IsGrounded = (isHitFootCollider && isHitCollider);
    }

    /// <summary>
    /// 当たり判定コライダーに接触
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == layerNumberOfGround)
        {
            if (isHitFootCollider) isHitCollider = true;
        }
    }
    /// <summary>
    /// 当たり判定コライダーから離脱
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == layerNumberOfGround)
        {
            if (!isHitFootCollider) isHitCollider = false;
        }
    }

    /// <summary>
    /// 接地判定用の足元コライダーに接触中トリガー
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        if (!isHitFootCollider && other.gameObject.layer == layerNumberOfGround) isHitFootCollider = true;
    }
    /// <summary>
    /// 接地判定用の足元コライダーから離脱トリガー
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == layerNumberOfGround) isHitFootCollider = false;
    }
}
