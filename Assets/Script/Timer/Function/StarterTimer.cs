using System;
using System.Collections;
using Data;
using Shop;
using Timer;
using UnityEngine;
using UnityEngine.UI;

public class StarterTimer : MonoBehaviour, ITimer
{
    [Header("Object")]
    [SerializeField] private Text shopText1;
    [SerializeField] private Text lobbyText;
    [SerializeField] private GameObject lobbyBox;
    [SerializeField] private GameObject starterPanel;
    [SerializeField] private Text lobby_gem;
    // 상점의 타이머 텍스트 
    // 로비의 타이머 텍스트 
    private DateTime targetTime;
    private Determine_StarterDAO _starterDao;
    private bool is_purchased_on_shop = false;
    
    // 상점 TR
    [SerializeField] private Transform shopTR;
    private void Start()
    {
        _starterDao = new Determine_StarterDAO();
        Package_DataDAO data = new Package_DataDAO(0);
        if (_starterDao.Get_Purchasable() && !data.Get_Data() && !_starterDao.Get_is_first())
        {
            StartCoroutine(Timer());
            lobbyBox.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 스킨 화면에 처음 들어간 경우 
    /// </summary>
    public void First_Start(bool is_Skin)
    {
        if (_starterDao.Get_is_first())
        {
            _starterDao.Set_is_first(); // 이제 처음 접근이 아님을 선언 
            PlayerPrefs.SetInt("First_Skin", 1);
            if (is_Skin)
                starterPanel.SetActive(true);

            StartCoroutine(Timer());
            lobbyBox.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 상품을 구매하면, 타이머를 작동 중단시킴. 
    /// </summary>
    public void Purchase()
    {
        is_purchased_on_shop = true;
        lobby_gem.text = String.Format("{0:#,0}", Playerdata_DAO.Player_Gem()+200);
    }

    public IEnumerator Timer()
    {
        targetTime = _starterDao.Get_TargetTime();
        while (true)
        {
            var delta_target = targetTime.Subtract(DateTime.UtcNow);
            lobbyText.text = (int) delta_target.TotalHours + delta_target.ToString(@"\:mm\:ss");
            shopText1.text = (int) delta_target.TotalHours + delta_target.ToString(@"\:mm\:ss");
            if (delta_target < TimeSpan.FromSeconds(1) || is_purchased_on_shop )
                break;
                
            yield return new WaitForSecondsRealtime(1.0f);
        }

        Action();
        yield return null;
    }

    public void Action()
    {
        lobbyBox.SetActive(false);
        shopTR.GetChild(2).gameObject.SetActive(false); // 상점에서 꺼줌 
        _starterDao.Set_Purchasable(); 
    }

    public void Open_Panel()
    {
        starterPanel.SetActive(true);
    }
    public void Set_TargetTime()
    {
        // 이건 필요 없음 
    }
}
