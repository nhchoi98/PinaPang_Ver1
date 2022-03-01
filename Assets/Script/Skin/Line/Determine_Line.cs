using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Determine_Line 
{
    /// <summary>
    /// 인덱스 값을 받아와 이에 맞는 매터리얼을 리턴해줌 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static Material line_material(int index)
    {
        Material material;
        material = Resources.Load<Material>("Ingame/Line/mat/" + index.ToString());
        
        return material;
    }
    
    /// <summary>
    /// 라인 렌더러의 두께를 리턴해주는 함수 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static int line_width(int index)
    {
        int value;
        switch (index)
        {
            default:
                value = 10;
                break;

            case 1:
                value = 15;
                break;

            case 5:
            case 6:
            case 8:
            case 9:
            case 11:
            case 13:
                value = 20;
                break;

            case 10:
                value = 16;
                break;

            case 7:
            case 12:
            case 14:
                value = 24;
                break;
        }

        return value;

    }

    /// <summary>
    /// Tiling되는 const를 Set해주는 함수 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static float Get_Line_tilingconst(int index)
    {
        switch (index)
        {
            default:
                return 10;
            
            case 1:
                return 0.035f;
            
            case 4:
                return 0.00525f;
            
            case 5:
                return 0.0041675f;
            
            case 6:
                return 0.01f;
            
            case 7:
                return 0.0047633f;

            case 8:
                return 0.01666f;
            
            case 9:
                return 0.00909f;
                
            case 10:
                return 0.01429f;
            
            case 11:
                return 0.00395f;
            
            case 12:
                return 0.01f;
            
            case 13:
                return 0.01f;
            
            case 14:
                return 0.00625005f;
            
            
        }
        
    }
}
