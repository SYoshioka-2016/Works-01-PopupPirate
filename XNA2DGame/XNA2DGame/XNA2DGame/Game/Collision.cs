using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNA2DGame
{
    /// <summary>
    /// 衝突処理クラス
    /// </summary>
    class Collision
    {
        // メンバー変数
        private CharacterManager characterManager;  // キャラクターマネージャー



        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="cm"></param>
        public Collision( CharacterManager cm ) 
        {
            this.characterManager = cm;  // キャラクターマネージャー
        }



        /// <summary>
        /// 更新
        /// </summary>
        public void Update() 
        {
            CollisionPlayerIronBall();  // プレイヤーと鉄球の衝突処理
        }



        /// <summary>
        /// キャラクタークラス同士の円の衝突判定
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        private bool CollisionCharacters( Character c1, Character c2 ) 
        {
            // 衝突判定を取らないなら衝突しない
            if ( !c1.GetEnableCollision() || !c2.GetEnableCollision() ) return false;



            // キャラクター1の座標
            Vector2 c1Pos = new Vector2( 
                                c1.GetPosition().X, 
                                c1.GetPosition().Z + c1.GetPosition().Y 
                            );

            // キャラクター2の座標
            Vector2 c2Pos = new Vector2( 
                                c2.GetPosition().X, 
                                c2.GetPosition().Z + c2.GetPosition().Y 
                            );



            // キャラクター間の距離
            float distance = Vector2.Distance( c1Pos, c2Pos );

            // 衝突する距離(最大)
            // 半径はそれぞれの画像サイズのX成分の半分とする
            float collisionDistance = ( c1.GetSize().X / 2 ) + ( c2.GetSize().X / 2 );

            // キャラクター間の距離が衝突する距離より大きいなら衝突しない
            if ( distance > collisionDistance ) return false;



            // 衝突する距離の重り
            // 2つのキャラクターのZ値の差(奥行の差)によって、衝突する距離を補正する
            float weight = 1.0f - Math.Abs( c1.GetZBuffer() - c2.GetZBuffer() ) * 30.0f;

            // 重りを掛けた衝突する距離
            collisionDistance = collisionDistance * weight;

            // 距離が小さければ衝突する
            if ( distance < collisionDistance )
            {
                // 衝突する
                return true;
            }
            else
            {
                // 衝突しない
                return false;
            }
        }



        /// <summary>
        /// プレイヤーと鉄球の衝突処理
        /// </summary>
        private void CollisionPlayerIronBall() 
        {
            // キャラクターの設定
            Character           player      = characterManager.GetPlayer();         // プレイヤー
            List< Character >   ironBalls   = characterManager.GetIronBalls();      // 鉄球コレクション



            // 鉄球毎に衝突処理
            foreach ( var i in ironBalls ) 
            {
                CollisionPlayerIronBallOne( player, i );
            }
        }



        /// <summary>
        /// プレイヤーと鉄球の衝突処理 個別
        /// </summary>
        /// <param name="e"></param>
        private void CollisionPlayerIronBallOne( Character p, Character i ) 
        {
            // 衝突したら
            if ( CollisionCharacters( p, i ) )
            {
                // SE再生
                Sound.Instance.Play( "sound\\se\\se_hit" );

                // 衝突応答
                p.Hit( i.GetPosition() );    // プレイヤー
                i.Hit( Vector3.Zero );    // 敵
            }
        }
    }
}
