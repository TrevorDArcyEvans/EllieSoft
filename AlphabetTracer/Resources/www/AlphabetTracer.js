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

var AlphabetBaseDirectory = ResourcesDirectory + "Alphabet/";
var AlphabetUpperDirectory = AlphabetBaseDirectory + "Upper/";
var AlphabetLowerDirectory = AlphabetBaseDirectory + "Lower/";

var AdjunctAlphabetBaseDirectory = ResourcesDirectory + "DeafAlphabet/";
var AdjunctAlphabetGBDirectory = AdjunctAlphabetBaseDirectory + "GB/";
var AdjunctAlphabetUSDirectory = AdjunctAlphabetBaseDirectory + "US/";

var AdjunctAlphabetFormationDirectory = ResourcesDirectory + "Formation/";
var AdjunctAlphabetFormationLowerDirectory = AdjunctAlphabetFormationDirectory + "Lower/";
var AdjunctAlphabetFormationUpperDirectory = AdjunctAlphabetFormationDirectory + "Upper/";

var IconsDirectory = ResourcesDirectory + "Icons/";
var FlagsDirectory = ResourcesDirectory + "Flags/";

var CurrentLanguageIcons = new Array();
CurrentLanguageIcons[0] = FlagsDirectory + "jp_draws_US_Flag.png";
CurrentLanguageIcons[1] = FlagsDirectory + "PanamaG_UK_Flag.png";

var CurrentLanguageIndex = 0;
function CycleLanguage()
{
  CurrentLanguageIndex++;
  if (CurrentLanguageIndex >= CurrentLanguageIcons.length)
  {
    // cycle back to start
    CurrentLanguageIndex = 0;
  }

  UpdateControllers();
  ReloadCanvas();
}

var VoiceIcons = new Array
(
  IconsDirectory + "1227973961782836377Farmeral_audio-icon.svg.med.off.png",
  IconsDirectory + "Gerald_G_Boy_Face_Cartoon_2.png",
  IconsDirectory + "Gerald_G_Lady_Face_Cartoon.png",
  IconsDirectory + "Gerald_G_Man_Face_8_-_World_Label.png",
  IconsDirectory + "Gerald_G_Cartoon_Cat_Face.png"
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

var AlphabetDirectory = new Array();
AlphabetDirectory[0] = AlphabetLowerDirectory;
AlphabetDirectory[1] = AlphabetUpperDirectory;
AlphabetDirectory[2] = AlphabetLowerDirectory;
AlphabetDirectory[3] = AlphabetUpperDirectory;

var AlphabetFiles = new Array();
AlphabetFiles[0] = AlphabetLowerFiles;
AlphabetFiles[1] = AlphabetUpperFiles;
AlphabetFiles[2] = AlphabetLowerFiles;
AlphabetFiles[3] = AlphabetUpperFiles;

var AdjunctAlphabetDirectory = new Array();
AdjunctAlphabetDirectory[0] = AdjunctAlphabetUSDirectory;
AdjunctAlphabetDirectory[1] = AdjunctAlphabetGBDirectory;
AdjunctAlphabetDirectory[2] = AdjunctAlphabetFormationLowerDirectory;
AdjunctAlphabetDirectory[3] = AdjunctAlphabetFormationUpperDirectory;

var AdjunctAlphabetFiles = new Array();
AdjunctAlphabetFiles[0] = AdjunctAlphabetUS;
AdjunctAlphabetFiles[1] = AdjunctAlphabetGB;
AdjunctAlphabetFiles[2] = FormationLowerFiles;
AdjunctAlphabetFiles[3] = FormationUpperFiles;

var LetterTypeIcons = new Array();
LetterTypeIcons[0] = IconsDirectory + "Lower_A.png";
LetterTypeIcons[1] = IconsDirectory + "Upper_A.png";
LetterTypeIcons[2] = IconsDirectory + "Formation_Lower_A_icon.png";
LetterTypeIcons[3] = IconsDirectory + "Formation_Upper_A_icon.png";

var CurrentLetterTypeIndex = 0;
function CycleLetterType()
{
  CurrentLetterTypeIndex++;
  if (CurrentLetterTypeIndex >= LetterTypeIcons.length)
  {
    // cycle back to start
    CurrentLetterTypeIndex = 0;
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

  var IsLowerCase = (CurrentLetterTypeIndex == 2);
  var IsiPad = navigator.userAgent.match(/iPad/i) != null;

  var IsLandscape = (window.orientation == "90" || window.orientation == "-90");
  OutputDebugString("IsLandscape = " + IsLandscape);
  OutputDebugString("  window.orientation = " + window.orientation);

    // show letter
  var img = new Image();
  img.src = AlphabetDirectory[CurrentLetterTypeIndex] + AlphabetFiles[CurrentLetterTypeIndex][index];
  img.onload = function()
  {
    // fix height so it works *only* in portrait on iPhone (480px)
    var ImgHeight = 270;
    if (IsLowerCase)
    {
      ImgHeight += 30;
    }
    if (IsiPad)
    {
      ImgHeight += 120;
    }

    // maintain aspect ratio
    var ImgWidth = ImgHeight * img.width/img.height;

    // a little bit off the top to allow for table margins
    var ImgMargin = 10;
    if (IsiPad)
    {
      ImgMargin += 100;
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

  // show adjunct alphabet letter
  var AdjunctIndex = CurrentLetterTypeIndex;
  if (AdjunctIndex == 0 || AdjunctIndex == 1)
  {
    // we're doing 'formation' language, so allow for language
    AdjunctIndex = CurrentLanguageIndex;
  }

  var adjunctImg = new Image();
  adjunctImg.src = AdjunctAlphabetDirectory[AdjunctIndex] + AdjunctAlphabetFiles[AdjunctIndex][index];
  adjunctImg.onload = function()
  {
    var ImgHeight = 100;
    if (IsLowerCase)
    {
      ImgHeight += 40;
    }
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
    var SoundFilePath = GetSoundFilePath(index, CurrentVoiceIndex, CurrentLanguageIndex);

    // play voice
    PlaySound(SoundFilePath);
  }

  // restore colour
  context.strokeStyle = CurrentColour;
}

function NextCanvas()
{
  CurrentImageIndex++;
  if (CurrentImageIndex >= AlphabetFiles[CurrentLetterTypeIndex].length)
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
    CurrentImageIndex = AlphabetFiles[CurrentLetterTypeIndex].length - 1;
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
  CurrentImageIndex = Math.floor(Math.random() * AlphabetFiles[CurrentLetterTypeIndex].length);
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

var CurrentLetterTypeIndexKey = "AlphabetTracer.CurrentLetterTypeIndex";
var CurrentLanguageIndexKey = "AlphabetTracer.CurrentLanguageIndex";
var CurrentVoiceIndexKey = "AlphabetTracer.CurrentVoiceIndex";
var CurrentColourKey = "AlphabetTracer.CurrentColour";

function SaveSettings()
{
  localStorage[CurrentLetterTypeIndexKey] = CurrentLetterTypeIndex;
  localStorage[CurrentLanguageIndexKey] = CurrentLanguageIndex;
  localStorage[CurrentVoiceIndexKey] = CurrentVoiceIndex;
  localStorage[CurrentColourKey] = CurrentColour;
}

function LoadSettings()
{
  CurrentLetterTypeIndex = parseInt(localStorage[CurrentLetterTypeIndexKey]);
  if (isNaN(CurrentLetterTypeIndex))
  {
    CurrentLetterTypeIndex = 0;
  }

  CurrentLanguageIndex = parseInt(localStorage[CurrentLanguageIndexKey]);
  if (isNaN(CurrentLanguageIndex))
  {
    CurrentLanguageIndex = 0;
  }

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
  var LangCtrl = document.getElementById("LanguageController");
  LangCtrl.src = CurrentLanguageIcons[CurrentLanguageIndex];

  var VoiceCtrl = document.getElementById("VoiceController");
  VoiceCtrl.src = VoiceIcons[CurrentVoiceIndex];

  var CaseCtrl = document.getElementById("LetterTypeController");
  CaseCtrl.src = LetterTypeIcons[CurrentLetterTypeIndex];

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
