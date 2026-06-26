# Project Dunwell

A neo-noir sci-fi locked-room murder mystery. One suspect. Nine clues. Three endings.

Built in Unity + Ink as the centrepiece of a junior game-dev portfolio targeting 
gameplay programming, tools, and narrative design roles.

**[Play on itch.io](https://harborviewgames.itch.io/project-dunwell)** · 
**[Case Study](https://verbatimaura64.github.io/project-dunwell.html)**

---

## The Setup

Marcus Dunwell is a former infrastructure engineer. He's not a detective. 
He's the prime suspect.

He's three hours and twenty-five minutes late to a dying man's call.
In Sificity — a future city where the towers block out the sun and 
the streets belong to aliens, robots, and people who couldn't afford 
to leave — that's not a window. That's a eulogy.

The victim, Dexter Clear, discovered something behind the apartment's wall 
screens. The man who put it there is someone Marcus used to know.

---

## How to Play

- Explore the apartment and interact with objects to discover clues
- Open the case board and connect related clues — Marcus will react to 
  each connection
- Pay attention to what you find, what you connect, and what you choose 
  to do with it
- ~10–20 minutes to complete

Three endings. What you find determines what you can prove. 
What you choose determines who you become.

---

## What Was Built

**InkBridge Architecture**
Unity and Ink stay fully decoupled. Unity owns the world — collision, 
player state, clue tracking. Ink owns all narrative output. InkBridge.cs 
sits between them: Unity calls knots by name, Ink returns text and tags, 
InkBridge fires Unity events in response. Neither system reaches into 
the other directly.

**Case Board**
Clue cards distributed along a spline. Connections drawn as real-time UI 
Image lines tracked via InverseTransformPoint. Each connection is backed 
by a dictionary keyed on a sorted clue-pair string, mapping to a unique 
Ink knot — so order of discovery never matters, only what you bring together.

**Two-Camera Inspection System**
Third-person exploration switches to a first-person inspection camera on 
interaction. Objects rotate in world space; original transforms are saved 
on entry and restored exactly on exit.

**Tag-Driven Events**
Story events (scene transitions, UI updates, manager encounters, story 
locks) are driven by Ink tags read by InkBridge — keeping narrative 
logic in Ink and game logic in Unity.

**TransitionManager**
Singleton with DontDestroyOnLoad. One smoothstep Fade coroutine handles 
the opening fade, the in-scene Ending B jail swap, scene loads, and the 
final fade-to-menu.

---

## Built With

- Unity (C#)
- [Ink](https://www.inklestudios.com/ink/) — narrative scripting
- [Ink Unity Integration](https://github.com/inkle/ink-unity-integration)

---

## What's Next

- WebGL build (currently hangs on load — compression and texture pass planned)
- Sound design and ambient audio
- Bluff mechanic refactor: replace inverted bool with explicit 
  `bluffAttempted` / `bluffFailed` two-flag system
- Resolution-independent case board spline
- Possible expansion: Marcus at the Downtown warehouse — Dante's servers

---

## Case Study

Full breakdown of the architecture, challenges, and retrospective:
[verbatimaura64.github.io/project-dunwell.html](https://verbatimaura64.github.io/project-dunwell.html)