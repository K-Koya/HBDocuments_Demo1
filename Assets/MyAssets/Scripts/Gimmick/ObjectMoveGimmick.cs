using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;




/// <summary>
/// 床・障害物等が動くギミックを管理
/// </summary>
public class ObjectMoveGimmick : MyMonoBehaviour
{
    /// <summary>
    /// 移動先順のループ方法
    /// </summary>
    public enum LoopType : byte
    {
        /// <summary>
        /// 最後の地点に着くと最初の地点まで動いてループする
        /// </summary>
        ContinuousMove = 0,
        /// <summary>
        /// 最後の地点に着くと最初の地点までワープしてループする
        /// </summary>
        ContinuousWrap = 1,
        /// <summary>
        /// 最後の地点に着くと逆の順番に最初の地点までたどる
        /// </summary>
        ContinuousReverse = 2,
    }

    /// <summary>
    /// 移動させる方法
    /// </summary>
    public enum HowToMove : byte
    {
        /// <summary>
        /// Transform.positionにVector3を加算する方法
        /// </summary>
        AddPosition = 0,
        /// <summary>
        /// 同じオブジェクトにアタッチされているrigidbodyのVelocityを用いる方法
        /// </summary>
        RigidbodyVelocity = 1,
    }


    /// <summary>
    /// 移動情報
    /// </summary>
    [System.Serializable]
    public class TripInfo
    {
        /// <summary>
        /// 名前
        /// </summary>
        [SerializeField]
        string name = "name";

        /// <summary>
        /// 移動先の座標(初期位置を0ベクトルとした相対座標)
        /// </summary>
        [SerializeField, Tooltip("移動先の座標集(初期位置を0ベクトルとした相対座標)\n最初の要素から順々に移動")]
        Vector3 point = new Vector3();

        /// <summary>
        /// 次の移動先へ向かうまでの速度値
        /// </summary>
        [SerializeField, Tooltip("次の移動先へ向かうまでの加算値\n" +
                                "移動先に到着するたびに次のリストの速度で移動させるようになる\n")]
        float speed = 1.0f;

        /// <summary>
        /// 移動先へ到着した時の滞在時間
        /// </summary>
        [SerializeField, Tooltip("移動先へ到着した時の滞在時間\n" +
                                "移動先に到着した時今のリストの時間だけ滞在する")]
        float stayTime = 0.0f;


        /// <summary>
        /// コンストラクタ 名前 相対位置 速度 滞在時間 を任意に設定
        /// </summary>
        public TripInfo(string name = "", Vector3 point = default, float speed = 1.0f, float stayTime = 0.0f)
        {
            this.name = name;
            this.point = point;
            this.speed = speed;
            this.stayTime = stayTime;
        }

        /* プロパティ */
        public string Name { get => name; set => name = value; }
        public Vector3 Point { get => point; set => point = value; }
        public float Speed { get => speed; set => speed = value; }
        public float StayTime { get => stayTime; set => stayTime = value; }
    }





    /// <summary>
    /// 同オブジェクト内のRigidbodyコンポーネント
    /// </summary>
    RigidbodyTimeline3D rb = default;




    /// <summary>
    /// 停止フラグ
    /// </summary>
    bool isStop = false;




    /// <summary>
    /// ベースとなるTransform.position情報
    /// </summary>
    Vector3 basePosition = default;

    /// <summary>
    /// 何番目の移動先へ向かっているか
    /// </summary>
    int tripCount = 1;

    /// <summary>
    /// 待機時間
    /// </summary>
    float stayTime = 0.0f;

    /// <summary>
    /// 待機中か
    /// </summary>
    bool isStaying = false;




    [Header("移動の設定")]

    /// <summary>
    /// 移動させる方法を指定
    /// </summary>
    [SerializeField, Tooltip("移動させる方法を指定\n同オブジェクト内にrigidbodyがなければ強制的にAddPositionと同じ動作になる")]
    HowToMove howToMove = HowToMove.AddPosition;

    /// <summary>
    /// 移動先ポイントへ向かうための情報をまとめたリスト
    /// 0番目のものを初期位置として1番目のものを先に利用する
    /// </summary>
    [SerializeField, Tooltip("移動情報リスト 0番目のものを初期位置として1番目のものを先に利用する\n" +
                                "移動先座標、速度、到着した時の滞在時間を登録")]
    List<TripInfo> tripInfos = new List<TripInfo>();




    [Header("ループの設定")]
    /// <summary>
    /// 移動先順のループ方法
    /// </summary>
    [SerializeField, Tooltip("移動先順のループ方法")]
    LoopType loopType = LoopType.ContinuousMove;

    /// <summary>
    /// 現在のループ回数
    /// </summary>
    int looptime = 0;

    /// <summary>
    /// loopTypeがContinuousReverseの時のみ使用
    /// 逆順に動かしているところか
    /// </summary>
    bool isReverse = false;

    /// <summary>
    /// ループする回数 0にすると無限にループする
    /// </summary>
    [SerializeField, Tooltip("ループする回数\n0にすると無限にループする")]
    int maxLoopTime = 0;



    /// <summary>
    /// このオブジェクトに停止処理を要求する
    /// </summary>
    /// <param name="doStop">true:停止させる false:停止を解除する</param>
    public void StopRequest(bool doStop = true)
    {
        isStop = doStop;
    }



    /// <summary>
    /// Transform.positionにVector3を加算する方法による移動処理
    /// 到着判定を返り値とする
    /// </summary>
    /// <param name="destiny">目的地座標</param>
    /// <param name="velocityMagnitude">速さ</param>
    /// <returns>目的地への到達フラグ</returns>
    bool moveTowards(Vector3 destiny, float velocityMagnitude)
    {
        //移動
        transform.position = Vector3.MoveTowards(transform.position, destiny + basePosition, velocityMagnitude * Time.deltaTime);

        //到着判定をして返す
        return (destiny + basePosition == transform.position);
    }

    /// <summary>
    /// rigidbodyのVelocityを用いる方法による移動処理
    /// </summary>
    /// <param name="destiny">目的地座標</param>
    /// <param name="velocityMagnitude">速さ</param>
    /// <returns>目的地への到達フラグ</returns>
    bool moveRigidbodyVelocity(Vector3 destiny, float velocityMagnitude)
    {
        //rigidbodyが未定義ならTransform.positionにVector3を加算する方法を利用
        if (rb == null || rb == default)
            return moveTowards(destiny, velocityMagnitude);

        bool isArrival = false;

        //現在地から目的地までの距離
        Vector3 remain = (destiny + basePosition) - transform.position;

        //rigidbody.velocityに代入する変数
        Vector3 velocity = remain.normalized * velocityMagnitude;

        //velocityよりも目的地までの距離のほうが短ければ、目的地までの距離を利用
        if (Mathf.Pow(velocityMagnitude * Time.deltaTime, 2.0f) >= remain.sqrMagnitude)
        {
            velocity = remain / Time.deltaTime;
            isArrival = true;
        }

        //移動
        rb.velocity = velocity;

        return isArrival;
    }


    // Start is called before the first frame update
    void Start()
    {
        //Timeline取得
        TimelineInit();

        //初期のTransform情報を保管
        basePosition = transform.position;

        //Rigidbody取得
        rb = time.rigidbody;
        
        //listに値がない場合は例外回避のため基準値をAdd
        if (tripInfos.Count <= 0) tripInfos.Add(new TripInfo());

        //初期位置を調整
        if (tripInfos[0].Point != Vector3.zero) transform.position += tripInfos[0].Point;
    }

    // Update is called once per frame
    void Update()
    {
        //ポーズ中であるか、停止指示がある場合、処理をしない
        if (IsPausing || isStop) return;

        //最大ループ回数が0でなく、かつループ回数が最大値に達した場合、処理をしない
        if (maxLoopTime != 0 && maxLoopTime <= looptime) return;

        //待機中である
        if(isStaying)
        {
            //タイマーを加算
            stayTime += Time.deltaTime;

            //rigidbodyがあればvelocityを0に
            if (rb != null && rb != default) rb.velocity = Vector3.zero;

            //待機時間が終了した
            if (stayTime >= tripInfos[tripCount].StayTime)
            {
                //次のフレームから移動処理を再開
                isStaying = false;

                //ループ方法に応じてカウントアップ
                switch (loopType)
                {
                    case LoopType.ContinuousMove:
                        {
                            //カウントアップ
                            tripCount = (tripCount + 1) % tripInfos.Count;
                            break;
                        }
                    case LoopType.ContinuousWrap:
                        {
                            //カウントアップ
                            tripCount = (tripCount + 1) % tripInfos.Count;
                            //ワープ処理
                            if (tripCount <= 0) transform.position = basePosition + tripInfos[0].Point;
                            break;
                        }
                    case LoopType.ContinuousReverse:
                        {
                            //カウントダウン
                            if (isReverse)
                            {
                                tripCount--;
                                if (tripCount <= 0) isReverse = false;
                            }
                            //カウントアップ
                            else
                            {
                                tripCount++;
                                if (tripCount >= tripInfos.Count - 1) isReverse = true;
                            }

                            break;
                        }
                }
            }

            return;
        }

        //到着フラグ
        bool isArrival = false;


        //移動方法別に処理を設定
        switch (howToMove)
        {
            case HowToMove.AddPosition:
                {
                    //移動処理&到着判定
                    isArrival = moveTowards(tripInfos[tripCount].Point, tripInfos[tripCount].Speed);
                    break;
                }
            case HowToMove.RigidbodyVelocity:
                {
                    //移動処理&到着判定
                    isArrival = moveRigidbodyVelocity(tripInfos[tripCount].Point, tripInfos[tripCount].Speed);
                    break;
                }
        }

        //到着した
        if (isArrival)
        {
            //待機フラグを立てる
            isStaying = true;

            //待機時間初期化
            stayTime = 0.0f;

            //初期位置(0番目の要素)に到着するたびにlooptimeを加算
            if (tripCount <= 0) looptime++;
        }
    }
}
