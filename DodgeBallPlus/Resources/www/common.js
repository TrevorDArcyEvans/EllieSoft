
var CommonHasInitialised = false;
var CommonHtml5AudioAvailable = false;

function PreventTouchMove()
{
  document.addEventListener("touchmove", preventBehavior, false);
}

function preventBehavior(e)
{
    e.preventDefault();
};

function $(id)
{
  return document.getElementById(id);
}

function CommonInit()
{
  if (CommonHasInitialised)
  {
    return;
  }

  // TODO   init

  // this is the recommended way to detect HTML5 audio support
  //CommonHtml5AudioAvailable = !!document.createElement('audio').canPlayType;

  // FFS!   Mobile WebKit on iPhone doesn't support HTML5 audio
  //        even though it says it does - f£$%^&g Apple,
  //        so we have to examine the browser OS
  CommonHtml5AudioAvailable = ("iPhone/iPod" != BrowserDetect.OS);

  CommonHasInitialised = true;
}

function PlaySound(url)
{
  OutputDebugString(BrowserDetect.OS + " / " + BrowserDetect.browser);
  OutputDebugString("Playing " + url);
  OutputDebugString("  using HTML5");
  var ThisSound = new Audio(url);
  ThisSound.play()
}

function pause(milliseconds)
{
  var dt = new Date();
  while ((new Date()) - dt <= milliseconds)
  {
    /* Do nothing */
  }
}

function OutputDebugString(str)
{
  console.log(str);
}

function SetElementVisibleForOSs(elem, arrOS, height)
{
  var currOS = BrowserDetect.OS;
  var vis = "hidden";
  var ht = 0;
  var width = 0;

  for (index in arrOS)
  {
    if (arrOS[index] == currOS)
    {
      vis = "visible";
      ht = height;

      break;
    }
  }

  var style = document.getElementById(elem).style;

  style.visibility = vis;
  style.height = ht;
  if (vis == "hidden")
  {
    style.width = 0;
    style.margin = 0;
    style.padding = 0;
    style.border = 0;
  }
}


