# RoboGrid

This is a C# console application. The project reads from the `commands.txt` file in the root of the repo and runs the project according to the commands defined the in the file.

The output is displayed in the console once the project is run.

Following are some example commands. Copy/paste them into the `commands.txt` file to see the desired result:

```
PLACE 0,0,NORTH
MOVE
REPORT
```
Expected output: `0,1,NORTH`

```
PLACE 0,0,NORTH
LEFT
REPORT
```
Expected output: `0,0,WEST`


```
PLACE 0,0,NORTH
PLACE 0,1,SOUTH
RIGHT
MOVE
REPORT
```
Expected output: `1,1,EAST`
