using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>
/// プレイヤー用の移動スクリプト
/// </summary>
public class MoveForPlayer : MoveForAbstruct
{
    /// <summary>
    /// 入力情報を持つコンポーネント
    /// </summary>
    MyInputManager input = default;

    /// <summary>
    /// 移動入力のうち、カメラの向きを正面とした二次元座標を保管
    /// </summary>
    Vector3 inputHorizontalDirection = Vector3.zero;

    /// <summary>
    /// プレイヤーが操作できるカメラ
    /// </summary>
    GameObject playerCamera = default;
    /// <summary>
    /// playerCameraを探すためのカメラタグ
    /// </summary>
    [SerializeField]
    string playerCameraTag = "MainCamera";
    
    

    



    // Start is called before the first frame update
    void Start()
    {
        TimelineInit();
        rb = time.rigidbody;
        playerCamera = GameObject.FindWithTag(playerCameraTag);
        status = GetComponent<Status>();
        input = FindObjectOfType<MyInputManager>();
        layerNumberOfGround = (byte)LayerMask.NameToLayer(layerNameOfGround);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPausing) return;

        GroundedCheck();
        HorizontalMove();
        VerticalMove();

        ResultSpeedCheck();
    }

    void FixedUpdate()
    {
        rb.AddForce(moveDirectionHorizontal, ForceMode.Acceleration);
        rb.AddForce(moveDirectionVertical, ForceMode.Acceleration);
    }



    /// <summary>
    /// 水平移動処理
    /// </summary>
    void HorizontalMove()
    {
        //rigidbodyにAddForceのAccelarateをかけるためのVector
        Vector3 accelarateForRb = Vector3.zero;

        //基本情報をローカルで定義
        //カメラ視点向き
        Transform cameraTransform = playerCamera.transform;
        //カメラ視点の正面(y軸無視)
        Vector3 forward = cameraTransform.forward.YEqual();
        forward = forward.normalized;
        //カメラ視点の右方向
        Vector3 right = cameraTransform.right;

        //プレイヤーの移動入力を取得
        float horizontal = input.Move.Axis2d.x;
        float vartical = input.Move.Axis2d.y;
        //ただし、行動できる状態に限る
        if (status.IsCommandRunning || status.IsFlirting || status.IsDefeated)
        {
            horizontal = 0.0f;
            vartical = 0.0f;
        }

        //プレーヤーを移動させることができる状態なら、移動させたい度合・方向を取得
        inputHorizontalDirection = Vector3.Normalize(horizontal * right + vartical * forward);



        //プレーヤーを移動させたい度合・方向が入力されていれば、移動方向および速度を計算
        if (inputHorizontalDirection.sqrMagnitude > 0.0f)
        {
            //移動方向は、空中にいれば向きに関係なく、地上ならキャラクターの向きへ
            //旋回速度は、5[m/s]以下か走行速度か、空中にいるかで決定
            float rotate = ROTATE_SPEED_BY_LOW_SPEED;
            if (!status.IsGrounded)
            {
                rotate = ROTATE_SPEED_ARIAL;
            }
            else if (status.ResultSpeed > ABLE_QUICK_ROTATE_SPEED)
            {
                rotate = status.MaxRotateSpeed;
            }


            //プレイヤーを向けたい方向に体を向ける
            CharacterRotation(inputHorizontalDirection, rotate);


            //速度値が最高速度以下なら加速度を設定、以上なら0
            float sqrMaxSpeed = Mathf.Pow(status.MaxWalkSpeed, 2.0f);
            //まず歩行判定と処理
            if(input.Move.NowPushType != PushType.doublePush)
            {
                sqrMaxSpeed = Mathf.Pow(status.MaxRunSpeed, 2.0f) * inputHorizontalDirection.sqrMagnitude;
            }
            //現速度と比較
            if (Mathf.Pow(status.ResultSpeed, 2.0f) < sqrMaxSpeed)
            {
                if (status.IsGrounded)
                {
                    accelarateForRb = transform.forward * status.RunAcceleration;
                }
                else
                {
                    accelarateForRb = inputHorizontalDirection.normalized * status.RunAcceleration;
                }
            }
            else
            {
                accelarateForRb = Vector3.zero.YEqual(rb.velocity.y);
            }
        }
        //playerDirectionが0で移動入力がなく、かつ移動速度がzeroでなければ減速処理
        else if (status.ResultSpeed > 0.0f)
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
                accelarateForRb = rb.velocity.YEqual().normalized * status.RunDeceleration;
            }
        }

        //地上時は速度(向き)を、入力方向へ設定
        if (status.IsGrounded)
        {
            float velY = rb.velocity.y;
            Vector3 vel = Quaternion.FromToRotation(rb.velocity.YEqual(), transform.forward) * rb.velocity;
            rb.velocity = vel.YEqual(velY);
        }

        //加速度を設定
        moveDirectionHorizontal = accelarateForRb;
    }

    /// <summary>
    /// 鉛直移動処理
    /// </summary>
    void VerticalMove()
    {
        //落下速度が落下の最高速度を上回っていない
        if (rb.velocity.y > FALLING_MAX_SPEED)
        {
            //重力をかける
            if (!status.IsGrounded || inputHorizontalDirection.sqrMagnitude > 0.0f)
            {
                moveDirectionVertical = status.GravitySize;
            }
        }


        //きつい坂ではない地面についているか
        if (status.IsGrounded)
        {
            //怯み中ややられ状態でなくジャンプキーが押されたか
            if ((!status.IsFlirting && !status.IsDefeated)
                && input.Jump.NowPushDown == PushType.onePush)
            {
                //ジャンプ上昇フラグを立てる
                status.IsJumping = true;
                //重力方向とは逆方向に、ジャンプ初速度を加算
                rb.AddForce(-(status.GravitySize.normalized * status.JumpFirstSpeed), ForceMode.VelocityChange);
            }
        }
        else
        {
            //ジャンプ上昇中かつジャンプキーが離されたら、少しの上昇力を残しつつジャンプ終了
            if ((status.IsJumping && rb.velocity.y > 0.0f)
                && input.Jump.NowPushType == PushType.noPush)
            {
                status.IsJumping = false;
                rb.velocity = rb.velocity.YEqual();
            }
            else
            {
                //各種ジャンプ上昇中でなくなれば落下状態へ
                if (status.IsJumping && rb.velocity.y <= 0.0f)
                {
                    status.IsJumping = false;
                }
            }

            //落下速度が落下最高速度を上回らないようにする
            rb.velocity = rb.velocity.YEqual(Mathf.Max(FALLING_MAX_SPEED, rb.velocity.y));
        }
    }

    /// <summary>
    /// 客観速度計測
    /// </summary>
    void ResultSpeedCheck()
    {
        status.ResultSpeed = rb.velocity.ConvVector2_XZ().magnitude;
    }
}
