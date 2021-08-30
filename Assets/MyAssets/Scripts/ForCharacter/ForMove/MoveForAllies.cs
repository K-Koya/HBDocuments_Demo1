using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Chronos;

/// <summary>
/// 味方キャラクターの移動スクリプト
/// </summary>
[RequireComponent(typeof(NavMeshAgentTimeline))]
public class MoveForAllies : MoveForAbstruct
{
    /// <summary>
    /// この距離分だけdestinationから離れると、再度移動を始める
    /// </summary>
    [SerializeField]
    [Tooltip("この距離分だけ目的地から離れると、再度移動を始める")]
    float researchDestinationDistance = 4.0f;

    /// <summary>
    /// この距離分だけdestinationに近づくと、目的地到着とみなす
    /// </summary>
    [SerializeField]
    [Tooltip("この距離分だけ目的地に近づくと、目的地到着とみなす")]
    float arrivalDestinationDistance = 2.0f;



    /// <summary>
    /// NavMeshAgent定義
    /// </summary>
    NavMeshAgent nav = default;


    /// <summary>
    /// 行動方針
    /// </summary>
    [SerializeField]
    CourseOfAction courseOfAction = CourseOfAction.Stop;
    /// <summary>
    /// 当該の行動方針における段階
    /// </summary>
    [SerializeField]
    StepOfAction stepOfAction = StepOfAction.Stay;
    /// <summary>
    /// 移動目的地
    /// </summary>
    Vector3? destination = null;
    /// <summary>
    /// 援護対象
    /// </summary>
    [SerializeField]
    [Tooltip("援護対象")]
    Transform followTarget = default;
    /// <summary>
    /// 攻撃対象
    /// </summary>
    [SerializeField]
    [Tooltip("攻撃対象")]
    Transform attackTarget = default;



    /// <summary>
    /// NavMesh操作による移動方向ベクトル
    /// </summary>
    Vector3 navMoveDirection = Vector3.zero;



    // Start is called before the first frame update
    void Start()
    {
        TimelineInit();
        rb = time.rigidbody;
        status = GetComponent<Status>();
        layerNumberOfGround = (byte)LayerMask.NameToLayer(layerNameOfGround);

        nav = time.navMeshAgent.component;

        InitCourceOfAction();
        StartCoroutine(NavMeshDestinationSettingCorutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPausing) return;

        GroundedCheck();
        NavMeshMove();
        VerticalMove();

        ResultSpeedCheck();
    }


    /// <summary>
    /// 一定間隔でNavMesh上の目的地を決定するコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator NavMeshDestinationSettingCorutine()
    {
        while (true)
        {
            //移動目的地が決まっていれば経路探索
            if (destination != null)
            {
                //上空10m程度にあたる座標点から真下の地形に向けRayを落とす
                RaycastHit hitInfo;
                bool isfound = Physics.Raycast((Vector3)destination + transform.up * status.Height, -transform.up, out hitInfo, 100.0f, layerOfGround);

                //地形に当たったら
                if (isfound)
                {
                    //移動先に決定
                    nav.SetDestination(hitInfo.point);
                    
                    //経路巡行を開始
                    nav.isStopped = false;
                }
                
                //見つかれば長めに待つ、見つからなければ次のフレームへ
                if (isfound) yield return time.WaitForSeconds(0.1f);
                else yield return null;
            }
            else
            {
                //経路巡行を停止
                nav.isStopped = true;

                yield return null;
            }
        }
    }


    /// <summary>
    /// courseOfActionが変更されたときに実行する初期化メソッド
    /// </summary>
    void InitCourceOfAction()
    {
        switch (courseOfAction)
        {
            case CourseOfAction.Stop:
                {
                    //目的地なし
                    destination = null;
                    //停止状態
                    stepOfAction = StepOfAction.Stay;
                    break;
                }
            case CourseOfAction.KeepPoint:
                {
                    //現在地を目的地に
                    destination = transform.position;
                    //移動性能は自分の値で
                    nav.speed = status.MaxRunSpeed;
                    nav.acceleration = status.RunAcceleration;
                    break;
                }
            case CourseOfAction.Follow:
                {
                    //補助対象を目的地に
                    destination = followTarget.position;
                    //移動性能は補助対象と同値に
                    Status fStatus = followTarget.gameObject.GetComponent<Status>();
                    nav.speed = fStatus.MaxRunSpeed;
                    nav.acceleration = fStatus.RunAcceleration;
                    break;
                }
            default: break;
        }
    }


    /// <summary>
    /// NavMeshAgentを用いた移動処理
    /// </summary>
    void NavMeshMove()
    {
        switch (courseOfAction)
        {
            case CourseOfAction.Stop:
                {
                    break;
                }
            case CourseOfAction.KeepPoint:
                {
                    //停止中に再度移動を開始するか判定
                    float sqrDistance = Vector3.SqrMagnitude((Vector3)destination - transform.position);
                    if ((stepOfAction == StepOfAction.Stay) 
                        && (sqrDistance > Mathf.Pow(researchDestinationDistance, 2.0f)))
                    {
                        nav.isStopped = false;
                        stepOfAction = StepOfAction.GoTowards;
                        InitCourceOfAction();
                    }
                    //目的地への移動を完了するかの判定
                    else if((stepOfAction == StepOfAction.GoTowards)
                            && (sqrDistance < Mathf.Pow(arrivalDestinationDistance, 2.0f)))
                    {
                        nav.isStopped = true;
                        stepOfAction = StepOfAction.Stay;
                        InitCourceOfAction();
                    }

                    break;
                }
            case CourseOfAction.Follow:
                {
                    //補助対象を目的地に
                    destination = followTarget.position;

                    //停止中に補助対象が移動を始めたか判定
                    float sqrDistance = Vector3.SqrMagnitude((Vector3)destination - transform.position);
                    if ((stepOfAction == StepOfAction.Stay)
                        && (sqrDistance > Mathf.Pow(researchDestinationDistance, 2.0f)))
                    {
                        nav.isStopped = false;
                        stepOfAction = StepOfAction.GoTowards;
                        InitCourceOfAction();
                    }
                    //補助対象が停止したかの判定
                    else if ((stepOfAction == StepOfAction.GoTowards)
                            && (Vector3.SqrMagnitude((Vector3)destination - followTarget.position) < 0.1f)
                            && (sqrDistance < Mathf.Pow(arrivalDestinationDistance, 2.0f)))
                    {
                        nav.isStopped = true;
                        stepOfAction = StepOfAction.Stay;
                        InitCourceOfAction();
                    }
                    break;
                }
            default: break;
        }


        //rigidbodyの移動に対する減速処理
        if (rb.velocity.YEqual().sqrMagnitude > 0.0f)
        {
            //水平移動速度が小さければ停止させる
            if (Mathf.Abs(rb.velocity.x) < MIN_MOVE_DISTANCE
                && Mathf.Abs(rb.velocity.z) < MIN_MOVE_DISTANCE)
            {
                rb.velocity = Vector3.zero.YEqual(rb.velocity.y);
            }
            //水平移動速度が小さくなければ減速処理
            else
            {
                //減速(現在の移動方向と逆方向にaccelarateForRbをかける)
                rb.AddForce(rb.velocity.YEqual().normalized * status.RunDeceleration * time.deltaTime / time.fixedDeltaTime, ForceMode.Acceleration);
            }
        }

        //プレイヤーを向けたい方向に体を向ける
        CharacterRotation(nav.velocity.normalized, ROTATE_SPEED_BY_LOW_SPEED);
    }

    /// <summary>
    /// 鉛直移動処理
    /// </summary>
    void VerticalMove()
    {
        //落下速度が落下の最高速度を上回っていない
        if (rb.velocity.y > FALLING_MAX_SPEED)
        {
            //地面に接地していない、もしくはNavMesh上にいない場合
            if (!status.IsGrounded || !nav.isOnNavMesh)
            {
                //重力をかける
                rb.AddForce(status.GravitySize * time.deltaTime / time.fixedDeltaTime, ForceMode.Acceleration);
            }
        }
    }

    /// <summary>
    /// 客観速度処理
    /// </summary>
    void ResultSpeedCheck()
    {
        status.ResultSpeed = nav.velocity.ConvVector2_XZ().magnitude;
    }
}
