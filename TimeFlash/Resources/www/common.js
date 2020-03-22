
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

function sign(x)
{
  if (x == 0)
  {
    return 0;
  }

  return x/Math.abs(x);
}

function ZeroPad(num, places)
{
  var zero = places - num.toString().length + 1;

  return Array(+(zero > 0 && zero)).join("0") + num;
}

function OutputDebugString(str)
{
  console.log(str);
}
