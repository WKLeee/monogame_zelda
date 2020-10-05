using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Sprint_1.Sounds
{
    public class SoundFactory
    {
        private static SoundFactory _instance;

        public static SoundFactory GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SoundFactory();
            }
            return _instance;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum SoundType
        {
            Dead, Key, LinkDamage, Rupee, Sword, GameOver, DoorUnlock, Heart, Time, Bomb, Boss
        }

        public static SoundEffectInstance AddToSoundList(ContentManager content, SoundType soundType)
        {
            switch (soundType)
            {
                case (SoundType.Dead):
                    SoundEffect dead = content.Load<SoundEffect>("Sounds/LOZ_Link_Die");
                    return dead.CreateInstance();
                case (SoundType.GameOver):
                    SoundEffect gameOver = content.Load<SoundEffect>("Sounds/LOZ_Recorder");
                    return gameOver.CreateInstance();
                case (SoundType.Key):
                    SoundEffect power = content.Load<SoundEffect>("Sounds/LOZ_Get_Item");
                    return power.CreateInstance();
                case (SoundType.LinkDamage):
                    SoundEffect damage = content.Load<SoundEffect>("Sounds/LOZFDS_Link_Hurt");
                    return damage.CreateInstance();
                case (SoundType.Rupee):
                    SoundEffect coin = content.Load<SoundEffect>("Sounds/LOZFDS_Get_Rupee");
                    return coin.CreateInstance();
                case (SoundType.Sword):
                    SoundEffect stomp = content.Load<SoundEffect>("Sounds/LTTP_Sword1");
                    return stomp.CreateInstance();
                case (SoundType.DoorUnlock):
                    SoundEffect appear = content.Load<SoundEffect>("Sounds/LOZFDS_Door_Unlock");
                    return appear.CreateInstance();
                case (SoundType.Heart):
                    SoundEffect life = content.Load<SoundEffect>("Sounds/LOZ_Get_Heart");
                    return life.CreateInstance();
                case (SoundType.Time):
                    SoundEffect time = content.Load<SoundEffect>("Sounds/LOZ_Fanfare");
                    return time.CreateInstance();
                case (SoundType.Bomb):
                    SoundEffect bomb = content.Load<SoundEffect>("Sounds/LOZ_Bomb_Blow");
                    return bomb.CreateInstance();
                case (SoundType.Boss):
                    SoundEffect boss = content.Load<SoundEffect>("Sounds/LOZFDS_Boss_Scream1");
                    return boss.CreateInstance();
                default:
                    return null;
            }
        }
    }
}
