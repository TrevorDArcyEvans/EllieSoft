
var CommonHasInitialised = false;
var CommonHtml5AudioAvailable = false;

// If you want to prevent dragging, uncomment this section
/*
function preventBehavior(e)
{
    e.preventDefault();
  };
document.addEventListener("touchmove", preventBehavior, false);
*/

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

function PlayRandomSound()
{
  var index = Math.floor(Math.random() * (Sounds.length + 1));
  var SoundPath = "Sounds/" + Sounds[index];
  PlaySound(SoundPath);
}

function GetMaxScore()
{
  var score = localStorage.getItem("max_score");
  if (null == score)
  {
    score = 0;
  }
  return score;
}

function SetMaxScore(value)
{
  localStorage.setItem("max_score", value);
}

function GetMaxLevel()
{
  var Retval = localStorage.getItem("max_level");
  if (null == Retval)
  {
    Retval = 0;
  }
  return Retval;
}

function SetMaxLevel(value)
{
  localStorage.setItem("max_level", value);
}

function pause(milliseconds)
{
  var dt = new Date();
  while ((new Date()) - dt <= milliseconds)
  {
    /* Do nothing */
  }
}

var BrowserDetect =
{
  init: function ()
  {
    this.browser = this.searchString(this.dataBrowser) || "An unknown browser";
    this.version = this.searchVersion(navigator.userAgent)
      || this.searchVersion(navigator.appVersion)
      || "an unknown version";
    this.OS = this.searchString(this.dataOS) || "an unknown OS";
  },
  searchString: function (data)
  {
    for (var i=0;i<data.length;i++)
    {
      var dataString = data[i].string;
      var dataProp = data[i].prop;
      this.versionSearchString = data[i].versionSearch || data[i].identity;
      if (dataString)
      {
        if (dataString.indexOf(data[i].subString) != -1)
        {
          return data[i].identity;
        }
      }
      else if (dataProp)
      {
        return data[i].identity;
      }
    }
  },
  searchVersion: function (dataString)
  {
    var index = dataString.indexOf(this.versionSearchString);
    if (index == -1)
    {
      return;
    }
    return parseFloat(dataString.substring(index + this.versionSearchString.length + 1));
  },
  dataBrowser: [
    {
      string: navigator.userAgent,
      subString: "Chrome",
      identity: "Chrome"
    },
    { 	string: navigator.userAgent,
      subString: "OmniWeb",
      versionSearch: "OmniWeb/",
      identity: "OmniWeb"
    },
    {
      string: navigator.vendor,
      subString: "Apple",
      identity: "Safari",
      versionSearch: "Version"
    },
    {
      prop: window.opera,
      identity: "Opera"
    },
    {
      string: navigator.vendor,
      subString: "iCab",
      identity: "iCab"
    },
    {
      string: navigator.vendor,
      subString: "KDE",
      identity: "Konqueror"
    },
    {
      string: navigator.userAgent,
      subString: "Firefox",
      identity: "Firefox"
    },
    {
      string: navigator.vendor,
      subString: "Camino",
      identity: "Camino"
    },
    {		// for newer Netscapes (6+)
      string: navigator.userAgent,
      subString: "Netscape",
      identity: "Netscape"
    },
    {
      string: navigator.userAgent,
      subString: "MSIE",
      identity: "Explorer",
      versionSearch: "MSIE"
    },
    {
      string: navigator.userAgent,
      subString: "Gecko",
      identity: "Mozilla",
      versionSearch: "rv"
    },
    { 		// for older Netscapes (4-)
      string: navigator.userAgent,
      subString: "Mozilla",
      identity: "Netscape",
      versionSearch: "Mozilla"
    }
  ],
  dataOS : [
    {
      string: navigator.platform,
      subString: "Win",
      identity: "Windows"
    },
    {
      string: navigator.platform,
      subString: "Mac",
      identity: "Mac"
    },
    {
         string: navigator.userAgent,
         subString: "iPhone",
         identity: "iPhone/iPod"
      },
    {
      string: navigator.platform,
      subString: "Linux",
      identity: "Linux"
    }
  ]

};
  BrowserDetect.init();
