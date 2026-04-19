VAR body_found = false
VAR gun_found = false
VAR hands_checked = false
# SIFICITY, 2842 - Landon's Apartment
VAR bad_count = 0
VAR good_count = 0
VAR clueRelevant = false

#EXT Forest - First date
->main
===main===
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
-> main

= no_good
There's an awkward silence.
-> main


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
-> main

= bad
You shouldn't have said that.
-> main
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
    ~hands_checked = true
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
*{not hands_checked} [Check hands]
    ->hands_clue
*[He noticed the gun]
    ->gun_clue
*[He noticed the body]
    ->landon_clue
+[Leave] 
    ->door

    
    
    
===landon_clue===
It's Landon and he is tied to a chair, his head is bagged and blood drips from it onto his shirt, but I know its him, he was the kind of guy I could pick clear out of a crowd.
~body_found = true
    ->search

    

=== gun_clue ===
There's a round missing from the gun on the floor. He doesn't need to imagine where it might've gone. But how did it get there, and why.
~gun_found = true
    ->search

    
===door===
{ hands_checked and body_found and gun_found:
        Marcus thinks he shot Landon with his gun.
        ->DONE
    -else:
        Marcus feels like he missed something.
        ->search
        
}        ->DONE

->END




