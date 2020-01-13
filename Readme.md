## Bioinformatika projekt
>Programsko rješenje za pronalaženje varijanti gena iz podataka dobivenih sekvenciranjem
## Upute za instalaciju:
>Aplikacija je napravljena na OS-u Ubuntu verzije 18.04. Ukoliko koristite OS iste verzije, moguće je preskočiti korake 2. i 4. te pokrenuti BioinfProjekt.exe file i pratiti upute za korištenje.
1. Instalirati spoa programsku knjižicu koristeći upute s https://github.com/rvaser/spoa, obavezno instalirati program tako da se dobije executable file.
2. Instalirati monodevelop IDE: https://www.monodevelop.com/download
3. Skinuti zip pa raspakirati lokalno repozitorij, ili klonirati ovaj repozitorij
4. Pokrenuti Monodevelop, dodati novi projekt (klonirani repozitorij)

## Upute za korištenje
> Prilikom pokretanja programa, program od korisnika zahtjeva 3 parametra. Prvi parametar je file path do datoteke u kojoj se nalaze podaci, u FASTA ili FASTQ formatu. 
Drugi parametar je putanja do executable file-a SPOA-e, koji se dobije ispravnim praćenjem 1. koraka uputa za instalaciju.
Posljednji parametar je putanja do direktorija u kojem će program zapisati dobivene clustere u FASTA formatu u kojima se nalaze sve sekvence koje spadaju unutar tog clustera.

