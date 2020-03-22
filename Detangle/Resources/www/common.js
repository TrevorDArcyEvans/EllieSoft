
function PreventTouchMove()
{
  document.addEventListener("touchmove", preventBehavior, false);
}

function preventBehavior(e)
{
  e.preventDefault();
}

function pause(milliseconds)
{
  var dt = new Date();
  while ((new Date()) - dt <= milliseconds)
  {
    /* Do nothing */
  }
}

function sign(x)
{
  if (x == 0)
  {
    return 0;
  }

  return x/Math.abs(x);
}

function OutputDebugString(str)
{
  console.log(str);
}

function SetVisibilityForElement(elem, OS)
{
  var thisOS = BrowserDetect.OS;
  var vis = (thisOS == OS) ? "visible" : "hidden";
  var ht = (thisOS == OS) ? 200 : 0;
  var style = document.getElementById(elem).style;

  style.visibility = vis;
  style.height = ht;
}

function GotoGamePage()
{
  var ua = navigator.userAgent;
  var Is_iPad = ua.match(/iPad/i) != null;
  var Is_Mac = ua.match(/Mac/i) != null;

  if (Is_iPad)
  {
    location.href='game_large.html';
  }
  else if (Is_Mac && window.innerWidth > 480)
  {
    location.href='game_medium.html';
  }
  else
  {
    // iPhone
    location.href='game.html';
  }
}

