using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNA2DGame
{
    /// <summary>
    /// キャラクタークラス
    /// 抽象クラス
    /// </summary>
    abstract class Character
    {
        // メンバー変数
        protected string        name;               // 名前
        protected float         speed;              // 速さ
        protected bool          isDead;             // 死亡情報
        protected bool          aboveTheStageFlg;   // 「ステージより上(手前)に有るか」のフラグ 
        protected bool          enableCollision;    // 衝突判定を取るか
        protected float         zBuffer;            // Z値
        protected float         xBuffer;            // X値
        protected Vector3       position;           // 座標
        protected Vector2       size;               // サイズ
        protected Vector2       formerSize;         // 元のサイズ
        protected Stage         stage;              // ステージ
        protected Direction     direction;          // 方向
        protected Shadow        shadow;             // 影



        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="collisionSize"></param>
        /// <param name="size"></param>
        /// <param name="stage"></param>
        protected Character(
            string      name,       // 名前
            float       speed,      // 速さ
            Vector2     size,       // 画像サイズ
            Stage       stage       // ステージ
        ) 
        {
            this.name               = name;                 // 名前
            this.speed              = speed;                // 速度
            this.formerSize         = size;                 // 元のサイズ
            this.stage              = stage;                // ステージ

            isDead                  = false;                // 死亡情報(「死んでない」に設定)
            aboveTheStageFlg        = true;                 // 「ステージより上に有るか」のフラグ
            enableCollision         = true;                 // 衝突判定を取るか
            speed                   = 0;                    // 速度
            zBuffer                 = 0.0f;                 // Z値
            xBuffer                 = 0.0f;                 // X値
            size                    = formerSize;           // サイズ
            direction               = Direction.RIGHT;      // 方向
            shadow                  = new Shadow();         // 影

            position = new Vector3( 0, 0, 0 );              // 座標
        }



        /// <summary>
        /// 初期化
        /// </summary>
        public abstract void Initialize();



        /// <summary>
        /// 更新
        /// </summary>
        public abstract void Update();



        /// <summary>
        /// 描画
        /// </summary>
        public virtual void Draw() 
        {
            // 死亡なら描画しない
            if ( isDead ) return;

            Vector2 pos     = new Vector2( position.X, position.Z + position.Y ) - (size / 2);  // 座標
            Vector2 scale   = new Vector2( zBuffer, zBuffer );                                  // 拡大倍率

            // 影の描画
            shadow.DrawShadow( position, scale, size );

            // テクスチャの描画
            Renderer.Instance.DrawTexture( name, pos, scale );
        }



        /// <summary>
        /// 衝突応答
        /// </summary>
        public abstract void Hit( Vector3 hitPos );



        /// <summary>
        /// 死亡情報の取得
        /// </summary>
        /// <returns></returns>
        public bool IsDead() { return isDead; }



        /// <summary>
        /// ゲッター群
        /// </summary>
        public bool         GetAboveTheStageFlg()   { return aboveTheStageFlg; }    // 「ステージより上に有るか」のフラグ
        public bool         GetEnableCollision()    { return enableCollision; }     // 衝突判定を取るか
        public float        GetZBuffer()            { return zBuffer; }             // Z値ゲッター
        public Vector3      GetPosition()           { return position; }            // 座標
        public Vector2      GetSize()               { return size; }                // サイズ
        public Direction    GetDirection()          { return direction; }           // 方向
        



        /// <summary>
        /// Z値の更新
        /// </summary>
        protected void ZBuffer() 
        {
            zBuffer = stage.ZBuffer( position.Z );
        }



        /// <summary>
        /// X値の更新
        /// </summary>
        protected void XBuffer() 
        {
            xBuffer = stage.XBuffer( position.X, position.Z );
        }



        /// <summary>
        /// サイズの更新
        /// </summary>
        protected void UpdateSize() 
        {
            size = formerSize * zBuffer;
        }



        /// <summary>
        /// ステージより上に有るか判定
        /// </summary>
        protected void CheckAboveTheStage() 
        {
            // Y座標が0以下、又はZ座標がステージの下端より下に有れば
            // ステージより上に有る
            //if ( position.Y <= 0 ||
            if ( position.Y <= 10 ||
                 position.Z > Stage.stageDownPosY - size.Y / 2) 
            {
                // フラグON
                aboveTheStageFlg = true;
            }
            else 
            {
                // フラグOFF
                aboveTheStageFlg = false;
            }
        }
    }
}
