
using UnityEngine;

public static class Calc_Index 
{

    [Header("Avatar_Const")]
    private const int NORMAL_AVA_MAX = 20; // 노말 아바타 개수 
    private const int RARE_AVA_MAX = 7;
    private const int UNI_AVA_MAX = 3;

    [Header("Ball_Const")] 
    private const int DEFAULT_BALL = 1;
    private const int _NORMAL_MAX = 23; // 그냥 살 수 있는 공 
    private const int _LEVELUP_MAX = 11;  // 레벨업 or 패키지 사야 얻을 수 있는 공 
    private const int PACKAGE = 4;
    
    #region  Avatar
    /// <summary>
    /// Skin에서의 Grid index를 읽어와 거꾸로 아바타 index를 리턴해주는 함수. 뽑기에도 사용될 수 있음
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static int Get_Avatar_Num(int index)
    {
        int return_value = 0;
        if (index < NORMAL_AVA_MAX)
            return_value += index;
        
        else if (index >= NORMAL_AVA_MAX && index < RARE_AVA_MAX + NORMAL_AVA_MAX)
            return_value += (1000 + index-NORMAL_AVA_MAX);

        else
            return_value += (2000 + index-(NORMAL_AVA_MAX+RARE_AVA_MAX));
            
        
        return return_value;
    }

    public static int Get_Avatar_index(int value)
    {
        if (value / 1000 == 0)
            return value;
        
        else if (value / 1000 == 1)
            return NORMAL_AVA_MAX + (value % 1000);

        else
            return NORMAL_AVA_MAX + RARE_AVA_MAX + (value % 1000);
    }

    /// <summary>
    /// 아바타의 총 개수를 리턴해주는 함수 
    /// </summary>
    /// <returns></returns>
    public static int Get_Avatar_Max_Index()
    {
        return NORMAL_AVA_MAX + RARE_AVA_MAX + UNI_AVA_MAX;
    }

    public static int Get_Avatar_Noraml_Target()
    {
        return NORMAL_AVA_MAX;
    }

    public static int Get_Avatar_Rare_Target()
    {
        return (NORMAL_AVA_MAX + RARE_AVA_MAX);
    }
    
    
    public static Sprite Panel_Img(int index)
    {
        Sprite Img;
        string path = "Lobby/UI/Skin/Card/Panel_";
        int which_panel = 0;
        if (index >= NORMAL_AVA_MAX && index < RARE_AVA_MAX + NORMAL_AVA_MAX)
            which_panel = 1;

        else
            which_panel = 2;

        path += which_panel.ToString();
        Img = Resources.Load<Sprite>(path);
        return Img;
    }
    
    #endregion

    #region  Ball

    /// <summary>
    /// 공의 총 개수를 리턴해주는 함수 
    /// </summary>
    /// <returns></returns>
    public static int Get_Ball_Max_Index()
    {
        return ( DEFAULT_BALL+_NORMAL_MAX + _LEVELUP_MAX+PACKAGE );
    }

    public static int Get_Ball_Num(int index)
    {
        if (index == 0)
            return 0;

        else if (index < (_NORMAL_MAX+DEFAULT_BALL))
            return (1000 + index - DEFAULT_BALL);

        else if (index >= _NORMAL_MAX+ DEFAULT_BALL && index < _NORMAL_MAX + _LEVELUP_MAX+ DEFAULT_BALL)
            return (3000 + index - _NORMAL_MAX - DEFAULT_BALL);

        else
            return (4000 + index - _NORMAL_MAX - _LEVELUP_MAX - DEFAULT_BALL);

    }

    public static int Get_Ball_index(int value)
    {
        if(value>=3000 && value <4000)
            return  DEFAULT_BALL + _NORMAL_MAX + (value % 3000);
        
        else if (value >= 4000)
            return  DEFAULT_BALL + _NORMAL_MAX + _LEVELUP_MAX +(value % 4000);

        else
            return 0;
        
    }

    public static int Get_Normal_index()
    {
        return _NORMAL_MAX+DEFAULT_BALL;

    }
    #endregion

    #region Line

    

    #endregion

}
