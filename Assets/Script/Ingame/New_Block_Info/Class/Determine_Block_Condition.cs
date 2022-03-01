
using UnityEngine;
using System;

namespace NewBlock
{
    public class Determine_Block_Condition
    {
        private bool value;
        private Sprite Img;
        private String description;
        private String name;
        public Determine_Block_Condition(int blockType, int type)
        {
            switch (type)
            {   
                default:
                    Determine_Transform_Condition(blockType);
                    break;

                case 1:
                    Determine_Classy_Condition(blockType);
                    break;
                
                case 2:
                    Determine_Obstacle_Condition(blockType);
                    break;
            }
        }

        public bool Get_Value()
        {
            return value;
        }

        public String Get_Name()
        {
            return name;
        }

        public String Get_Description()
        {
            return description;
        }

        public Sprite Get_Img()
        {
            return Img;
        }
        
        private void Determine_Transform_Condition(int blockType)
        {
            value = false;
            var path = "Ingame/UI/Block_Preview/Transformed/";
            switch (blockType)
            {
                default:
                    break;
                
                case 0:
                    if (PlayerPrefs.GetInt("triangle_new", 0) == 0)
                    {
                        value = true;
                        name = "Triangle";
                        description = "A triangular box.";
                        Img = Resources.Load<Sprite>(path + name);
                        PlayerPrefs.SetInt("triangle_new",1);
                    }
                    break;

                case 1:
                    if (PlayerPrefs.GetInt("half_new", 0) == 0)
                    {
                        value = true;
                        PlayerPrefs.SetInt("half_new",1);
                        name = "Half and Half";
                        description =  "Two triangular boxes appear in one compartment.You have to destroy both, but they have half HP.";
                        Img = Resources.Load<Sprite>(path + name);
                    }
                    break;
                
                case 2:
                    if (PlayerPrefs.GetInt("circle_new", 0) == 0)
                    {
                        value = true;
                        PlayerPrefs.SetInt("circle_new",1);
                        name = "Circle";
                        description = "A circular box.No one knows where the ball will bounce.";
                        Img = Resources.Load<Sprite>(path + name);
                    }
                    break;
                    
            }
            
        }

        private void Determine_Classy_Condition(int blockType)
        {
                       
            value = false;
            var path = "Ingame/Box/x2_Box/";
            switch (blockType)
            {
                default:
                    break;

                case 0:
                    if (PlayerPrefs.GetInt("rect_x2_new", 0) == 0)
                    {
                        value = true;
                        name = "Wooden Square";
                        description = "A rectangular box with double HP.";
                        Img = Resources.Load<Sprite>(path + "Square");
                        PlayerPrefs.SetInt("rect_x2_new", 1);
                    }

                    break;

                case 1:
                    if (PlayerPrefs.GetInt("tri_x2_new", 0) == 0)
                    {
                        value = true;
                        PlayerPrefs.SetInt("tri_x2_new", 1);
                        name = "Wooden Triangle";
                        description =
                            "A triangular box with double HP.";
                        Img = Resources.Load<Sprite>(path + "Triangle");
                    }

                    break;
                

                case 2:
                    if (PlayerPrefs.GetInt("cir_x2_new", 0) == 0)
                    {
                        value = true;
                        PlayerPrefs.SetInt("cir_x2_new", 1);
                        name = "Wooden Circle";
                        description =
                            "A circular boxes with double HP.";
                        Img = Resources.Load<Sprite>(path + "Circle");
                    }

                    break;
            }

        }
        

        private void Determine_Obstacle_Condition(int blockType)
        {
            value = false;
            var path = "Ingame/UI/Block_Preview/Obstacle/";
            switch (blockType)
            {
                default:
                    break;
                
                case 0:
                    if (PlayerPrefs.GetInt("rect_ob_new", 0) == 0)
                    {
                        value = true;
                        name = "Square-Obstacle";
                        description = "A special box that will not be destroyed. But you will not be game-over if it reaches the bottom.";
                        Img = Resources.Load<Sprite>(path + name);
                        PlayerPrefs.SetInt("rect_ob_new",1);
                    }
                    break;
                
                case 1:
                    if (PlayerPrefs.GetInt("tri_ob_new", 0) == 0)
                    {
                        value = true;
                        PlayerPrefs.SetInt("tri_ob_new",1);
                        name = "Triangle-Obstacle";
                        description = "A special box that will not be destroyed with triangle shape.But you will not be game-over if it reaches the bottom.";
                        Img = Resources.Load<Sprite>(path + name);
                    }
                    break;
            }
        }
    }
}
