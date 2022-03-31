using System.Collections.Generic;
using Block;
using Ingame_Data;
using Manager;
using Particle;
using Progetile;
using Score;
using Tutorial;
using UnityEngine;

namespace Ingame
{
    /// <summary>
    /// 명령을 받으면 해당 박스를 리스폰 해주는 컴포넌트 
    /// </summary>
    public class RespawnBox : MonoBehaviour
    {
        [SerializeField] private Transform respawnGroup;
        
        [Header("Basic")]
        public GameObject box, triangle1, triangle2, triangle3, triangle4,  circle, half, half_2;

        [Header("x2_Box")]
        public GameObject x2Box, x2Triangle1, x2Triangle2, x2Triangle3, x2Triangle4,  x2Circle; 

        [Header("Obstacle")] 
        public GameObject boxObstacle, triObstacle1, triObstacle2, triObstacle3, triObstacle4;
        
        [Header("Particle")] 
        [SerializeField] private BlockParticle_Manage particle;

        [Header("HP_INFO")] private int stage;
        [SerializeField] private ScoreManager scoreManager;

        [SerializeField] private Progetile_Particle _particle;
        public Transform boxGroup;
        
        
        #region Spawn_Box_ONFIELD   
        private void Spawn_Transformed_Box(int transformedNum)
        {
            int rand;
            GameObject obj;
            IBox ibox;
            IBox ibox_2 = null;
            int rand_2 = 0;
            switch (transformedNum)
            {
                default:
                    return;

                case 0:
                    rand = UnityEngine.Random.Range(0, 4);
                    blocktype type;
                    switch (rand)
                    {
                        default:
                            obj = Instantiate(triangle1);
                            type = blocktype.NORMAL_TRI1;
                            break;
                        
                        case 1:
                            obj = Instantiate(triangle2);
                            type = blocktype.NORMAL_TRI2;
                            break;
                        
                        case 2:
                            obj = Instantiate(triangle3);
                            type = blocktype.NORMAL_TRI3;
                            break;
                
                        case 3:
                            obj = Instantiate(triangle4);
                            type = blocktype.NORMAL_TRI4;
                            break;         
                    }
                    ibox = obj.GetComponent<IBox>();
                    ibox.Set_ColorType(1,particle.Set_Particle());
                    ibox.Set_Type(type);
                    break;
                
                
                case 1:
                    rand_2 = UnityEngine.Random.Range(0, 2);
                    switch (rand_2)
                    {
                        default:
                            obj = Instantiate(half);
                            break;
                        case 1:
                            obj = Instantiate(half_2);
                            break;
                    }
                    ibox = obj.transform.GetChild(0).GetComponent<IBox>();
                    ibox_2 = obj.transform.GetChild(1).GetComponent<IBox>();
                    break;
                
                case 2:
                    obj = Instantiate(circle);
                    ibox = obj.GetComponent<IBox>();
                    ibox.Set_ColorType(2,particle.Set_Particle()); // 
                    ibox.Set_Type(blocktype.NORMAL_CIRCLE);
                    break;
            }
            
            // 반반 박스는 다르게 설정해줘야함 
            switch (transformedNum)
            {
                default:
                    ibox.Set_HP(stage);
                    break;

                case 1: // 반반박스인경우 
                    if (rand_2 == 0)
                    {
                        ibox.Set_Type(blocktype.NORMAL_TRI1);
                        ibox_2.Set_Type(blocktype.NORMAL_TRI4);
                    }
                    else
                    {
                        ibox.Set_Type(blocktype.NORMAL_TRI2);
                        ibox.Set_Type(blocktype.NORMAL_TRI3);
                    }

                    ibox.Set_HP(stage/2);
                    ibox.Set_Event(scoreManager.args, ref _particle);
                    ibox.Set_ColorType(1,particle.Set_Particle());
                    ibox_2.Set_HP(stage/2);
                    ibox_2.Set_ColorType(1,particle.Set_Particle());
                    ibox_2.Set_Event(scoreManager.args, ref _particle);
                    break; 
            }
            
            ibox.Set_Event(scoreManager.args, ref _particle);
            obj.transform.SetParent(respawnGroup);
            obj.transform.SetAsLastSibling();
        }

        private void Spawn_Basic_Box()
        {
            GameObject obj;
            IBox ibox;
            obj = Instantiate(box);
            ibox = obj.GetComponent<IBox>();                  
            ibox.Set_ColorType(0,particle.Set_Particle());
            obj.transform.SetParent(respawnGroup);
            obj.transform.SetAsLastSibling();
            ibox.Set_HP(stage);
            ibox.Set_Event(scoreManager.args, ref _particle);
            ibox.Set_Type(blocktype.NORMAL_RECT);
        }

        private void Spawn_Classy_Box(int classyNum)
        {
            GameObject obj = null;
            IBox ibox;
            blocktype type;
            switch (classyNum)
            {
                default:
                    return;
                
                case 0:
                    obj = Instantiate(x2Box);
                    type = blocktype.X2_RECT;
                    break;
                
                case 1:
                    int rand = UnityEngine.Random.Range(0, 4);
                    switch (rand)
                    {
                        default:
                            obj = Instantiate(x2Triangle1);
                            type = blocktype.X2_TRI1;
                            break;
                        
                        case 1:
                            obj = Instantiate(x2Triangle2);
                            type = blocktype.X2_TRI2;
                            break;
                        
                        case 2:
                            obj = Instantiate(x2Triangle3);
                            type = blocktype.X2_TRI3;
                            break;
                        
                        case 3:
                            obj = Instantiate(x2Triangle4);
                            type = blocktype.X2_TRI4;
                            break;
                    }
                    break;
                
                
                case 2:
                    obj = Instantiate(x2Circle);
                    type = blocktype.X2_CIRCLE;
                    break;
            }
            
            ibox = obj.GetComponent<IBox>();
            ibox.Set_ColorType(classyNum,particle.Set_Particle());
            obj.transform.SetParent(respawnGroup);
            obj.transform.SetAsLastSibling();
            ibox.Set_Event(scoreManager.args, ref _particle);
            ibox.Set_HP(2*stage);
            ibox.Set_Type(type);
        }

        private void Spawn_Obstacle(int obstacleNum)
        {
            int rand;
            if (obstacleNum != 0)
                rand = UnityEngine.Random.Range(0, 2);

            else
                rand = 0;
                
            IBox ibox;
            GameObject obj;
            blocktype type;
            switch (rand)
            {
                default:
                    obj = Instantiate(boxObstacle);
                    type = blocktype.OBSTACLE_RECT;
                    break;
                
                case 1:
                    rand = UnityEngine.Random.Range(0, 4);
                    switch (rand)
                    {
                        default:
                            obj = Instantiate(triObstacle1);
                            type = blocktype.OBSTACLE_TRI1;
                            break;
                        
                        case 1:
                            obj = Instantiate(triObstacle2);
                            type = blocktype.OBSTACLE_TRI2;
                            break;
                        // 수정 해야함 
                        case 2:
                            obj = Instantiate(triObstacle3);
                            type = blocktype.OBSTACLE_TRI3;
                            break;
                        
                        case 3:
                            obj = Instantiate(triObstacle4);
                            type = blocktype.OBSTACLE_TRI4;
                            break;
                    }

                    break;
            }
            ibox = obj.GetComponent<IBox>();
            ibox.Set_Type(type);
            ibox.Set_ColorType(obstacleNum,particle.Set_Particle());
            ibox.Set_Event(scoreManager.args, ref _particle);
            obj.transform.SetParent(respawnGroup);
            obj.transform.SetAsLastSibling();
            // obj 타입을 반영하여 데미지 안입도록 해야함 
            
        }

        #endregion

        #region  Action
        public void Event_Occur(int type, int num)
        {
            switch (type)
            {
                // 노말 박스 띄우는 경우 
                default:
                    Spawn_Basic_Box();
                    break;
                
                case 1:
                    Spawn_Transformed_Box(num);
                    break;
                
                case 2:
                    Spawn_Classy_Box(num);
                    break;
                
                case 3:
                    Spawn_Obstacle(num);
                    break;
                
                case 4:
                    // 피냐타 생성 
                    break;
                
                // 첫 튜토리얼에 필요한 박스 생성 
                case 90:
                    GameObject obj;
                    IBox ibox;
                    obj = Instantiate(box); 
                    ibox = obj.GetComponent<IBox>();                  
                    ibox.Set_ColorType(0,particle.Set_Particle());
                    obj.transform.SetParent(respawnGroup);
                    obj.transform.SetAsLastSibling();
                    obj.transform.position = _Determine_Pos.Which_Pos(1, 0);
                    ibox.Set_HP(1);
                    ibox.Set_Event(scoreManager.args, ref _particle);
                    break;
                
                case 91:
                    GameObject obj2;
                    IBox ibox2;
                    obj = Instantiate(box); 
                    ibox = obj.GetComponent<IBox>();                  
                    ibox.Set_ColorType(0,particle.Set_Particle());
                    obj.transform.SetParent(respawnGroup);
                    obj.transform.SetAsLastSibling();
                    obj.transform.position = new Vector3(473.2f, -380f);
                    ibox.Set_HP(1);
                    ibox.Set_Event(scoreManager.args, ref _particle);
                    break;

                
                // HP 초기화
                case 99:
                    stage = num;
                    if(stage == 5 && !Tutorial_Candle_Data.Get_Done_Data())
                        Tutorial_Candle_Data.Set_respawn_Data();
                    break;
            }
        }
        
        #endregion

        #region Load_Data

        public void Load_SpawnData(ref List<BoxInfoVO> dataList)
        {
            for (int i = 0; i < dataList.Count; i++)
                Spawn_OnField(dataList[i]);


        }


        /// <summary>
        /// 필드에 박스를 스폰하는 함수인데, 데이터 로드 할 때 쓰는 함수 
        /// </summary>
        /// <param name="spawnData"></param>
        private void Spawn_OnField(BoxInfoVO spawnData)
        {
            GameObject obj;
            IBox ibox;
            switch (spawnData.type)
            {
                default: // 기본 박스 스폰하는 경우 
                    obj = Instantiate(box);
                    break;
                case blocktype.NORMAL_TRI1:
                    obj = Instantiate(triangle1);
                    break;
                
                case blocktype.NORMAL_TRI2:
                    obj = Instantiate(triangle2);
                    break; 
                    
                case blocktype.NORMAL_TRI3:
                    obj = Instantiate(triangle3);
                    break;
                    
                case blocktype.NORMAL_TRI4:
                    obj = Instantiate(triangle4);
                    break;
            }

            obj.transform.position = spawnData.pos; // 위치 지정 
            obj.transform.SetParent(boxGroup);
            ibox = obj.GetComponent<IBox>();
            ibox.Set_Type(spawnData.type); // type 지정 
            ibox.Set_HP(spawnData.hp); // hp 지정 
            ibox.Set_ColorType(1,particle.Set_Particle()); // 파티클 색상 지정 
            if (spawnData.isCandle != 1)
            {
                ibox.Set_Candle(spawnData.isCandle); // 캔들 여부 지정 
            }

            ibox.Set_Event(scoreManager.args, ref _particle);
        }
        #endregion

    }
}
