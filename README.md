# AgarIO tcp server implementation for hackathons and such

## The game

### Objective

The game is a clone to [agar.io](http://agar.io/). You can go there and fiddle around
for a bit to get a feel for the game.

Your bot is a 'blob' that is spawned into a world full of other blobs and viruses.
There is food all over the place. Your objective is to become the biggest blob of them all.

It is quite simple to do that: 
- you eat food 
- avoid bigger players
- canibalise smaller players

There are some more advanced mechanics like splitting and shooting parts of your blob
towards other players. Coming soon.

### Playing the game

You will communicate with the server over TCP/IP. Your client will send and receive stringified
[JSON](https://www.json.org/) objects followed by a new line character `\n`.

To start moving your blob around you will need to: 
- connect to the TCP host
- login by sending `{"Login": "username", "Password": "pass"}\n` to the server
  `Login` and `Password` will be provided by admin server
- join current game session by sending `{"Type": "Join", "PlayerId": "playerId"}\n`
  `PlayerId` will be provided by admin server

You are in! You can start controlling your blob now.

You can read on to learn the details of communication interface or dive right into code.
There are 2 example implementations of dumb randomly moving bots that you can refer to:
- [C#](./AgarIo.ClientExample/Program.cs)
- [nodejs](./nodeJS.ClientExample/index.js)

## API

### Knowing what is around you

You can get information of your immediate surroundings by sending `{"Type": "GetView"}\n` and
awaiting a response.

The server will respond with something like this:
```
{
  blobs: [
    {
      Id: 0,
      Name: '', // only for player-type blobs
      Radius: 23,
      Position: {X: 45, Y: 67},
      Type: 0
    },
    {...},
    {...},
    ...
  ],
  error: 0
}
```

Blob types are as follows: 
- 0: Player
- 1: Virus
- 2: Food

### Moving your blob

To move your blob send `{"Type": "Move", "Dx": dx, "Dy": dy}\n`.

(Dx, Dy) pair is a [displacement vector](https://en.wikipedia.org/wiki/Displacement_(vector)) in
cartesian coordinate system - it tells the server in which direction your blob should move. The
origin of the coordinate system is in the center of the world. The world size is configurable and
will probably change from game to game.

You don't have control over blob speed, suffice it to say that smaller blobs move faster 
than bigger blobs.

## Running the server

## Visualisation

[https://github.com/kfazi/AgarIoVisualisation](https://github.com/kfazi/AgarIoVisualisation)
