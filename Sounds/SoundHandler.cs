using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Sprint_1.Sounds
{
    public class SoundHandler
    {
        Collection<SoundEffectInstance> playerSounds;
        Collection<SoundEffectInstance> gameSounds;
        Collection<SoundEffectInstance> enemySounds;
        private static SoundHandler _instance;

        public static SoundHandler GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SoundHandler();
            }
            return _instance;
        }


        public Collection<SoundEffectInstance> PlayerSounds
        {
            get { return playerSounds; }
        }

        public Collection<SoundEffectInstance> EnemySounds
        {
            get { return enemySounds; }
        }

        public Collection<SoundEffectInstance> GameSounds
        {
            get { return gameSounds; }
        }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum SoundType
        {
            Dead, Key, LinkDamage, Rupee, Sword, GameOver, DoorUnlock, Heart,Time, Bomb, Boss
        }
        public void CreatePlayerList(ContentManager content)
        {
            playerSounds = new Collection<SoundEffectInstance>() { 
            AddToSoundList(content, SoundType.Dead),  //new = 0 old = 0
            AddToSoundList(content, SoundType.Rupee),  //new = 1 old = 1
            AddToSoundList(content, SoundType.Key), //new = 2 old = 2
            AddToSoundList(content, SoundType.Heart), //new = 3 old = 3
            AddToSoundList(content, SoundType.Sword),  //new = 4  old = 6        
            AddToSoundList(content, SoundType.LinkDamage), // new = 5 old = 7
            AddToSoundList(content, SoundType.Bomb) //new = 6 old = X

            }; 
        }
        public void CreateGameSoundsList(ContentManager content)
        {
                gameSounds = new Collection<SoundEffectInstance>() { 
                AddToSoundList(content, SoundType.Time), //new = 0 old = 0
                AddToSoundList(content, SoundType.DoorUnlock), //new = 1 old = 1
                AddToSoundList(content, SoundType.GameOver)//new = 2 old = 4 

            };           
        }

        public void CreateEnemySounds(ContentManager content)
        {
            enemySounds = new Collection<SoundEffectInstance>()
            {
                AddToSoundList(content, SoundType.Boss)
            };
        }

        public static SoundEffectInstance AddToSoundList(ContentManager content, SoundType soundType)
        {
            switch (soundType)
            {
                case (SoundType.Dead):
                    SoundEffect dead = content.Load<SoundEffect>("Sounds/LOZ_Link_Die"); //0
                    return dead.CreateInstance();
                case (SoundType.GameOver):
                    SoundEffect gameOver = content.Load<SoundEffect>("Sounds/LOZ_Recorder");//1
                    return gameOver.CreateInstance();
                case (SoundType.Key):
                    SoundEffect power = content.Load<SoundEffect>("Sounds/LOZ_Get_Item");//2
                    return power.CreateInstance();
                case (SoundType.LinkDamage):
                    SoundEffect damage = content.Load<SoundEffect>("Sounds/LOZFDS_Link_Hurt");//3
                    return damage.CreateInstance();
                case (SoundType.Rupee):
                    SoundEffect coin = content.Load<SoundEffect>("Sounds/LOZFDS_Get_Rupee");//4
                    return coin.CreateInstance();
                case (SoundType.Sword):
                    SoundEffect stomp = content.Load<SoundEffect>("Sounds/LTTP_Sword1");//5
                    return stomp.CreateInstance();                
                case (SoundType.DoorUnlock):
                    SoundEffect appear = content.Load<SoundEffect>("Sounds/LOZFDS_Door_Unlock");//6
                    return appear.CreateInstance();
                case (SoundType.Heart):
                    SoundEffect life = content.Load<SoundEffect>("Sounds/LOZ_Get_Heart");//7
                    return life.CreateInstance();                
                case (SoundType.Time):
                    SoundEffect time = content.Load<SoundEffect>("Sounds/LOZ_Fanfare"); // 8
                    return time.CreateInstance();
                case (SoundType.Bomb):
                    SoundEffect bomb = content.Load<SoundEffect>("Sounds/LOZ_Bomb_Blow"); //9
                    return bomb.CreateInstance();
                case (SoundType.Boss):
                    SoundEffect boss = content.Load<SoundEffect>("Sounds/LOZFDS_Boss_Scream1"); //10
                    return boss.CreateInstance();
                default:
                    return null;
            }
        }
    }
}
