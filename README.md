# Interview Task

## Thought Process

First step was to set up a git hub repository and create a new Unity project
using the required 2021.3.2f1 version. After which I wanted to create this
readme file to help gide me trough the creation process and make it easier.
Keeping in mind that this is an interview project I've decided to write all the
code from scratch.

I wanted to first get some code working especially for the player and the
interactions, so that's what I did, just start with a basic white cube and make
it move and be able to "interact" with other cubes. For the interaction I wanted
to make it a bit more complex by checking for the closest interactable object in
cases where the player might be in range of 2 interactables simultaneously I
also decided to create a separate interface IInteractable to be able to handle
multiple different interactables if I need to do so later. I originally wanted
a prefab of a canvas where some text about how to interact and what that
interaction was, that I could spawn in and destroy when not needed but thinking
more about this I decided to have the player keep that canvas to him self and
enable it / edit it's text when needed so we don't spam the garbage collector.

Update after Diner:
After looking a bit more into the game I decided to change the movement system
to be more similar to the one found in the game. For this I used the
[NavMeshPlus][0] extension that allowed me to create a navmesh on a 2d plane,
this is used when the player clicks the screen to automatically navigate him to
the area where he click, and will also be used latter to move the player to the
correct interaction location. This type of movement can be cancelled by pressing
the keys to move performing another click to move or holding the click button
(which makes the player follow the mouse).

Today I started by importing the 2D sprites for the player and the shop keeper,
I decided to go with the [Mighty Heroes (Rogue) 2D Fantasy Characters Pack][1]
cause of their use of skeletal animation making it easier to swap sprites around
when the player changes clothes.
I created a separate script to handle animations and flipping the character
around depending if the player is moving left or right, the animations where
set-up is a very basic way using triggers with just 3 options (Idle, Walk,
Interact).

After setting up the movement I wanted to do a bit of a uplift to the visuals
I did this using the [2D Fantasy Forest Tileset][2]. I set up a tilemap for the
ground and used the provided sprites to serve as obstacles and interactables.
After setting up the map I wanted to move on to enabling mouse interactions but
for this I would need to display an outline around the interactable object to
inform the player he could interact. At first I thought about coding an outliner
for the project but seeing I'll only have 3 interactables I decided to create
an outline sprite in Photoshop making the hole process easier.

When starting to code the mouse interactions I wanted to first change the way
interactables work to be able to display the interaction buttons and display the
outline. For this I changed the `IInteractable` interface to support 2 more
methods `OutlineInteractable()` and `RemoveInteractableOutline()` both these
methods start a coroutine that can Fade In/Out the outline sprite. Having done
this I had to go back and restructure the code on the `PlayerInteraction` script
to be able to call for the outline Fade In/Out.

I really wanted to mimic the style of the game so I went with 2 different types
of interactions both do the same but in different ways. Both Clicking the
interactable or being close to it and pressing E will spawn the buttons of the
available interactions with that object, the only difference being where the
buttons are spawned, with a mouse click they spawn around the mouse with the E
key they spawn around the interactable. After choosing the type of interaction
the player is moved to the interact location of the object and interacts with it.

Being done with the movement it's finally time to start working on the actual
interactions, I wanted to work on the coins first so I could setup the store
after it and already have the player have coins to spend. So I added another
2D asset for the coin and started working on a script to control the coins UI,
I also decided to create a simple animation of the number of coins being added
or spent for some better visuals.

When starting to build the shop UI I ran into an issue that cause me to
reconsider the original way I had planed to set it up. The sprites had to much
empty space and where not centered, fixing this in Unity would break the
character, so I decided to clump some sprites (Example: The glove sprite is made
of 4 different sprites) and create a new display sprite for each piece. After
this I created a new Clothes Scriptable Object this SO would allow me to have
at hand the display sprite, all the separate sprites that would be added to the
player, the price for selling and buying and the type of clothing it was to make
it easier later when we need to apply the clothes to the player.

Since we need to be able to Buy and Sell clothes I decided to go with the
Strategy Pattern for the player's wardrobe and the shop, cause they'll function
in very similar ways. There's a List of Available clothes and List of the
clothes inside the basket (Can be for purchase or sale). Both these lists are
manipulated in the same way so it made sense to do it this way considering the
time I have left.

I took a similar approach in the Wardrobe UI, the code is very similar to what
was done in the Buy/Sell UI, this is because the functionally is almost the same
with the exception of equipping clothing items.

## High Level Checklist

- [X] Player character capable of walking and interacting with the game world.
- [X] Functional clothes shop within a top-down view game like Stardew Valley.
- [X] Required features: shopkeeper interaction, buying/selling items, item
icons, item prices, and the ability to equip purchased outfits, which should be
visible on the character.
- [X] Design a suitable UI for this prototype.

## Low Level Checklist

`Might change throughout the development`

- [X] Create the readme (In constant update).
- [X] Create the project.
- [X] Create the player movement.
- [X] Create a basic interaction system.
- [X] Add player sprites and animations.
- [X] Create a controller to handle the player animations.
- [X] Add other 2D assets to complement the game.
- [X] Implement the coin system so the player can make purchases.
- [X] Create the store using UI.
- [X] Allow the player to purchase clothes.
- [X] Allow the player to sell clothes.
- [X] Allow the player to change clothes.
- [X] Create a simple start menu for the game.

<!-- Reference Links -->
[0]: https://github.com/h8man/NavMeshPlus
[1]: https://assetstore.unity.com/packages/2d/characters/mighty-heroes-rogue-2d-fantasy-characters-pack-85770
[2]: https://assetstore.unity.com/packages/2d/environments/2d-fantasy-forest-tileset-19553
