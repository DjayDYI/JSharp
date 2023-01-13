# JSharp
Learning C# is fun what about J#

## JStack
```
     push 3
     push 2
     add
     print
     push x
     mov 
     print
     push x
     load
     print
     push 4
     print
     push 0
     push x
     mov
  for:
     push x
     load
     print
     push 1
     add
     push x
     mov
     push 5
     push x
     load 
     lt branch for
     halt
 ```
## Jsharp
Here how to use the language!

Name a variable
```
VAR x 0
```
Add,sub,mul,div some stuff to the variable
```
ADD 2 x
MOV
```
Increment, Decrement
```
INC x
DEC x
```

Print Stuff
```
PRINT x
```

This is a for loop 
```
 VAR X 0
 for: 
    INC X
    PRINT X
    GOTO X lt 5 for
 ```
Also a for loop ... 
```
FOR x 0 .. 5 
  [ PRINT ]
```
This is a if ... if in case
```
VAR X 0
VAR Y 1
IF X eq 0 
[ 
     ADD X 2
]
ELSE 
[
     ADD X Y 
] 
PRINT X
```
 
