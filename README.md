Progetto: "MazeQuest: Labirinto Interattivo"
Descrizione:
"MazeQuest" è un gioco basato su browser dove i giocatori devono navigare attraverso labirinti usando i controlli di movimento direzionali. L'obiettivo è raggiungere l'uscita del labirinto nel minor tempo possibile, evitando trappole e raccogliendo oggetti speciali.

Funzionalità Principali:
Generazione e Visualizzazione del Labirinto:

Labirinti Dinamici: Usa algoritmi di generazione dei labirinti come il Depth-First Search o l'Algoritmo di Prim per creare labirinti casuali di diverse dimensioni e difficoltà.
Rendering del Labirinto: Utilizza l'elemento <canvas> di HTML5 per disegnare il labirinto. Ogni cella del labirinto può essere rappresentata da un quadrato con colori o pattern diversi per muri, percorsi e oggetti speciali.
Controllo del Personaggio:

Movimento: Implementa il movimento del personaggio usando i tasti direzionali (freccia su, giù, destra, sinistra) tramite JavaScript. Assicurati che il personaggio si muova di una cella alla volta e non attraversi i muri.
Animazioni: Aggiungi animazioni per il movimento del personaggio per rendere il gioco più dinamico.
Timer e Record:

Cronometro: Mostra un cronometro che conta il tempo impiegato per completare il labirinto.
Record e Classifiche: Salva i tempi di completamento e visualizza una classifica per i migliori tempi globali o per ogni utente.
Trappole e Bonus:

Trappole: Aggiungi trappole che possono rallentare il progresso del giocatore o causare danni.
Bonus: Inserisci oggetti bonus che il giocatore può raccogliere per ottenere vantaggi come velocità aumentata o visibilità temporanea del percorso.
Pannello di Controllo e Feedback:

Interfaccia di Gioco: Fornisci pulsanti per avviare e riavviare il gioco, un pannello per visualizzare il punteggio e il tempo rimanente.
Feedback Visivo e Testuale: Mostra messaggi di successo o di errore, come quando il giocatore raggiunge l'uscita o colpisce una trappola.
Modalità Multiplayer (Opzionale):

Competizione in Tempo Reale: Permetti ai giocatori di sfidarsi per completare lo stesso labirinto nel minor tempo possibile.
Leaderboard: Visualizza una classifica con i tempi di completamento dei giocatori in modalità multiplayer.
