using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNA2DGame
{
    /// <summary>
    /// 移動方向列挙
    /// </summary>
    enum MoveDirection
    {
        DOWN    = -1,   // 下方向
        UP      = 1,    // 上方向
    }



    /// <summary>
    /// 大砲クラス
    /// </summary>
    class Cannon : Character
    {
        // メンバー変数
        bool    turnFlg;    // 移動方向転換フラグ
        bool    shotFlg;    // 発射フラグ
        int     timer;      // タイマー
        float   startAccelerateTime;    // 加速を開始する時間
        float   startDecelerateTime;    // 減速を開始する時間
        float   startShotTime;          // 発射を開始する時間
        float   movement;               // 移動量  等加速度運動用にフィールド化

        MoveDirection   moveDirection;  // 移動方向



        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="time"></param>
        /// <param name="stage"></param>
        /// <param name="pos"></param>
        /// <param name="dir"></param>
        /// <param name="mdir"></param>
        public Cannon( 
            float           speed, 
            float           time,
            Stage           stage, 
            Vector3         pos, 
            Direction       dir, 
            MoveDirection   mdir 
        ) : 
            base (
                "red",                  // 名前
                speed,                  // 速さ
                new Vector2( 64, 64 ),  // 画像サイズ
                stage                   // ステージ
            )
        {
            this.position   = pos;  // 座標
            this.direction  = dir;  // 方向

            turnFlg = false;    // 移動方向転換フラグ
            shotFlg = false;    // 発射フラグ

            startAccelerateTime = 60 * time;                           // 加速を開始する時間
            startDecelerateTime = startAccelerateTime + 60 * time;     // 減速を開始する時間
            startShotTime       = startDecelerateTime + 60 * time;     // 発射を開始する時間
            
            timer               = (int)startAccelerateTime;     // タイマー
            this.moveDirection  = mdir;                         // 移動方向
            movement            = 0.0f;                         // 移動量
        }



        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize() { }



        /// <summary>
        /// 更新
        /// </summary>
        public override void Update()
        {
            ZBuffer();              // Z値の更新
            XBuffer();              // X値の更新
            UpdateSize();           // サイズの更新
            CheckAboveTheStage();   // ステージより上に有るか判定
            Move();                 // 移動
            InScreen();             // 移動範囲の制限
        }



        /// <summary>
        /// 描画
        /// </summary>
        public override void Draw() { }



        /// <summary>
        /// 衝突応答
        /// </summary>
        public override void Hit( Vector3 hitPos ) { }



        /// <summary>
        /// 鉄球の生成(発射)
        /// </summary>
        /// <returns></returns>
        public Character CreateIronBall() 
        {
            // 発射不可能なら生成しない
            if ( !(shotFlg) ) return null;

            // 発射不可能にする
            shotFlg = false;



            const float minVelocityZero = 15.0f;
            const float minAngle = (float)( 45 * (Math.PI / 180) );

            float checkPosX;
            float plus;
            if ( position.X < Stage.stagePosition.X + (Stage.stageSize.X / 2) ) 
            {
                checkPosX = stage.LeftSideLimit( position.Z );
            }
            else
            {
                checkPosX = stage.RightSideLimit( position.Z );
            }
            

            plus = Math.Abs( (checkPosX - position.X) / minVelocityZero / 3 );



            // 鉄球を生成して返す
            return new IronBall( stage, position, direction, minAngle, minVelocityZero + plus );
        }



        /// <summary>
        /// 移動
        /// </summary>
        private void Move() 
        {
            Vector2 pos     = new Vector2( position.X, position.Z + position.Y );   // 座標
            double  angle   = stage.GetAngleFromStageVanishingPointToPoint(         // 角度
                                  pos,
                                  VanishingPoint.CENTER
                              );

            float velocity      = speed * zBuffer;  // 速さ
            float acceleration  = 1 * 0.1f;         // 加速度の大きさ



            // タイムカウント
            ++timer;


            // 減速を開始する時間になったら減速
            if ( timer > startDecelerateTime )
            {
                // 減速
                movement -= acceleration;
            }
            else
            {
                // 加速を開始する時間になったら加速
                if ( timer > startAccelerateTime )
                {
                    // 加速
                    movement += acceleration;
                }
            }



            // 移動量制限

            float movementMax = 5.0f;       // 移動量の最大
            float movementMin = 0.0f;       // 移動量の最小

            // 移動量が最大に達したら等速にする
            if ( movement > movementMax )
            {
                movement = movementMax;
            }
            // 移動量が0に達したら停止する
            if ( movement < movementMin )
            {
                movement = movementMin;

                // 発射を開始する時間になったら発射
                if ( timer > startShotTime )
                {
                    // 発射可能にする
                    shotFlg = true;

                    // タイマーを初期化
                    timer   = 0;
                }
            }



            // 移動ベクトル
            Vector3 moveVector = Vector3.Zero;
            moveVector.X = movement * velocity * (float)Math.Cos( angle ) * (int)moveDirection;
            moveVector.Z = movement * velocity * (float)Math.Sin( angle ) * (int)moveDirection;



            // 移動方向転換フラグの切り替え

            float stageLowerLimit = Stage.stageUpPosY - size.Y / 2;     // ステージの上の限界
            float stageUpperLimit = Stage.stageDownPosY - size.Y / 2;   // ステージの下の限界

            if ( position.Z < stageLowerLimit ||
                 position.Z > stageUpperLimit )
            {
                turnFlg = ( !turnFlg )? true : false;
            }


            // 移動ベクトルの切り替え
            switch ( turnFlg )
            {
                case true: break;

                // 移動方向を反対にする
                case false: moveVector = -moveVector; break;
            }



            // 座標を更新
            position += moveVector;
        }



        /// <summary>
        /// 移動範囲の制限
        /// </summary>
        private void InScreen() 
        {
            float stageLowerLimit       = Stage.stageUpPosY - size.Y / 2;                // ステージの上の限界
            float stageUpperLimit       = Stage.stageDownPosY - size.Y / 2;              // ステージの下の限界
            //float stageLeftSideLimit    = stage.LeftSideLimit( position.Z )  - 100;      // ステージの左の限界
            //float stageRightSideLimit   = stage.RightSideLimit( position.Z ) + 100;      // ステージの右の限界


            
            if ( position.Z < stageLowerLimit )     position.Z = stageLowerLimit;
            if ( position.Z > stageUpperLimit )     position.Z = stageUpperLimit;
            //if ( position.X < stageLeftSideLimit )  position.X = stageLeftSideLimit;
            //if ( position.X > stageRightSideLimit ) position.X = stageRightSideLimit;
        }
    }
}
