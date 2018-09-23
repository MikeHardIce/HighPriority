# HighPriority
Little console app that can change the process priority. I use this to improve the performance of some games. The app will check every 30 secs for processes listed in the priority.ini and sets the priority as specified.

# Steps
1. Build/Publish the code
2. Create a priority.ini file in the same location of your dll/executable.
3. Execute (to stop either write exit or close)

# Example content priority.ini
HeroesOfTheStorm_x64=High
Overwatch=High
QuakeChampions=High