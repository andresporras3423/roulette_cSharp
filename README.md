# roulette_cSharp

Web API for a roulette game, created by Oscar Russi

# What it does

- Create users, every users has a name, id and credit
- Create Games, a roulette game has an id and status. Status should be 0, 1 or 2
- Game status meaning: 0=created, 1=Open, 2=Closed
- Create bets, a bet has an id, game id, player id, number for the bet and value
- bet number must be in the range 0 and 38 inclusive, where 38 is for even numbers, 37 for odd numbers and any other to bet for an specific number
- if player gets the color (evens are red, odds are black) they get 1.8 times the bet value, if they get the number then they get 5 times the bet value
- The game outsome is given when the game is closed
- the database is published in MongoDB Cloud
- the Web API is published in Azure

# EndPoints:

- game/get_all (GET): get all the roulette games with it's id and status
- player/create (POST): create a new player, return true if created
- game/create (POST): create a new game with initial status 0, return game id
- game/open?id= (PUT): update game status to 1 (open), return true if successful
- bet/create (POST): create a new bet, return bet id
- game/close?idGame= (PUT): close the game and return outcome of the bets. Also update credit for players if they win


# Build with

- C#
- .Net Framework
- MongoDB
- Azure
- MongoDB Cloud

# Live demo

[Live Demo](https://roulettegame.azurewebsites.net/game/get_all)


#### and deployed to GitHub

## Authors

**Oscar Russi**
- Github: [@andresporras3423](https://github.com/andresporras3423/)
- Linkedin: [Oscar Russi](https://www.linkedin.com/in/oscar-andres-russi-porras)

## Show your support

Give a ⭐️ if you like this project!

## Enjoy!
