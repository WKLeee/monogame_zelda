using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint_1.Interfaces;
using Sprint_1.Sprites;

namespace Sprint_1
{

    class LevelDefinition
    {

        //These values public to allow serialization/deserialization. Warning disabled since values will be set through deserialization
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value 
        //Temporarily used for quickly generating a floor for testing
        public bool floorBlocks;
        public int playerX;
        public int playerY;
        public List<SpriteData> foregroundSpriteData;
        public List<SpriteData> midgroundSpriteData;
        public List<SpriteData> backgroundSpriteData;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value

        private ISprite link;
        private List<ISprite> foregroundSprites;
        private List<ISprite> midgroundSprites;
        private List<ISprite> backgroundSprites;

        //We plan to only use Xml deserialization to create the level definition, so this is to prevent any other instantiation
        private LevelDefinition()
        {
        }

        //Moved from constructor to own method to call after link is made
        public void MakeLevel(SpriteFactory factory, Grid grid, Transition transition, int widthMult)
        {            
            foregroundSprites = new List<ISprite>();
            MakeMidAndBackground(factory);

            MakeRoom(grid, factory, widthMult, transition, 0, 0);
            MakeRoom(grid, factory, widthMult, transition, 400, 240);
            MakeRoom(grid, factory, widthMult, transition, 400, 0);
            MakeRoom(grid, factory, widthMult, transition, 800, 240);
            MakeRoom(grid, factory, widthMult,  transition, 400, 480);
            MakeRoom(grid, factory, widthMult, transition, 400, 720);
            MakeRoom(grid, factory, widthMult, transition, 0, 720);
            //Below room is the boss room
            MakeRoom(grid, factory, widthMult, transition, 800, 720);

            MakeForeground(factory, grid, (PlayerSprite)link, transition);
        }

        private void MakeRoom(Grid grid, SpriteFactory factory, int widthMult, Transition transition, int baseWidth, int baseHeight)
        {
            if (floorBlocks)
            {
                for (int i = 0; i < grid.TotalWidth / 32 / widthMult; i++)
                {
                    ISprite floor = factory.MakeSprite(SpriteFactory.SpriteType.Wall, new Vector2(baseWidth+i * 16, baseHeight+224), grid, (PlayerSprite)link, transition);
                    ISprite ceiling = factory.MakeSprite(SpriteFactory.SpriteType.Wall, new Vector2(baseWidth + i * 16, baseHeight), grid, (PlayerSprite)link, transition);

                    foregroundSprites.Add(floor);
                    foregroundSprites.Add(ceiling);
                }

                for (int j = 0; j < grid.TotalHeight / 96; j++)
                {
                    ISprite side = factory.MakeSprite(SpriteFactory.SpriteType.Wall, new Vector2(baseWidth, baseHeight+j * 16), grid, (PlayerSprite)link, transition);
                    ISprite side2 = factory.MakeSprite(SpriteFactory.SpriteType.Wall, new Vector2(baseWidth+transition.RoomSize, baseHeight+j * 16), grid, (PlayerSprite)link, transition);

                    foregroundSprites.Add(side);
                    foregroundSprites.Add(side2);
                }


            }
        }

        private void MakeForeground(SpriteFactory factory, Grid grid, PlayerSprite player, Transition transition)
        {            

            foreach(SpriteData data in foregroundSpriteData)
            {
                ISprite sprite = factory.MakeSprite(data.type, new Vector2(data.x, data.y), grid, player, transition);
                foregroundSprites.Add(sprite);

                if (data.hasItems)
                {
                    foreach (SpriteFactory.SpriteType type in data.hidden)
                    {
                        //<0,0> is temporary location, actual location will be set by block before revealing the item
                        ISprite item = factory.MakeSprite(type, Vector2.Zero, grid, (PlayerSprite)link, transition);
                        item.ToggleVisibility();
                        //((BlockSprite)sprite).AddItem((ItemSprite)item);
                        foregroundSprites.Add(item);
                    }
                }
            }
        }

        private void MakeMidAndBackground(SpriteFactory factory)
        {
            midgroundSprites = new List<ISprite>();

            foreach (SpriteData data in midgroundSpriteData)
            {
                ISprite sprite = factory.MakeSprite(data.type, new Vector2(data.x, data.y));
                midgroundSprites.Add(sprite);
            }

            backgroundSprites = new List<ISprite>();

            foreach (SpriteData data in backgroundSpriteData)
            {
                ISprite sprite = factory.MakeSprite(data.type, new Vector2(data.x, data.y));
                backgroundSprites.Add(sprite);
            }
        }

        public void MakeLink(SpriteFactory factory, Grid grid, Transition transition)
        {
            link = factory.MakeSprite(SpriteFactory.SpriteType.PlayerSprite, new Vector2(playerX, playerY),
                grid, null, transition);
        }

        public ISprite GetLink()
        {
            return link;
        }
        public List<ISprite> GetForeground()
        {
            return foregroundSprites;
        }

        public List<ISprite> GetMidground()
        {
            return midgroundSprites;
        }

        public List<ISprite> GetBackground()
        {
            return backgroundSprites;
        }
    }

    class SpriteData
    {
        //These values public to allow serialization/deserialization. Warning disabled since values will be set through deserialization
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
        public SpriteFactory.SpriteType type;
        public int x;
        public int y;
        public bool hasItems;
        public List<SpriteFactory.SpriteType> hidden;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value

        //We plan to only use Xml deserialization to create the SpriteData objects, so this is to prevent any other instantiation
        private SpriteData()
        {

        }
    }

}
