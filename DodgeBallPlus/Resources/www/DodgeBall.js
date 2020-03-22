var ArenaCtx;
var CtrlPadCtx;

// true to use images rather than simple shapes
var UseImages = true;

// our ball object holder
// dia 10 circle
var balls = new Array();
var BallRadius = 5;

// position of finger on control pad
var FingerPos = {x:-100, y:-100};

// user
// dia 20 circle
var player = null;
var PlayerWidth = 20;
var PlayerHeight = 20;

// our prey we want to hunt
// 20x20 square
var prey;
var PreyWidth = 20;
var PreyHeight = 20;

// cache 2*PI for arc()
var circle = Math.PI * 2;

var level = GetSelectedLevel();
var score = 0;
var max_score = 0;
var max_level = 1;

var inc_score = 15;

var PlaySounds = false;
var SoundsDirectory = "./sounds/";
var ThrobberSoundPlaying = false;

var HasInitialised = false;

var StartTime = (new Date()).getTime();
var ElapsedTime = 0;
var FinishTime;
var RemainingTime = 0;

var Reverses = 0;

var Throbber = 0;

var Theme = new Array();
var CurrentTheme = 0;

function OnBodyLoad()
{
  document.addEventListener("deviceready", OnDeviceReady, false);
}

/* When this function is called, PhoneGap has been initialized and is ready to roll */
function OnDeviceReady()
{
}

function Initialise()
{
  CommonInit();

  if (HasInitialised)
  {
    return;
  }

  max_score = GetMaxScore();
  max_level = GetMaxLevel();
  PlaySounds = GetPlaySounds();
  CurrentTheme = GetSelectedTheme();

  InitThemes();
  ArenaCtx = $('arena').getContext('2d');
  CtrlPadCtx = $('ControlPad').getContext('2d');
  player = new Player(-100, -100);
  prey = new Prey(Math.random()*(GetArenaWidth() - PreyWidth), Math.random()*(GetArenaHeight() - PreyHeight));

  for (var i = 0; i < level; i++)
  {
    CreateBall();
  }

  try
  {
    clock();
  }
  catch (ex)
  {
  }

  var Arena = $('arena');
  Arena.onclick = ReverseBalls;

  var CtrlPad = $('ControlPad');
  CtrlPad.onmousemove = OnMouseMove;
  CtrlPad.ontouchmove = OnTouchMove;

  UpdateScores();
  UpdateFinishTime();

  HasInitialised = true;
}

function UpdateScores()
{
  SetMaxScore(max_score);
  SetMaxLevel(max_level);
}

function InitPlayAreas()
{
  $('arena').height = GetArenaHeight();
  $('arena').width = GetArenaWidth();
  $('ControlPad').height = GetControlPadHeight();
  $('ControlPad').width = GetControlPadWidth();
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

function GetSelectedLevel()
{
  var Retval = localStorage.getItem("sel_level");
  if (null == Retval)
  {
    Retval = 0;
  }
  return Retval;
}

function SetSelectedLevel(value)
{
  localStorage.setItem("sel_level", value);
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

function SetSelectedTheme(value)
{
  localStorage.setItem("sel_theme", value);
}

function GetSelectedTheme()
{
  var Retval = localStorage.getItem("sel_theme");
  if (null == Retval)
  {
    Retval = 0;
  }
  return Retval;
}

function SetIsFullVersion(value)
{
  localStorage.setItem("Is_Full_Version", value);
}

function GetIsFullVersion()
{
  var Retval = localStorage.getItem("Is_Full_Version");
  if (null == Retval)
  {
    Retval = false;
  }
  return Retval == "true";
}

function GetPlaySounds()
{
  var RetVal = localStorage.getItem("play_sounds");
  if (null == RetVal)
  {
    RetVal = false;
  }
  return RetVal == "true";
}

function SetPlaySounds(NewValue)
{
  localStorage.setItem("play_sounds", NewValue);
}

function UpdateScores()
{
  SetMaxScore(max_score);
  SetMaxLevel(max_level);
}

function GetArenaHeight()
{
  return 0.666 * window.innerHeight - 10;
}

function GetArenaWidth()
{
  return window.innerWidth;
}

function GetControlPadHeight()
{
  return window.innerHeight - GetArenaHeight() - 10;
}

function GetControlPadWidth()
{
  return GetArenaWidth();
}

function DrawStatus()
{
  var Baseline = GetControlPadHeight() - 55;
  var line2offset = 20;

  CtrlPadCtx.save();

  //CtrlPadCtx.font = "dejavu_sans 11px sans-serif";
  CtrlPadCtx.font = "12px sans-serif";
  CtrlPadCtx.fillText("Level: " + level, 5, Baseline);
  CtrlPadCtx.fillText("Score: " + score, 65, Baseline);
  CtrlPadCtx.fillText("Best Score: " + max_score, 130, Baseline);
  CtrlPadCtx.fillText("Best Level: " + max_level, 230, Baseline);

  CtrlPadCtx.fillText("Reverses: " + Reverses, 5, Baseline + line2offset);
  if (balls.length > 0)
  {
    ElapsedTime = ((new Date()).getTime() - StartTime) / 1000;
  }
  CtrlPadCtx.fillText("Total: " + ElapsedTime.toFixed(1) + " s", 95, Baseline + line2offset);

  if (balls.length == 0)
  {
    // only recalculate remaining time if something happening
    UpdateFinishTime();
  }
  RemainingTime = (FinishTime - (new Date()).getTime()) / 1000;

  if (RemainingTime <= 1.5)
  {
    // big red warning for last bit - no pressure
    CtrlPadCtx.fillStyle = "red";

    // make this throb!
    Throbber++;
    if (Throbber % 20 <= 10)
    {
      CtrlPadCtx.font = "bold 18px sans-serif";
    }
    else
    {
      CtrlPadCtx.font = "bold 24px sans-serif";
    }

    // reset throbber to avoid possible integer overflow
    if (Throbber % 20 == 0)
    {
      Throbber = 0;
    }

    if (PlaySounds && !ThrobberSoundPlaying)
    {
      // don't play twice
      ThrobberSoundPlaying = true;
      PlaySound(Theme[CurrentTheme].ThrobberSoundPath);
    }
  }
  var RemainingTimeDisplay = Math.max(RemainingTime, 0.0);

  CtrlPadCtx.fillText("Time: " + RemainingTimeDisplay.toFixed(1) + " s", 180, Baseline + line2offset);

  CtrlPadCtx.restore();
}

function Ball(x, y, xsee, ysee)
{
  this.Image = new Image();
  this.Image.src = Theme[CurrentTheme].BallImagePath;

  this.x = x;
  this.y = y;
  this.xsee = xsee;
  this.ysee = ysee;

  this.move = function()
  {
    var WidthLimit = GetArenaWidth() - BallRadius;
    if (this.x > WidthLimit)
    {
      this.x = WidthLimit;
      this.xsee = -this.xsee;
    }
    else if (this.x < BallRadius)
    {
      this.x = BallRadius;
      this.xsee = -this.xsee;
    }

    var HeightLimit = GetArenaHeight() - BallRadius;
    if (this.y > HeightLimit)
    {
      this.y = HeightLimit;
      this.ysee = -this.ysee;
    }
    else if (this.y < BallRadius)
    {
      this.y = BallRadius;
      this.ysee = -this.ysee;
    }

    this.x += this.xsee;
    this.y += this.ysee;
  }

  this.draw = function()
  {
    // draw ball
    if (UseImages)
    {
      ArenaCtx.drawImage(this.Image, this.x, this.y, 2*BallRadius, 2*BallRadius);
    }
    else
    {
      ArenaCtx.save();

      ArenaCtx.fillStyle = "#4246FF";
      /*
      var Grad = ArenaCtx.createLinearGradient(this.x, this.y - 2, this.x, this.y + 2);
      Grad.addColorStop(0, 'grey');
      Grad.addColorStop(1, 'blue');
      ArenaCtx.fillStyle = Grad;
      */


      ArenaCtx.beginPath();
      ArenaCtx.arc(this.x, this.y, BallRadius, 0, circle, true);
      ArenaCtx.closePath();
      ArenaCtx.fill();

      ArenaCtx.restore();
    }
  }
}

function Prey(x, y)
{
  this.Image = new Image();
  this.Image.src = Theme[CurrentTheme].PreyImagePath;
  this.width = 20;
  this.height = 20;

  this.x = x;
  this.y = y;

  this.draw = function()
  {
    if (UseImages)
    {
      ArenaCtx.drawImage(this.Image, this.x, this.y, this.width, this.height);
    }
    else
    {
      ArenaCtx.save();

      ArenaCtx.fillStyle = "#c00";
      ArenaCtx.fillRect(this.x, this.y, this.width, this.height);

      ArenaCtx.restore();
    }
  }
}

function Player(x, y)
{
  this.Image = new Image();
  this.Image.src = Theme[CurrentTheme].PlayerImagePath;

  this.x = x;
  this.y = y;

  this.draw = function()
  {
    if (UseImages)
    {
      ArenaCtx.drawImage(this.Image, this.x, this.y, 20, 20);
      CtrlPadCtx.drawImage(this.Image, FingerPos.x - 10, FingerPos.y - 10, 20, 20);
    }
    else
    {
      ArenaCtx.save();

      ArenaCtx.fillStyle = "#c00";
      ArenaCtx.beginPath();
      ArenaCtx.arc(this.x, this.y, 10, 0, circle, true);
      ArenaCtx.closePath();
      ArenaCtx.fill();

      ArenaCtx.restore();

      CtrlPadCtx.save();

      CtrlPadCtx.fillStyle = "#c00";
      CtrlPadCtx.beginPath();
      CtrlPadCtx.arc(FingerPos.x, FingerPos.y, 10, 0, circle, true);
      CtrlPadCtx.closePath();
      CtrlPadCtx.fill();

      CtrlPadCtx.restore();
    }
  }
}

function CreateBall()
{
  // CHECK    ball is generated wrt prey - should be player
  do
  {
    x = Math.random() * (GetArenaWidth() - BallRadius);
    y = Math.random() * (GetArenaHeight() - BallRadius);
  } while (prey.x <= x + 35 && x <= prey.x + 55 && prey.y <= y + 35 && y <= prey.y + 55);

  // balls get progressively faster
  var MaxInitialVel = 2.25 + 0.05*(level - 1);
  balls.push(new Ball(x, y, Math.random() * (2*MaxInitialVel) - MaxInitialVel, Math.random() * (2*MaxInitialVel) - MaxInitialVel));
}

function ResetForRestart()
{
  balls = new Array();
  level = 1;
  score = 0;
  Reverses = 0;
  StartTime = (new Date()).getTime();
  ElapsedTime = 0;
  ThrobberSoundPlaying = false;
  PlayerHasStartedMotion = false;
  UpdateFinishTime();
}

var PlayerHasStartedMotion = false;
var PlayerMotionStartTime = (new Date()).getTime();

function clock()
{
  if (RemainingTime < 0.0)
  {
    if (!PromptPlayAgain())
    {
      return;
    }
    ResetForRestart();
  }

  window.setTimeout('clock()', 20);

  // global clear is faster for many balls
  ArenaCtx.clearRect(ArenaCtx.canvas.clientLeft, ArenaCtx.canvas.clientTop, ArenaCtx.canvas.clientWidth, ArenaCtx.canvas.clientHeight);

  // note a bit extra at top & bottom as we were getting redraw artifacts
  CtrlPadCtx.clearRect(CtrlPadCtx.canvas.clientLeft, CtrlPadCtx.canvas.clientTop - 10, CtrlPadCtx.canvas.clientWidth, CtrlPadCtx.canvas.clientHeight + 15);

  // draw prey
  prey.draw();

  // draw us
  player.draw();

  DrawStatus();

  var graceTime = ((new Date()).getTime() - PlayerMotionStartTime) / 1000;

  for (var i = 0; i < balls.length; i++)
  {
    if (PlayerHasStartedMotion && graceTime > 1.5)
    {
      balls[i].move();
    }
    balls[i].draw();

    // check if ball has hit us
    // 15 = Ball.radius + Player.radius
    // 15 = Ball.radius + Player.radius
    // 225 = 15^2
    if (
      balls[i].x <= player.x + 15 && player.x <= balls[i].x + 15 &&
      balls[i].y <= player.y + 15 && player.y <= balls[i].y + 15 &&
      ((player.x - balls[i].x) * (player.x - balls[i].x) + (player.y - balls[i].y) * (player.y - balls[i].y)) <= 225)
    {
      max_score = Math.max(max_score, score);
      max_level = Math.max(max_level, level);

      if (PlaySounds)
      {
        PlaySound(Theme[CurrentTheme].EndGameSoundPath);
      }

      ResetForRestart();
      UpdateScores();

      // HACK   for window.setTimeout workaround on Mac
      // force RemainingTime to be less than zero
      RemainingTime = -1;

      if (!PromptPlayAgain())
      {
        return;
      }
    }
  }

  ArenaCtx.stroke();
  CtrlPadCtx.stroke();

  if (inc_score > 5.5)
  {
    inc_score -= 0.2;
  }
}

function UpdateFinishTime()
{
  // allow 4s + 0.15s/level
  FinishTime = new Date();
  FinishTime.setTime(FinishTime.getTime() + 4*1000 + (level - 1)*150);
}

function PromptPlayAgain()
{
  jConfirm("Do you want to play again?", "Confirm",
    function(r)
    {
      if (r)
      {
        ResetForRestart();

        // add a bit of buffer for clock() otherwise we can timeout
        RemainingTime = (FinishTime - (new Date()).getTime()) / 1000;
        RemainingTime = 0.5;

        window.setTimeout('clock()', 20);
      }
      else
      {
        window.location.href = "index.html";
      }
    });

  // game may be restarted by anonymous function above
  return false;
}

function ReverseBalls()
{
  if (Reverses <= 0)
  {
    return;
  }
  Reverses--;

  if (PlaySounds)
  {
    PlaySound(Theme[CurrentTheme].ReverseBallsSoundPath);
  }

  for (var i = 0; i < balls.length; i++)
  {
    balls[i].xsee = -balls[i].xsee;
    balls[i].ysee = -balls[i].ysee;
  }
}

function OnMouseMove(e)
{
  //debug.log("OnTouchMove " + e.pageX + "/" + e.pageY);

  FingerPos.x = e.offsetX;
  FingerPos.y = e.offsetY;

  player.x = e.offsetX;
  player.y = e.offsetY * GetArenaHeight() / GetControlPadHeight();

  ProcessUserMove(e);
}

function OnTouchMove(e)
{
  var CtrlPadX = e.changedTouches[0].clientX;
  var CtrlPadY = e.changedTouches[0].clientY;
  //debug.log("OnTouchMove " + CtrlPadX + "/" + CtrlPadY);

  FingerPos.x = CtrlPadX;
  FingerPos.y = CtrlPadY - GetArenaHeight();

  player.x = CtrlPadX;
  player.y = (GetArenaHeight() / GetControlPadHeight()) * (CtrlPadY - GetArenaHeight() + 23);

  ProcessUserMove(e);
}

function ProcessUserMove(e)
{
  if (!PlayerHasStartedMotion)
  {
    PlayerHasStartedMotion = true;
    PlayerMotionStartTime = (new Date()).getTime();
  }

  // check if we have hit prey
  // 10 = Prey.radius - Player.radius
  // 30 = Prey.radius + Player.radius
  if (prey.x <= player.x + 10 && player.x <= prey.x + 30 &&
      prey.y <= player.y + 10 && player.y <= prey.y + 30)
  {
    prey = new Prey(Math.random()*(GetArenaWidth() - PreyWidth), Math.random()*(GetArenaHeight() - PreyHeight));
    CreateBall();
    score += Math.floor(inc_score);
    level++;
    inc_score = 15;
    ThrobberSoundPlaying = false;
    UpdateScores();

    if (PlaySounds)
    {
      PlaySound(Theme[CurrentTheme].CompletedLevelSoundPath);
    }

    // only get a reverse every so often
    if (level % 10 == 0)
    {
      Reverses++;
    }

    if (balls.length == 1)
    {
      StartTime = (new Date()).getTime();
    }

    UpdateFinishTime();
    UpdateScores();
  }

  e.preventDefault();
}

function initCanvas(canvas)
{
  if (window.G_vmlCanvasManager && window.attachEvent && !window.opera)
  {
    canvas = window.G_vmlCanvasManager.initElement(canvas);
  }

  return canvas;
}

function ThemeInfo(name, playerImgPath, ballImgPath, preyImgPath)
{
  this.Name = name;
  this.PlayerImagePath = playerImgPath;
  this.BallImagePath = ballImgPath;
  this.PreyImagePath = preyImgPath;

  this.ThrobberSoundPath = SoundsDirectory + "33723__jobro__1_alarm_long_a.mp3";
  this.EndGameSoundPath = SoundsDirectory + "3045__starpause__k9dhhNoiseCym.mp3";
  this.ReverseBallsSoundPath = SoundsDirectory + "30184__DJ_Chronos__Robot_Foot_Loop_2.mp3";
  this.CompletedLevelSoundPath = SoundsDirectory + "95043__Robinhood76__01600_robotics_button.mp3";
}

function InitThemes()
{
  // TODO   themes
  Theme[0] = new ThemeInfo("Default", "Player/runner_simple_small_black.png","Ball/Cirblue.png", "Prey/go-home.png");
  Theme[1] = new ThemeInfo("MacVsPc", "TODO","TODO", "TODO");
  Theme[2] = new ThemeInfo("LinuxVsWindows", "TODO","TODO", "TODO");
  Theme[3] = new ThemeInfo("Pirate", "TODO","TODO", "TODO");
  Theme[4] = new ThemeInfo("Navy", "Player/Contship.png","Ball/Kidnapper.png", "Prey/Port.png");
  Theme[5] = new ThemeInfo("Zombie", "TODO","TODO", "TODO");
  Theme[6] = new ThemeInfo("DrWho", "TODO","TODO", "TODO");
  Theme[7] = new ThemeInfo("BeautyGeek", "TODO","TODO", "TODO");
}

