
using System.Collections;
using Skin;
using UnityEngine;

namespace Manager
{
    public class Line_Animation : MonoBehaviour
    {
        public LineRenderer _LineRenderer;
        public LineRenderer _secondLine;
        private IEnumerator make_line, remove_line; // 반복자를 저장해놓는 변수. 동작 전 반복자에 할당되어있는 Ienumorator를 중지 시키고, 그 다음 작업 수행 
        private Gradient active, deactive;
        private float const_tiling;
        private float distance;
        private float const_width;
        private bool second_active;
        
        public void Set_SecondLine(bool second_on)
        {
            if (second_on)
            {
                _secondLine.gameObject.SetActive(true);
                second_active = true;
            }

            else
            {
                _secondLine.gameObject.SetActive(false);
                second_active = true;
            }
        }
        void Start()
        {
            LineDAO data = new LineDAO();
            int target_index = data.Get_Equip_Data();
            const_width = Determine_Line.line_width(target_index);
            _LineRenderer.material = Determine_Line.line_material(target_index);
            _secondLine.material = Determine_Line.line_material(target_index);
            const_tiling = Determine_Line.Get_Line_tilingconst(target_index);

            active = new Gradient();
            active.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) },
                new GradientAlphaKey[] {new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f)}
            );

            deactive = new Gradient();
            deactive.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) },
                new GradientAlphaKey[] {new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f)}
            );

        }
        /// <summary>
        /// 첫 번쨰 점과 두 번째 점의 위치를 조정해 표현해줌 
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        public void Set_Line_Pos(Vector2 point1, Vector2 point2)
        {
            _LineRenderer.SetPosition(1,point1);
            if (second_active)
            {
                _secondLine.SetPosition(0,point1);
                _secondLine.SetPosition(1, point2);
                _secondLine.startWidth = const_width;
                _secondLine.endWidth = const_width;
                float second_distance = Vector2.Distance(point1, point2);
                _secondLine.material.mainTextureScale = new Vector2(second_distance*const_tiling,1);
            }
            
            _LineRenderer.startWidth = const_width;
            _LineRenderer.endWidth = const_width;
            distance = Vector2.Distance(_LineRenderer.GetPosition(0), point1);
            _LineRenderer.material.mainTextureScale = new Vector2(distance * const_tiling, 1);
            Make_Line();
        }

        /// <summary>
        /// 라인렌더러의 시작점을 조정해주는 함수 
        /// </summary>
        /// <param name="start_point"></param>
        public void Set_Start_Pos(Vector2 start_point)
        {
            _LineRenderer.SetPosition(0, start_point);
        } 


        /// <summary>
        /// 발사를 시작하면 선을 생성해주는 함수 
        /// </summary>
        public void Make_Line()
        {
            if (_LineRenderer.gameObject.activeSelf)
                return;
            
            _LineRenderer.gameObject.SetActive(true);
            _secondLine.gameObject.SetActive(true);
            if (remove_line != null)
                StopCoroutine(remove_line);

            make_line = _Make_Line();
            StartCoroutine(make_line);
            // 작업 수행 
            
        }

        private IEnumerator _Make_Line()
        {
            float target_time = 0.2f;
            float time = 0f;
            float alpha = _LineRenderer.colorGradient.alphaKeys[0].alpha;
            Gradient _gradient = new Gradient();
            
            while (true)
            {
                if (alpha < 0.98)
                {
                    alpha += Time.deltaTime/target_time;
                    _gradient.SetKeys(
                        _LineRenderer.colorGradient.colorKeys,
                        new GradientAlphaKey[] {new GradientAlphaKey(alpha, 0f), new GradientAlphaKey(alpha, 1f)});
                    _LineRenderer.colorGradient = _gradient;
                }

                else
                    break;
                
                yield return null;
            }

            _LineRenderer.colorGradient.alphaKeys = active.alphaKeys;
            make_line = null;
            yield return null;
        }


        /// <summary>
        /// 발사 중지시 선을 없애주는 함수 
        /// </summary>
        public void Remove_Line()
        {
            if (make_line != null)
                StopCoroutine(make_line);

            remove_line = _Remove_Line();
            StartCoroutine(remove_line);

            // 작업 수행 
        }

        IEnumerator _Remove_Line()
        {
            float target_time = 0.2f;
            float time = 0f;
            float alpha = _LineRenderer.colorGradient.alphaKeys[0].alpha;
            Gradient _gradient = new Gradient();
            /*
            while (true)
            {
                if (alpha > 0.05)
                {
                    Debug.Log("호출");
                    alpha -= Time.deltaTime/target_time;
                    _gradient.SetKeys(
                        _LineRenderer.colorGradient.colorKeys,
                        new GradientAlphaKey[] {new GradientAlphaKey(alpha, 0f), new GradientAlphaKey(alpha, 1f)});
                    _LineRenderer.colorGradient = _gradient;
                }

                else
                    break;
                
                yield return null;
            }
        */
            _LineRenderer.colorGradient.alphaKeys = deactive.alphaKeys;
            remove_line = null;
            _LineRenderer.gameObject.SetActive(false);
            _secondLine.gameObject.SetActive(false);
            yield return null;

        }
    }
}
