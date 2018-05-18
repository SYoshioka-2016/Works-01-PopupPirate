using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNA2DGame
{
    /// <summary>
    /// 鉄球クラス
    /// </summary>
    class IronBall : Character
    {
        // メンバー変数

        private bool    fallCheckFlg;   // 落下判定用フラグ
        private float   velocityZero;   // 初速
        private Vector3 velocity;       // 速度


        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="stage"></param>
        /// <param name="pos"></param>
        /// <param name="dir"></param>
        /// <param name="angle"></param>
        /// <param name="vZero"></param>
        public IronBall( 
            Stage       stage, 
            Vector3     pos, 
            Direction   dir, 
            double      angle, 
            float       vZero 
        ) :
            base( 
                "ball",                 // 名前
                3,                      // 速さ
                new Vector2( 64, 64 ),  // 画像サイズ
                stage                   // ステージ
            )
        {
            this.position   = pos;      // 座標
            this.direction  = dir;      // 方向
            fallCheckFlg    = false;    // 落下判定用フラグ

            velocityZero    = vZero;            // 初速
            velocity        = Vector3.Zero;     // 速度



            // 速度の成分を設定
            // 垂直方向の初速(vo * cos(theta))
            switch ( direction ) 
            {
                // 左
                case Direction.LEFT:    
                    velocity.X = -velocityZero * (float)Math.Cos( angle ); 
                    break;

                // 右
                case Direction.RIGHT:   
                    velocity.X =  velocityZero * (float)Math.Cos( angle ); 
                    break;

                default: break;
            }

            // 鉛直方向の初速(vo * sin(theta))
            velocity.Y = -velocityZero * (float)Math.Sin( angle );
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
        /// 衝突応答
        /// </summary>
        public override void Hit( Vector3 hitPos ) 
        {
            isDead = true;
        }



        /// <summary>
        /// 移動
        /// </summary>
        private void Move()
        {
            float gravity = 0.98f;  // 重力

            // 鉛直方向の自由落下
            velocity.Y += gravity;


            // 座標を更新
            position += velocity * zBuffer;
        }



        /// <summary>
        /// 移動範囲の制限
        /// </summary>
        private void InScreen() 
        {
            float stageLowerLimit       = Stage.stageDownPosY - size.Y / 2;                     // ステージの上の限界
            float stageUpperLimit       = Stage.stageUpPosY - size.Y / 2;                       // ステージの下の限界
            float stageLeftSideLimit    = stage.LeftSideLimit( position.Z )  - size.X / 2;      // ステージの左の限界
            float stageRightSideLimit   = stage.RightSideLimit( position.Z ) + size.X / 2;      // ステージの右の限界



            // ステージの外に出たら
            if ( !stage.IsInStage( this ) )
            {
                // ステージの外でY座標が0以上になったら落下
                if ( position.Y >= 0 )
                {
                    // 落下判定用フラグON
                    fallCheckFlg = true;
                }
            }
            // ステージ内なら
            else 
            {
                // ステージ内で、落下判定用フラグがOFFで、Y座標が0を超えたら着地
                if ( !fallCheckFlg && position.Y > 0 ) 
                {
                    position.Y      = 0;    // Y座標


                    // 水平方向はほぼ等速
                    velocity.X *= 0.999f;

                    // 鉛直方向の跳ね返り
                    velocity.Y *= -0.6f;
                }
            }


            // 下まで落ちたら死亡
//            if ( position.Y > Screen.Height )
            if ( position.Y > 60 )
            {
                isDead = true;
            }
        }
    }
}
