using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    public class Sheep : MonoBehaviour
    {
        public event EventHandler sheep_event;
        private IEnumerator OnCollisionEnter2D(Collision2D other)
        {
            other.gameObject.GetComponent<IDestroy_Action>().Destroy_Action();
            yield return null;  
        }

        public void End()
        {
            object obj = new object();
            EventArgs arge = new EventArgs();
            sheep_event.Invoke(obj, arge);
        }
    }
}
