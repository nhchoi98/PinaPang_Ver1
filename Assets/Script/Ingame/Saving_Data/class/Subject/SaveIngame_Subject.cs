using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame_Data
{
    public class SaveIngame_Subject : MonoBehaviour, ISubject_Ingame
    {
        private List<IObserver_Ingame> listObserver;
        [Header("Observer")]
        private IObserver_Ingame observer_Stage;

        private void Awake()
        {
            ResisterObserver();
        }

        public void ResisterObserver()
        {
            listObserver = new List<IObserver_Ingame>();
            for(int i =0; i<this.transform.childCount; i++)
                listObserver.Add(this.gameObject.transform.GetChild(i).gameObject.GetComponent<IObserver_Ingame>());
        }

        public void RemoveObserver()
        {
            
        }

        public void notifyObserver()
        {
            for (int i = 0; i < listObserver.Count; i++)
                listObserver[i].Update_Status();
            
        }
    }
}
