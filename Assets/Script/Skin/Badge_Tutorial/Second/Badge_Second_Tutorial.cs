using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class Badge_Second_Tutorial : MonoBehaviour
    {
        
        [Header("Tutorial")] 
        [SerializeField] private GameObject firstTutorial;
        [SerializeField] private GameObject secondTutorial;
        [SerializeField] private GameObject thirdTutorial;
        [SerializeField] private GameObject lastTutorial;
        [SerializeField] private Transform badgeTR;

        [SerializeField] private GameObject parent;
        public void OnClick_FirstClick()
        {
            firstTutorial.SetActive(false);
            for(int i = 1; i< badgeTR.childCount; i++)
                badgeTR.GetChild(i).gameObject.SetActive(false);
            
            badgeTR.GetChild(0).gameObject.SetActive(true);
            secondTutorial.SetActive(true);
        }

        public void Second_Tutorial()
        {
            secondTutorial.SetActive(false);
            thirdTutorial.SetActive(true);
        }

        public void Third_Tutorial()
        {
            thirdTutorial.SetActive(false);
            lastTutorial.SetActive(true);
            parent.SetActive(false);
        }
        
    }
}
