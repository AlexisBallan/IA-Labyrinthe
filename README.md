# IA-Labyrinthe

## Présentation du projet en vidéo : https://youtu.be/1GmZ7sGdai4

![Capture d’écran 2023-05-04 183107](https://user-images.githubusercontent.com/54318416/236343757-c4fee492-8ff7-4ca8-b340-215ae0519bb1.png)

# Description

Je vous présente mon nouveau projet qui porte sur l'intelligence artificielle. Mon objectif était d'explorer les réseaux de neurones et de les intégrer à un algorithme génétique pour obtenir un produit fonctionnel.


Le fonctionnement du projet est le suivant : à chaque génération, 40 agents apparaissent et peuvent se déplacer dans la direction de leur choix en utilisant les 5 rayons équipés sur eux. Les rayons permettent de détecter les murs et d'éviter les collisions. Seuls les 10 meilleurs agents de chaque génération sont sélectionnés pour la génération suivante.

Les agents évoluent ainsi : plus ils s'éloignent de leur point d'apparition, plus ils gagnent de points. S'ils parcourent une grande distance rapidement, ils obtiennent également des points, ce qui les encourage à aller le plus vite possible. Cependant, leur score diminue au fil du temps et ils perdent des points s'ils heurtent un mur. Il est donc crucial qu'ils se déplacent continuellement pour être sélectionnés par l'algorithme génétique.

Dans le labyrinthe, des checkpoints rapportant 10 points chacun sont placés. Il y a aussi une sortie qui est l'objectif ultime des agents, cela leur permet de gagner 10 000 points. Au fil du temps et des échecs, les agents apprennent à naviguer dans le labyrinthe pour atteindre leur objectif.
