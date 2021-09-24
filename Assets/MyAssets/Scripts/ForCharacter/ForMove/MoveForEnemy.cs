using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Chronos;

/// <summary>
/// 敵キャラクターの移動スクリプト
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class MoveForEnemy : MoveForAbstruct
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
    /// キャラクターの向き
    /// </summary>
    Vector3 characterDirection = Vector3.zero;

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
    /// 通常コンボの入力
    /// </summary>
    PushType combo = PushType.noPush;

    /// <summary>
    /// 回避の入力
    /// </summary>
    PushType dodge = PushType.noPush;

    /// <summary>
    /// 各コマンドの入力
    /// </summary>
    PushType[] commands = new PushType[12];


    public PushType Combo { get => combo; set => combo = value; }
    public PushType Dodge { get => dodge; set => dodge = value; }
    public PushType[] Commands { get => commands; set => commands = value; }
    public Transform FollowTarget { get => followTarget; set => followTarget = value; }
    public Transform AttackTarget { get => attackTarget; set => attackTarget = value; }



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

        /* 入力初期化 */
        combo = PushType.noPush;
        dodge = PushType.noPush;
        for (int i = 0; i < commands.Length; i++) commands[i] = PushType.noPush;

        GroundedCheck();
        NavMeshStop();
        NavMeshMove();
        VerticalMove();

        ResultSpeedCheck();
    }

    /// <summary>
    /// ステータス値より、NavMeshによる移動を停止させる
    /// </summary>
    void NavMeshStop()
    {
        //コマンド実行中、怯み中、倒された時はNavMeshを停止させる
        if (status.IsCommandRunning)
        {
            nav.isStopped = true;
        }
        else if (status.IsFlirting || status.IsDefeated)
        {
            nav.isStopped = true;
            nav.velocity = Vector3.zero;
            rb.velocity = Vector3.zero.YEqual(rb.velocity.y);
        }
        else
        {
            nav.isStopped = false;
        }
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
                bool isfound = Physics.Raycast(destination.Value + transform.up * status.Height, -transform.up, out hitInfo, 100.0f, layerOfGround);

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
            case CourseOfAction.Wander:
                {
                    //補助対象を中心にランダム位置を目的地に
                    destination = followTarget.position + new Vector3(Random.Range(-15f, 15f), 0f, Random.Range(-15f, 15f));
                    //移動性能は自分の値で
                    nav.speed = status.MaxRunSpeed;
                    nav.acceleration = status.RunAcceleration;
                    break;
                }
            case CourseOfAction.Chase:
                {
                    //補助対象を目的地に
                    destination = attackTarget.position;
                    break;
                }
            case CourseOfAction.Fight:
                {
                    if(stepOfAction == StepOfAction.Attack)
                    {
                        //敵との距離分ステップを踏む
                        Vector3 step = attackTarget.position - transform.position;
                        rb.AddForce(step * 1.5f, ForceMode.VelocityChange);
                    }
                    else if(stepOfAction == StepOfAction.Dodge)
                    {
                        //右に飛ぶか左に飛ぶか選択
                        Vector3 step = transform.right;
                        if (Random.value < 0.5f) step *= -1f;
                        //ステップを踏む
                        rb.AddForce(transform.forward + step * 5.0f, ForceMode.VelocityChange);
                    }
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
                    float sqrDistance = Vector3.SqrMagnitude(destination.Value - transform.position);
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

                    characterDirection = nav.velocity.normalized;


                    break;
                }
            case CourseOfAction.Follow:
                {
                    //補助対象を目的地に
                    destination = followTarget.position;

                    //停止中に補助対象が移動を始めたか判定
                    float sqrDistance = Vector3.SqrMagnitude(destination.Value - transform.position);
                    if ((stepOfAction == StepOfAction.Stay)
                        && (sqrDistance > Mathf.Pow(researchDestinationDistance, 2.0f)))
                    {
                        nav.isStopped = false;
                        stepOfAction = StepOfAction.GoTowards;
                        InitCourceOfAction();
                    }
                    //補助対象が停止したかの判定
                    else if ((stepOfAction == StepOfAction.GoTowards)
                            && (Vector3.SqrMagnitude(destination.Value - followTarget.position) < 0.1f)
                            && (sqrDistance < Mathf.Pow(arrivalDestinationDistance, 2.0f)))
                    {
                        nav.isStopped = true;
                        stepOfAction = StepOfAction.Stay;
                        InitCourceOfAction();
                    }

                    characterDirection = nav.velocity.normalized;

                    break;
                }
            case CourseOfAction.Wander:
                {
                    //目的地が未定義なら検索
                    float sqrDistance = Vector3.SqrMagnitude(destination.Value - transform.position);
                    if (stepOfAction == StepOfAction.Stay
                        && Random.value < 0.02f)
                    {
                        nav.isStopped = false;
                        stepOfAction = StepOfAction.GoTowards;
                        InitCourceOfAction();
                    }
                    //目的地にたどり着いたかの判定
                    else if (stepOfAction == StepOfAction.GoTowards
                            && sqrDistance < Mathf.Pow(arrivalDestinationDistance, 2.0f))
                    {
                        nav.isStopped = true;
                        stepOfAction = StepOfAction.Stay;
                        InitCourceOfAction();
                    }

                    characterDirection = nav.velocity.normalized;

                    break;
                }
            case CourseOfAction.Chase:
                {
                    //敵対象を目的地に
                    destination = attackTarget.position;

                    //停止中に補助対象が移動を始めたか判定
                    float sqrDistance = Vector3.SqrMagnitude(destination.Value - transform.position);
                    if ((stepOfAction == StepOfAction.Stay)
                        && (sqrDistance > Mathf.Pow(researchDestinationDistance, 2.0f)))
                    {
                        nav.isStopped = false;
                        stepOfAction = StepOfAction.GoTowards;
                        InitCourceOfAction();
                    }
                    //補助対象に近づいたかの判定
                    else if ((stepOfAction == StepOfAction.GoTowards)
                            && (sqrDistance < Mathf.Pow(arrivalDestinationDistance, 2.0f)))
                    {
                        nav.isStopped = true;
                        courseOfAction = CourseOfAction.Fight;
                        stepOfAction = StepOfAction.Stay;
                        InitCourceOfAction();
                    }

                    characterDirection = nav.velocity.normalized;

                    break;
                }
            case CourseOfAction.Fight:
                {
                    //補助対象を目的地に
                    destination = attackTarget.position;
                    //停止中に補助対象が移動を始めたか判定
                    float sqrDistance = Vector3.SqrMagnitude(destination.Value - transform.position);
                    if ((stepOfAction == StepOfAction.Stay)
                        && (sqrDistance > Mathf.Pow(researchDestinationDistance, 2.0f)))
                    {
                        nav.isStopped = false;
                        courseOfAction = CourseOfAction.Chase;
                        stepOfAction = StepOfAction.GoTowards;
                        InitCourceOfAction();
                    }
                    else
                    {
                        //待機状態のとき攻撃、攻撃せずに待機、横移動のいずれかを選択
                        if (stepOfAction == StepOfAction.Stay)
                        {
                            float rand = Random.value;
                            if (rand > 0.995f)
                            {
                                stepOfAction = StepOfAction.Attack;
                                combo = PushType.onePush;
                                InitCourceOfAction();
                            }
                            else if (rand > 0.9f)
                            {
                                stepOfAction = StepOfAction.Dodge;
                                dodge = PushType.onePush;
                                InitCourceOfAction();
                            }

                            characterDirection = attackTarget.position - transform.position;
                        }
                        else if (stepOfAction == StepOfAction.Attack)
                        {
                            if (!status.IsCommandRunning)
                            {
                                stepOfAction = StepOfAction.Stay;    
                            }
                        }
                        else if (stepOfAction == StepOfAction.Dodge)
                        {
                            if (!status.IsCommandRunning)
                            {
                                if (sqrDistance > Mathf.Pow(researchDestinationDistance, 2.0f))
                                {
                                    nav.isStopped = false;
                                    courseOfAction = CourseOfAction.Chase;
                                    stepOfAction = StepOfAction.GoTowards;
                                    InitCourceOfAction();
                                }
                                else if (Random.value > 0.4f)
                                {
                                    stepOfAction = StepOfAction.Attack;
                                    combo = PushType.onePush;
                                    characterDirection = attackTarget.position - transform.position;
                                    InitCourceOfAction();
                                }
                                else
                                {
                                    stepOfAction = StepOfAction.Stay;
                                }
                            }
                        }
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
        CharacterRotation(characterDirection, ROTATE_SPEED_BY_LOW_SPEED);
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
                if(time.deltaTime > 0f) rb.AddForce(status.GravitySize * time.deltaTime / time.fixedDeltaTime, ForceMode.Acceleration);
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
