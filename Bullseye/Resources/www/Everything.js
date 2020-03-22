/*
Target Shooter + Engine
Target Shooter es un juego, y Engine es un pequeño motor que hice par hacer juegos
en HTML5 canvas.

Copyright (C) 2011 Ezequiel Adrián Schwartzman

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

var Calc=
{
  distance:function(x1,y1,x2,y2)
  {
    return Math.sqrt((x1-x2)*(x1-x2)+(y1-y2)*(y1-y2));
  },

  circleCollision:function(x1,y1,r1,x2,y2,r2)
  {
    return ((x1-x2)*(x1-x2)+(y1-y2)*(y1-y2))<((r1+r2)*(r1+r2));
  },

  rotatePoint:function(cx,cy,rot,x,y)
  {
    if(rot==0||(x==cx&&y==cy))
    {
      return[x,y];
    }

    var dx=x-cx;
    var dy=y-cy;
    var h=Math.sqrt(dx*dx+dy*dy);
    var ang=(dx<0)?Math.PI-Math.asin(dy/h):Math.asin(dy/h);
    var nang=ang+rot;
    var rotx=h*Math.cos(nang)+cx;
    var roty=h*Math.sin(nang)+cy;

    return[rotx,roty];
  },

  rotatePoints:function(cx,cy,rot,points)
  {
    var rotatedPoints=[];
    for(var i in points)
    {
      rotatedPoints.push(Calc.rotatePoint(cx, cy, rot, points[i][0], points[i][1]));
    }

    return rotatedPoints;
  },

  distanceToLine:function(x,y,a,b)
  {
    return Math.abs(a*x-y+b)/Math.sqrt(a*a+1);
  },

  distanceToAxis:function(xp,yp,x1,y1,x2,y2)
  {
    if(x1==x2)
    {
      return Math.abs(x1-xp);
    }

    if(y1==y2)
    {
      return Math.abs(y1-yp);
    }

    a=(y1-y2)/(x1-x2);b=y1-x1*a;

    return Math.abs(a*xp-yp+b)/Math.sqrt(a*a+1);
  },

  random:function(min,max)
  {
    if(!max)
    {
      max=min;
      min=0;
    }

    return min + parseInt(Math.random()*max);
  }
}

function Element(shape)
{
  if(typeof(Element.IDs)=='undefined')
  {
    Element.IDs=0;
  }

  var ID=Element.IDs++;
  var shape=shape;
  var style={x:0,y:0,z:0,rot:0,alpha:1,scale:1}
  var realPosition=
  {
    x:0,y:0,rot:0,
    bb:
    {
      points:[[0,0],[0,0],[0,0],[0,0],[0,0]],complex:[]
    },
    center:{x:0,y:0}
  }

  var realPos=realPosition;
  var flags=  {
                positionUpdated:false,
                bbPositionUpdated:false,
                bcPositionUpdated:false,
                complexPositionUpdated:false,
                fixed:false
              }
  var childs=[];
  var parent=false;

  function draw(ctx)
  {
    ctx.save();
    translate(ctx);
    ctx.globalAlpha*=style.alpha;
    ctx.scale(style.scale,style.scale);
    shape.draw(ctx);
    drawChilds(ctx);
    ctx.restore();
  }

  function drawChilds(ctx)
  {
    for(i in childs)
    {
      childs[i].draw(ctx);
    }
  }

  function translate(ctx)
  {
    ctx.translate(style.x+shape.center.x, style.y+shape.center.y);
    ctx.rotate(style.rot);
    ctx.translate(-shape.center.x,-shape.center.y);
  }

  function appendChild(element)
  {
    var parent=element.parent();
    if(parent)
    {
      if(parent.ID===ID)
      {
        return;
      }

      parent.removeChild(element);
    }

    element.parent(that);
    childs.push(element);
  }

  function removeChild(element)
  {
    for(var i in childs)
    {
      if(childs[i].ID==element.ID)
      {
        childs.splice(i,1);
        element.parent(false);
        return;
      }
    }
  }

  function setAbsolutePosition()
  {
    if(parent)
    {
      var point=Calc.rotatePoint(
                        parent.realPos.center.x(),
                        parent.realPos.center.y(),
                        parent.realPos.rot(),
                        parent.realPos.x()+style.x,parent.realPos.y()+style.y);
      realPos.x=point[0];
      realPos.y=point[1];
      realPos.rot=style.rot+parent.realPos.rot();
    }
    else
    {
      realPos.x=style.x;
      realPos.y=style.y;
      realPos.rot=style.rot;
    }

    var centerPoints=Calc.rotatePoint(
                            realPos.x,
                            realPos.y,
                            realPos.rot,
                            realPos.x+shape.center.x*style.scale,
                            realPos.y+shape.center.y*style.scale);
    realPos.center.x=centerPoints[0];
    realPos.center.y=centerPoints[1];
  }

  function setAbsoluteRotation()
  {
    if(parent)
    {
      realPos.rot=style.rot+parent.realPos.rot();
    }
    else
    {
      realPos.rot=style.rot;
    }
  }

  function setFlagPositionUpdated(b)
  {
    if(b===true)
    {
      flags.positionUpdated=true;
    }
    else if(b===false)
    {
      if(flags.positionUpdated)
      {
        flags.positionUpdated=flags.bbPositionUpdated=flags.bcPositionUpdated=flags.complexPositionUpdated=false;
        setFlagPositionUpdatedForChilds(false);
      }
    }

    return flags.positionUpdated;
  }

  function setFlagPositionUpdatedForChilds(b)
  {
    for(var i in childs)
    {
      childs[i].setFlagPositionUpdated(false);
    }

    flags.bbPositionUpdated=flags.bcPositionUpdated=flags.complexPositionUpdated=false;
  }

  function setX(x)
  {
    if(typeof(x)=="number")
    {
      style.x=x;setFlagPositionUpdated(false);
    }

    return style.x;
  }

  function setY(y)
  {
    if(typeof(y)=="number")
    {
      style.y=y;setFlagPositionUpdated(false);
    }

    return style.y;
  }

  function setZ(z)
  {
    if(typeof(z)=="number")
    {
      style.z=z;
    }

    return style.z;
  }

  function setAlpha(alpha)
  {
    if(typeof(alpha)=="number")
    {
      style.alpha=alpha;
    }

    return style.alpha;
  }

  function setScale(scale)
  {
    if(typeof(scale)=="number")
    {
      style.scale=scale;
      if(flags.positionUpdated)
      {
        setFlagPositionUpdatedForChilds(false);
      }
    }

    return style.scale;
  }

  function setRot(rot)
  {
    if(typeof(rot)=="number")
    {
      style.rot=rot;
      if(flags.positionUpdated)
      {
        setAbsoluteRotation();
        setFlagPositionUpdatedForChilds(false);
      }
    }

    return style.rot;
  }

  function updateAbsolutePosition()
  {
    if(flags.positionUpdated)
    {
      return false;
    }

    if(parent&&!parent.flags.positionUpdated())
    {
      parent.updateAbsolutePosition();
    }

    setAbsolutePosition();
    setFlagPositionUpdated(true);

    return true;
  }

  function updateBCAbsolutePosition()
  {
    if(flags.bcPositionUpdated||flags.bbPositionUpdated)
    {
      return false;
    }

    updateAbsolutePosition();
    var point=Calc.rotatePoint(
                    realPos.center.x,
                    realPos.center.y,
                    realPos.rot,
                    realPos.x+shape.bb.x*style.scale,
                    realPos.y+shape.bb.y*style.scale);
    realPos.bb.points[0]=point;
    flags.bcPositionUpdated=true;
  }

  function updateBBAbsolutePosition()
  {
    if(flags.bbPositionUpdated)
    {
      return false;
    }

    updateAbsolutePosition();
    var points=Calc.rotatePoints(
                      realPos.center.x,
                      realPos.center.y,
                      realPos.rot,
                      [
                        [realPos.x+shape.bb.x*style.scale,realPos.y+shape.bb.y*style.scale],
                        [realPos.x+(shape.bb.x+shape.bb.w)*style.scale,realPos.y+shape.bb.y*style.scale],
                        [realPos.x+(shape.bb.x+shape.bb.w)*style.scale,realPos.y+(shape.bb.y+shape.bb.h)*style.scale],
                        [realPos.x+shape.bb.x*style.scale,realPos.y+(shape.bb.y+shape.bb.h)*style.scale],
                        [realPos.x+(shape.bb.x+shape.bb.w/2)*style.scale,realPos.y+(shape.bb.y+shape.bb.h/2)*style.scale]
                      ]);
    realPos.bb.points=points;
    flags.bcPositionUpdated=true;
    flags.bbPositionUpdated=true;
  }

  function circleToCircleCollision(element)
  {
    updateBCAbsolutePosition();
    element.updateBCAbsolutePosition();

    return Calc.circleCollision(
                  realPos.bb.points[0][0],
                  realPos.bb.points[0][1],
                  shape.bb.r*style.scale,
                  element.realPos.bb.points[0][0],
                  element.realPos.bb.points[0][1],
                  element.shape.bb.r*element.scale());
  }

  function circleToPointCollision(x,y)
  {
    updateBCAbsolutePosition();

    return Calc.circleCollision(
                  realPos.bb.points[0][0],
                  realPos.bb.points[0][1],
                  shape.bb.r*style.scale,
                  x,
                  y,
                  0);
  }

  function boxPreCollision(element,y)
  {
    if(typeof y=="number")
    {
      return Calc.circleCollision(
                    realPos.bb.points[4][0],
                    realPos.bb.points[4][1],
                    shape.bb.r*style.scale,
                    element,
                    y,
                    0);
    }
    else
    {
      return Calc.circleCollision(
                    realPos.bb.points[4][0],
                    realPos.bb.points[4][1],
                    shape.bb.r*style.scale,
                    element.realPos.bb.points[4][0],
                    element.realPos.bb.points[4][1],
                    element.shape.bb.r*element.scale());
    }
  }

  function distancesToBoxAxes(x,y)
  {
    var distances=[];
    var j;

    for(var i=0;i<4;++i)
    {
      j=((i+1)>3)?0:(i+1);
      distances.push(Calc.distanceToAxis(
                            x,
                            y,
                            realPos.bb.points[i][0],
                            realPos.bb.points[i][1],
                            realPos.bb.points[j][0],
                            realPos.bb.points[j][1]));
    }

    return distances;
  }

  function boxToPointCollision(x,y)
  {
    updateBBAbsolutePosition();
    if(!boxPreCollision(x,y))
    {
      return false;
    }

    distances=distancesToBoxAxes(x,y);

    return (distances[0]<shape.bb.h && distances[1]<shape.bb.w && distances[2]<shape.bb.h && distances[3]<shape.bb.w) ? true : false;
  }

  function boxToBoxCollision(element)
  {
    updateBBAbsolutePosition();
    element.updateBBAbsolutePosition();
    if(!boxPreCollision(element))
    {
      return false;
    }

    for(var i=0;i<4;++i)
    {
      if(boxToPointCollision(element.realPos.bb.points[i][0],element.realPos.bb.points[i][1]))
      {
        return true;
      }

      if(element.boxToPointCollision(realPos.bb.points[i][0],realPos.bb.points[i][1]))
      {
        return true;
      }
    }

    return false;
  }

  var that =
  {
    ID:ID,
    boxToPointCollision:boxToPointCollision,
    boxToBoxCollision:boxToBoxCollision,
    circleToPointCollision:circleToPointCollision,
    circleToCircleCollision:circleToCircleCollision,
    x:setX,
    y:setY,
    z:setZ,
    rot:setRot,
    alpha:setAlpha,
    scale:setScale,
    parent:function(p)
    {
      if(typeof(p)!=="undefined")
      {
        parent=p
      };

      return parent
    },
    shape:shape,
    updateAbsolutePosition:updateAbsolutePosition,
    updateBBAbsolutePosition:updateBBAbsolutePosition,
    updateBCAbsolutePosition:updateBCAbsolutePosition,
    removeChild:removeChild,
    appendChild:appendChild,
    realPos:
    {
      x:function()
      {
        return realPos.x
      },
        y:function()
      {
        return realPos.y
      },
      rot:function()
      {
        return realPos.rot
      },
      center:
      {
        x:function()
        {
          return realPos.center.x
        },
        y:function()
        {
          return realPos.center.y
        }
      },
      bb:realPos.bb
    },
    flags:
    {
      positionUpdated:function()
      {
        return flags.positionUpdated
      },
      bbPositionUpdated:function()
      {
        return flags.bbPositionUpdated
      },
      bcPositionUpdated:function()
      {
        return flags.bcPositionUpdated
      },
      complexPositionUpdated:function()
      {
        return flags.complexPositionUpdated
      },
      fixed:function(b)
      {
        if(typeof(b)!="undefined")
        {
          flags.fixed=b;
        }

        return flags.fixed;
      }
    },
    setFlagPositionUpdated:setFlagPositionUpdated,draw:draw
  }

  return that;
}

function Engine(canvas)
{
  canvas.width=canvas.clientWidth;
  canvas.height=canvas.clientHeight;
  var ctx=canvas.getContext("2d");
  var elements=[];
  var elementsToRemove=[];
  var mouseX=0;
  var mouseY=0;
  var cameraX=0;
  var cameraY=0;
  var cameraRot=0;
  var cameraZoom=1;

  function isElementLoaded(element)
  {
    for(var i in elements)
    {
      if(elements[i]===element)
      {
        return true;
      }
    }

    return false;
  }

  function loadElement(element)
  {
    if(!isElementLoaded(element))
    {
      return elements.push(element);
    }
  }

  function unloadElement(element)
  {
    element.remove=true;
  }

  function removeAll()
  {
    elements=[];
  }

  function removeElement(element)
  {
    for(var i in elements)
    {
      if(elements[i]===element)
      {
        elements.splice(i,1);

        return;
      }
    }
  }

  function reloadElement(element)
  {
    unloadElement(element);
    loadElement(element);
  }

  function draw()
  {
    ctx.clearRect(0,0,canvas.width,canvas.height);
    ctx.save();
    ctx.translate(canvas.width/2,canvas.height/2);
    ctx.rotate(-cameraRot);
    ctx.translate(-cameraX,-cameraY);
    ctx.scale(cameraZoom,cameraZoom);

    var all=elements.length;
    for(var i=0;i<all;++i)
    {
      if(!elements[i].remove)
      {
        if(!elements[i].flags.fixed())
        {
          ctx.save();
          elements[i].draw(ctx);
          ctx.restore();
        }
      }
      else
      {
        elements.splice(i,1);
        --i;
        --all;
      }
    }

    ctx.restore();
    for(var i in elements)
    {
      if(elements[i].flags.fixed())
      {
        ctx.save();
        elements[i].draw(ctx);
        ctx.restore();
      }
    }
  }

  function pointer(s,c)
  {
    var c=c||"#F00";
    var s=s||3;
    ctx.beginPath();
    ctx.fillStyle=c;
    ctx.arc(0,0,s,0,Math.PI*2,false);
    ctx.closePath();
    ctx.fill();
  }

  var debug=
  {
    points:[],
    shape:Shape(
    {
      draw:function(ctx)
      {
        ctx.beginPath();
        ctx.fillStyle="#FF0";
        ctx.arc(0,0,3,0,Math.PI*2,false);
        ctx.closePath();
        ctx.fill();
      }
    }),
    drawPoints:function(points)
    {
      for(var i in debug.points)
      {
        unloadElement(debug.points[i]);
      }

      debug.points=[];
      for(var i in points)
      {
        var elem=Element(debug.shape);
        elem.x(points[i][0]);
        elem.y(points[i][1]);
        loadElement(elem);
        debug.points.push(elem);
      }
    }
  }

  var that=
  {
    debug:debug,
    canvas:canvas,
    ctx:ctx,
    draw:draw,
    loadElement:loadElement,
    isElementLoaded:isElementLoaded,
    unloadElement:unloadElement,
    reloadElement:reloadElement,
    removeAll:removeAll,
    camera:
    {
      x:function(x)
      {
        if(typeof(x)=="number")
        {
          cameraX=x;
        }
        return cameraX;
      },
      y:function(y)
      {
        if(typeof(y)=="number")
        {
          cameraY=y;
        }
        return cameraY;
      },
      rot:function(rot)
      {
        if(typeof(rot)=="number")
        {
          cameraRot=rot;
        }
        return cameraRot;
      },
      zoom:function(zoom)
      {
        if(typeof(zoom)=="number")
        {
          cameraZoom=zoom;
        }
        return cameraZoom;
      }
    },
    mouse:
    {
      x:function()
      {
        return mouseX;
      },
      y:function()
      {
        return mouseY;
      }
    }
  }

  that.events=Events(that);

  return that;
}

function Events(engine)
{
  var engine = engine;
  var absoluteMouse = [0,0];
  var relativeMouse = [0,0];
  var realMouse = [0,0];
  var canvasPosition = [0,0];

  function getCanvasPosition()
  {
    var _x=0;
    var _y=0;
    el=engine.canvas;

    while(el && !isNaN(el.offsetLeft) && !isNaN(el.offsetTop))
    {
      _x+=el.offsetLeft;
      _y+=el.offsetTop;
      el=el.parentNode;
    }

    return[_x,_y];
  }

  canvasPosition = getCanvasPosition();

  function isOverCanvas()
  {
    if(absoluteMouse[0] > canvasPosition[0] &&
        absoluteMouse[0] < canvasPosition[0] + engine.canvas.width &&
        absoluteMouse[1] > canvasPosition[1] &&
        absoluteMouse[1] < canvasPosition[1] + engine.canvas.height)
    {
      return true;
    }

    return false;
  }

  engine.canvas.onselectstart = function()
  {
    return false;
  }

  // START  touch support
  // from:
  //    http://ross.posterous.com/2008/08/19/iphone-touch-events-in-javascript/
  function touchHandler(event)
  {
    var touches = event.changedTouches;
    var first = touches[0];
    var type = "";

    //alert(event.type + " [" + first.screenX + "," + first.screenY + "]");
    //console.log(event.type);

    switch (event.type)
    {
      case "touchstart":
        type = "mousedown";
        FireMouseEvent(type, first);
        //event.preventDefault();

        type = "click";
        //FireMouseEvent(type, first);
        event.preventDefault();
        break;

      case "touchmove":
        type="mousemove";
        FireMouseEvent(type, first);
        event.preventDefault();
        break;

      case "touchend":
        type="mouseup";
        FireMouseEvent(type, first);
        event.preventDefault();
        break;

      default:
        return;
    }

    //alert(type);

    //event.preventDefault();
  }

  function FireMouseEvent(type, first)
  {

    /*
      javascript event initMouseEvent( )

        initMouseEvent( )	NN 6 IE n/a DOM 2
        initMouseEvent
        (
          "eventType",
          bubblesFlag, cancelableFlag,
          view, detailVal,
          screenX, screenY,
          clientX, clientY,
          ctrlKeyFlag, altKeyFlag, shiftKeyFlag, metaKeyFlag,
          buttonCode,
          relatedTargetNodeRef
        )

        Initializes a newly created event object with a complete set of property values
        associated with any mouse event. All parameters must be present, and must be set
        to default values (such as false for Boolean key flags, zero for integer values,
        or null for a node reference) if the values are not significant for the event type.

        Parameters
          String identifier for the event's type, such as click, mousedown, mousemove, mouseout, mouseover, mouseup.
          Boolean value (true | false) that determines whether the event's default propagation behavior is to bubble.
          Boolean value (true | false) that determines whether the event's default action may be prevented via the preventDefault( ) method.
          Reference to the window or frame object in which the dynamically-generated event is supposed to have occurred.
          Integer code for detail data associated with the event.
          Integer for horizontal screen coordinate.
          Integer for vertical screen coordinate.
          Integer for horizontal browser window coordinate.
          Integer for vertical browser window coordinate.
          Boolean value (true | false) of the Control key state for this event.
          Boolean value (true | false) of the Alt key state for this event.
          Boolean value (true | false) of the Shift key state for this event.
          Boolean value (true | false) of the Meta key(e.g., Macintosh Command key) state for this event.
          Integer button code for this event.
          Reference to node receiving the previous or next mouse event.

        Returned Value
          None.
    */

    var simulatedEvent = document.createEvent("MouseEvent");
    simulatedEvent.initMouseEvent(
                    type,
                    true, true,
                    window, 1,
                    first.screenX, first.screenY,
                    first.clientX, first.clientY,
                    false, false, false, false,
                    0 /* left button */,
                    null);

    first.target.dispatchEvent(simulatedEvent);
  }

  function InitForTouch()
  {
    document.addEventListener("touchstart", touchHandler, true);
    document.addEventListener("touchmove", touchHandler, true);
    document.addEventListener("touchend", touchHandler, true);
  }

  InitForTouch();
  // END    touch support

  // TODO   touch support
  function extendEvent(e)
  {
    e.canvasX = realMouse[0];
    e.canvasY = realMouse[1];
    e.overCanvas = isOverCanvas();
    if (e.overCanvas)
    {
      e.preventDefault();
    }

    return e;
  }

  // TODO   touch support
  window.onmousemove = function(e)
  {
    absoluteMouse[0] = e.pageX;
    absoluteMouse[1] = e.pageY;
    relativeMouse[0] = e.pageX - canvasPosition[0] - engine.canvas.width / 2;
    relativeMouse[1] = e.pageY - canvasPosition[1] - engine.canvas.height / 2;
    realMouse = Calc.rotatePoint(
      engine.camera.x(), engine.camera.y(),
      engine.camera.rot(),
      relativeMouse[0] + engine.camera.x(), relativeMouse[1] + engine.camera.y());
    realMouse[0] /= engine.camera.zoom();
    realMouse[1] /= engine.camera.zoom();
    that.mouse.onMove(extendEvent(e));
  }

  // TODO   touch support
  window.onclick = function(e)
  {
    that.mouse.onClick(extendEvent(e));
  }

  // TODO   touch support
  window.onmousedown = function(e)
  {
    that.mouse.onDown(extendEvent(e));
  }

  // TODO   touch support
  window.onmouseup = function(e)
  {
    that.mouse.onUp(extendEvent(e));
  }

  window.oncontextmenu = function(e)
  {
    extendEvent(e);
  }

  window.addEventListener('mousewheel', function(e)
  {
    e.canvasX = realMouse[0];
    e.canvasY = realMouse[1];
    that.mouse.onScroll(e);
  }, true);

  window.onkeypress = function(e)
  {
    that.keys.onPress(e);
  }

  window.onkeydown = function(e)
  {
    that.keys.onDown(e);
  }

  window.onkeyup = function(e)
  {
    that.keys.onUp(e);
  }

  function hideMouse()
  {
    engine.canvas.style.cursor="url(cursor.cur), auto";
  }

  function showMouse()
  {
    engine.canvas.style.cursor="auto";
  }

  var that =
  {
    mouse:
    {
      onMove:function(){},
      onClick:function(){},
      onDown:function(){},
      onUp:function(){},
      onScroll:function(){},
      hide:hideMouse,
      show:showMouse
    },
    keys:
    {
      onDown:function(){},
      onUp:function(){},
      onPress:function(){}
    }
  }

  return that;
}

function Shape(data)
{
  if(!data||!data.draw)
  {
    return false;
  }

  data.animation=data.animation||{};
  var animation=
  {
    lastTime:0,
    pause:false,
    time:data.animation.time||false,
    current:0,
    repeat:data.animation.repeat||false,
    count:0,
    drawCount:0,
    passedTime:0,
    onStop:data.animation.onStop||false,
    drawInPause:data.animation.drawInPause && true,
    reset:function()
    {
      animation.passedTime=0;
      animation.lastTime=0;
      animation.count=0;
      animation.drawCount=0;
      animation.pause=false;
    },
    restart:function()
    {
      animation.passedTime=0;
    },
    stop:function()
    {
      animation.reset();
      animation.pause=true;
    }
  }

  var inputDraw=data.draw;
  var draw=function(ctx)
  {
    if(!animation.pause||(animation.pause&&animation.drawInPause))
    {
      if(animation.time)
      {
        if(!animation.pause)
        {
          if(animation.drawCount==0)
          {
            animation.lastTime=(new Date).getTime();
            animation.passedTime=0;
          }
          else
          {
            animation.passedTime+=(new Date).getTime()-animation.lastTime;
          }
        }

        if(animation.passedTime>animation.time)
        {
          inputDraw(ctx,animation.time);
          if(animation.repeat)
          {
            ++animation.count;
            if(animation.count>=animation.repeat)
            {
              animation.stop();
              animation.onStop();
            }
            else
            {
              animation.restart();
            }
          }
          else
          {
            animation.restart();
          }
        }
        else
        {
          inputDraw(ctx,animation.passedTime);
        }
      }
      else
      {
        inputDraw(ctx);
      }

      ++animation.drawCount;
    }

    animation.lastTime=(new Date).getTime();
  }

  var center={x:0,y:0}
  var boundingBox={x:0,y:0,w:0,h:0,r:0,complex:[]}

  function edit(data)
  {
    if(!data)
    {
      return false;
    }

    inputDraw=data.draw||inputDraw;
    if(data.center)
    {
      center=
      {
        x:data.center.x||center.x,
        y:data.center.y||center.y
      }
    }

    if(data.boundingBox)
    {
      boundingBox=
      {
        x:data.boundingBox.x||boundingBox.x,
        y:data.boundingBox.y||boundingBox.y,
        w:data.boundingBox.width||data.boundingBox.w||boundingBox.w,
        h:data.boundingBox.height||data.boundingBox.h||boundingBox.h,
        complex:data.complex||[]
      }

      boundingBox.r=data.boundingBox.radius||data.boundingBox.r||Math.sqrt((boundingBox.w/2)*(boundingBox.w/2)+(boundingBox.h/2)*(boundingBox.h/2));
    }
  }

  edit(data);

  var that=
  {
    draw:function(fn)
    {
      if(typeof(fn)=="function")
      {
        inputDraw=fn;
      }
      else
      {
        draw(fn);
      }
    },
    boundingBox:boundingBox,
    bb:boundingBox,
    center:center,
    edit:edit,
    animation:
    {
      reset:animation.reset,
      stop:animation.stop,
      time:function(q)
      {
        if(q)
        {
          animation.time=q;
        }

        return animation.time;
      },
      endFn:function(fn)
      {
        animation.onStop=fn;
      },
      repeat:function(b)
      {
        animation.repeat=b;
      },
      pause:function()
      {
        animation.pause=true
      },
      play:function()
      {
        animation.play=false
      },
      drawInPause:function(b)
      {
        animation.drawInPause=b;
      }
    }
  }

  return that;
}

function Cartelito(text,engine,callback)
{
  var acceptButton=Shape(
  {
    draw:function(ctx)
    {
      ctx.fillStyle="#555";
      ctx.fillRect(-60,-20,120,40);
      ctx.fillStyle="#EEE";
      ctx.font="23pt Arial";
      ctx.textAlign="center";
      ctx.fillText("OK",0,10);
    },
    boundingBox:{x:-60,y:-20,w:120,h:40}
  });

  var shape=Shape(
  {
    draw:function(ctx)
    {
      ctx.fillStyle="#333";
      ctx.fillRect(-200,-100,400,200);
      ctx.fillStyle="#EEE";
      ctx.font="12pt Arial";
      ctx.textAlign="center";
      var exploded=text.split("\n");
      var startY=-exploded.length*10;
      for(var i in exploded)
      {
        ctx.fillText(exploded[i],0,startY+i*20-30);
      }
    }
  });

  var win=Element(shape);
  var button=Element(acceptButton);
  button.y(50);
  win.appendChild(button);
  var lastOnClick = engine.events.mouse.onClick;
  engine.events.mouse.onClick = function(e)
  {
    if(button.boxToPointCollision(e.canvasX,e.canvasY))
    {
      engine.unloadElement(win);
      engine.events.mouse.onClick=lastOnClick;
      callback();
    }
  }

  engine.loadElement(win);
}

function FloatingText(x,y,num,text,engine)
{
  var color=(num<0)?"#F00":"#0F0";
  var shape=Shape(
  {
    draw:function(ctx,timePassed)
    {
      var ani=timePassed/1000;
      ctx.globalAlpha=1-ani;
      ctx.fillStyle=color;
      ctx.beginPath();
      ctx.shadowBlur=5;
      ctx.shadowColor="#000";
      ctx.font="23pt Arial";
      ctx.textAlign="center";
      ctx.fillText(text+""+num+"",x,y-100*ani);
    },
    animation:
    {
      time:1000,
      repeat:1,
      drawInPause:false,
      onStop:function()
      {
        engine.unloadElement(element);
      }
    }
  });

  var element=Element(shape);
  engine.loadElement(element);
}

function Game(canvas)
{
  var config=
  {
    drawInterval:33,
    processInterval:33,
    stageListerWidth:100,
    infoBarWidth:768,
    infoBarHeight:25
  }

  var engine=Engine(canvas);
  engine.camera.x(-config.stageListerWidth/2);
  var targetTimeout=50;
  var data=
  {
    level:0,
    stage:0,
    stages:0,
    maxLevel:0,
    score:0,
    maxScore:0,
    ended:false
  }
  var targets=[];
  var targetsToRemove=[];
  var stagesUI;
  var differentsStages=10;
  var infoUI;

  engine.events.mouse.onClick=function(e)
  {
    mouse.click(e);
  }

  engine.events.mouse.onDown=function(e)
  {
    mouse.down(e);
  }

  engine.events.mouse.onUp=function(e)
  {
    mouse.up(e);
  }

  var mouse=
  {
    click:function(){},
    down:function(){},
    up:function(){},
    reset:function()
    {
      mouse.show();
      mouse.click=mouse.down=mouse.up=function(){};
    },
    hide:engine.events.mouse.hide,
    show:engine.events.mouse.show
  }

  function collisions(x,y,fn)
  {
    for(var i=targets.length-1;i>=0;--i)
    {
      if(targets[i].clickable)
      {
        if(targets[i].e.circleToPointCollision(x,y))
        {
          fn(targets[i]);
          {
            return;
          }
        }
      }
    }
  }

  function setRecordCookie()
  {
    var exdate=new Date();
    exdate.setDate(exdate.getDate()+9999);
    document.cookie="record"+"="+escape(data.maxScore)+";expires="+exdate.toUTCString();
  }

  function getRecordCookie()
  {
    if(document.cookie.length>0)
    {
      var c_start=document.cookie.indexOf("record=");
      if(c_start!=-1)
      {
        c_start=c_start+7;
        var c_end=document.cookie.indexOf(";",c_start);
        if(c_end==-1)
        {
          c_end=document.cookie.length;
        }

        return unescape(document.cookie.substring(c_start,c_end));
      }
    }

    return false;
  }

  function eachTarget(fn)
  {
    for(var i in targets)
    {
      fn(targets[i]);
    }
  }

  function randomStage()
  {
    return parseInt(Math.random()*stages.length);
  }

  function levelUp()
  {
    ++level;
  }

  function addScore(num,x,y,text)
  {
    text=text||"";
    x=x||0;
    y=y||0;
    FloatingText(x,y,num,text,engine);
    data.score+=num;
    if(data.score>data.maxScore)
    {
      data.maxScore=data.score;
      infoUI.maxScore(data.score);
    }

    infoUI.score(data.score);
  }

  function maxRadius()
  {
    return((canvas.width-config.stageListerWidth)/2)/engine.camera.zoom();
  }

  function createNewTarget(radius)
  {
    var randomHipo=Math.random()*(maxRadius()-radius/2);
    var randomRad=Math.random()*Math.PI*2;
    var randomX=Math.cos(randomRad)*randomHipo;
    var randomY=Math.sin(randomRad)*randomHipo;

    return Target(randomX,randomY,radius,engine.ctx);
  }

  function loadTarget(target)
  {
    targets.push(target);
    engine.loadElement(target.e);
  }

  function removeTarget(target)
  {
    for(var i in targets)
    {
      if(targets[i].ID==target.ID)
      {
        engine.unloadElement(target.e);
        targets.splice(i,1);

        return;
      }
    }
  }

  function firstStart()
  {
    data.maxScore=getRecordCookie();
    data.maxScore=data.maxScore?data.maxScore:0;
    var newTarget=Target(canvas.width/2,0,300,engine.ctx);
    newTarget.normal();
    var shape=Shape(
    {
      draw:function(ctx)
      {
        ctx.fillStyle="#000";
        ctx.fillRect(0,0,canvas.width,canvas.height);
        ctx.fillStyle="#DDD";
        ctx.font="25pt Arial";
        ctx.shadowBlur=10;
        ctx.shadowColor="rgba(255,255,255,0.5)";
        ctx.textAlign="center";
        ctx.fillText(LANG.start,canvas.width/2,canvas.height*2/3);
        ctx.font="14pt Arial";
        ctx.fillText(LANG.optimus,canvas.width/2,canvas.height*4/5);
      }
    });

    var element=Element(shape);
    element.flags.fixed(true);
    newTarget.e.flags.fixed(true);
    engine.loadElement(element);
    engine.loadElement(newTarget.e);
    var lastClick=engine.events.mouse.onClick;
    engine.events.mouse.onClick=function()
    {
      engine.unloadElement(element);
      engine.unloadElement(newTarget.e);
      engine.events.mouse.onClick=lastClick;
      start();
    }
  }

  function start()
  {
    data.ended=false;
    for(var i in stages)
    {
      stages.times=0;
    }

    data.stages=0;
    data.score=0;
    engine.removeAll();
    stagesUI=StageLister(canvas,config.stageListerWidth);
    stagesUI.push([0,1,2,3]);
    for(var i=0;i<8;++i)
    {
      stagesUI.push(randomStage());
    }

    stagesUI.e.flags.fixed(true);
    engine.loadElement(stagesUI.e);
    infoUI=InfoBar(canvas,config.infoBarWidth,config.infoBarHeight);
    infoUI.e.flags.fixed(true);
    infoUI.e.x(100);
    infoUI.maxScore(data.maxScore);
    engine.loadElement(infoUI.e);
    changeStage(stagesUI.current());
  }

  function changeStage(num)
  {
    infoUI.stages(data.stages);
    ++data.stages;
    data.stage=num;
    stages[data.stage].start();
  }

  function endStage(win)
  {
    mouse.reset();
    if(win)
    {
      stagesUI.push(randomStage());
      stagesUI.shift();
      setTimeout(function()
      {
        changeStage(stagesUI.current());
      },1500);
    }
    else
    {
      data.ended = true;
      setRecordCookie(data.maxScore);
      var newRecord=(data.score>=data.maxScore)?LANG.recordBeat:(LANG.noRecord+data.maxScore);
      Cartelito(LANG.gameOver + "\n" + LANG.scored + " " + data.score + " " + LANG.score + "!\n" + newRecord,engine,function()
      {
        window.location.reload();
      });
    }
  }

  function stageProcess(){}

  function process()
  {
    for(var i in targets)
    {
      targets[i].process();
    }

    stages[data.stage].process();
  }

  function draw()
  {
    engine.draw();
    setTimeout(draw,config.drawInterval);
  }

  draw();
  setInterval(process,config.processInterval);

  var stagesInterface=
  {
    mouse:mouse,
    endStage:endStage,
    createNewTarget:createNewTarget,
    removeTarget:removeTarget,
    loadTarget:loadTarget,
    addScore:addScore,
    collisions:collisions,
    maxRadius:maxRadius,
    engine:engine,
    eachTarget:eachTarget,
    targetsLeft:function()
    {
      return targets.length;
    },
    ended:function(b)
    {
      if(typeof(b)!=="undefined")
      {
        data.ended=b;
      }

      return data.ended;
    }
  }

  var stages=Stages(stagesInterface);

  return {start:firstStart}
}

function InfoBar(canvas,width,height)
{
  var height=height;
  var width=width;
  var score=0;
  var stages=0;
  var maxScore=0;
  var shape=Shape(
  {
    draw:function(ctx)
    {
      ctx.fillStyle="rgba(0,0,0,0.3)";
      ctx.fillRect(0,0,width,height);
      ctx.fillStyle="#FFF";
      ctx.beginPath();
      ctx.scale(2,2);
      ctx.textAlign="right";
      ctx.fillText(LANG.score+": "+score,width/2-2.5,10);
      ctx.textAlign="left";
      ctx.fillText(LANG.stage+": "+stages,2.5,10);
      ctx.closePath();
      ctx.textAlign="center";
      ctx.fillText(LANG.record+": "+maxScore,310/2,10);
      ctx.fill();
    }
  });

  var element=Element(shape);
  var that=
  {
    e:element,
    element:element,
    score:function(s)
    {
      score=s;
    },
    stages:function(s)
    {
      stages=s;
    },
    maxScore:function(s)
    {
      maxScore=s
    }
  }

  return that;
}

var Resources=
{
  stagesImages:[],
  stagesBar:{}
}

var dsa=function()
{
  var LANGs=
  {
    en:
    {
      gameOver:"Game over!",
      scored:"You scored",
      noRecord:"You didn't beat the record of ",
      recordBeat:"You beat the record!",
      accept:"Accept",
      score:"Score",
      record:"Record",
      stage:"Stage",
      start:"Click to start",
      optimus:"",
      play:"Play ",
    }
  }

  window.LANG=LANGs.en;

  for(var i=0;i<4;++i)
  {
    var img=new Image();
    img.src="game2/images/"+i+".png";
    img.onload=function()
    {
      Resources.stagesImages.push(this);
    }
  }

    Resources.stagesBar=new Image();
    Resources.stagesBar.src="game2/images/stagesBar.png";
};

dsa();

function StageLister(canvas,width)
{
  var stages=[];
  var width=width;
  var height=canvas.height;
  var margin=3;
  var stageHeight=60;
  var animationTime=1000;
  var distanceShifted=0;
  var shiftAnimationSpeed=3;
  var shiftedStages=[];
  var shape=Shape(
  {
    draw:function(ctx,timePassed)
    {
      var currentTime=(new Date).getTime();
      ctx.translate(margin,0);
      if(shiftedStages.length>0)
      {
        for(var i=0,j=shiftedStages.length;i<j;++i)
        {
          var sAni=1-(currentTime-shiftedStages[i][1])/animationTime;
          if(sAni>0)
          {
            var alphaAni=sAni>0.75?(sAni-0.75)*4:0;
            var lastAlpha=ctx.globalAlpha;
            ctx.globalAlpha*=alphaAni;
            ctx.scale(1,sAni);
            drawStage(shiftedStages[i][0],ctx);
            ctx.scale(1,1/sAni);ctx.globalAlpha=lastAlpha;
          }
          else
          {
            shiftedStages.splice(i,1);
            --i;
            --j;
          }
        }
      }

      var quantity=parseInt(height/(stageHeight+margin))+2;
      quantity=(quantity>stages.length)?stages.length:quantity;

      for(var i=0;i<quantity;++i)
      {
        drawStage(stages[i],ctx);
      }
    }
  });

  function drawStage(num,ctx)
  {
    if(Resources.stagesImages[num])
    {
      var img=Resources.stagesImages[num];
      ctx.drawImage(img,0,0,img.width,img.height);
    }
    ctx.translate(0,stageHeight);
  }

  function pushStage(num)
  {
    if(typeof(num)=="object")
    {
      for(var i in num)
      {
        stages.push(num[i]);
      }
    }
    else
    {
      stages.push(num);
    }
  }

  function shiftStage(num)
  {
    num=num||1;
    for(var i=0;i<num;++i)
    {
      shiftedStages.push([stages.shift(),(new Date).getTime()]);
    }

    distanceShifted+=(stageHeight+margin)*num;
  }

  var element=Element(shape);

  return{element:element,e:element,push:pushStage,shift:shiftStage,current:function(){return stages[0]}}
}

function Stages(game)
{
  var game=game;
  var utils=
  {
    addTarget:function(config)
    {
      var newTarget=game.createNewTarget(config.targetMinRadius+Math.random()*config.targetMaxRadius);
      newTarget.appear(config.appearTime,function()
      {
        newTarget.timeout(config.timeout,function()
        {
          newTarget.missed(config.missedTime,function()
          {
            game.removeTarget(newTarget);
          });
          newTarget.clickable=false;game.endStage(false);
        });
      });

      game.loadTarget(newTarget);
    },
    setMouseDown:function(config)
    {
      game.mouse.down=function(e)
      {
        var collided=false;
        game.collisions(e.canvasX,e.canvasY,function(t)
        {
          collided=true;
          t.destroy(1000,t.e.x()-e.canvasX,t.e.y()-e.canvasY,function()
          {
            game.removeTarget(t);
          });

          t.clickable=false;
          config.timeout*=config.timeoutDiminution;
          config.targetsLeft-=1;
          var x=t.e.x()-e.canvasX;
          var y=t.e.y()-e.canvasY;
          utils.addScore(1-Math.sqrt(x*x+y*y)/t.radius(),t.radius(),e.canvasX,e.canvasY,0);
          if(config.targetsLeft>0)
          {
            setTimeout(function()
            {
              utils.addTarget(config);
            },
            config.intervalBetweenTargets);
          }
          else
          {
            game.endStage(true);
          }
        });

        if(!collided)
        {
          game.addScore(-200,e.canvasX,e.canvasY);
        }
      }
    },
    addScore:function(score,size,x,y)
    {
      var score=Math.round(score*10)*10;
      if(size>0&&size<=30)
      {
        size=100;
      }
      else if(size>130)
      {
        size=0;
      }
      else
      {
        size=100-Math.round((size-30)/10)*10;
      }

      game.addScore(score+size,x,y);
    }
  }

  var that=
  [
    {
      times:0,
      start:function()
      {
        var difficulty=Math.pow(0.90,that[0].times);
        ++that[0].times;
        var config=
        {
          timeout:2500*difficulty,
          missedTime:1000,
          appearTime:200,
          timeoutDiminution:0.96,
          targetMinRadius:60*difficulty,
          targetMaxRadius:60*difficulty,
          targetsLeft:30,
          intervalBetweenTargets:100,
          scoreMulti:that[0].times
        }

          utils.setMouseDown(config);
          utils.addTarget(config);
      },
      process:function(){}
    },
    {
      times:0,
      start:function()
      {
        var difficulty=Math.pow(0.90,that[0].times);
        ++that[1].times;
        var config=
        {
          timeout:3000*difficulty,
          missedTime:1000,
          appearTime:200,
          timeoutDiminution:0.97,
          targetMinRadius:90*difficulty,
          targetMaxRadius:50*difficulty,
          targetsLeft:30,
          intervalBetweenTargets:300,
          scoreMulti:that[1].times
        }

        game.mouse.hide();
        utils.setMouseDown(config);
        utils.addTarget(config);
      },
      process:function(){}
    },
    {
      times:0,
      start:function()
      {
        var difficulty=Math.pow(0.90,that[2].times);
        ++that[2].times;
        var maxRadius=game.maxRadius();
        console.log("Difficulty: "+difficulty);
        var config=
        {
          speed:10/difficulty,
          speedIncrease:1.02,
          targetMinRadius:50*(1-(1-difficulty)/2),
          targetMaxRadius:40*(1-(1-difficulty)/2),
          targetsLeft:30,targetsLeftToHit:30,
          targetInterval:2000*difficulty,
          intervalDiminution:0.97,
          missedTime:1000,
          destroyTime:1000,
          lastTimeout:false,
          ended:false,
          getCoords:function(radius)
          {
            var from=Math.PI*2*Math.random();
            var to=Math.PI/5*Math.random();
            var p1=[0,0];
            var p2=[0,0];
            radius=maxRadius*2+radius;
            p1[0]=Math.cos(from)*radius;
            p1[1]=Math.sin(from)*radius;
            p2[0]=Math.cos(from+Math.PI*9/10)*radius;
            p2[1]=Math.sin(from+Math.PI*9/10)*radius;

            return[p1,p2];
          },
          end:function(b)
          {
            if(!config.ended)
            {
              config.ended=true;
              config.clearTargets(b);
              game.endStage(b);
            }
          },
          clearTargets:function(b)
          {
            clearTimeout(config.lastTimeout);
            game.eachTarget(function(t)
            {
              t.clickable=false;
              if(b)
              {
                t.destroy(config.missedTime,0,0,function()
                {
                  game.removeTarget(t);
                });
              }
              else
              {
                t.missed(config.missedTime,function()
                {
                  game.removeTarget(t);
                });
              }
            });
          },
          addTarget:function()
          {
            if(!config.ended)
            {
              var radius=config.targetMinRadius+Math.random()*config.targetMaxRadius;
              var newTarget=game.createNewTarget(radius);
              newTarget.normal();
              var fromTo=config.getCoords(radius);
              newTarget.e.x(fromTo[0][0]);
              newTarget.e.y(fromTo[0][1]);
              newTarget.process=function()
              {
                var x=newTarget.e.x();
                var y=newTarget.e.y();
                var dx=x-fromTo[1][0];
                var dy=y-fromTo[1][1];
                var hip=Math.sqrt(dx*dx+dy*dy);
                x-=dx/hip*config.speed;
                y-=dy/hip*config.speed;
                if(!config.ended)
                {
                  if(x*x+y*y>((maxRadius*2+radius+5)*(maxRadius*2+radius+5)))
                  {
                    game.removeTarget(newTarget);
                    config.end(false);
                  }
                }

                newTarget.e.x(x);
                newTarget.e.y(y);
              }

              game.loadTarget(newTarget);
              --config.targetsLeft;
              if(config.targetsLeft>0)
              {
                config.lastTimeout=setTimeout(config.addTarget,config.targetInterval);
              }
            }
          }
        }

        game.mouse.down=function(e)
        {
          if(!config.ended)
          {
            var collided=false;
            game.collisions(e.canvasX,e.canvasY,function(t)
            {
              t.clickable=false;
              collided=true;
              var x=t.e.x()-e.canvasX;
              var y=t.e.y()-e.canvasY;
              utils.addScore(1-Math.sqrt(x*x+y*y)/t.radius(),t.radius(),e.canvasX,e.canvasY,0);
              config.speed*=config.speedIncrease;
              config.targetInterval*=config.intervalDiminution;
              --config.targetsLeftToHit;
              if(config.targetsLeftToHit<=0)
              {
                config.end(true);
              }
              else
              {
                t.destroy(config.destroyTime,x,y,function()
                {
                  game.removeTarget(t);
                });
              }
              });

            if(!collided)
            {
              game.addScore(-200,e.canvasX,e.canvasY);
            }
          }
        }

        config.addTarget();
      },
      process:function(){}
    },
    {
      times:0,
      start:function()
      {
        var difficulty=Math.pow(0.95,that[3].times);
        ++that[3].times;
        var config=
        {
          targetsNeeded:10,
          startRadius:70*difficulty,
          startTimeout:3000*difficulty,
          missedTime:1000,
          destroyTime:500,
          appearTime:200,
          sizeDiminution:0.97,
          speedDecrease:1.03,
          rounds:5+that[3].times,
          ended:false,
          addTargets:function(level)
          {
            for(var i=0;i<(level+1);++i)
            {
              config.addTarget(level);
            }
          },
          addTarget:function(level)
          {
            var level=level?level:0;
            var levelDiff=Math.pow(config.sizeDiminution,level);
            var levelEasyness=Math.pow(config.speedDecrease,level);
            ++level;
            var newTarget=game.createNewTarget(config.startRadius*levelDiff);
            newTarget.level=level;
            newTarget.appear(config.appearTime,function()
            {
              newTarget.timeout(config.startTimeout*levelEasyness,function()
              {
                newTarget.missed(config.missedTime,function()
                {
                  game.removeTarget(newTarget);
                });
                game.endStage(false);
                newTarget.clickable=false;
              });
            });
            game.loadTarget(newTarget);
          }
        }
        game.mouse.down=function(e)
        {
          if(!config.ended)
          {
            var collided=false;
            game.collisions(e.canvasX,e.canvasY,function(t)
            {
              t.clickable=false;
              collided=true;
              var x=t.e.x()-e.canvasX;
              var y=t.e.y()-e.canvasY;
              utils.addScore(1-Math.sqrt(x*x+y*y)/t.radius(),t.radius(),e.canvasX,e.canvasY,0);
              t.destroy(config.destroyTime,x,y,function()
              {
                game.removeTarget(t);
                if(game.targetsLeft()<=0&&!config.ended)
                {
                  --config.rounds;
                  if(config.rounds<=0)
                  {
                    config.ended=true;
                    game.endStage(true);
                  }
                  else
                  {
                    config.addTargets(t.level);
                  }
                }
              });
            });
            if(!collided)
            {
              game.addScore(-200,e.canvasX,e.canvasY);
            }
          }
        }
        config.addTargets(0);
      },
      process:function(){}
    }
  ];

  return that;
}

function start()
{
  game=new Game(document.getElementById("canvas"));
  game.start();
}

var TargetsIDs=0;

function Target(x,y,radius,ctx)
{
  var ID=TargetsIDs++;
  var radius=radius;
  var gradient=ctx.createRadialGradient(0,0,0,0,0,radius);
  gradient.addColorStop(0,'#F00');
  gradient.addColorStop(1/7-0.005,'#F00');
  gradient.addColorStop(1/7+0.005,'#FFF');
  gradient.addColorStop(2/7-0.005,'#FFF');
  gradient.addColorStop(2/7+0.005,'#F00');
  gradient.addColorStop(3/7-0.005,'#F00');
  gradient.addColorStop(3/7+0.005,'#FFF');
  gradient.addColorStop(4/7-0.005,'#FFF');
  gradient.addColorStop(4/7+0.005,'#F00');
  gradient.addColorStop(5/7-0.005,'#F00');
  gradient.addColorStop(5/7+0.005,'#FFF');
  gradient.addColorStop(6/7-0.005,'#FFF');
  gradient.addColorStop(6/7+0.005,'#F00');
  gradient.addColorStop(1-0.005,'#F00');
  gradient.addColorStop(1,'#F00');

  var shape=Shape(
  {
    draw:function(ctx,frame){},
    animation:{drawInPause:false},
    boundingBox:{r:radius}
  });

  var element=Element(shape);
  element.x(x);
  element.y(y);

  function drawNormal(ctx)
  {
    ctx.fillStyle=gradient;
    ctx.beginPath();
    ctx.arc(0,0,radius,0,Math.PI*2,false);
    ctx.closePath();
    ctx.fill();
  }

  function normal()
  {
    shape.draw(function(ctx)
    {
      drawNormal(ctx);
    });
  }

  function timeout(time,endFn)
  {
    shape.animation.reset();
    shape.animation.repeat(1);
    shape.animation.time(time);
    shape.animation.endFn(endFn);
    shape.draw(function(ctx,passedTime)
    {
      var ani=passedTime/time;
      drawNormal(ctx);
      ctx.fillStyle="rgba(0,0,0,0.3)";
      ctx.beginPath();
      ctx.arc(0,0,radius,0,Math.PI*2*ani,false);
      ctx.lineTo(0,0);
      ctx.closePath();
      ctx.fill();
    });
  }

  function destroy(time,shootX,shootY,endFn)
  {
    var fragments=[];
    var minFragments=5;
    var fragmentsCount=minFragments+parseInt(Math.random()*10);
    for(var i=0;i<fragmentsCount;++i)
    {
      fragments.push(Math.PI*2/fragmentsCount);
    }

    shape.animation.reset();
    shape.animation.repeat(1);
    shape.animation.time(time);
    shape.animation.endFn(endFn);
    shape.draw(function(ctx,passedTime)
    {
      var ani=passedTime/time;
      var totalAngle=0;
      var moveX;
      var moveY;
      var cos;
      var sin;
      ctx.fillStyle=gradient;
      ctx.globalAlpha=1-ani;
      for(var i=0;i<fragmentsCount;++i)
      {
        totalAngle+=fragments[i];
        cos=Math.cos(totalAngle-fragments[i]/2);
        sin=Math.sin(totalAngle-fragments[i]/2);
        moveX=radius*ani*(cos*cos*(cos>0?1:-1));
        moveY=radius*ani*(sin*sin*(sin>0?1:-1));
        ctx.save();
        ctx.translate(moveX,moveY);
        ctx.beginPath();
        ctx.arc(0,0,radius,totalAngle-fragments[i],totalAngle,false);
        ctx.lineTo(-shootX,-shootY);
        ctx.closePath();
        ctx.fill();
        ctx.restore();
      }
    });
  }

  function missed(time,endFn)
  {
    var radiusFraction=1/9*radius;shape.animation.reset();
    shape.animation.repeat(1);
    shape.animation.time(time);
    shape.animation.endFn(endFn);
    shape.draw(function(ctx,timePassed)
    {
      var ani=timePassed/time;
      ctx.scale(1-ani,1-ani);
      drawNormal(ctx);
      ctx.fillStyle="rgba(0,0,0,0.5";
      ctx.beginPath();
      ctx.arc(0,0,radius,0,Math.PI*2,false);
      ctx.fill();
      ctx.fillStyle="rgba(255,0,0,"+(1-ani)+")";
      ctx.scale(1+ani+0.5+2*ani,1+ani+0.5+2*ani);
      ctx.rotate(Math.PI/4);
      ctx.fillRect(-radius-radiusFraction,-radiusFraction,(radius+radiusFraction)*2,radiusFraction*2);
      ctx.rotate(Math.PI/2);
      ctx.fillRect(-radius-radiusFraction,-radiusFraction,(radius+radiusFraction)*2,radiusFraction*2);
    });
  }

  function appear(time,endFn)
  {
    shape.animation.reset();
    shape.animation.repeat(1);
    shape.animation.time(time);
    shape.animation.endFn(endFn);
    shape.draw(function(ctx,timePassed)
    {
      var ani=timePassed/time;ctx.globalAlpha=ani;drawNormal(ctx);
    });
  }

  function collision(time,ang,endFn)
  {
    destroy(time,Math.cos(ang)*radius,Math.sin(ang)*radius,endFn);
  }

  var that=
  {
    ID:ID,
    element:element,
    e:element,
    normal:normal,
    timeout:timeout,
    destroy:destroy,
    missed:missed,
    appear:appear,
    collision:collision,
    clickable:true,
    process:function(){},
    radius:function()
    {
      return radius;
    }
  }

  return that;
}

