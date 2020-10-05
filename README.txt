README.txt
This is our Zelda game. The goal is to move through levels and to defeat the boss at the end to win. Link needs keys to unlock
doors, which are spread throughout the level or dropped by enemies. Link has a health bar, which decreases if he is hit by an 
enemy. He dies if all hearts are lost.
NEW KEY FUNCTIONS:
To display the minimap, press p. To attack an enemy press k. w/up arrow moves Link up, s/down arrow moves Link down.

SUPPRESSED WARNINGS:
CA 1812 LevelDefinition is an internal class that is never instantiated...
	-It does have a private constructor. We plan to only use Xml deserialization to create the level definition, 
		so this is to prevent any other instantiation.

CA 1812 SpriteData is an internal class that is never instantiated...
	-It does have a private constructor. We plan to only use Xml deserialization to create the SpriteData objects, 
		so this is to prevent any other instantiation.

CA 1024 Change SoundHandler.GetInstance() to a property.
	-This is a singleton pattern.

CA 1024 Change SpriteFactory.GetInstance() to a property.
	-This is a singleton pattern.

CA 1822 The this parameter of SpriteFactory.MakeSprite is never used...
	-This is a singleton pattern, we don't want this to be static.

CA 1822 The this parameter of SpriteFactory.MakeSprite (BackgroundSprites) is never used...
	-This is a singleton pattern, we don't want this to be static.
CA 1024 Change GUD.GetInstance() to a property.
	- Again, this is a singleton pattern.