# TwitchColonyStats

A simple RimWorld mod to communicate colony stats on Twitch

# Special Thanks

A very special thank to hodldeeznuts (https://github.com/hodldeeznuts)
Without you this wouldnt be possible!

# Default commands

There are vary basic commands in this iteration, all of them NEED to start with '!':
- animals -> Will bring a list of all possible animals registered
- animals <animalDefName> -> Will bring all tamed animals in the colony
- animal <animalName> -> Will bring all details for the animal named <animalName>
- colonists -> Will bring a list of all player colonists
- colonist <colonistName> -> Will bring the details for the colonist named <colonistName>
- help -> Quite easy to understand what it does right?

# Settings Explanation

The settings for the mod are quite understandable. To have the settings take effect, close the settings menu and open it again.

# Instructions to add new Commands

This mod works by using an abstract class called ITwitchTranslator. 
If you need to add a new command, simply create a new css class, extend ITwitchTranslator and designate the methods.
The ITwitchTranslator constructor parameter will take care of adding your new class to the registered commands for the !help command.
The ITwitchTranslator constructor parameter is strictly the name of the new command to be added WITHOUT the '!' symbol.
Finally, the custom class created needs to be added to the registered commands in order for it to work.
To do this you must use the class called TranslatorRegistrator, which has this utility methods:
- AddTranslator(translator) -> Adds a new command translator to the party
- RemoveAllTypeTranslator(translator) -> Removes all translators that match the class type of the translator received
- RemoveTranslator(translator) -> Removes a translator from the list of translators
IMPORTANT NOTE: Only 1 translator will match a single command, therefore commands need to be unique.

# Instructions to add modded animals

This mod works by using a list of possible animals to use as commands.
Unfortunately I couldnt find a good way to extract only tamable animals from the KindDef database.
An upside to this, is that this allows custom command names for each animal, even if that means adding them manually to the party.
And so, here we go with the explanation:
There is a class called AnimalRegistrator, this class has various utility methods:
- RegisteNewDef(commandName, defName) -> This method will register the string commandName as a command for the KindDef defName. Ie: calling RegisterNewDef("donkey", "Donkey"); will register the donkey keyword for the Donkey def
- UnregisterDef(commandName) -> As the name indicates, this method will unregister an animal from the definition list
- IsRegistered(commandName) -> This method will check if a commandName has already been registered

# TL;DR - Final notes

To add new commands use ITwitchTranslator abstract class and override its methods. Use the TranslatorRegistrator methods to add the class to the registered command translators.
To add modded animals use the class AnimalRegistrator by calling its methods