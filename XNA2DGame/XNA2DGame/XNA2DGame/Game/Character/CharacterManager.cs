using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNA2DGame
{
    /// <summary>
    /// キャラクターマネージャークラス
    /// </summary>
    class CharacterManager
    {
        // メンバー変数
        private int     bornCannonTimer;    // 大砲の発生のタイマー
        private Random  rnd;                // 乱数クラス
        private Stage   stage;              // ステージ
        private Player  player;             // プレイヤー

        // オブジェクトコレクション
        private List<Character> ironBalls;      // 鉄球コレクション
        private List<Character> cannons;        // 大砲コレクション
        private List<Character> characters;     // 描画用キャラクターコレクション



        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="stage"></param>
        public CharacterManager( Stage stage ) 
        {
            this.stage  = stage;                    // ステージ 
            player      = new Player( stage );      // プレイヤー
            rnd         = new Random();             // 乱数


            ironBalls   = new List<Character>();    // 鉄球コレクション
            cannons     = new List<Character>();    // 大砲コレクション
            characters  = new List<Character>();    // キャラクターコレクション
        }



        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize() 
        {
            // コレクションのクリア
            ironBalls.Clear();      // 鉄球コレクション
            cannons.Clear();        // 大砲コレクション
            characters.Clear();     // キャラクターコレクション

            // 大砲の発生のタイマー
            bornCannonTimer = 0;


            player.Initialize();  // プレイヤー

            // 大砲の初期位置セット
            InitCannons();
        }



        /// <summary>
        /// 更新
        /// </summary>
        public void Update() 
        {
            player.Update();  // プレイヤー

            foreach ( var i in ironBalls )  i.Update();  // 鉄球コレクション 
            foreach ( var c in cannons )    c.Update();  // 大砲コレクション 


            // 大砲の発生
            BornCannon();

            // 鉄球の生成
            BornIornBall();

            // キャラクターコレクションのソート
            SortCharacterCollection();


            // コレクションの死んだ要素を削除
            ironBalls.RemoveAll( obj => obj.IsDead() );     // 鉄球コレクション
            cannons.RemoveAll( obj => obj.IsDead() );       // 大砲コレクション
        }



        /// <summary>
        /// 描画
        /// </summary>
        public void Draw() 
        {
        }



        /// <summary>
        /// ゲッター群
        /// </summary>
        public Character        GetPlayer()     { return player; }
        public List<Character>  GetIronBalls()  { return ironBalls; }
        public List<Character>  GetCharacters() { return characters; }



        /// <summary>
        /// 大砲コレクションの追加
        /// </summary>
        /// <param name="setPos"></param>
        private void AddCannons( Vector3 setPos ) 
        {
            Direction dir;  // 方向

            // 発射の方向を決定
            if ( setPos.X > Stage.stagePosition.X + (Stage.stageSize.X / 2) )
            {
                dir = Direction.LEFT;
            }
            else
            {
                dir = Direction.RIGHT;
            }

            // 移動方向を決定
            MoveDirection mDir = MoveDirection.DOWN;
            if ( rnd.Next(2) == 0 )
            {
                mDir = MoveDirection.DOWN;
            }
            else
            {
                mDir = MoveDirection.UP;
            }

            // 移動の速さを決定
            float speed = (float)( (2 - 1) * rnd.NextDouble() + 1 );

            // 開始する時間の補正値を決定
            float time = (float)( (3 - 1) * rnd.NextDouble() + 1 );


            // 大砲コレクションに追加
            cannons.Add( new Cannon(speed, time, stage, setPos, dir, mDir) );
        }



        /// <summary>
        /// 大砲の初期位置セット
        /// </summary>
        private void InitCannons() 
        {
            float posZ = Stage.stageUpPosY + Stage.stageSize.Y / 2;
            float leftSidePosX  = stage.LeftSideLimit( posZ );      // 左側のX座標
            float rightSidePosX = stage.RightSideLimit( posZ );     // 右側のX座標

            // 座標テーブル
            Vector3[] posTable = { 
                new Vector3( leftSidePosX - 500, 0, posZ ),
                new Vector3( leftSidePosX - 500, 0, posZ ),
                new Vector3( leftSidePosX - 500, 0, posZ ),
                new Vector3( leftSidePosX - 500, 0, posZ ),

                new Vector3( rightSidePosX + 500, 0, posZ ),
                new Vector3( rightSidePosX + 500, 0, posZ ),
                new Vector3( rightSidePosX + 500, 0, posZ ),
                new Vector3( rightSidePosX + 500, 0, posZ ),
            };


            // 大砲の生成
            foreach ( var p in posTable ) 
            {
                AddCannons( p );
            }
        }



        /// <summary>
        /// 大砲の発生
        /// </summary>
        private void BornCannon() 
        {
            // 開始から数秒は処理しない
            if ( GameTimer.Instance.GetElapsedGameTime() <= 60 * 10 ) return;

            // 大砲の数が最大なら処理しない
            if ( 80 <= cannons.Count ) return;



            float posZ          = Stage.stageUpPosY + Stage.stageSize.Y / 2;    // Z座標
            float leftSidePosX  = stage.LeftSideLimit( posZ );                  // 左側のX座標
            float rightSidePosX = stage.RightSideLimit( posZ );                 // 右側のX座標

            const int interval = 60 * 2;    // 発生間隔
            Vector3 setPos;                 // 発生座標


            // カウントアップ
            bornCannonTimer++;


            // カウントが発生間隔を超えたら発生
            if ( bornCannonTimer >= interval ) 
            {
                // カウンターの初期化
                bornCannonTimer = 0;

                if ( rnd.Next(2) == 0 )
                {
//                    setPos = new Vector3( leftSidePosX - rnd.Next(70, 200), 0, posZ );
                    setPos = new Vector3( leftSidePosX - 500, 0, posZ );
                }
                else 
                {
//                    setPos = new Vector3( rightSidePosX + rnd.Next(70, 200), 0, posZ );
                    setPos = new Vector3( rightSidePosX + 500, 0, posZ );
                }

                // コレクションに追加
                AddCannons( setPos );
            }
        }






        /// <summary>
        /// 鉄球の生成
        /// </summary>
        private void BornIornBall() 
        {
            Character   ironball;   // 保存用の鉄球
            Cannon      cannon;     // 保存用の大砲

            for ( int i = 0; i < cannons.Count; ++i )
            {
                // 基本クラス型を派生クラス型に変換
                if ( cannons[i] is Cannon ) cannon = cannons[i] as Cannon;
                else return;

                // 鉄球を受け取る
                ironball = cannon.CreateIronBall();

                // 鉄球がnullでないならコレクションに追加
                if ( ironball != null )
                { 
                    ironBalls.Add( ironball );

                    // SE再生
                    Sound.Instance.Play( "sound\\se\\se_shot" );
                }
            }
        }



        /// <summary>
        /// キャラクターコレクションのソート
        /// </summary>
        private void SortCharacterCollection() 
        {
            float[]         depthBuf    = new float[2];             // 深度バッファ
            Character       temp;                                   // キャラクター保存用


            // キャラクターコレクションのクリア
            characters.Clear();



            // 全てのキャラクターをキャラクターコレクションに格納
            characters.Add( player );                                // プレイヤー
            foreach ( var i in ironBalls )  characters.Add( i );     // 鉄球コレクション
            foreach ( var c in cannons )    characters.Add( c );     // 大砲コレクション


            // キャラクターコレクションの昇順ソート
            for ( int i = 0; i < characters.Count - 1; i++ ) 
            {
                for ( int j = 0; j < characters.Count - i - 1; j++ ) 
                {
                    // Z値を深度バッファとして保存
                    depthBuf[0] = characters[j].GetZBuffer();
                    depthBuf[1] = characters[j + 1].GetZBuffer();

                    // 深度バッファでスワッピング
                    if ( depthBuf[0] > depthBuf[1] ) 
                    {
                        temp                = characters[j];
                        characters[j]       = characters[j + 1];
                        characters[j + 1]   = temp;
                    }
                }
            }
        }
    }
}
