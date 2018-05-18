using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNA2DGame
{
    /// <summary>
    /// プレイヤークラス
    /// </summary>
    class Player : Character
    {
        // メンバー変数
        private int         stock;                      // 残機
        private int         timer;                      // カウントタイマー
        private int         animeTimer;                 // アニメーションタイマー
        private int         invincibleModeCountTimer;   // 無敵モードのカウンター
        private float       velocityZero;               // 初速
        private float       leapParameter;              // 飛距離のパラメーター
        private bool        fallCheckFlg;               // 落下判定用フラグ
        private Vector3     moveVector;                 // 移動ベクトル
        private ActionMode  mode;                       // 行動モード

        private const int interval = 20;    // ダメージから動き出すまでの間隔



        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="stage"></param>
        public Player( Stage stage ) : 
            base(
                "player",               // 名前
                5,                      // 速さ
                new Vector2( 72, 96 ),  // 画像サイズ
                stage                   // ステージ
            )
        {
        }



        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize() 
        {
            stock                       = 3;                    // 残機
            timer                       = 0;                    // カウントタイマー
            animeTimer                  = 0;                    // アニメーションタイマー
            invincibleModeCountTimer    = -1;                   // 無敵モードのカウンター
            leapParameter               = 1.0f;                 // 飛距離のパラメーター
            moveVector                  = Vector3.Zero;         // 移動ベクトル
            mode                        = ActionMode.STAND;     // 行動モード 
            isDead                      = false;                // 死亡情報
            fallCheckFlg                = false;                // 落下判定用フラグ
            direction                   = Direction.DOWN;       // 方向

            

            ZBuffer();      // Z値の更新
            XBuffer();      // X値の更新

            InitializePosition();  // 座標の初期化
        }



        /// <summary>
        /// 更新
        /// </summary>
        public override void Update() 
        {
            Move();                 // 移動
            InScreen();             // 移動範囲の制限
            Jump();                 // ジャンプ
            Fall();                 // 落下
            Damage();               // ダメージ
            ZBuffer();              // Z値の更新
            XBuffer();              // X値の更新
            UpdateSize();           // サイズの更新
            CheckAboveTheStage();   // ステージより上に有るか判定
            EnableCollision();      // 衝突判定を取るかの設定


            // カウントアップ
            timer++;




            InvincibleMode();
        }



        /// <summary>
        /// 描画
        /// </summary>
        public override void Draw() 
        {
            // 死亡なら描画しない
            if ( isDead ) return;

            int     alpha   = 255;    // アルファ

            Vector2 pos     = new Vector2( position.X, position.Z + position.Y ) - (size / 2);  // 座標
            Vector2 scale   = new Vector2( zBuffer, zBuffer );



            // 無敵モードなら点滅表示
            if ( IsInvincibleMode() ) 
            {
                // 1フレーム毎に点滅
                if ( invincibleModeCountTimer % 2 == 0 ) 
                {
                    alpha = 255;
                }
                else 
                {
                    alpha = 100;
                }
            }



            // アニメーション描画
            DrawAnimation( pos, scale, alpha );
        }



        /// <summary>
        /// 衝突応答
        /// </summary>
        public override void Hit( Vector3 hitPos ) 
        {
            // ダメージモードか無敵モードなら処理しない
            if ( mode == ActionMode.DAMAGE || IsInvincibleMode() ) return;



            // ダメージモードにする
            mode = ActionMode.DAMAGE;


            // プレイヤーから相手の角度
            double angle = Math.Atan2( 
                               ((position.Z) - (hitPos.Z)), 
                               position.X - hitPos.X 
                           );
            

            velocityZero    = 1.0f;             // 初速
            moveVector      = Vector3.Zero;     // 移動ベクトル

            // 移動ベクトルの設定
            moveVector.X =  velocityZero * (float)Math.Cos( angle ) * 1.5f;
            moveVector.Y = -velocityZero * (float)Math.Sin(MathHelper.ToRadians(45)) * 15 * leapParameter;
            moveVector.Z =  velocityZero * (float)Math.Cos( angle ) * 1.5f;

            // 落下判定用フラグOFF
            fallCheckFlg = false;

            // タイマーの初期化
            timer = 0;

            // 飛距離のパラメーターを更新
            leapParameter += 0.25f;



            // 無敵モードにする
            invincibleModeCountTimer = 0;
        }



        /// <summary>
        /// 残機ゲッター
        /// </summary>
        /// <returns></returns>
        public int GetStock() { return stock; }



        /// <summary>
        /// アニメーション描画
        /// </summary>
        private void DrawAnimation( Vector2 pos, Vector2 scale, int alpha ) 
        {
            // 死亡なら描画しない
            if ( isDead ) return;



            // 切り出し位置番号
            int sx = 0, sy = 0;

            // 1コマのカウント数
            int count = 0;

            // モード別に処理
            switch ( mode ) 
            {
                // 立ち
                case ActionMode.STAND:

                // ジャンプ
                case ActionMode.JUMP:

                    // 6カウント3パターン
                    count = (int)60.0f / 10;
                    if ( count * 3 <= animeTimer ) 
                    {
                        animeTimer = 0;
                    }

                    sx = animeTimer / count;

                    break;

                // ダメージ
                case ActionMode.DAMAGE:

                    sx = 2;

                    break;


                default: break;
            }

            // 方向別に処理
            switch ( direction ) 
            {
                case Direction.UP:      sy = 0; break;
                case Direction.RIGHT:   sy = 1; break;
                case Direction.DOWN:    sy = 2; break;
                case Direction.LEFT:    sy = 3; break;
            }

            // 切り出し位置
            sx *= (int)formerSize.X;
            sy *= (int)formerSize.Y;
            Rectangle rect = new Rectangle( sx, sy, (int)formerSize.X, (int)formerSize.Y );

            // 影の描画
            shadow.DrawShadow( position, scale, size );

            // プレイヤーを描画
            Renderer.Instance.DrawTexture( name, pos, rect, alpha, scale );



            // タイマーの更新
            animeTimer++;
        }



        /// <summary>
        /// 移動
        /// </summary>
        private void Move() 
        {
            // ダメージモードなら処理しない
            if ( mode == ActionMode.DAMAGE ) return;


            Vector2 pos             = new Vector2(                      // 座標
                                         position.X, 
                                         position.Z
                                      );
            double  angle           = 0.0;                              // 角度
            float   velocity        = speed * zBuffer;                  // 速度
            float   weight          = xBuffer * zBuffer;                // 重り


            // 移動ベクトルの初期化
            moveVector.X = 0;
            moveVector.Z = 0;



            // 移動
            // 右
            if ( InputKeyboard.Instance.State( Keys.Right ) == InputKeyboard.KS.PRESS )
            {
                angle           = Math.Atan2( 0, 1 );
                moveVector.X    = velocity * (float)( Math.Cos( angle ) );
                direction       = Direction.RIGHT;
            }
            // 左
            if ( InputKeyboard.Instance.State( Keys.Left )  == InputKeyboard.KS.PRESS )
            {
                angle           = Math.Atan2( 0, -1 );
                moveVector.X    = velocity * (float)( Math.Cos( angle ) );
                direction       = Direction.LEFT;
            }
            // 下
            if ( InputKeyboard.Instance.State( Keys.Down )  == InputKeyboard.KS.PRESS )
            {
                // 上から下方向
                angle           = stage.GetAngleFromStageVanishingPointToPoint( pos, VanishingPoint.CENTER );
                moveVector.X    = velocity * (float)( Math.Cos( angle ) );
                moveVector.Z    = velocity * (float)( Math.Sin( angle ) ) * weight;
                direction       = Direction.DOWN;
            }
            // 上
            if ( InputKeyboard.Instance.State( Keys.Up )    == InputKeyboard.KS.PRESS )
            {
                // 下から上方向
                angle           = stage.GetAngleFromPointToStageVanishingPoint( pos, VanishingPoint.CENTER );
                moveVector.X    = velocity * (float)( Math.Cos( angle ) );
                moveVector.Z    = velocity * (float)( Math.Sin( angle ) ) * weight;
                direction       = Direction.UP;
            }
            // 右下
            if ( InputKeyboard.Instance.State( Keys.Right ) == InputKeyboard.KS.PRESS &&
                 InputKeyboard.Instance.State( Keys.Down )  == InputKeyboard.KS.PRESS )
            {
                // 左上から右下方向
                angle           = stage.GetAngleFromStageVanishingPointToPoint( pos, VanishingPoint.LEFT );
                moveVector.X    = velocity * (float)( Math.Cos( angle ) );
                moveVector.Z    = velocity * (float)( Math.Sin( angle ) ) * weight;
                direction       = Direction.DOWN;
            }
            // 右上
            if ( InputKeyboard.Instance.State( Keys.Right ) == InputKeyboard.KS.PRESS &&
                 InputKeyboard.Instance.State( Keys.Up )    == InputKeyboard.KS.PRESS )
            {
                // 左下から右上方向
                angle           = stage.GetAngleFromPointToStageVanishingPoint( pos, VanishingPoint.RIGHT );
                moveVector.X    = velocity * (float)( Math.Cos( angle ) );
                moveVector.Z    = velocity * (float)( Math.Sin( angle ) ) * weight;
                direction       = Direction.UP;
            }
            // 左下
            if ( InputKeyboard.Instance.State( Keys.Left )  == InputKeyboard.KS.PRESS &&
                 InputKeyboard.Instance.State( Keys.Down )  == InputKeyboard.KS.PRESS )
            {
                // 右上から左下方向
                angle           = stage.GetAngleFromStageVanishingPointToPoint( pos, VanishingPoint.RIGHT );
                moveVector.X    = velocity * (float)( Math.Cos( angle ) );
                moveVector.Z    = velocity * (float)( Math.Sin( angle ) ) * weight;
                direction       = Direction.DOWN;
            }
            // 左上
            if ( InputKeyboard.Instance.State( Keys.Left )  == InputKeyboard.KS.PRESS &&
                 InputKeyboard.Instance.State( Keys.Up )    == InputKeyboard.KS.PRESS )
            {
                // 右下から左上方向
                angle           = stage.GetAngleFromPointToStageVanishingPoint( pos, VanishingPoint.LEFT );
                moveVector.X    = velocity * (float)( Math.Cos( angle ) );
                moveVector.Z    = velocity * (float)( Math.Sin( angle ) ) * weight;
                direction       = Direction.UP;
            }




/*
            Vector2 thumStickVector = InputGamePad.Instance.LeftThumbStickVector();
            float f = 0, f2 = 0.3f;
            if ( f != thumStickVector.Length() ) {

                if ( f < thumStickVector.X ) {

                    if ( f2 < thumStickVector.Y ) {
                        
                        angle     = stage.GetAngleFromPointToStageVanishingPoint( pos, VanishingPoint.RIGHT );
                        direction = Direction.UP;
                    }
                    else if ( -f2 > thumStickVector.Y ) {
                    
                        angle     = stage.GetAngleFromStageVanishingPointToPoint( pos, VanishingPoint.LEFT );
                        direction = Direction.DOWN;
                    }
                    else {
                    
                        angle     = Math.Atan2( 0, 1 );
                        direction = Direction.RIGHT;
                    }
                }
                else {

                    if ( f2 < thumStickVector.Y ) {
                        
                        angle     = stage.GetAngleFromPointToStageVanishingPoint( pos, VanishingPoint.LEFT );
                        direction = Direction.UP;
                    }
                    else if ( -f2 > thumStickVector.Y ) {
                    
                        angle     = stage.GetAngleFromStageVanishingPointToPoint( pos, VanishingPoint.RIGHT );
                        direction = Direction.DOWN;
                    }
                    else {
                    
                        angle     = Math.Atan2( 0, -1 );
                        direction = Direction.LEFT;
                    }
                }



                if ( f < thumStickVector.Y ) {

                    if ( f2 < thumStickVector.X ) {
                        
                        angle     = stage.GetAngleFromStageVanishingPointToPoint( pos, VanishingPoint.RIGHT );
                        direction = Direction.UP;
                    }
                    else if ( -f2 > thumStickVector.X ) {
                    
                        angle     = stage.GetAngleFromPointToStageVanishingPoint( pos, VanishingPoint.LEFT );
                        direction = Direction.DOWN;
                    }
                    else {
                    
                        angle     = Math.Atan2( -1, 0 );
                        direction = Direction.UP;
                    }
                }
                else {

                    if ( f2 < thumStickVector.X ) {
                        
                        angle     = stage.GetAngleFromPointToStageVanishingPoint( pos, VanishingPoint.LEFT );
                        direction = Direction.UP;
                    }
                    else if ( -f2 > thumStickVector.X ) {
                    
                        angle     = stage.GetAngleFromPointToStageVanishingPoint( pos, VanishingPoint.RIGHT );
                        direction = Direction.DOWN;
                    }
                    else {
                    
                        angle     = Math.Atan2( 1, 0 );
                        direction = Direction.DOWN;
                    }
                }
                moveVector.X = velocity * thumStickVector.X;
                moveVector.Z = velocity * thumStickVector.Y * weight;
            }*/





            // 速度を調整
            velocity = Vector2.Distance( Vector2.Zero, new Vector2( moveVector.X, moveVector.Z ) );



            // 移動ベクトルを更新
            moveVector.X = velocity * (float)( Math.Cos( angle ) );
            moveVector.Z = velocity * (float)( Math.Sin( angle ) );


            // 座標を更新
            position.X += moveVector.X;
            position.Z += moveVector.Z;
        }



        /// <summary>
        /// 移動範囲の制限
        /// </summary>
        private void InScreen() 
        {
            float stageLowerLimit       = Stage.stageUpPosY - size.Y / 2;                       // ステージの上の限界
            float stageUpperLimit       = Stage.stageDownPosY - size.Y / 2;                     // ステージの下の限界
            float stageLeftSideLimit    = stage.LeftSideLimit( position.Z )  - size.X / 2;      // ステージの左の限界
            float stageRightSideLimit   = stage.RightSideLimit( position.Z ) + size.X / 2;      // ステージの右の限界



            // ステージの外に出たら
            if ( !stage.IsInStage( this ) )
            {
                // ジャンプモードに設定
                mode = ( mode != ActionMode.DAMAGE )? ActionMode.JUMP : ActionMode.DAMAGE;


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
                    // 移動ベクトルのY成分が小さい値になり、
                    // タイマーカウントが間隔を超えたら立ちモードに設定
                    if ( moveVector.Y <= 1 && timer > interval ) 
                    {
                        mode = ActionMode.STAND;
                    }

                    // Y座標の初期化
                    position.Y = 0;


                    switch ( mode )
                    {
                        // ダメージモードなら跳ね返り
                        case ActionMode.DAMAGE:

                            // 水平方向はほぼ等速
                            moveVector.X *= 0.999f;
                            moveVector.Z *= 0.999f;

                            // 鉛直方向の跳ね返り
                            if ( timer > interval ) // 最初の1回(衝突した直後)は跳ね返りにしない
                            {
                                moveVector.Y *= -0.5f;
                            }

                            break;


                        default: 
                            
                            // 移動ベクトルのY成分の初期化
                            moveVector.Y = 0;

                            break;
                    }
                }
            }


            // 下まで落ちたら座標を初期化
//            if ( position.Y > Screen.Height )
            if ( position.Y > 60 )
            {
                // SE再生
                Sound.Instance.Play( "sound\\se\\se_splash" );

                // 座標の初期化
                InitializePosition();

                // 無敵モードにする
                invincibleModeCountTimer = 0;

                // 落下判定用フラグOFF
                fallCheckFlg = false;

                // 立ちモードに設定
                mode = ActionMode.STAND;

                // ジャンプの移動量
                moveVector.Y = 0;

                // 飛距離のパラメーター
                leapParameter = 1.0f;

                // 残機を減らす
                stock -= ( stock > 0 )? 1 : 0;

                // 残機0で死亡
                isDead = ( stock <= 0 )? true : false;
            }
        }



        /// <summary>
        /// 座標の初期化
        /// </summary>
        private void InitializePosition() 
        {
            position.X = Screen.Width  / 2;
            position.Y = 0;
            position.Z = Screen.Height / 2;
        }



        /// <summary>
        /// ジャンプ
        /// </summary>
        private void Jump()
        {
            // ジャンプ、ダメージモードなら処理しない
            switch ( mode ) 
            {
                case ActionMode.JUMP:
                case ActionMode.DAMAGE: return;

                default: break;
            }


            float movement = 13;    // 移動量


            // ボタンが押されたらジャンプ
            if ( 
                InputKeyboard.Instance.State( Keys.Z ) == InputKeyboard.KS.DOWN ||
                InputGamePad.Instance.State( Buttons.A, PlayerIndex.One ) == InputGamePad.BS.DOWN
            )
            {
                // ジャンプモードに設定
                mode = ActionMode.JUMP;

                // ジャンプの移動量
                moveVector.Y = -movement;

                // 落下判定用フラグOFF
                fallCheckFlg = false;
            }
        }



        /// <summary>
        /// 落下
        /// </summary>
        private void Fall() 
        {
            float gravity = 0.98f;      // 重力


            // ジャンプの移動量を更新
            moveVector.Y += gravity;

            // Y座標の更新
            position.Y += moveVector.Y * zBuffer * 1.5f;
        }



        /// <summary>
        /// ダメージ
        /// </summary>
        private void Damage() 
        {
            // ダメージモード以外なら処理しない
            if ( mode != ActionMode.DAMAGE ) return;



            position.X += moveVector.X * zBuffer;
            // Yは落下メソッドで実行
            position.Z += moveVector.Z * zBuffer;
        }



        /// <summary>
        /// 無敵モード
        /// </summary>
        private void InvincibleMode() 
        {
            // 無敵モードでないなら処理しない
            if ( !IsInvincibleMode() ) return;



            const int interval = 60 * 2;    // 無敵モードの間隔

            // カウントアップ
            invincibleModeCountTimer++;


            // カウントが無敵モードの間隔を超えたら無敵モード終了
            if ( invincibleModeCountTimer >= interval )
            {
                // 無敵モードでない
                invincibleModeCountTimer = -1;
            }        
        }



        /// <summary>
        /// 無敵モード判定
        /// </summary>
        /// <returns></returns>
        private bool IsInvincibleMode() 
        {
            // カウントが0以上なら無敵モード
            return (invincibleModeCountTimer >= 0);
        }



        /// <summary>
        /// 衝突判定を取るかの設定
        /// </summary>
        private void EnableCollision() 
        {
            // 無敵モードでないなら衝突判定を取る
            enableCollision = ( !(IsInvincibleMode()) )? true : false;
        }
    }
}
