# Artikel-Import

Published on S: and D:
	Make sure to publish a Windows Application Output in "S:\Preispflege\Artikel Import\Files"
	and a Console Application Output in "D:\Artikel Import"
	
Beschreibung:
Inha verkauft Artikel von Lieferanten an Kunden. 
Diese Lieferanten schicken regelmäßig CSV  Dateien mit Preislisten an uns. 
Jeder Lieferant hat dort seine eigene Formatierung und Sortierung der Daten. 
Diese Preislisten müssen in die Echtzeitdatenbank eingespielt werden, damit alle Informationen in der Enventa richtig angezeigt und verwendet werden können. 
Artikel Import hat die Funktion den Import von diesen Preislisten für Mitarbeiter*innen zu vereinfachen, sodass keine Hilfe vom IT-Bereich mehr notwendig ist.

Lösung:
Ziel war es, den Anwendern viel Freiheit zu geben, ohne an Übersicht einbüßen zu müssen. 
Für jede Preisliste erstellen Nutzer*innen einmalig ein Mapping. 
Mappings enthalten Paare, die entscheiden welche Spalten der Preisliste in welche Spalte der Zwischendatenbank geladen wird. 
Welche Spalten die Zwischendatenbank enthält wird über die Felder entschieden. 
Wenn alle Preislisten importiert sind, kann die Zwischendatenbank in die Echtzeitdatenbank exportiert werden. 
Felder beschreiben welche Spalte der Zwischendatenbank in welche Spalte der Echtzeitdatenbank geladen wird.

Verwendung einer Zwischendatenbank
Die Verwendung einer Zwischendatenbank hat folgende Vorteile:
1.	Im Falle eines Fehlers ist nicht gleich die Echtzeitdatenbank in Gefahr.
2.	Die Echtzeitdatenbank wird in nur einem Schritt bearbeiten. Dies sorgt dafür, dass zu keinem Zeitpunkt Informationen fehlen oder fehlerhafte Informationen vorhanden sind. 
3.	Die Konvertierung von Werten ist sehr viel einfacher
4.	Es gibt Artikel, die nur Werte anderer Artikel überschreiben sollen, die auch neu importiert werden

Die Verwendung einer Zwischendatenbank hat folgende Nachteile:
1.	Alle Daten müssen doppelt verschoben werden.
2.	Die Zwischendatenbank verbraucht zusätzlichen Speicherplatz

Datenbank
1.	Die Artikelnummer muss einzigartig sein.
2.	Die EAN muss einzigartig sein.

CSV
1.	Die Preisliste gespeichert als Typ „Unicode“ und umbenannt als .csv
2.	Die erste Zeile muss die Spaltennamen enthalten.
3.	Getrennt mit Tabs[\t]
4.  Es duefern keine Zeilenumbruechen in den Zellen geben -> da Excel das durch [\t] das trennt.

Fehlerbehebung
Beim Auftreten von Fehlern, bitte zunächst die Einträge zum Zeitpunkt des Fehlers im Log [log.txt] lesen. 
Falls der Fehler oder Fragen nicht gelöst werden kann bitte eine E-Mail an it@inha.de schreiben.
