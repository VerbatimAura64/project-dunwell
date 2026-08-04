VAR foundLockedDoor = false
VAR foundWipedDesk = false
VAR foundBody = false
VAR foundMonitor = false
VAR foundGun = false
VAR foundStorageTerminal = false
VAR foundObsRoom = false
VAR foundMorrowTrace = false
VAR foundFiles = false
VAR foundSignature = false
VAR foundFineDoc = false
VAR convManagerDone = false
VAR managerAlerted = false
VAR managerKnowsMonitor = false
VAR datapadsChoiceMade = 0 //0 = left, 1 = taken, 2 = destroyed
VAR morrowSuspicion = 0
VAR managerCaught = false
VAR foundWallTerminal = false
VAR foundOneWayGlass = false
VAR foundScreenLast = false
VAR foundNeighborDatapad = false
VAR foundDexterDatapad = false
VAR good_count = 0
VAR seenIntMonologue = false
VAR seenClueRoom = false
VAR seenObsRoom = false
VAR seenExt = false
VAR gameOver = false
VAR attemptToBluff = false
VAR bluffed = false




->extMonologue

===extMonologue===
#SCENE_EXTERIOR

The message came in at 1:47 a.m. I read it at 5:12.
Three hours and twenty-five minutes. In this city, that's not a window. That's a eulogy.
His name was in the header. Dexter Clear. I didn't recognize it. I almost deleted it — the kind of encrypted routing he used, you see it from cranks mostly, people who think the city is listening. At least he was smart enough about that.
He said he found something in the walls, that he needed someone who knew how the old network was built. 
What more was in the walls besides rats and mold? He didn’t say how he heard my name, maybe he didn’t want to risk it. But he did know I was the kind of person who kept a spare key to all the locked doors.
The evening curfew hasn't been lifted yet, I need to get inside.
~seenExt = true
+ [Go inside] -> intMonologue

->intMonologue

===intMonologue===
#SCENE_INTERIOR_HALLWAY
~seenExt = true
{ not seenIntMonologue: 
These apartments were cheap, The Studios they called them, basically saving space in the building by building the hallways around the outside of the apartments instead of between them. Leaner buildings, smaller but equally sized apartments, and cheaper rents.
 It didn’t take too many persuasion prompts to convince the artificial reception to let me through and up to his floor. 
  ~ seenIntMonologue = true
 - else: Where should I go?
 }
+[Examine the notice on the door] ->clueFineNotice
+[Knock on Dexter's door] -> dextersDoor
+{dextersDoor } [There's the service closet.] ->findTerminal
+{foundStorageTerminal } [Go inside Dexter's Apartment] ->aptUnlocked

===wrongApt===
This isn't Dexter's Apartment...
{ not seenIntMonologue && not managerAlerted:
    ->intMonologue
}
->DONE

===notRelevant===
{~ Doesn't look important... | Sificity needs a facelift... | Maybe it's decorative... | Really? | Nothing here that helps me | Marcus looks. Moves on.}
-{not seenExt: ->extMonologue}
-{not seenIntMonologue: ->intMonologue}
-{not managerAlerted: ->intMonologue}
-{not seenClueRoom: ->aptUnlocked}
//-{seenClueRoom: ->clueHub}
 ->DONE

===dextersDoor===
I knocked on the door. The room sounds hollow, like no one could possibly be inside. He told me to meet him here. Maybe he fell asleep - I'm three hours late.
The door is locked. Override lock, not a standard residential fit. I can force the lock, or maybe there's a floor terminal around here somewhere.

*[Force the lock] ->bruteForce
+[Maybe there's a floor terminal around here somewhere.] -> intMonologue
+{foundStorageTerminal } [It's unlocked now.] ->aptUnlocked

=== bruteForce ===
# DOOR_FORCED
~ managerAlerted = true
Four override attempts and some patience. The lock gives.
-> aptUnlocked

===findTerminal ===
~ foundStorageTerminal = true
The terminal was in the storage closet. Go figure. This terminal is older than the building deserves though. Repurposed city-issue, the kind that felt familiar six, seven years ago. Before droids took over the jobs and contracts.
Before I left.
~ managerAlerted = false
-> storageTerminal

===storageTerminal===
City-issue hardware running apartment management software it was never designed for. Whoever set this up knew what they were doing — or knew someone who did.
I found Dexter's unit in the directory. Ran the unlock sequence.
There's something else in here. A ghost signal, low bandwidth, encrypted. I can't access it from here.
->DONE


===aptUnlocked===
The door swings open. It's dim in here, difficult to see — save for the blinding glow of the wall screen at the far end.
I can make him out. Sitting in a chair, head down like he fell asleep watching something. But there's something on his head.
A bag. And around his feet — a pool of blood.
I've found my way into a crime scene. The droids will pop in eventually. When they do, they'll harden the logic around whatever they find — and right now everything they'll find points to me.
If only I had answered him sooner.
~seenClueRoom = true
-> clueHub

===clueHub===
{not managerAlerted && not seenClueRoom: 
The door swings open. It's dim in here, difficult to see — save for the blinding glow of the wall screen at the far end.
I can make him out. Sitting in a chair, head down like he fell asleep watching something. But there's something on his head.
A bag. And around his feet — a pool of blood.
I've found my way into a crime scene. The droids will pop in eventually. When they do, they'll harden the logic around whatever they find — and right now everything they'll find points to me.
If only I had answered him sooner.
~seenClueRoom = true
}
What can I find here, I don't have a lot of time. 
{ foundLockedDoor && foundWipedDesk && foundBody && foundGun && foundMonitor: 
    The picture is becoming clearer. -> beatTwo
- else:
    +[The door] ->clueLockedDoor
    +[The desk] ->clueWipedDesk
    +[The wall screen] ->clueMonitor
    +[There's a gun] ->clueGun
    +[Who is that] ->clueBody
}


===connectionBodyGun===
If the gun is on the table, then it couldn't have been suicide.
->DONE

===connectionNeighborFine===
This guy better watch his back, or he might wind up like Dexter. There’s no way to warn him though, or I would.
->DONE

===connectionScreenFine===
Maybe Dexter told his neighbor about feeling like he was being watched, but something must've got Dexter to take down the tapestry. Unless someone else took it down for him.
->DONE

===connectionDeskGun===
Seems like Dexter put up a fight before he lost to the best of his opponent and ended up, well, in the chair.
->DONE

===connectionDatapads===
There's a bunch of these just laying around and on this floor alone. I can't imagine the whole building is like this...
->DONE

===connectionDexDrive===
It's Dexter's drive, but it has my programs on it. I think the observer was trying to plant evidence against Dexter. Maybe as a deal gone wrong, or something.
->DONE

=== clueLockedDoor ===
# CLUE_FOUND_1
~ foundLockedDoor = true
~ good_count = good_count + 1
~ seenClueRoom = true
An overridden privacy lock can only come from the inside. 
{ foundStorageTerminal: The stack trace from the terminal didn't log any entry requests or breach attempts after the lock was set.} 
So how did our killer get in? How did they get out?

-> clueHub

=== clueWipedDesk ===
# CLUE_FOUND_2
~ foundWipedDesk = true
~ good_count = good_count + 1
~ seenClueRoom = true
The rest of the place is tossed, or maybe this is how he lived. Boxes of junk, trash piling up in the corner. But his desk? Clean and empty, but the trash around it doesn't look like it's been there long. Tools on the wrong side, leftover food thrown over where it didn't belong.
This place had been wrecked.
-> clueHub

=== clueBody ===
# CLUE_FOUND_3
~ foundBody = true
~ good_count = good_count + 1
~ seenClueRoom = true
It looks like Dexter, but I can't be sure. Not with the bag on his head and the blood splatter dripping into his shirt and onto the floor. I never met the guy, but something tells me there wouldn't be anyone else here except him anyhow.
One shot, straight between the eyes.

-> clueHub


=== clueGun ===
# CLUE_FOUND_4
~ foundGun = true
~ good_count = good_count + 1
~ seenClueRoom = true
I shouldn't be touching the gun, but I need to know if this was what left Dexter in the chair for good. There's a round missing from its chamber.
But why leave it to be found? Why drop it here from where they shot Dexter?

-> clueHub


=== clueMonitor ===
# CLUE_FOUND_5
{ good_count == 4:  
    ~foundScreenLast = true 
}
~ foundMonitor = true
~ good_count = good_count + 1
~ seenClueRoom = true
There aren’t any windows in these apartments by design, but these wallscreens weren't any better as a source of artificial sunshine. But why is it still on, glowing with life if the rest of the room has died.
There's something more off though, the emitted light is further behind the glass instead of on it. I'm beginning to feel more disconnected than the panel from where it ought to be emitting from.
-> clueHub

===clueFineNotice===
#CLUE_FOUND_6A
~ foundFineDoc = true 
There's a notice pinned to the door. A fine - a tapestry hung over a wall screen, flagged as a possible fire hazard. The letterhead is corporate, not municipal. Someone with money sent this. Not important.
{aptUnlocked or seenClueRoom or managerAlerted: ->clueHub}
 ->intMonologue

=== clueNeighborDatapad ===
# CLUE_FOUND_6B
~ foundNeighborDatapad = true
~ good_count = good_count + 1
According to the observer's notes, they were suspicious of this tenant's behavior and awareness — he'd put up a tapestry. He's blocking the view.
I don't think he knows. Rebellion without awareness.
There's a request written at the end. Have the tenant placed under further investigation. Followed.
{ foundFineDoc: The letterhead on this datapad matches the fine notice on the door. Same corporation. Same enforcement. }

-> obsRoom

=== clueDexterDatapad ===
# CLUE_FOUND_7
~ foundDexterDatapad = true
~ good_count = good_count + 1
Here's Dexter's file. His occupation was listed as a disgruntled droid engineer and repairman, but I don't think the feeling of disgruntle is an occupation.
Initial label — innocuous resident. No known connections to resistance groups or verified threat assessment.
But there's a postmark. An addendum at the bottom, profiling his callstack to rebellious anarchists. A whole list of names at the end of the file.
The one that stuck out the most was mine.

-> obsRoom

=== clueBackupDrive ===
# CLUE_FOUND_8
~ foundFiles = true
~ foundSignature = true
~ good_count = good_count + 1
There's a terminal drive on the center command table and my interface says it needs no authentication. Who doesn't lock their data before stepping away, unless they had to leave in a hurry.
But there's something more pressing. The directory list has a set of applications only I could recognize — applications I know I personally named. Not some corpo slave making a quick buck.
Phelps. Chief. Morgan. Names from the History Archive Museum that seemed inspiring at the time.
My architecture. My work. In the wild and in violation of everything I imagined its use for.

-> obsRoom

=== morrowRevealed ===
# CLUE_FOUND_9
# ALARM_TRIGGERED
~ good_count = good_count + 1
Last authentication — 5:45 a.m. this morning.
The name attached to it: Dante Morrow.
I know that name. I worked alongside him on the original infrastructure contracts. He knew this architecture because he helped build it. He knew where the skeleton keys were because I showed him.
Morgan flags the trace. The alarm is live. I need to move.
-> datapadsChoice

 
 === beatTwo ===
 { foundScreenLast:
Something about that wallscreen isn't sitting right with me. Tracing the edge trying to bring the lights to the front glass, there's a click and a reader reveals itself from out behind the wall.
The something in the walls.
}
{ foundStorageTerminal: That ghost signal from the storage terminal. It was coming from here. }
Could this have been what Dexter was thinking? How did he find it himself, what was he looking for, what did he find? Could he have left something behind that whoever killed him was looking for? Where would he hide that kind of thing.
The scanner needs a different kind of authentication, a handshake that seems too familiar though.
~ foundWallTerminal = true
~ good_count = good_count + 1
+ [Use the skeleton key] -> obsRoom
->DONE


=== obsRoom ===
{not seenObsRoom:
    One way glass. Four apartments visible from here, Dexter's included. There's an elevator too — an interior service one. This is how he got in. This is how he got out.
~seenObsRoom = true
- else: 
    What are they doing here...
}

{ clueNeighborDatapad && clueDexterDatapad && clueBackupDrive:
    -> portTrace
-   else:
    + [The neighbor's datapad] -> clueNeighborDatapad
    + [Dexter's datapad] -> clueDexterDatapad
    + [The terminal] -> clueBackupDrive
}


=== portTrace ===
# PORT_TRACE_ACTIVE
I need to know more about who last logged in. Does each tenant get their own agent filling out these notes, or is this one operation running across the whole floor?
I can run a port trace using Phelps. Pull the last handshake, the last authentication. Find out who these people are, why they have my code, and what they're using it for.
The irony isn't lost on me — using my own work to chase down whoever stole it.
+ [Run the trace] -> morrowRevealed


=== datapadsChoice ===
# DATAPAD_CHOICE
All this data. The invasion of privacy makes me realize how much of a mistake this whole system was. The money moves fast in this city, changing things from bad to worse. 
No one needs all this personal information on any of these people. I need to figure out what to do with this.
~ foundMorrowTrace = true
* [Take the datapads] -> datapadsSceneTake
* [Destroy them] -> datapadsSceneDestroy
* [Leave them] -> datapadsSceneLeave



=== datapadsSceneTake ===
I could use these to build a case against Dante, against this corporation. But my name is already on one of them — I might just implicate myself too. 
Is this going to be worth it if I get caught with them? They'll know I've been here and that'd be tampering with evidence as a possible suspect for an investigation.
But it'd clear my name if I can connect the dots.
~ datapadsChoiceMade = 1
-> closingMonologue

=== datapadsSceneDestroy ===
There's rage sparking in my arms. I just want to smash the glass, crush the pads and erase all the data, wipe everything clean. 
The tablet is in my hands, arms shaking, and I can feel the relief of what it would be like to see it break. To see them all break.
And it's incredibly fulfilling to do so.
~ datapadsChoiceMade = 2
-> closingMonologue

=== datapadsSceneLeave ===
I don't have the time to think about these datapads. I gotta get out of here. I'd love to smash them, but I can't leave any more fingerprints. If I could carry them or back them up, I would.
But there's no time.
~ datapadsChoiceMade = 0
-> closingMonologue


=== bldManagerConv ===
{ managerAlerted:
    The lock system must've flagged the override. He had the walk of a man who'd been called up for something and wasn't sure yet how serious it was.
- else:
    He was already in the hallway, doing his rounds, or something close enough to rounds that he'd call it that. He clocked me the way building managers clock everyone — not suspicion, just inventory.
}

{ managerCaught:
    "I've called the droids. They'll sentence you for this murder!" 
    ->endingB
- else:

"Help you with something?"

Not unfriendly. The tone of a man whose job is to know who belongs and who doesn't, and who has so far always been right.

* ["Just checking on a tenant. Dexter Clear, 4C."]

    "Mr. Clear." He said it like he was pulling a file. "We've had some correspondence with Mr. Clear recently. Lease matter."

    "What kind of lease matter?"

    "Wall fixture. Tapestry, specifically. Hung over one of the room screens — the decorative panels, you know the ones. It's in the lease. Nothing personal, just building standards."

    { foundMonitor:
        Building standards. I thought about the screen in Dexter's apartment. The depth that was wrong. The faint glow with the power cut.
        
        A tapestry over a screen. Dexter had felt it too — something on the other side of that glass he couldn't name. He covered it the only way he knew how and they fined him for it within the week.
        
        This man had no idea he was a instrument of something. That was the worst part. He was just doing his job.
    - else:
        A tapestry. I filed it away. Probably nothing. Probably the kind of administrative friction that accumulates around anyone who lives somewhere long enough.
    }

    "He comply?"

    "Eventually. These things usually resolve themselves." He straightened slightly, the posture of a man who considers resolution his professional contribution. "Privacy was their whole selling point for these apartments. Residents take it seriously. Sometimes too seriously, if you follow me."

    { foundMonitor:
        I followed him. More than he knew.
    - else:
        I told him I followed him.
    }

    ~ convManagerDone = true

    -> conveManagerDone

* ["Wrong floor. Sorry."]
~attemptToBluff = true
{bluffed:
    He looked at me the way people look at someone who has said something slightly too convenient.

    "Elevator's back that way."

    "I know. Thanks."

   I could feel the inventory running — does he belong, does he not — I didn't want to stick around to find out.

    ~ convManagerDone = true
    -> conveManagerDone
- else:
    "You're trespassing. You need to leave."
    ->endingB
    }
}

=== conveManagerDone ===
#CONVO_DONE
-{seenClueRoom: ->clueHub}
-{not seenIntMonologue: ->intMonologue}
-> DONE

=== closingMonologue ===
Dante was here. He might've just left the building as I walked in, and he left the door wide open for me to be caught in it.
I have his credential now. And an address to the central hub he's pinged authentications from.
My interface saves the drive scan, and I can't help but think back to the room. Seeing Dexter still sitting there. I'm sorry I couldn't have been here sooner for him.
I never intended for my work to be used like this. Targeting innocent civilians for the wrong reasons. Nobody is safe if everyone is distrustful.
There's a long list of things I need to do to nail Dante to this elaborate scheme. 

-> endingCheck

=== endingCheck ===
{ managerCaught:
    -> endingB
- else:
    { good_count >= 9 && datapadsChoiceMade > 0:
        -> endingA
    - else:
        { datapadsChoiceMade == 0:
            -> endingC
        - else:
            -> endingB
        }
    }
}

=== endingA ===
# ENDING_A
The case closed the way cases rarely do in Sificity — with the right person named.

Dante Morrow is in the wind. He was before I walked through that door, and he will be long after whatever passes for justice here finishes deliberating. 

The city doesn't move fast enough to catch men like him. It barely moves fast enough to catch men like me.

But the record is clean. My name is off it.

Dexter Clear died trying to hand someone a truth they weren't ready for. He handed it to me instead. Three hours and twenty-five minutes too late to save him. Just in time to do something with it.

I don't know if that counts as winning.

The city keeps raining. I keep looking.
~gameOver = true
-> END

=== endingB ===
#ENDING_B
{seenObsRoom:
Dante Morrow is outside these walls. Dexter Clear is in the ground. The observation network is still running in every Studio apartment in Sificity, behind every screen, through every sightline nobody thought to cover.
}
{ managerCaught:
The cell is smaller than the apartments in The Studios. Not by much.

I keep doing what I always do — noticing things. The crack in the upper left corner of the ceiling suggests the building settled unevenly. The guard who favors his right leg on the evening shift. 

Observation without application. The instinct running on nothing, like an engine with nowhere to go.
I had the thread. I just ran out of room to pull it.
- else:
    I could've had everything. The city just didn't want to hear it.
}
I don't know what I'm doing here.

The city keeps raining. I stop looking.
~gameOver = true
-> END

=== endingC ===
# ENDING_C
My instinct is to take everything, build the case, hand it to someone with a badge and watch the machinery move. That's what I came here to do. That's what Dexter's message asked me to do, even if he didn't know he was asking.

But my name is in that network. My directories. My architecture. Chief, Phelps, Morgan — running surveillance on civilians in their own homes, and it doesn't matter that I didn't know. 

It doesn't matter that Dante took it without asking. The code is mine. The sightlines are mine. Any case built on that drive has me in it, and a case with me in it is a case Dante Morrow can dismantle in an afternoon.

So, I'll leave it for someone else.

Dexter Clear spent the last months of his life trying to assemble something — evidence, a droid, a contingency. He didn't trust institutions. He trusted systems he built himself. 

I understand that now in a way I didn't when I walked through that door.

Will Sificity know what to do with something as large as this? Would there even be justice.

I don't know. But I think I know who to ask.

The city keeps raining. Someone else keeps looking.
~gameOver = true
-> END

