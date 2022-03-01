using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loading
{
    public class Quad_Script : MonoBehaviour
    {
        public float scrollSpeed = 0.1f;
        private Material myMaterial;
        void Start()
        {
            myMaterial = GetComponent<Renderer>().material;
        }

        // Update is called once per frame
        void Update()
        {
            float _OffsetY = (myMaterial.mainTextureOffset.x + scrollSpeed * Time.deltaTime);
            Vector2 Offset = new Vector2(_OffsetY,_OffsetY);
            myMaterial.mainTextureOffset = Offset;
        }
    } 
}
