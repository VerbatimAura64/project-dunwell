VAR foundLockedDoor = false
VAR foundWipedDesk = false
VAR foundBody = false
VAR foundMonitor = false
VAR foundGun = false
VAR foundTerminal = false
VAR foundObsRoom = false
VAR foundMorrowTrace = false
VAR foundFiles = false
VAR foundSignature = false
VAR foundFineDoc = false
VAR convManagerDone = false
VAR managerAlerted = false
VAR managerKnowsMonitor = false
VAR datapadsChoice = false
VAR morrowSuspicion = 0
VAR managerCaught = false


VAR bad_count = 0
VAR good_count = 0



->extMonologue

===extMonologue===

# SCENE_EXTERIOR
The message came in at 2:47 a.m. I read it at 6:12.
Three hours and twenty-five minutes. In this city, that's not a window. That's a eulogy.
His name was in the header. Dexter Clear. I didn't recognize it. I almost deleted it — the kind of encrypted routing he used, you see it from cranks mostly, people who think the city is listening. At least he was smart enough about that.
He said he found something in the walls, that he needed someone who knew how the old network was built. What more was in the walls besides rats and mold? He didn’t say how he heard my name, maybe he didn’t want to risk it. But he did know I was the kind of person who kept a spare key to all the locked doors.
The rain is coming down harder now, and I need to get inside.

+ [Go inside] -> intMonologue

->intMonologue

===intMonologue===

#SCENE_INTERIOR_HALLWAY
These apartments were cheap, The Studios they called them, basically saving space in the building by building the hallways around the outside of the apartments instead of between them. Leaner buildings, smaller but equally sized apartments, and cheaper rents.
 It didn’t take too many override prompts to convince the artificial reception to let me through and up to his floor. 

+[Examine the notice on the door] ->clueFineNotice
+[Knock on Dexter's door] -> dextersDoor
+{dextersDoor } [There's the service closet.] ->findTerminal


===dextersDoor===
I knocked on the door. The room sounds hollow, like it couldn't possibly have anyone inside. He told me to meet here. Maybe he fell asleep - I'm three hours late.
The door is locked. Override lock, not a standard residential fit.

*[Force the lock] ->bruteForce
+[Maybe there's a floor terminal around here somewhere.] -> intMonologue

=== bruteForce ===
# DOOR_FORCED
Four override prompts and some patience. The lock gives.
~ managerAlerted = true
-> aptUnlocked

=== findTerminal ===
The terminal was in the storage closet. Go figure. This terminal is older than the building deserves though. Repurposed city-issue, the kind that felt familiar six, seven years ago. Before droids took over the jobs and contracts.
Before I left.

~ managerAlerted = false
-> storageTerminal

=== storageTerminal===
# TERMINAL_ACTIVE
City-issue hardware running apartment management software it was never designed for. Whoever set this up knew what they were doing — or knew someone who did.
I found Dexter's unit in the directory. Ran the unlock sequence.
There's something else in here. A ghost signal, low bandwidth, encrypted. I can't read it from here.
~ foundTerminal = true
->intMonologue

===clueFineNotice===
#CLUE_FOUND_9

There's a notice pinned to the door. A fine - a tapestry hung over a wall screen, flagged as a possible fire hazard. The letterhead is corporate, not municipal. Someone with money sent this.
Not important.
~foundFineDoc = true
->intMonologue


===aptUnlocked===
# SCENE_INTERIOR_APARTMENT
The door unlocks and slides into the wall. Dim in here, difficult to see — save for the blinding glow of the wall screen at the far end.
I can make him out. Sitting in a chair, head down like he fell asleep watching something. But there's something on his head.
A bag. And around his feet — a pool of blood.
I've just forced my way into a crime scene. The droids are going to pop in eventually. When they do, they'll harden the logic around whatever they find — and right now everything they find points at me.
If only I had answered him sooner.
+ [Look around] -> clueHub


===clueHub===

->DONE



===clueGun===
There's a round missing from the gun on the floor.
~ foundGun = true
->clueHub




===bldManagerConv===

->intMonologue
->storageTerminal









===main2===
We start with the couple walking into the forest.
//{  good_count < 4  }
    +[good] 
    ~ good_count++  
    -> good_choice 

//{ bad_count < 4 }
+[bad]
    ~bad_count++
->bad_choice

=== good_choice ===
{ good_count:
-1: ->no_good.
-2: ->some_good
-3: ->lots_of_good
  
}

= lots_of_good
Your date seems genuinely happy.
-> END

= some_good
Things feel okay between you two.
-> main2

= no_good
There's an awkward silence.
-> main2


===bad_choice===

{ bad_count:
-1: ->bad
-2: ->worse
-3: ->its_over
  
}

= its_over
The date is over, take her home
-> END


= worse
Things are getting worse
-> main2

= bad
You shouldn't have said that.
-> main2
->END

=== clueInspection===
{ clueRelevant:
 -true: This looks important.
 -false: I don't see the connection.
 }
 ->DONE

-> apartment

===apartment===
# SIFICITY, 2842 - Landon's Apartment
Marcus wakes up on the floor, a throbbing pain in his head and an ache in the rest of his body. His vision is blurred, but the picture starts to clear.

 * ["Where am I?"]-> look
 
 ===look===
    The room was a mess. Or maybe it always looked like this.
        *[Check hands]
            ->hands_clue
        *[Get up]
            ->getup
        
    
    
 ===hands_clue===
    Clean. That meant something, but he wasn't sure what yet. his hand still felt warm, and his shoulder had an ache in it, which means he must've shot someone or something.
   
    {not getup: 
        "I should get up" 
        ->getup
    }
    ->search
    
    
===getup===
    The pain is subsiding as he stands, but it doesn't get any easier. 
    ->search
 
 
===search===
       
He takes a second to look around.
    ->hands_clue
*[He noticed the gun]
    ->gun_clue
*[He noticed the body]
    ->landon_clue
+[Leave] 
    ->door

    
    
    
===landon_clue===
It's Landon and he is tied to a chair, his head is bagged and blood drips from it onto his shirt, but I know its him, he was the kind of guy I could pick clear out of a crowd.

->search

    

=== gun_clue ===
There's a round missing from the gun on the floor. He doesn't need to imagine where it might've gone. But how did it get there, and why.

    ->search

    
===door===

        Marcus thinks he shot Landon with his gun.
        ->DONE
    -else:
        Marcus feels like he missed something.
        ->search
        
}        ->DONE

->END




