var ResourcesDirectory = "./Resources/";

var ClockBaseDirectory = ResourcesDirectory + "Clock/";
var ClockAnalogDirectory = ClockBaseDirectory + "Analog/";
var ClockDigitalDirectory = ClockBaseDirectory + "Digital/";

var IconsDirectory = ResourcesDirectory + "Icons/";

var VoiceIcons = new Array
(
  IconsDirectory + "1227973961782836377Farmeral_audio-icon.svg.med.off.png",
  IconsDirectory + "Gerald_G_Lady_Face_Cartoon.png",
  IconsDirectory + "Gerald_G_Man_Face_8_-_World_Label.png"
);

var CurrentVoiceIndex = 0;
function CycleVoice()
{
  CurrentVoiceIndex++;
  if (CurrentVoiceIndex >= VoiceIcons.length)
  {
    // cycle back to start
    CurrentVoiceIndex = 0;
  }

  UpdateControllers();
  ReloadClock();
}

var ClockDirectory = new Array();
ClockDirectory[0] = ClockDigitalDirectory;
ClockDirectory[1] = ClockAnalogDirectory;

var ClockTypeIcons = new Array();
ClockTypeIcons[0] = IconsDirectory + "Digital.png";
ClockTypeIcons[1] = IconsDirectory + "Analog.png";
ClockTypeIcons[2] = IconsDirectory + "Random.png";

var CurrentClockTypeIndex = 0;
function CycleClockType()
{
  CurrentClockTypeIndex++;
  if (CurrentClockTypeIndex >= ClockTypeIcons.length)
  {
    // cycle back to start
    CurrentClockTypeIndex = 0;
  }

  SetClockVisibilities();
  UpdateControllers();
  ReloadClock();
}

function SetClockVisibilities()
{
  var clockIndex = GetCurrentClockTypeIndex();
  var elemToHide = (clockIndex == 0) ? "analog" : "digital";
  var elemToShow = (clockIndex == 1) ? "analog" : "digital";
  var docToHide = document.getElementById(elemToHide);
  var docToHideStyle = docToHide.style;
  var docToShow = document.getElementById(elemToShow);
  var docToShowStyle = docToShow.style;

  docToHideStyle.visibility = "hidden";
  docToHideStyle.height = "0px";

  docToShowStyle.visibility = "visible";
  docToShowStyle.height = "400px";

  docToHide.height = "0px";
  docToShow.height = "400px";

  // extra hackery to hide sub-divs of digital clock
  document.getElementById("digiclock").hidden = (clockIndex == 1);
  document.getElementById("plugin_container").hidden = (clockIndex == 1);
}

var CurrentTime = new Date();

function GetCurrentClockTypeIndex()
{
  var retVal = CurrentClockTypeIndex;

  // 2 == random
  if (2 == CurrentClockTypeIndex)
  {
    retVal = Math.floor(Math.random() * (ClockTypeIcons.length - 1));
  }

  return retVal;
}

function LoadClock(thisDate)
{
  OutputDebugString("Time = " + ZeroPad(thisDate.getHours(), 2) + ":" + ZeroPad(thisDate.getMinutes(), 2));

  SetClockVisibilities();

  // send time to two clocks
  SetTimeDigital();
  SetTimeAnalog();
}

function ReloadClock()
{
  // reload current time
  LoadClock(CurrentTime);
}

function RandomClock()
{
  // generate a random time
  CurrentTime = new Date();

  // generate hour [1, 12]
  var hrs = Math.ceil(Math.random()*12);
  CurrentTime.setHours(hrs);

  // generate mins in 5 min increments [0, 55]
  var mins = Math.floor(Math.random()*12) * 5;
  CurrentTime.setMinutes(mins);

  LoadClock(CurrentTime);
}

/* When this function is called, PhoneGap has been initialized and is ready to roll */
function OnDeviceReady()
{
  // do your thing!
  WatchForShake(1.25);
}

function WatchForShake(threshold)
{
  var axl = new Accelerometer();

  axl.watchAcceleration
  (
   function (Accel)
   {
    if (true === Accel.is_updating)
    {
      return;
    }

   if (Math.abs(Accel.x) >= threshold &&
      Math.abs(Accel.y) >= threshold )
    {
      //debug.log("acc(" + Accel.x + "," + Accel.y + "," + Accel.z + ")");
      RandomClock();
    }
   }
   , function(){}
   , { frequency : 60 }
   );
}

function ClearLocalStorage()
{
  localStorage.clear();
}

var CurrentClockTypeIndexKey = "ClockFlash.CurrentClockTypeIndex";
var CurrentVoiceIndexKey = "ClockFlash.CurrentVoiceIndex";

function SaveSettings()
{
  localStorage[CurrentClockTypeIndexKey] = CurrentClockTypeIndex;
  localStorage[CurrentVoiceIndexKey] = CurrentVoiceIndex;
}

function LoadSettings()
{
  CurrentClockTypeIndex = parseInt(localStorage[CurrentClockTypeIndexKey]);
  if (isNaN(CurrentClockTypeIndex))
  {
    CurrentClockTypeIndex = 0;
  }

  CurrentVoiceIndex = parseInt(localStorage[CurrentVoiceIndexKey]);
  if (isNaN(CurrentVoiceIndex))
  {
    CurrentVoiceIndex = 0;
  }
}

function UpdateControllers()
{
  var VoiceCtrl = document.getElementById("VoiceController");
  VoiceCtrl.src = VoiceIcons[CurrentVoiceIndex];

  var ClockCtrl = document.getElementById("ClockTypeController");
  ClockCtrl.src = ClockTypeIcons[CurrentClockTypeIndex];

  SaveSettings();
}

function PlayCurrentTime()
{
  if (CurrentVoiceIndex != 0)
  {
    var SoundFilePath = GetSoundFilePath(CurrentTime, CurrentVoiceIndex);

    // play voice
    PlaySound(SoundFilePath);
  }
}

function OnBodyLoad()
{
  CommonInit();
  LoadSettings();
  UpdateControllers();
  SetClockVisibilities();

  document.addEventListener("deviceready", OnDeviceReady, false);

  PreventTouchMove();

  RandomClock();
}
