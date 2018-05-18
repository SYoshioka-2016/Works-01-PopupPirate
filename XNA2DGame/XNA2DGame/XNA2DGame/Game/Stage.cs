/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNA2DGame
{
    /// <summary>
    /// 消失点列挙
    /// </summary>
    public enum VanishingPoint 
    {
        LEFT,       // ステージの左の消失点
        CENTER,     // ステージの中央の消失点
        RIGHT,      // ステージの右の消失点
    }



    /// <summary>
    /// ステージクラス
    /// </summary>
    class Stage
    {
        // メンバー変数
        private Vector2         stageSize;                      // ステージ画像のサイズ
        private Vector2         stagePosition;                  // ステージ画像の座標
        private Vector2[]       stageVertex;                    // ステージの4つの頂点
        private Vector2         stageCenterVanishingPoint;      // ステージの中央の消失点
        private Vector2         stageLeftVanishingPoint;        // ステージの左の消失点
        private Vector2         stageRightVanishingPoint;       // ステージの右の消失点



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Stage() 
        {
            stageSize           = new Vector2( 600, 200 );                          // ステージ画像のサイズ
            stagePosition       =                                                   // ステージ画像の座標
                new Vector2( 
                    ( Screen.Width  - stageSize.X ) / 2 , 
                    ( Screen.Height - stageSize.Y ) / 2 
                );

            stageVertex         = new Vector2[]{                                    // ステージの4つの頂点

                new Vector2( 160,   0 ) + stagePosition,  // 左上
                new Vector2(   0, 200 ) + stagePosition,  // 左下
                new Vector2( 600, 200 ) + stagePosition,  // 右下
                new Vector2( 440,   0 ) + stagePosition,  // 右上
            };

            stageCenterVanishingPoint   = StageCenterVanishingPoint();              // ステージの中央の消失点
            stageLeftVanishingPoint     = StageLeftVanishingPoint();                // ステージの左の消失点
            stageRightVanishingPoint    = StageRightVanishingPoint();               // ステージの右の消失点
        }



        /// <summary>
        /// 描画
        /// </summary>
        public void Draw() 
        {
            Renderer.Instance.DrawTexture( "stage", stagePosition );  // ステージ画像
        }



        /// <summary>
        /// ゲッター群
        /// </summary>
        public Vector2      GetStageSize()                      { return stageSize; }
        public Vector2      GetStagePosition()                  { return stagePosition; }
        public Vector2      GetStageCenterVanishingPoint()      { return stageCenterVanishingPoint; }
        public Vector2      GetStageLeftVanishingPoint()        { return stageLeftVanishingPoint; }
        public Vector2      GetStageRightVanishingPoint()       { return stageRightVanishingPoint; }
        public float        GetStageUpperSize()                 { return stageVertex[3].X - stageVertex[0].X; }
        public float        GetStageLowerSize()                 { return stageVertex[2].X - stageVertex[1].X; }



        /// <summary>
        /// ステージの上の限界
        /// </summary>
        /// <returns></returns>
        public float UpperLimit() 
        {
            return stagePosition.Y + stageSize.Y;
        }



        /// <summary>
        /// ステージの下の限界
        /// </summary>
        /// <returns></returns>
        public float LowerLimit() 
        {
            return stagePosition.Y;
        }



        /// <summary>
        /// ステージの左の限界
        /// </summary>
        /// <param name="objPosY"></param>
        /// <returns></returns>
        public float LeftSideLimit( float objPosY ) 
        {
            float x = PointX( stageVertex[0], stageVertex[1], objPosY );

            return x;
        }



        /// <summary>
        /// ステージの右の限界
        /// </summary>
        /// <param name="objPosY"></param>
        /// <returns></returns>
        public float RightSideLimit( float objPosY )
        {
            float x = PointX( stageVertex[3], stageVertex[2], objPosY );

            return x;
        }



        /// <summary>
        /// オブジェクトがステージ内にあるか判定
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IsInStage( Character obj ) 
        {
            float objPosX = obj.GetPosition().X;    // オブジェクトのX座標
            float objPosZ = obj.GetPosition().Z;    // オブジェクトのZ座標

            float stageLowerLimit       = LowerLimit() - obj.GetSize().Y / 2;                   // ステージの上の限界
            float stageUpperLimit       = UpperLimit() - obj.GetSize().Y / 2;                   // ステージの下の限界

            float stageLeftSideLimit    = LeftSideLimit( objPosZ )  - obj.GetSize().X / 2;      // ステージの左の限界
            float stageRightSideLimit   = RightSideLimit( objPosZ ) + obj.GetSize().X / 2;      // ステージの右の限界


            // オブジェクトがステージ内にあるか判定
            if ( objPosZ >= stageLowerLimit        &&
                 objPosZ <= stageUpperLimit        &&
                 objPosX >= stageLeftSideLimit     &&
                 objPosX <= stageRightSideLimit
            )
            {
                return true;    // 有る
            }
            else
            {
                return false;   // 無い
            }
        }



        /// <summary>
        /// 任意の点からステージの消失点の角度
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="vanishingPointNumber"></param>
        /// <returns></returns>
        public double GetAngleFromPointToStageVanishingPoint( Vector2 pos, VanishingPoint vp ) 
        {
            Vector2 vec = Vector2.Zero;
            switch ( vp )
            {
                case VanishingPoint.LEFT:   vec = stageLeftVanishingPoint   - pos;  break;
                case VanishingPoint.CENTER: vec = stageCenterVanishingPoint - pos;  break;
                case VanishingPoint.RIGHT:  vec = stageRightVanishingPoint  - pos;  break;

                default: break;
            }

            return Math.Atan2( vec.Y, vec.X );
        }



        /// <summary>
        /// ステージの消失点から任意の点の角度
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="vanishingPointNumber"></param>
        /// <returns></returns>
        public double GetAngleFromStageVanishingPointToPoint( Vector2 pos, VanishingPoint vp ) 
        {
            Vector2 vec = Vector2.Zero;
            switch ( vp )
            {
                case VanishingPoint.LEFT:   vec = pos - stageLeftVanishingPoint;    break;
                case VanishingPoint.CENTER: vec = pos - stageCenterVanishingPoint;  break;
                case VanishingPoint.RIGHT:  vec = pos - stageRightVanishingPoint;   break;

                default: break;
            }

            return Math.Atan2( vec.Y, vec.X );
        }



        /// <summary>
        /// Z値ゲッター
        /// </summary>
        /// <param name="posZ"></param>
        /// <returns></returns>
        public float ZBuffer( float posZ ) 
        {
            // Z値の最大、最小
            float zBufferMax = 1.0f;
            float zBufferMin = GetStageUpperSize() / GetStageLowerSize();

            // Z値
            float zBuffer = ( (zBufferMax - zBufferMin) * ((posZ - GetStagePosition().Y) / GetStageSize().Y) ) + zBufferMin;


            return zBuffer;
        }



        /// <summary>
        /// X値ゲッター
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posZ"></param>
        /// <returns></returns>
        public float XBuffer( float posX, float posZ ) 
        {
            // ステージの中央から任意の1点までの距離
            float distance =( stagePosition.X + (stageSize.X / 2) ) - posX;

            // ステージの中央からステージの左右の直線上のある1点までの距離
            float Distance;

            // ステージの左右の直線上のある1点のX座標
            float pointX;


            // 任意の1点がステージの中央より左にある場合
            if ( distance > 0 )
            {
                pointX      = PointX( stageVertex[1], stageVertex[0], posZ );
                Distance    = (stagePosition.X + stageSize.X / 2) - pointX;
            }
            // 任意の1点がステージの中央より右にある場合
            else
            {
                pointX      = PointX( stageVertex[3], stageVertex[2], posZ );
                Distance    = pointX - (stagePosition.X + stageSize.X / 2);
            }


            // X値の最大、最小
            float xBufferMax = 1.0f;
            float xBufferMin = stageSize.Y / Vector2.Distance( stageVertex[0], stageVertex[1] );

            // X値
            float xBuffer = ( (xBufferMax - xBufferMin) * (distance / Distance) ) + xBufferMin;


            return xBuffer;
        }



        /// <summary>
        /// 中央の消失点の計算
        /// </summary>
        /// <returns></returns>
        private Vector2 StageCenterVanishingPoint() 
        {
            // 中央の消失点(2直線の交点)
            Vector2 scvp = IntersectionPoint( 

                               // 左下から左上の直線
                               stageVertex[1],
                               stageVertex[0],

                               // 右下から右上の直線
                               stageVertex[2],
                               stageVertex[3] 
                           );

            return scvp;
        }



        /// <summary>
        /// 左の消失点の計算
        /// </summary>
        /// <returns></returns>
        private Vector2 StageLeftVanishingPoint() 
        {
            // 左の消失点のX、Y座標
            float y = stageCenterVanishingPoint.Y;
            float x = PointX( stageVertex[0], stageVertex[2], y );

            // 左の消失点
            Vector2 slvp = new Vector2( x, y );


            return slvp;
        }



        /// <summary>
        /// 右の消失点の計算
        /// </summary>
        /// <returns></returns>
        private Vector2 StageRightVanishingPoint()
        {
            // 右の消失点のX、Y座標
            float y = stageCenterVanishingPoint.Y;
            float x = PointX( stageVertex[3], stageVertex[1], y );

            // 右の消失点
            Vector2 slvp = new Vector2( x, y );


            return slvp;
        }



        /// <summary>
        /// 傾きの計算
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private float Slope( Vector2 p1, Vector2 p2 ) 
        {
            // 1次関数の通る1点
            float x1 = p1.X;
            float y1 = p1.Y;

            // 1次関数の通るもう1点
            float x2 = p2.X;
            float y2 = p2.Y;

            // 傾き
            float a = ( y2 - y1 ) / ( x2 - x1 );


            return a;
        }



        /// <summary>
        /// 切片の計算
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="slope"></param>
        /// <returns></returns>
        private float Intercept( Vector2 p1, float slope ) 
        {
            // 切片
            float b = (-slope * p1.X) + p1.Y;

            return b;
        }



        /// <summary>
        /// X座標f(y)の計算
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="posY"></param>
        /// <returns></returns>
        private float PointX( Vector2 p1, Vector2 p2, float posY ) 
        {
            // 1次関数の通る1点
            float x1 = p1.X;
            float y1 = p1.Y;

            // 1次関数の通るもう1点
            float x2 = p2.X;
            float y2 = p2.Y;

            // 座標( f(y), y )算出
            float y = posY;
            float x = ( ( (y - y1) * (x2 - x1) ) / (y2 - y1) ) + x1;


            return x;
        }



        /// <summary>
        /// 2直線の交点座標の計算
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <returns></returns>
        private Vector2 IntersectionPoint( Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2 ) 
        {
            // 1次関数の傾き、切片
            float a1 = Slope( p1, p2 );
            float b1 = Intercept( p1, a1 );

            // もう1つの1次関数の傾き、切片
            float a2 = Slope( q1, q2 );
            float b2 = Intercept( q1, a2 );


            // 交点のX、Y座標
            float x = (b2 - b1) / (a1 - a2);
            float y = ( (a2 * b1) - (a1 * b2) ) / (a2 - a1);


            return new Vector2( x, y );
        }
    }
}*/




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNA2DGame
{
    /// <summary>
    /// 消失点列挙
    /// </summary>
    public enum VanishingPoint 
    {
        LEFT,       // ステージの左の消失点
        CENTER,     // ステージの中央の消失点
        RIGHT,      // ステージの右の消失点
    }



    /// <summary>
    /// ステージクラス
    /// </summary>
    class Stage
    {
        // メンバー変数

        // ステージ画像のサイズ
        public static readonly Vector2 stageSize = new Vector2( 600, 320 );
        
        // ステージ画像の座標
        public static readonly Vector2 stagePosition = 
            new Vector2( 
                ( Screen.Width  - stageSize.X ) / 2 , 
                ( Screen.Height - stageSize.Y ) / 2 
            );

        // ステージの上端のY座標
        public static readonly float stageUpPosY = stagePosition.Y;

        // ステージの下端のY座標
        public static readonly float stageDownPosY = stageUpPosY + stageSize.Y;

        // ステージの4つの頂点
        public static readonly Vector2[] stageVertex = new Vector2[] {
                new Vector2( 100,   0 ) + stagePosition,  // 左上
                new Vector2(   0, 320 ) + stagePosition,  // 左下
                new Vector2( 600, 320 ) + stagePosition,  // 右下
                new Vector2( 490,   0 ) + stagePosition,  // 右上
            };

        // 消失点座標
        public static readonly Vector2 stageCenterVanishingPoint    = StageCenterVanishingPoint();  // ステージの中央の消失点
        public static readonly Vector2 stageLeftVanishingPoint      = StageLeftVanishingPoint();    // ステージの左の消失点
        public static readonly Vector2 stageRightVanishingPoint     = StageRightVanishingPoint();   // ステージの右の消失点



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Stage() { }



        /// <summary>
        /// 描画
        /// </summary>
        public void Draw() 
        {
//            Renderer.Instance.DrawTexture( "stage_back",    Vector2.Zero );     // ステージ背景画像
            Renderer.Instance.DrawTexture( "stage",         stagePosition );    // ステージ画像
        }



        /// <summary>
        /// ゲッター群
        /// </summary>
        public float GetStageUpperSize() { return stageVertex[3].X - stageVertex[0].X; }
        public float GetStageLowerSize() { return stageVertex[2].X - stageVertex[1].X; }



        /// <summary>
        /// ステージの左の限界
        /// </summary>
        /// <param name="objPosY"></param>
        /// <returns></returns>
        public float LeftSideLimit( float objPosY ) 
        {
            float x = PointX( stageVertex[0], stageVertex[1], objPosY );

            return x;
        }



        /// <summary>
        /// ステージの右の限界
        /// </summary>
        /// <param name="objPosY"></param>
        /// <returns></returns>
        public float RightSideLimit( float objPosY )
        {
            float x = PointX( stageVertex[3], stageVertex[2], objPosY );

            return x;
        }



        /// <summary>
        /// オブジェクトがステージ内にあるか判定
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IsInStage( Character obj ) 
        {
            float objPosX = obj.GetPosition().X;    // オブジェクトのX座標
            float objPosZ = obj.GetPosition().Z;    // オブジェクトのZ座標

            float stageLowerLimit       = stageUpPosY - obj.GetSize().Y / 2;                    // ステージの上の限界
            float stageUpperLimit       = stageDownPosY - obj.GetSize().Y / 2;                  // ステージの下の限界

            float stageLeftSideLimit    = LeftSideLimit( objPosZ )  - obj.GetSize().X / 2;      // ステージの左の限界
            float stageRightSideLimit   = RightSideLimit( objPosZ ) + obj.GetSize().X / 2;      // ステージの右の限界


            // オブジェクトがステージ内にあるか判定
            if ( objPosZ >= stageLowerLimit        &&
                 objPosZ <= stageUpperLimit        &&
                 objPosX >= stageLeftSideLimit     &&
                 objPosX <= stageRightSideLimit
            )
            {
                return true;    // 有る
            }
            else
            {
                return false;   // 無い
            }
        }



        /// <summary>
        /// 任意の点からステージの消失点の角度
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="vanishingPointNumber"></param>
        /// <returns></returns>
        public double GetAngleFromPointToStageVanishingPoint( Vector2 pos, VanishingPoint vp ) 
        {
            Vector2 vec = Vector2.Zero;
            switch ( vp )
            {
                case VanishingPoint.LEFT:   vec = stageLeftVanishingPoint   - pos;  break;
                case VanishingPoint.CENTER: vec = stageCenterVanishingPoint - pos;  break;
                case VanishingPoint.RIGHT:  vec = stageRightVanishingPoint  - pos;  break;

                default: break;
            }

            return Math.Atan2( vec.Y, vec.X );
        }



        /// <summary>
        /// ステージの消失点から任意の点の角度
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="vanishingPointNumber"></param>
        /// <returns></returns>
        public double GetAngleFromStageVanishingPointToPoint( Vector2 pos, VanishingPoint vp ) 
        {
            Vector2 vec = Vector2.Zero;
            switch ( vp )
            {
                case VanishingPoint.LEFT:   vec = pos - stageLeftVanishingPoint;    break;
                case VanishingPoint.CENTER: vec = pos - stageCenterVanishingPoint;  break;
                case VanishingPoint.RIGHT:  vec = pos - stageRightVanishingPoint;   break;

                default: break;
            }

            return Math.Atan2( vec.Y, vec.X );
        }



        /// <summary>
        /// Z値ゲッター
        /// </summary>
        /// <param name="posZ"></param>
        /// <returns></returns>
        public float ZBuffer( float posZ ) 
        {
            // Z値の最大、最小
            float zBufferMax = 1.0f;
            float zBufferMin = GetStageUpperSize() / GetStageLowerSize();

            // Z値
            float zBuffer = ( (zBufferMax - zBufferMin) * ((posZ - stagePosition.Y) / stageSize.Y) ) + zBufferMin;


            return zBuffer;
        }



        /// <summary>
        /// X値ゲッター
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posZ"></param>
        /// <returns></returns>
        public float XBuffer( float posX, float posZ ) 
        {
            // ステージの中央から任意の1点までの距離
            float distance = ( stagePosition.X + (stageSize.X / 2) ) - posX;

            // ステージの中央からステージの左右の直線上のある1点までの距離
            float Distance;

            // ステージの左右の直線上のある1点のX座標
            float pointX;


            // 任意の1点がステージの中央より左にある場合
            if ( distance > 0 )
            {
                pointX      = PointX( stageVertex[1], stageVertex[0], posZ );
                Distance    = (stagePosition.X + stageSize.X / 2) - pointX;
            }
            // 任意の1点がステージの中央より右にある場合
            else
            {
                pointX      = PointX( stageVertex[3], stageVertex[2], posZ );
                Distance    = pointX - (stagePosition.X + stageSize.X / 2);
            }


            // X値の最大、最小
            float xBufferMax = 1.0f;
            float xBufferMin = stageSize.Y / Vector2.Distance( stageVertex[0], stageVertex[1] );

            // X値
            float xBuffer = ( (xBufferMax - xBufferMin) * (distance / Distance) ) + xBufferMin;


            return xBuffer;
        }



        /// <summary>
        /// 中央の消失点の計算
        /// </summary>
        /// <returns></returns>
        private static Vector2 StageCenterVanishingPoint() 
        {
            // 中央の消失点(2直線の交点)
            Vector2 scvp = IntersectionPoint( 

                               // 左下から左上の直線
                               stageVertex[1],
                               stageVertex[0],

                               // 右下から右上の直線
                               stageVertex[2],
                               stageVertex[3] 
                           );

            return scvp;
        }



        /// <summary>
        /// 左の消失点の計算
        /// </summary>
        /// <returns></returns>
        private static Vector2 StageLeftVanishingPoint() 
        {
            // 左の消失点のX、Y座標
            float y = stageCenterVanishingPoint.Y;
            float x = PointX( stageVertex[0], stageVertex[2], y );

            // 左の消失点
            Vector2 slvp = new Vector2( x, y );


            return slvp;
        }



        /// <summary>
        /// 右の消失点の計算
        /// </summary>
        /// <returns></returns>
        private static Vector2 StageRightVanishingPoint()
        {
            // 右の消失点のX、Y座標
            float y = stageCenterVanishingPoint.Y;
            float x = PointX( stageVertex[3], stageVertex[1], y );

            // 右の消失点
            Vector2 slvp = new Vector2( x, y );


            return slvp;
        }



        /// <summary>
        /// 傾きの計算
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private static float Slope(Vector2 p1, Vector2 p2) 
        {
            // 1次関数の通る1点
            float x1 = p1.X;
            float y1 = p1.Y;

            // 1次関数の通るもう1点
            float x2 = p2.X;
            float y2 = p2.Y;

            // 傾き
            float a = ( y2 - y1 ) / ( x2 - x1 );


            return a;
        }



        /// <summary>
        /// 切片の計算
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="slope"></param>
        /// <returns></returns>
        private static float Intercept( Vector2 p1, float slope ) 
        {
            // 切片
            float b = (-slope * p1.X) + p1.Y;

            return b;
        }



        /// <summary>
        /// X座標f(y)の計算
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="posY"></param>
        /// <returns></returns>
        private static float PointX( Vector2 p1, Vector2 p2, float posY ) 
        {
            // 1次関数の通る1点
            float x1 = p1.X;
            float y1 = p1.Y;

            // 1次関数の通るもう1点
            float x2 = p2.X;
            float y2 = p2.Y;

            // 座標( f(y), y )算出
            float y = posY;
            float x = ( ( (y - y1) * (x2 - x1) ) / (y2 - y1) ) + x1;


            return x;
        }



        /// <summary>
        /// 2直線の交点座標の計算
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <returns></returns>
        private static Vector2 IntersectionPoint( Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2 ) 
        {
            // 1次関数の傾き、切片
            float a1 = Slope( p1, p2 );
            float b1 = Intercept( p1, a1 );

            // もう1つの1次関数の傾き、切片
            float a2 = Slope( q1, q2 );
            float b2 = Intercept( q1, a2 );


            // 交点のX、Y座標
            float x = (b2 - b1) / (a1 - a2);
            float y = ( (a2 * b1) - (a1 * b2) ) / (a2 - a1);


            return new Vector2( x, y );
        }
    }
}