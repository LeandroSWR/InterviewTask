# Interview Task

## Thought Process

First set was to set up a git hub repository and create a new Unity project using
the required 2021.3.2f1 version. After which I wanted to create this readme file
to help gide me trough the creation process and make it easier.
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

## High Level Checklist

- [ ] Player character capable of walking and interacting with the game world.
- [ ] Functional clothes shop within a top-down view game like Stardew Valley.
- [ ] Required features: shopkeeper interaction, buying/selling items, item icons,
item prices, and the ability to equip purchased outfits, which should be
visible on the character.
- [ ] Design a suitable UI for this prototype.
- [ ] You may use pre-made art assets or create your own for this task.
- [ ] Please provide 300-word documentation explaining the system, your thought
process during the interview, and your personal assessment of your
performance. Attach this as a PDF file to Github.

## Low Level Checklist

`Might change throughout the development`

- [X] Create the readme (In constant update).
- [X] Create the project.
- [ ] Create the player movement.
- [ ] Create a basic interaction system.
- [ ] Add 2D assets to complement the game.
- [ ] Create the store using UI.
- [ ] Allow the player to purchase clothes.
- [ ] Allow the player to dress said clothes.
- [ ] Create a simple start menu for the game.
