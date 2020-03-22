//find path to end point

// w = wall
// a = empty air
// e = exit
// t = trail

//remove old trail, not important for this deom, but in a real game if you try to use this 
//and there is already a trail in place, you will have trouble
for (mx = 0; mx < 45; mx += 1)
{
    for (my = 0; my < 34; my += 1)
    {
       if global.maze[mx, my] = "t"
       {
          global.maze[mx, my] = "a"
       }                      
    }             
}


currentX = startx
currentY = starty

global.maze[currentX, currentY] = "t"

endmaze = false

retern = false

while (endmaze = false)
{  
    moved = false

    if (global.maze[currentX, currentY + 1] = "a") && (moved = false)  //move down
    { 
        global.maze[currentX, currentY] = "t" //trail
        currentY = currentY + 1
        moved = true
    }   
        
    if (global.maze[currentX, currentY - 1] = "a") && (moved = false) //move up
    { 
        global.maze[currentX, currentY] = "t" //trail
        currentY = currentY - 1
        moved = true       
    }  
        
    if (global.maze[currentX + 1, currentY] = "a") && (moved = false) //move right
    { 
        global.maze[currentX, currentY] = "t" //trail 
        currentX = currentX + 1
        moved = true
    }                         
        
    if (global.maze[currentX - 1, currentY] = "a") && (moved = false) //move left
    { 
        global.maze[currentX, currentY] = "t" //trail
        currentX = currentX - 1
        moved = true    
    }       
      
    if global.maze[currentX + 1, currentY] = "e" || 
       global.maze[currentX - 1, currentY] = "e" || 
       global.maze[currentX, currentY + 1] = "e" || 
       global.maze[currentX, currentY - 1] = "e" then
    {
        //FOUND EXIT all finished
        global.maze[currentX, currentY] = "t"
        endmaze = true
    }

 
   if (moved = false) then //boxed in or no move found
   {
     returned = false //don't go back more than one step per cycle
    
     global.maze[currentX, currentY] = "z"  //z for bad trail, places we have already searched        
    
     
     //no position is free, so backtrack and look again, follow the trail of 5's
     if global.maze[currentX + 1, currentY] = "t" && (returned = false) then//return right
     {
       currentX = currentX + 1
       returned = true
     }
    
    
    if global.maze[currentX - 1, currentY] = "t"  && (returned = false) then//return left
    {
      currentX = currentX - 1
      returned = true
    }
   
    if global.maze[currentX, currentY + 1] = "t"  && (returned = false) then//return down
    {
      currentY = currentY + 1
      returned = true    
    }
 
    if global.maze[currentX, currentY - 1] = "t"  && (returned = false) then//return up
    {
      currentY = currentY - 1
      returned = true
    }
    
    }   

}//endmaze = false

// put trail in the maze
for (mx = 1; mx < 43; mx += 1)
{
  for (my = 1; my < 32; my += 1)
  {
    if global.maze[mx,my] = "t"
    {
      instance_create((mx - 1) * global.SpriteWidth - global.SpriteWidth/2, (my - 1) * global.SpriteHeight - global.SpriteHeight/2, trail)
    }  
  }             
}
   

    

