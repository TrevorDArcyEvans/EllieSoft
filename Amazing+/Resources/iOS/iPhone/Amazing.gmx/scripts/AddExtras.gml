// go through empty spaces in interior of maze
for (mx = 1; mx < 42; mx += 1)
{
    for (my = 1; my < 30; my += 1)
    {
      // "a" = air
      if (global.maze[mx, my] != "a"   )
      {
        continue;
      }
      
      // chance = [0.0 - 5000.0]
      chance = random(5000)
      level = lives

      // clamp level otherwise distribution of extras
      // gets a bit skewed
      if (level > 40)
      {
         level = 40;
      }

      xPos = (mx - 1) * global.SpriteWidth
      yPos = (my - 1) * global.SpriteHeight

      // a bit of extra time for the first few levels
      if (level <= 5)
      {
         if (0 < chance && chance < 35)
         {
            // Clock +10 time
            instance_create(xPos, yPos, Clock)
         }
         if (45 < chance && chance < 55)
         {
            // WaterMelon +50 score
            instance_create(xPos, yPos, WaterMelon)
         }
         if (65 < chance && chance < 70)
         {
            // Pizza +100 score
            instance_create(xPos, yPos, Pizza)
         }
         continue;
      }


      // only get here for > level 5
      if (level >= 1)
      {
         if (0 < chance && chance < 3.5 * level)
         {
            // Clock +10 time
            instance_create(xPos, yPos, Clock)
         }
         if (3.5 * level < chance && chance < 6 * level)
         {
            // WaterMelon +50 score
            instance_create(xPos, yPos, WaterMelon)
         }
         if (6 * level < chance && chance < 7.5 * level)
         {
            // Pizza +100 score
            instance_create(xPos, yPos, Pizza)
         }
      }

      if (level >= 5)
      {
         if (7.5 * level < chance && chance < 12.5 * level)
         {
            // Clock +10 time
            instance_create(xPos, yPos, Clock)
         }
         if (12.5 * level < chance && chance < 15 * level)
         {
            // Skull1 -10 time
            instance_create(xPos, yPos, Skull1)
         }
         if (15 * level < chance && chance < 17.5 * level)
         {
            // Apple +100 score
            instance_create(xPos, yPos, Apple)
         }
      }
        
      if (level >= 10)
      {
         if (17.5 * level < chance && chance < 20 * level)
         {
            // Cherry +100 score
            instance_create(xPos, yPos, Cherry)
         }
         if (20 * level < chance && chance < 21 * level)
         {
            // Burger +150 score
            instance_create(xPos, yPos, Cherry)
         }
         if (21 * level < chance && chance < 22 * level)
         {
            // Jewel2 +200 score
            instance_create(xPos, yPos, Jewel2)
         }
         if (22 * level < chance && chance < 24.5 * level)
         {
            // Skull2 -20 time
            instance_create(xPos, yPos, Skull2)
         }
         if (24.5 * level < chance && chance < 27 * level)
         {
            // Timer + 20 time
            instance_create(xPos, yPos, Timer)
         }
      }
        
      if (level >= 15)
      {
         if (27 * level < chance && chance < 29 * level)
         {
            // Lollypop +150 score
            instance_create(xPos, yPos, Lollypop)
         }
         if (29 * level < chance && chance < 30 * level)
         {
            // Silver +250 score
            instance_create(xPos, yPos, Silver)
         }
         if (30 * level < chance && chance < 31 * level)
         {
            // Rocket
            instance_create(xPos, yPos, Rocket)
         }
      }

      if (level >= 20)
      {
         if (31 * level < chance && chance < 32 * level)
         {
            // Jewel1 +400 score
            instance_create(xPos, yPos, Jewel1)
         }
      }

      if (level >= 25)
      {
         if (32 * level < chance && chance < 33.5 * level)
         {
            // Bomb -500 score
            instance_create(xPos, yPos, Bomb)
         }
         if (33.5 * level < chance && chance < 34 * level)
         {
            // Gold +500 score
            instance_create(xPos, yPos, Gold)
         }
      }
    }
}

