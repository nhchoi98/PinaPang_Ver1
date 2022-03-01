using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    public class Revive_Control : MonoBehaviour
    {
        public void End()
        {
            Destroy(this.gameObject);
        }
    }
}
