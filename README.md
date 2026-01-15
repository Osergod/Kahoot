PROJECTE INCOMPLERT


Projecte II: Kahoot 
Abstract 
L’objectiu de la pràctica és posar a prova els coneixements d'accés a dades sobre fitxers, 
així com el de directoris i control d'excepcions mitjançant el desenvolupament individual 
d’una aplicació a l’estil Kahoot que llegeixi preguntes d’un arxiu JSON, les mostri 
seqüencialment i en temps limitat en una interfície rel·lativament treballada i finalment generi 
un arxiu de resultats emmagatzemats sobre un arxiu XML que en qualsevol moment pugui 
ser llegit per una part de l’aplicació que mostra les puntuacions més altes dels usuaris que 
hagin pres els diferents tests disponibles. Addicionalment, els usuaris han de poder generar 
dinàmicament nous Kahoots sense sortir de l’aplicació. 
Lògica de l’aplicació 
L’aplicació al ser estil Kahoot ha de respondre a la següent lògica: - 
Existeix un menú principal des del qual accedir a les diferents funcions de l’aplicació. - - - - - - - 
Existeix una escena a la qual el usuaris poden veure un llistat dinàmic de 
qüestionaris emmagatzemats en fitxers .json: - 
Alguns .json existeixen dins una carpeta d’assets del projecte (kahoots 
default)  - - 
L’aplicació té la capacitat de llegir arxius .json trobats a una carpeta concreta 
sota el directori corresponent a Application.persistentDataPath, de forma que 
si s’afegeixen a aquesta carpeta, l’aplicació serà capaç de generar un llistat 
afegint-les i donant la opció de seleccionar-les. 
Addicionalment, en aquesta escena l’usuari pot establir el seu nom d’usuari 
per guardar puntuacions. 
Un cop seleccionat el kahoot que l’usuari desitja realitzar dels disponibles, es canvia 
a l’escena de joc, on seqüencialment apareixen les preguntes i les respostes 
possibles i un indicador de temps restat. La lògica del joc deixarà clar un cop l’usuari 
respon (o el temps limitat termina) quina era la resposta correcta. 
Un cop totes les preguntes han passat el temps límit o han estat respostes, l’usuari 
veurà els seus resultats, i podrà veure també a quina posició es troben les seves 
respostes a un “LeaderBoard” on es podràn veure les millors puntuacions i temps 
d’usuaris anteriors. Tots els resultats dels Leaderboards seran emmagatzemats a 
diversos arxius .XML sota un directory a Application.persistentDatapath. Cada XML 
estarà relacionat amb un únic arxiu .JSON de forma que hi haurà un arxiu XML de 
resultats per cada qüestionari. 
Ha d'existir una forma de visualitzar els Leaderboards de tots els Kahoots 
disponibles sense necessitat de jugar una partida. 
Les excepcions per falta d’arxius, carpetes, així com de format d’arxius estan 
gestionades correctament i generen informes en format d’arxiu pla. 
Existeix una escena a la qual tots els informes recollits d'excepcions es poden 
visualitzar, i en seleccionar un, es pot llegir el contingut (a una nova escena o amb 
algún element de UI a la mateixa). 
Existeix una escena de creació de Kahoots. Què de forma directa (sense tancar 
aplicació) i dinàmica, són seleccionables des del selector de kahoot. 
