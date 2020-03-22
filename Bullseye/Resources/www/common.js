
var CommonHasInitialised = false;
var CommonHtml5AudioAvailable = false;

function PreventTouchMove()
{
  document.addEventListener("touchmove", preventBehavior, false);
}

function preventBehavior(e)
{
  e.preventDefault();
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

function PlaySound(SoundPath)
{
  OutputDebugString(BrowserDetect.OS + " / " + BrowserDetect.browser);
  OutputDebugString("Playing " + SoundPath);

  if (CommonHtml5AudioAvailable)
  {
    OutputDebugString("  using HTML5");
    var ThisSound = new Audio(SoundPath);
    ThisSound.play()
  }
  else
  {
    // use PhoneGap sound
    OutputDebugString("  using PhoneGap");
    new Media(SoundPath).play();
  }
  OutputDebugString("  Played " + SoundPath);
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
  if (PhoneGap.available)
  {
    debug.log(str);
  }
  else
  {
    console.log(str);
  }
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

