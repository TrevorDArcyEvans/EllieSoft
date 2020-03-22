var CurrentColour = "#333399";

function InitColourPicker()
{
  jQuery(document).ready(function($)
  {
    $('#color1').colorPicker();
    $('#color1').change(function()
    {
      // alert("color changed = " + $('#color1').val());
      CurrentColour = $('#color1').val();
      context.strokeStyle = CurrentColour;
      context.fillStyle = context.strokeStyle;
      SaveSettings();
    });
  });
}

var ResourcesDirectory = "./Resources/";
var AlphabetDirectory = ResourcesDirectory + "Alphabet/";
var FormationDirectory = ResourcesDirectory + "Formation/";
var IconsDirectory = ResourcesDirectory + "Icons/";

var VoiceIcons = new Array
(
  IconsDirectory + "1227973961782836377Farmeral_audio-icon.svg.med.off.png",
  IconsDirectory + "1227973961782836377Farmeral_audio-icon.svg.med.png"
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
  ReloadCanvas();
}

var CurrentImageIndex = 0;

function ClearCanvas()
{
  context.clearRect(0, 0, canvas.width, canvas.height);
  contexto.clearRect(0, 0, canvaso.width, canvaso.height);
}

function LoadCanvas(index)
{
  ClearCanvas();

  var IsiPad = navigator.userAgent.match(/iPad/i) != null;

  var IsLandscape = (window.orientation == "90" || window.orientation == "-90");
  OutputDebugString("IsLandscape = " + IsLandscape);
  OutputDebugString("  window.orientation = " + window.orientation);

    // show letter
  var img = new Image();
  img.src = AlphabetDirectory + AlphabetFiles[index];
  img.onload = function()
  {
    // fix height so it works *only* in portrait on iPhone (480px)
    var ImgHeight = 420;
    if (IsiPad)
    {
      ImgHeight += 320;
    }

    // maintain aspect ratio
    var ImgWidth = ImgHeight * img.width/img.height;

    // a little bit off the top to allow for table margins
    var ImgMargin = -20;
    if (IsiPad)
    {
      ImgMargin += 70;
    }

    // Five arguments:
    //  the element,
    //  destination (x,y) coordinates
    //  destination width and height (if you want to resize the source image).
    contexto.drawImage(img,
      // center letter horizontally
      (canvaso.width - ImgWidth) / 2, ImgMargin,

      ImgWidth, ImgHeight);
  };

  // show formation
  var adjunctImg = new Image();
  adjunctImg.src = FormationDirectory + FormationFiles[index];
  adjunctImg.onload = function()
  {
    var ImgHeight = 100;
    if (IsiPad)
    {
      ImgHeight += 180;
    }

    // maintain aspect ratio
    var ImgWidth = ImgHeight * adjunctImg.width/adjunctImg.height;

    var ImgWidthMargin = 0;
    var ImgHeightMargin = -5;

    // Five arguments:
    //  the element,
    //  destination (x,y) coordinates
    //  destination width and height (if you want to resize the source image).
    contexto.drawImage(adjunctImg,
      canvaso.width - (ImgWidth + ImgWidthMargin), canvaso.height - (ImgHeight + ImgHeightMargin),
      ImgWidth, ImgHeight);
  };

  if (CurrentVoiceIndex != 0)
  {
    var SoundFilePath = GetSoundFilePath(index, CurrentVoiceIndex);

    // play voice
    PlaySound(SoundFilePath);
  }

  // restore colour
  context.strokeStyle = CurrentColour;
}

function NextCanvas()
{
  CurrentImageIndex++;
  if (CurrentImageIndex >= AlphabetFiles.length)
  {
    // cycle around to start
    CurrentImageIndex = 0;
  }
  LoadCanvas(CurrentImageIndex);
}

function PreviousCanvas()
{
  CurrentImageIndex--;
  if (CurrentImageIndex < 0)
  {
    // cycle around to end
    CurrentImageIndex = AlphabetFiles.length - 1;
  }
  LoadCanvas(CurrentImageIndex);
}

function ReloadCanvas()
{
  // reload current image
  LoadCanvas(CurrentImageIndex);
}

function RandomCanvas()
{
  // load a random image
  CurrentImageIndex = Math.floor(Math.random() * AlphabetFiles.length);
  LoadCanvas(CurrentImageIndex);
}

function ResizeCanvas()
{
  var Header = document.getElementById("header");
  var HeaderHeight = Header.clientHeight;
  var Footer = document.getElementById("footer");
  var FooterHeight = Footer.clientHeight;
  var HeightOffset = HeaderHeight + FooterHeight + 10;

  canvas.height = window.innerHeight - HeightOffset;
  canvaso.height = window.innerHeight - HeightOffset;

  var WidthOffset = 2;

  canvas.width = window.innerWidth - WidthOffset;
  canvaso.width = window.innerWidth - WidthOffset;

  context.lineWidth = 8;
}

/* When this function is called, PhoneGap has been initialized and is ready to roll */
function OnDeviceReady()
{
  // do your thing!
  // except for Android as the accelerometer calibrations are not consistent across devices :-(
  // to support 'shake to erase', we'd have to calibrate the accelerometer which might be
  // beyond a parents' or child's ability
  if (BrowserDetect.OS != "Android")
  {
    WatchForShake(1.25);
  }
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
      ReloadCanvas();
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

var CurrentVoiceIndexKey = "HiraganaTracer.CurrentVoiceIndex";
var CurrentColourKey = "HiraganaTracer.CurrentColour";

function SaveSettings()
{
  localStorage[CurrentVoiceIndexKey] = CurrentVoiceIndex;
  localStorage[CurrentColourKey] = CurrentColour;
}

function LoadSettings()
{
  CurrentVoiceIndex = parseInt(localStorage[CurrentVoiceIndexKey]);
  if (isNaN(CurrentVoiceIndex))
  {
    CurrentVoiceIndex = 0;
  }

  CurrentColour = localStorage[CurrentColourKey];
  if (CurrentColour == undefined)
  {
    CurrentColour = "#333399";
  }
}

function UpdateControllers()
{
  var VoiceCtrl = document.getElementById("VoiceController");
  VoiceCtrl.src = VoiceIcons[CurrentVoiceIndex];

  $('#color1').val(CurrentColour).change();

  SaveSettings();
}

function OnBodyLoad()
{
  CommonInit();
  InitDrawing();
  LoadSettings();
  UpdateControllers();

  document.addEventListener("deviceready", OnDeviceReady, false);

  PreventTouchMove();

  InitColourPicker();
  ResizeCanvas();
  RandomCanvas();

  context.strokeStyle = $('#color1').val();
  context.fillStyle = context.strokeStyle;
}
