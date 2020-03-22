var kBoardWidth = 9;
var kBoardHeight= 9;
var kNumPieces = 9;
var kPieceWidth = 32;
var kPieceHeight= 32;
var kPixelWidth = 1 + (kBoardWidth * kPieceWidth);
var kPixelHeight= 1 + (kBoardHeight * kPieceHeight);

var kMaxMoveCount = 1000000;

var gCanvasElement = new Array(2);
var gDrawingBuffer = 0;
var gDrawingContext;

var gPieces;
var gNumPieces;
var gSelectedPieceIndex;
var gSelectedPieceHasMoved;
var gScoreboardElement;
var gMoveCount;
var gMoveCountElem;
var gLowestMoveCountElem;
var gLowestMoveCount = kMaxMoveCount;
var gGameInProgress;

function Cell(row, column)
{
  this.row = row;
  this.column = column;
}

function getCursorPosition(e)
{
  /* returns Cell with .row and .column properties */
  var x;
  var y;
  if (e.pageX || e.pageY)
  {
    x = e.pageX;
    y = e.pageY;
  }
  else
  {
    x = e.clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
    y = e.clientY + document.body.scrollTop + document.documentElement.scrollTop;
  }
  x -= gCanvasElement[gDrawingBuffer].offsetLeft;
  y -= gCanvasElement[gDrawingBuffer].offsetTop;
  x = Math.min(x, kBoardWidth * kPieceWidth);
  y = Math.min(y, kBoardHeight * kPieceHeight);
  var cell = new Cell(Math.floor(y/kPieceWidth), Math.floor(x/kPieceHeight));
  
  return cell;
}

function halmaOnClick(e)
{
  var cell = getCursorPosition(e);
  for (var i = 0; i < gNumPieces; i++)
  {
    if ((gPieces[i].row == cell.row) && 
        (gPieces[i].column == cell.column))
    {
      clickOnPiece(i);
      
      return;
    }
  }
  
  clickOnEmptyCell(cell);
}

function clickOnEmptyCell(cell)
{
  if (gSelectedPieceIndex == -1)
  {
    return;
  }
  
  var rowDiff = Math.abs(cell.row - gPieces[gSelectedPieceIndex].row);
  var columnDiff = Math.abs(cell.column - gPieces[gSelectedPieceIndex].column);
  if ((rowDiff <= 1) &&
      (columnDiff <= 1))
  {
    /* we already know that this click was on an empty square,
     so that must mean this was a valid single-square move */
    gPieces[gSelectedPieceIndex].row = cell.row;
    gPieces[gSelectedPieceIndex].column = cell.column;
    gMoveCount += 1;
    gSelectedPieceIndex = -1;
    gSelectedPieceHasMoved = false;
    
    drawBoard();
    
    return;
  }
  
  if ((((rowDiff == 2) && (columnDiff == 0)) ||
       ((rowDiff == 0) && (columnDiff == 2)) ||
       ((rowDiff == 2) && (columnDiff == 2))) && 
      isThereAPieceBetween(gPieces[gSelectedPieceIndex], cell))
  {
    /* this was a valid jump */
    if (!gSelectedPieceHasMoved)
    {
      gMoveCount += 1;
    }
    gSelectedPieceHasMoved = true;
    gPieces[gSelectedPieceIndex].row = cell.row;
    gPieces[gSelectedPieceIndex].column = cell.column;
    
    drawBoard();
    
    return;
  }
  
  gSelectedPieceIndex = -1;
  gSelectedPieceHasMoved = false;
  
  drawBoard();
}

function clickOnPiece(pieceIndex)
{
  if (gSelectedPieceIndex == pieceIndex)
  {
    return;
  }
  
  gSelectedPieceIndex = pieceIndex;
  gSelectedPieceHasMoved = false;
  
  drawBoard();
}

function isThereAPieceBetween(cell1, cell2)
{
  /* note: assumes cell1 and cell2 are 2 squares away
   either vertically, horizontally, or diagonally */
  var rowBetween = (cell1.row + cell2.row) / 2;
  var columnBetween = (cell1.column + cell2.column) / 2;
  for (var i = 0; i < gNumPieces; i++)
  {
    if ((gPieces[i].row == rowBetween) &&
        (gPieces[i].column == columnBetween))
    {
	    return true;
    }
  }
  
  return false;
}

function isTheGameOver()
{
  for (var i = 0; i < gNumPieces; i++)
  {
    if (gPieces[i].row > 2)
    {
	    return false;
    }
    
    if (gPieces[i].column < (kBoardWidth - 3))
    {
	    return false;
    }
  }
  
  return true;
}

function clearBoard()
{
  gDrawingContext.clearRect(0, 0, kPixelWidth, kPixelHeight);
}

function drawGrid()
{
  gDrawingContext.beginPath();
  
  // vertical lines
  for (var x = 0; x <= kPixelWidth; x += kPieceWidth)
  {
    gDrawingContext.moveTo(0.5 + x, 0);
    gDrawingContext.lineTo(0.5 + x, kPixelHeight);
  }
  
  // horizontal lines
  for (var y = 0; y <= kPixelHeight; y += kPieceHeight)
  {
    gDrawingContext.moveTo(0, 0.5 + y);
    gDrawingContext.lineTo(kPixelWidth, 0.5 +  y);
  }
}

function drawPieces()
{
  // pieces
  gDrawingContext.strokeStyle = "#ccc";
  gDrawingContext.stroke();
  
  for (var i = 0; i < 9; i++)
  {
    drawPiece(gPieces[i], i == gSelectedPieceIndex);
  }
  
  // moves
  gMoveCountElem.innerHTML = gMoveCount;
  if (gLowestMoveCount == kMaxMoveCount)
  {
    gLowestMoveCountElem.innerHTML = "???";
  }
  else
  {
    gLowestMoveCountElem.innerHTML = gLowestMoveCount;
  }
}

function drawPiece(p, selected)
{
  var column = p.column;
  var row = p.row;
  var x = (column * kPieceWidth) + (kPieceWidth/2);
  var y = (row * kPieceHeight) + (kPieceHeight/2);
  var radius = (kPieceWidth/2) - (kPieceWidth/10);
  
  gDrawingContext.beginPath();
  gDrawingContext.arc(x, y, radius, 0, Math.PI*2, false);
  gDrawingContext.closePath();
  gDrawingContext.strokeStyle = "#000";
  gDrawingContext.stroke();
  
  if (selected)
  {
    gDrawingContext.fillStyle = "#000";
    gDrawingContext.fill();
  }
}

function drawBoard()
{
  if (gGameInProgress && isTheGameOver())
  {
    endGame();
  }
  
  // reacquire screen metrics and resize/reposition visuals
  initScreenMetrics();
  sizeCanvas(0);
  sizeCanvas(1);
  initScoreboard();
  
  
  // switch to back buffer
  gDrawingBuffer = 1 - gDrawingBuffer;
  gDrawingContext = gCanvasElement[gDrawingBuffer].getContext("2d");


  // draw to back buffer
  clearBoard();  
  drawGrid();
  drawPieces();
  
  
  // flip buffers
  gCanvasElement[1 - gDrawingBuffer].style.visibility = 'hidden';
  gCanvasElement[gDrawingBuffer].style.visibility = 'visible';
  
  
  saveGameState();
}

function supportsLocalStorage()
{
  return ('localStorage' in window) && window['localStorage'] !== null;
}

function ClearLocalStorage()
{
  localStorage.clear();
}

function saveGameState()
{
  if (!supportsLocalStorage())
  {
    return false;
  }
  
  localStorage["halma.game.in.progress"] = gGameInProgress;
  for (var i = 0; i < kNumPieces; i++)
  {
    localStorage["halma.piece." + i + ".row"] = gPieces[i].row;
    localStorage["halma.piece." + i + ".column"] = gPieces[i].column;
  }
  localStorage["halma.selectedpiece"] = gSelectedPieceIndex;
  localStorage["halma.selectedpiecehasmoved"] = gSelectedPieceHasMoved;
  localStorage["halma.movecount"] = gMoveCount;
  localStorage["halma.lowestmovecount"] = gLowestMoveCount;
  
  return true;
}

function resumeGame()
{
  if (!supportsLocalStorage())
  {
    return false;
  }
  
  gGameInProgress = (localStorage["halma.game.in.progress"] == "true");
  if (!gGameInProgress)
  {
    return false;
  }
  
  gPieces = new Array(kNumPieces);
  for (var i = 0; i < kNumPieces; i++)
  {
    var row = parseInt(localStorage["halma.piece." + i + ".row"]);
    var column = parseInt(localStorage["halma.piece." + i + ".column"]);
    gPieces[i] = new Cell(row, column);
  }
  
  gNumPieces = kNumPieces;
  gSelectedPieceIndex = parseInt(localStorage["halma.selectedpiece"]);
  gSelectedPieceHasMoved = localStorage["halma.selectedpiecehasmoved"] == "true";
  gMoveCount = parseInt(localStorage["halma.movecount"]);
  gLowestMoveCount = parseInt(localStorage["halma.lowestmovecount"]);
  
  drawBoard();
  
  return true;
}

function newGame()
{
  gPieces = [new Cell(kBoardHeight - 3, 0),
             new Cell(kBoardHeight - 2, 0),
             new Cell(kBoardHeight - 1, 0),
             new Cell(kBoardHeight - 3, 1),
             new Cell(kBoardHeight - 2, 1),
             new Cell(kBoardHeight - 1, 1),
             new Cell(kBoardHeight - 3, 2),
             new Cell(kBoardHeight - 2, 2),
             new Cell(kBoardHeight - 1, 2)];
  
  gNumPieces = gPieces.length;
  gSelectedPieceIndex = -1;
  gSelectedPieceHasMoved = false;
  gMoveCount = 0;
  gGameInProgress = true;
  
  drawBoard();
}

function resetGame()
{
  newGame();
  saveGameState();
}

function endGame()
{   
  gLowestMoveCount = Math.min(gLowestMoveCount, gMoveCount);
  gSelectedPieceIndex = -1;
  gGameInProgress = false;
}

function sizeCanvas(index)
{
  gCanvasElement[index].width = kPixelWidth;
  gCanvasElement[index].height = kPixelHeight;
  
  // center canvas horizontally on screen
  gCanvasElement[index].style.left = (window.innerWidth - kPixelWidth) / 2 + "px";
}

function initCanvas(index, canvasElement)
{
  gCanvasElement[index] = canvasElement;
  sizeCanvas(index);
  gCanvasElement[index].addEventListener("click", halmaOnClick, false);
}

function initScoreboard()
{
  // move scoreboard to bottom of screen
  gScoreboardElement.style.top = kPixelHeight + "px";;
  gScoreboardElement.style.width = kPixelWidth + "px";
  
  // center scoreboard horizontally on screen
  gScoreboardElement.style.left = (window.innerWidth - kPixelWidth) / 2 + "px";
}

function initScreenMetrics()
{
  var kMoveCountElemPixelHeight = 60;
  var canvasDim = Math.min(window.innerWidth - 20, window.innerHeight - kMoveCountElemPixelHeight);
  kPieceWidth = (canvasDim - 1) / kBoardWidth;
  kPieceHeight = (canvasDim - 1) / kBoardHeight;
  kPixelWidth = 1 + (kBoardWidth * kPieceWidth);
  kPixelHeight = 1 + (kBoardHeight * kPieceHeight);
}

function initGame(canvasElement0, canvasElement1, moveCountElement, lowestMoveCountElement, scoreboardElement)
{
  initScreenMetrics();
  
  initCanvas(0, canvasElement0);
  initCanvas(1, canvasElement1);
  
  gMoveCountElem = moveCountElement;
  gLowestMoveCountElem = lowestMoveCountElement;
  gScoreboardElement = scoreboardElement;
  initScoreboard();

  gDrawingBuffer = 0;
  
  if (!resumeGame())
  {
    newGame();
  }
}
