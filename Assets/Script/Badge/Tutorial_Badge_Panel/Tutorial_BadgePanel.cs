using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class Tutorial_BadgePanel : MonoBehaviour
    {
        [Header("First")] 
        [SerializeField] private GameObject firstTutorial;

        [SerializeField] private GameObject secondTutorial;
        [SerializeField] private GameObject thirdTutorial;
        [SerializeField] private GameObject skin_second_tutorial;
        
        public void OnClick_Badge()
        {
            firstTutorial.SetActive(false);
            secondTutorial.SetActive(true);
        }

        public void OnClick_Second_Exit()
        {
            secondTutorial.SetActive(false);
            thirdTutorial.SetActive(true);
        }

        public void OnClick_Third_Exit()
        {
            thirdTutorial.SetActive(false);
            skin_second_tutorial.SetActive(true);
            // 스킨에서 두 번째 튜토리얼에 대한 Setting을 진행해줌 
            this.gameObject.SetActive(false);
        }
    }
}
