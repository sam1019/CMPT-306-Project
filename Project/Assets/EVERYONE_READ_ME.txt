Please DO NOT copy the code from the tutorial, it was designed for 3D and with only one single player type (UserPlayer). In our project we have 6 (Solider,Medic,Specialist,Tank,Jet, and Helicopter).
Similarly it was only designed with one AI type, whereas we have 4 (AlienSoldier, AlienShip, AlienSupport, and Berserker).


It was asked several times how to access script from a GameObject. I went over this during the meeting but I will repeat it again.
To access a script of a GameObject: 
ex I want to access Tile script but I only have access to List<List<GameObject>> map; However I can do the following

Tile temp = map[i index][j index].getComponent<SCRIPT NAME, In this case it is Tile>(); Then you will be able to make functions calls within the Tile script by doing the following
temp.functionNameToCall(PARAMETERS);
or if you want to access the fields (variables).
temp.fieldIWantToAcess; 
or set the field (variables)
temp.fieldIWantToAcess = new value;