
using System.Collections;
using Ingame;
using UnityEngine;
using DG.Tweening;

namespace Charater
{
    //Q. 시작 위치 
    public class Charater_Flight : MonoBehaviour
    {
        public SpriteRenderer sprite;
        public int index; // 어떤 양초인지 저장하는 변수 
        public GameObject target_obj;
        public ParticleSystem particle;
        private Vector2 target_pos;
        private Vector2 target_scale = new Vector2(0.669f, 0.669f);
        private Sequence comboSequence;
        


        private void Start()
        {
            var Flight_info = new Flight_Info(index); // 비행에 필요한 변수들을 instance를 통해 가져옴 
            target_pos = Flight_info.flight_Pos();
            sprite.sprite = Flight_info.charater_img(); // 이미지를 설정해줌 
            Set_color();
            comboSequence = DOTween.Sequence()
                .SetAutoKill(false)
                .Append(transform.DOMove(target_pos, 1.5f)
                    .SetEase(Ease.OutExpo).OnComplete((() => StartCoroutine(Flight()))))
                .Join(transform.DOScale(target_scale, 1.0f));
            
        }

        private IEnumerator Flight()
        {
            target_obj.SetActive(true);
            transform.localScale = target_scale;
            if (PlayerPrefs.GetInt("Tutorial_Advance_Candle_Charater", 0) == 0)
            {
                GameManage manage = GameObject.FindWithTag("GameController").GetComponent<GameManage>();
                manage.Event_Receive(Event_num.TUTORIAL_CHAR);
            }
            yield return new WaitForSeconds(1.0f);
            comboSequence.Kill();
            Destroy(this.gameObject);
            yield break;
        }


        /// <summary>
        /// 파티클 컬러를 지정해주는 함수 
        /// </summary>
        private void Set_color()
        {
            Color[] color = new Color[2];
            Gradient ourGradientMin, ourGradientMax;
            var colormodule = particle.colorOverLifetime;
            var color_mode = colormodule.color;
            color_mode.mode = ParticleSystemGradientMode.TwoGradients;
            
            switch (index)
            {
                default: // P
                    color[0] = new Color(255f / 255f, 103f / 255f, 234f / 255f);
                    color[1] = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                    break;
                
                case 1:  // I
                    color[0] = new Color(255f / 255f, 175f / 255f, 112f / 255f);
                    color[1] = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                    break;
                    
                case 2: // N
                    color[0] = new Color(255f / 255f, 226f / 255f, 93f / 255f);
                    color[1] = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                    break;
                
                case 3: // A
                    color[0] = new Color(165f / 255f, 246f / 255f, 149f / 255f);
                    color[1] = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                    break;
                
                case 4: // t
                    color[0] = new Color(156f / 255f, 181f / 255f, 240f / 255f);
                    color[1] = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                    break;
                
                case 5: // A
                    color[0] = new Color(218f / 255f, 152f / 255f, 240f / 255f);
                    color[1] = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                    break;
            }

            ourGradientMin = new Gradient();
            ourGradientMin.mode = GradientMode.Blend;
            ourGradientMin.SetKeys(                
                new GradientColorKey[]
                {
                    new GradientColorKey(color[0], 0.85f), new GradientColorKey(color[1], 0.88f), new GradientColorKey(color[0],0.91f)
                    ,new GradientColorKey(color[1], 0.94f), new GradientColorKey(color[0], 0.97f), new GradientColorKey(color[1], 0.1f)
                },
                new GradientAlphaKey[]
                    {new GradientAlphaKey(1f, 1.0f), new GradientAlphaKey(1f, 1.0f)});
                
            switch (index)
            {
                default: // P
                    color[1] = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                    color[0] = new Color(255f / 255f, 82f / 255f, 170f / 255f);
                    break;
                
                case 1:  // I
                    color[1] = new Color(255f / 255f, 255f / 255f, 255f / 255f);;
                    color[0] = new Color(255f / 255f, 122f / 255f, 93f / 255f);
                    break;
                    
                case 2: // N
                    color[1] = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                    color[0] = new Color(255f / 255f, 184f / 255f, 93f / 255f);
                    break;
                
                case 3: // A
                    color[1] = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                    color[0] = new Color(101f / 255f, 219f / 255f, 161f / 255f);
                    break;
                
                case 4: // t
                    color[1] = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                    color[0] = new Color(143f / 255f, 145f / 255f, 240f / 255f);
                    break;
                
                case 5: // A
                    color[1] = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                    color[0] = new Color(195f / 255f, 104f / 255f, 245f / 255f);
                    break;
            }
            
            ourGradientMax = new Gradient();
            ourGradientMax.mode = GradientMode.Blend;
            ourGradientMax.SetKeys(                
                new GradientColorKey[]
                {
                    new GradientColorKey(color[0], 0.85f), new GradientColorKey(color[1], 0.88f), new GradientColorKey(color[0],0.91f)
                    ,new GradientColorKey(color[1], 0.94f), new GradientColorKey(color[0], 0.97f), new GradientColorKey(color[1], 1f)
                },
                new GradientAlphaKey[]
                    {new GradientAlphaKey(1f, 1.0f), new GradientAlphaKey(1f, 1.0f)});
            colormodule.color= new ParticleSystem.MinMaxGradient(ourGradientMin, ourGradientMax);
        }
            
        
    }
}



