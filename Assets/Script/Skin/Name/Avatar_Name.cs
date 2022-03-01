
namespace Skin
{
    public class Avatar_Name
    {
        public string Set_Charater_Name(int index, bool is_skin)
        {
            int avatar_num;
            if (is_skin)
                avatar_num = Calc_Index.Get_Avatar_Num(index);

            else
                avatar_num = index;
                
            
            switch (avatar_num/1000)
            {
                default:
                    return normal_name(avatar_num % 1000);
                
                case 1:
                    return rare_name(avatar_num % 1000);
                
                case 2:
                    return unique_name(avatar_num % 1000);
            }
        }

        private string normal_name(int index)
        {
            switch (index)
            {
                default:
                    return "Kevin";

                case 1:
                    return "Minx";

                case 2:
                    return "Tennis Boy";

                case 3:
                    return "Baseball Boy";

                case 4:
                    return "Basketball Boy";

                case 5:
                    return "Marine Boy";

                case 6:
                    return "Scamp";

                case 7:
                    return "Firefighter";

                case 8:
                    return "Magician";
                
                case 9:
                    return "Dracula";

                case 10:
                    return "Ninja";

                case 11:
                    return "Alien";

                case 12:
                    return "Nerd";

                case 13:
                    return "Scientist";
                
                case 14:
                    return "Secret Agent";

                case 15:
                    return "Disco singer";

                case 16:
                    return "Noblesse";
                
                case 17:
                    return "Silent Comedian";
                
                case 18:
                    return "Ballerina";

                case 19:
                    return "Emperor";
                
            }

        }
        
        private string rare_name(int index)
        {
            switch (index)
            {
                default:
                    return "Baby Driver";

                case 1:
                    return "Clown";

                case 2:
                    return "Mermaid";

                case 3:
                    return "Pizza Boy";

                case 4:
                    return "Statue";
                
                case 5:
                    return "Monkey Suit";
                
                case 6:
                    return "Party Girl";
            }
        }

        private string unique_name(int index)
        {
            switch (index)
            {
                default:
                    return "Astronaut";

                case 1:
                    return "Pajama Boy";
                
                case 2:
                    return "Teddy Bear";
            }
            
        }
        
    }
}
