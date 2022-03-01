using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimer
{
    public IEnumerator Timer(); // 타이머를 구동시켜주는 IEnumerator
    public void Action(); // 특정 시간이 지나면 action을 발생시켜주는 함수 
    public void Set_TargetTime(); // 타이머에서 목적인 시간을 설정해주는 함수 
}
