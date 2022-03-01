using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPackagePanel
{
    public IEnumerator Popup_Action(); // 1초뒤 액션 
    public void Timer_Set();  // 타이머를 Set
    public void OnClick_BuyBtn();
}
