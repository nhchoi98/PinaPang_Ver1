using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public static class _Determine_Pos
    {
        private const float X_START_POS = -471.8f;
        private const float Y_START_POS = 571f;
        private const float X_offset = 135f;
        private const float Y_offset = -155f;
        
        /// <summary>
        /// 열, 행을 매개변수로 입력받아 이걸 Vector2 타입으로 반환해주는 함수 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static Vector2 Which_Pos(int row, int col)
        {
            Vector2 Pos = new Vector2(X_START_POS + (float)(X_offset * col), Y_START_POS + (float)(Y_offset * row));
            return Pos;
        }

        /// <summary>
        /// 스킬을 쓸 때, 그 박스가 스킬 영역안에 포함되는지를 판단해주기 위해서 박스의 행, 열 정보를 계산해주는 함수 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="pos"></param>
        public static void Calc_Which_Grid(ref int row, ref int col, Vector2 pos)
        {
            var row_pos = pos.y;
            var col_pos = pos.x;

            row = (int) ((row_pos - Y_START_POS) / Y_offset);
            col = (int) ((col_pos - X_START_POS) / X_offset);
        }
        
        
        public static void Calc_Which_Grid(ref int row, ref int col, Vector2 pos, bool is_moving_left)
        {
            var row_pos = pos.y;
            var col_pos = pos.x;

            row = (int) ((row_pos - Y_START_POS-90) / Y_offset);
            col = (int) ((col_pos - X_START_POS+67.5f) / X_offset);
        }
        
        public static void Calc_Which_Grid_Skill(ref int row, ref int col, Vector2 pos)
        {
            var row_pos = pos.y;
            var col_pos = pos.x;

            row = (int) ((row_pos - Y_START_POS) / Y_offset);
            col = (int) ((col_pos - X_START_POS-67.5f) / X_offset);
        }
        
        


    }

}

