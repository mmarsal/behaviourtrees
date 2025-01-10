Projektname:
Behaviour Trees (Klassische AI): Gegnerische KI inspiriert von Alien:
Isolation

Namen der Teammitglieder:
Martin Marsal, Lau Kailany

Dieses Projekt ist inspiriert von Alien: Isolation, einem Horror-
Survival Spiel von 2014, das bis heute für die KI des Antagonisten, das
Alien, bekannt ist, welches auch durch Behaviour Trees realisiert wurde.
Hier ein kurzer Clip, welcher einen kleinen Einblick in das Spiel gibt:
https://youtu.be/YWD-3T0lCTs?si=cSB26YVD1HKEq5yL

Unser Projekt spielt sich auf einem kleinen Raumschiff ab, wo ein Alien
patrouilliert und alles tötet, was sich in den Weg stellt. Ziel ist es,
drei Minuten auf dem Raumschiff zu überleben.

Die Steuerung lautet wie folgt:
WASD - Bewegen
Shift - Sprinten
Space - Springen
Ctrl - Ducken
Linksklick - Flammenwerfer abfeuern

Mit dem Flammenwerfer kann der Spieler sich das Alien für eine kurze
Zeit vom Hals halten. Des Weiteren gibt es Tische, sogenannte Hidespots,
unter denen sich der Spieler verstecken kann, allerdings nur für eine
bestimmte Zeit, denn früher oder später fängt das Alien auch an dort
nachzuschauen. Außerdem gibt es zwei Portale, zwischen denen der Spieler
hin- und herteleportieren kann, was aber das Alien alarmiert.

Das Alien wurde durch das Behavior Designer Asset umgesetzt und verfügt
über eine Vielzahl von Fähigkeiten. Um diese einzusehen, muss man im
Unity-Editor in der Mitte das Behavior Designer Fenster öffnen und oben
"AIThirdPersonController" auswählen. Nun sieht man den Behavior Tree des
Aliens.
Der Behavior Tree funktioniert recht simpel: Zum Start des Spiels wird
von links nach rechts jedes Behavior überprüft und ausgeführt. Falls
keines der Behavior links ausgeführt wird, fällt es in das Fallback-
Behavior ganz rechts. Falls doch später etwas weiter links im Tree
getriggert wird, wird dieses ausgeführt, da Nodes weiter links im Tree
eine höhere Priorität haben. Alle Custom Tasks, die wir erstellt haben,
befinden sich im Tasks-Ordner. Die Hauptszene befindet sich in
"Assets/_Creepy_Cat/_3D Scifi Kit Starter Kit_HD/Level.unity".

Betrachten wir die einzelnen Behavior des Aliens von links nach rechts:
1. Fliehen: Falls das Alien vom Flammenwerfer getroffen wird, flieht es
zu einem von zwei Punkten auf der Map, um sich zu heilen. Dies hat die
höchste Priorität: Das heißt, dass während dieses Behavior ausgeführt
wird, kann kein anderes es unterbrechen.
2. Töten: Falls das Alien sich nah genug am Spieler befindet und er sich
nicht versteckt, töten es den Spieler und das Spiel ist vorbei.
3. Spieler gesichtet: Falls das Alien den Spieler sehen kann, wartet es
kurz und macht sich dann unsichtbar, um den Spieler anzugreifen. Bevor 
es dies tut, wird noch eine Sache überprüft, nämlich ob es vor dem
Angriff aus eine der Flankenpositionen angreifen soll, welche wir über
die Map verteilt gesetzt haben, um zum Beispiel dem Spieler den Weg
abschneiden zu können.
4. Portale: Auf der Map gibt es zwei Portale, zwischen welchen man sich
hin- und her teleportieren kann. Falls der Spieler durch ein solches
durchläuft, kriegt das Alien das mit und entscheidet dann, ob es
schlauer wäre auch durch das Portal zu laufen, oder direkt zum anderen
Portal zu laufen.
5. Geräusche: Falls der Spieler seinen Flammenwerfer abfeuert, kriegt
das Alien das mit und läuft zur Position, wo er abgefeuert wurde.
6. Letzte bekannte Position: Falls das Alien die letzte bekannte
Position des Spielers kennt, läuft es dort hin und greift bei
Sichtkontakt an.
7. Fallback: Obwohl es sich hier nur um das Fallback handelt, passiert
hier sehr viel. Zunächst wird durch den Random Selector zufällig eine
von drei Events ausgelöst: Die Sicht des Spielers wird für einige
Sekunden beeinträchtigt, ein Doppelgänger wird auf der Map gespawnt (der
Doppelgänger hat ebenfalls einen Behavior Tree, aber in sehr simpler
Form), welcher einen nicht töten, aber hindern kann oder das Alien
begibt sich in eine der Ambush-Positionen, die wir auf der Map verteilt
haben, wo es für eine Zeit wartet und angreift, falls der Spieler
vorbeiläuft. Nach dem zufällig ausgewählten Event schaut sich das Alien
kurz um und fängt an zu patrouillieren. Durch den Parallel Selector
werden während des Patrouillierens noch zwei andere Sachen ausgeführt.
Zum einen gibt es Tische (sogenannte Hidespots) auf der Map verteilt,
unter welchen sich der Spieler verstecken kann und das Alien ihn nicht
sehen kann. Wenn das Alien bei einem Tisch stehen bleibt, wird innerhalb
eines Radius überprüft, ob sich ein Spieler dort versteckt. Falls ja,
wird ein Counter hochgezählt, welches dem Alien erlaubt sich an das
Verhalten des Spielers anzupassen. Wenn dieser Counter zweimal
hochgezählt wurde, wird der Parallel Selector verlassen und durch den
Repeater weiter oben die Sequenz links mit der Int Comparison
ausgeführt. Diese Reihe von Nodes, die dann folgen, stellen das
Nachschauen unter den Tischen dar. Was während des Patrouillierens
ebenfalls ausgeführt wird, ist ein Node, welcher alle 30 Sekunden die
Position des Spielers an das Alien gibt. Dies soll eine Art AI-Director
implementieren und verhindern, dass der Spieler campen kann.

Es war ingesamt eine sehr interessante Erfahrung, dieses Projekt zu
implementieren. Dadurch konnte man einen Einblick erhalten, wie komplex
KI in Spielen wie Alien: Isolation eigentlich aufgebaut ist. Am meisten
Zeit hat wahrscheinlich das Umsetzen der Custom Nodes gedauert, da man
geschickt entscheiden muss, wann welcher TaskStatus zurückgegeben wird,
damit der nächste Node korrekt ausgeführt wird. Es war ebenfalls
herausfordern, dass alle Behavior sauber miteinander funktionieren und
die Übergänge zwischen denen auch gut funktionieren. Manchmal war die
geringe Menge an Dokumentation etwas mühsam, da selbst die offizielle an
manchen Stellen etwas knapp gehalten ist.

Verwendete externe Assets:
- Behavior Designer: Vom Dozenten übergeben
- Standard Assets: In ILIAS aus dem "Skripte + Assets"-Ordner entnommen
- SCIFI Map: https://assetstore.unity.com/packages/3d/environments/3d-scifi-kit-starter-kit-92152
- Unsichtbar Sound: https://www.youtube.com/watch?v=IlzdKMg5ZEA
- Sichtbar Sound: https://www.youtube.com/watch?v=SfrJ5QRAMOk
- Flammen Sound: https://www.youtube.com/watch?v=LQ4Xg7NsbyM
- Ungefähre Spielerposition Sound: https://www.youtube.com/watch?v=BmbM5B4NjxY

Troubleshooting:
Falls das Projekt nach dem Klonen/Entpacken Fehler wirft, einmal das
Projekt schließen und den Library-Ordner löschen. Danach das Projekt neu
starten und die Packages installieren lassen. Anschließend muss die
Hauptszene wieder geöffnet werden, welche sich im folgenden Pfad
befindet: Assets/_Creepy_Cat/_3D Scifi Kit Starter Kit_HD/Level.unity
