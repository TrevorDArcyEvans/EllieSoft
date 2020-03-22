global.PlayMusic = 1;

// w = wall
// a = empty air
// e = exit
// t = trail

// create the random maze
global.SpriteWidth = 24;
global.SpriteHeight = 24;

// put border around maze of air as a boundry
// change this section to make the maze bigger.
// just be sure there is a border of air around the block of wall
for (mx = 0; mx < 45; mx += 1)
{
    for (my = 0; my < 34; my += 1)
    {
      // "a" =  empty air in maze
      global.maze[mx, my] = "a"
    }
}

// set maze array to solid wall
for (mx = 1; mx < 42; mx += 1)
{
    for (my = 1; my < 30; my += 1)
    {
      // "w" = wall
      global.maze[mx, my] = "w"  
    }
}
    

runs = 0
    
currentX = 2
currentY = (round(random(6)) * 2) + 2


// home position "t" for trail
global.maze[currentX, currentY] = "t"   

// use these to remember where the start of the maze is
startx = currentX
starty = currentY

// these hold co-ordinates for the max distance from maze start. Where the exit, treasure etc goes 
maxx = currentX
maxy = currentY


// number of steps since start
steps = 0 

// record max steps. Highest number of steps gives you the max distance from the start, so that is where you put the treasure etc
stepsmax = 0 

endmaze = false
goback = false


while (endmaze = false) 
{
    moved = false    

    if (global.maze[currentX, currentY + 2] = "w") && (random(4) < 1) && (moved = false)//move down
    { 
        global.maze[currentX, currentY + 1] = "t"   //trail used to backtrack to start point
        global.maze[currentX, currentY + 2] = "t"
        currentY = currentY + 2
        steps = steps + 2
        goback = false
        moved = true
    }   
        
    if (global.maze[currentX, currentY - 2] = "w") && (random(4) < 1) && (moved = false)//move up
    { 
        global.maze[currentX, currentY - 1] = "t"
        global.maze[currentX, currentY - 2] = "t"
        currentY = currentY - 2
        steps = steps + 2
        goback = false
        moved = true
    }  
        
    if (global.maze[currentX + 2, currentY] = "w") && (random(4) < 1) && (moved = false)//move right
    { 
        global.maze[currentX + 2, currentY] = "t"
        global.maze[currentX + 1, currentY] = "t"
        currentX = currentX + 2
        steps = steps + 2
        goback = false
        moved = true
    }
               
        
     if (global.maze[currentX - 2, currentY] = "w") && (random(4) < 1) && (moved = false)//move left
     { 
        global.maze[currentX - 2, currentY] = "t"
        global.maze[currentX - 1, currentY] = "t"
        currentX = currentX - 2
        steps = steps + 2
        goback = false
        moved = true
     }       
      
      

 // if the new position has nowhere else to go then move back until you find a free position
 
   if (global.maze[currentX + 2, currentY] <> "w") &&
      (global.maze[currentX - 2, currentY] <> "w") &&
      (global.maze[currentX, currentY + 2] <> "w") &&
      (global.maze[currentX, currentY - 2] <> "w")   
    {
    
       if (steps >= stepsmax) then // we have found a new max distance from startpoint
       {
         stepsmax = steps
         maxx = currentX
         maxy = currentY
       }
       
    
    returned = false
    goback = false
    // no position is free, so backtrack and look again
    if (global.maze[currentX + 1, currentY] = "t") && (returned = false)// return right
    {
      global.maze[currentX, currentY] = "a"
      global.maze[currentX + 1, currentY] = "a"
      currentX = currentX + 2
      steps = steps - 2
      returned = true
      goback = true
    }
    
    
    if (global.maze[currentX - 1, currentY] = "t") && (returned = false)//return left
    {
       global.maze[currentX, currentY] = "a"
       global.maze[currentX - 1, currentY] = "a"
       currentX = currentX - 2
       steps = steps - 2
       returned = true
       goback = true
    }
   
    if (global.maze[currentX, currentY + 1] = "t") && (returned = false)//return down
    {
       global.maze[currentX, currentY] = "a"
       global.maze[currentX, currentY + 1] = "a"
       currentY = currentY + 2
       steps = steps - 2
       returned = true
       goback = true
    }
 
    if (global.maze[currentX, currentY - 1] = "t") && (returned = false)//return up
    {
      global.maze[currentX, currentY] = "a"
      global.maze[currentX, currentY - 1] = "a"
      currentY = currentY - 2
      steps = steps - 2
      returned = true
      goback = true
    }
 
    }
    
  if (global.maze[currentX - 2, currentY] <> "w") && 
     (global.maze[currentX + 2, currentY] <> "w") && 
     (global.maze[currentX, currentY + 2] <> "w") &&  
     (global.maze[currentX, currentY - 2] <> "w") && 
     (goback = false)
    {
      //all finished
      endmaze = true
    }
 }

 
// draw the maze
for (mx = 1; mx < 43; mx += 1)
{
    for (my = 1; my < 32; my += 1)
    {
        if global.maze[mx, my] = "w"
        {
          instance_create((mx - 1) * global.SpriteWidth,(my - 1) * global.SpriteHeight, wall)
        }                      
    }             
}
 
// put in start and end items, mark the maze grid to indicate where they are so the find path routine can find them
instance_create((startx - 1) * global.SpriteWidth, (starty - 1) * global.SpriteHeight, Player)
instance_create((maxx - 1) * global.SpriteWidth, (maxy - 1) * global.SpriteHeight, end_point)
global.maze[maxx, maxy] = "e" // e = exit

instance_create(0, 708, ShowPath);

