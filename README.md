# CrystalGuard

A tower defense meets auto battler! Made as part of my time at TwinRayj Studios.  

All code is my own unless marked otherwise. 
Nearly all art and audio assets are purchased/free use, with the exception of the few I created.

## Milestones

Design/Concept created September 12th

Prototype completed on September 23th

Gameplay Overhaul started on October 6th

First alpha test ran from November 15th 2023 - December 1st 2023.

Internal Beta test running currently!

## Thoughts on the project

### Singletons and my code

Prior to this, my view on singletons was simply; "I've heard that Singletons are bad, so I'll never use them". 
For this project, a key decision was to purposely OVERUSE singletons to truly understand their strengths and weaknesses.

Having now gotten a better idea of what a singleton does to your code structure, I like the concept, but in practice it becomes very messy.
I kind of want to tinker with the idea of somehow making the singletons I use be "readonly" (public get private set kind of thing) and allowing "singletons" to be not so single. 
Something like there being a "main" instance, but you can inject a different instance with different data. Not entirely sure, but I want to play around with it.

### Refactor Thursdays

Every Thursday was supposed to be "Refactor Thrusday", where I clean up my messes from the previous week. I liked this a LOT but there were a number of instances where I ended up saying "this code is a self contained mess, good enough".
Towards the end of alpha development, I stopped doing Refactor Thursdays. I super regret this, but as a solo dev I think its more or less inconsequential due to most of the messy points being a small enough mess to conceptualize in my head without much stress. 

### Design

Theres a HUGE issue with the design of this game that I didn't figure out until it felt "too late". A big part of systems in autobattlers is that its PvP, not PvE. A bit part of the difficulty curve and how everything is balanced is over the fact that everyone is using the same tools. This meant that in creating waves/levels for this game, I had to make a choice. I had to choose how "good" of a build would the player have to have to clear a wave. I ended up making the waves easy, meaning that someone with a bad "build" can still experience the game and have fun, while someone using a very good build can just smash through the content and hopefully experience fun that way. 
