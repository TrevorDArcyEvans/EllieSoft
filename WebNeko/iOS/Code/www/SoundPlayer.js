
var Sounds =
[
 "24966_mich3d_2CF836_01.mp3",
 "24966_mich3d_2CF836_02.mp3",
 "24966_mich3d_2CF836_03.mp3",
 "31540_freedomrhodes2CF824_1.mp3",
 "31540_freedomrhodes2CF824_2.mp3",
 "31540_freedomrhodes2CF824_3.mp3",
 "62033_LukeIRL_Cat_miaow.mp3",
 "64017_Department64_2CF814.mp3",
 "98671_Hamface_cat01.mp3",
 "98671_Hamface_cat02.mp3",
];
              
var HasInitialised = false;
var Html5AudioAvailable = false;

function init()
{
  if (HasInitialised)
  {
    return;
  }
  
  // TODO   init
  
  // this is the recommended way to detect HTML5 audio support
  //Html5AudioAvailable = !!document.createElement('audio').canPlayType;
  
  // FFS!   Mobile WebKit on iPhone doesn't support HTML5 audio
  //        even though it says it does - f£$%^&g Apple,
  //        so we have to examine the browser OS
  Html5AudioAvailable = ("iPhone/iPod" != BrowserDetect.OS);

  HasInitialised = true;
}

function PlaySound(SoundPath)
{
  //debug.log(BrowserDetect.OS + " / " + BrowserDetect.browser);
  //debug.log("Playing " + SoundPath);
  
  if (Html5AudioAvailable)
  {
    //debug.log("  using HTML5");
    var ThisSound = new Audio(SoundPath);
    ThisSound.play()
  }
  else
  {
    // use PhoneGap sound
    //debug.log("  using PhoneGap");
    new Media(SoundPath).play();
  }
  //debug.log("  Played " + SoundPath);
}

function PlayRandomSound()
{
  var index = Math.floor(Math.random() * Sounds.length);
  //debug.log("Playing " + index);
  var SoundPath = "Sounds/" + Sounds[index];
  PlaySound(SoundPath);
}
