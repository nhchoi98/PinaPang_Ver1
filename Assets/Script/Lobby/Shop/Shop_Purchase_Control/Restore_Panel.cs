using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shop
{
    public class Restore_Panel : MonoBehaviour
    {
        public GameObject backupPanel;
        public GameObject backup_raycast;

        private void OnEnable()
        {
            backupPanel.SetActive(false);
            backup_raycast.SetActive(false);
        }


        public void OnClick_Ok()
        {
            this.gameObject.SetActive(false);
        }
    }
    
}
