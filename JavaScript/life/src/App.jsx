import React, { useState, useEffect, useRef } from 'react';

const ROWS = 16;
const COLS = 32;
const CELL_SIZE = 20;

const GameOfLife = () => {
  const [grid, setGrid] = useState([]);
  const [isRunning, setIsRunning] = useState(false);
  const timerRef = useRef(null);

  // Create a new random grid
  const createRandomGrid = () => {
    const newGrid = Array(ROWS).fill().map(() => 
      Array(COLS).fill().map(() => Math.random() < 0.3)
    );
    return newGrid;
  };

  // Count live neighbors for a cell
  const countLiveNeighbors = (grid, row, col) => {
    let liveNeighbors = 0;
    for (let i = -1; i <= 1; i++) {
      for (let j = -1; j <= 1; j++) {
        if (i === 0 && j === 0) continue;
        
        const newRow = row + i;
        const newCol = col + j;
        
        // Check if neighbor is within grid bounds
        if (
          newRow >= 0 && newRow < ROWS && 
          newCol >= 0 && newCol < COLS && 
          grid[newRow][newCol]
        ) {
          liveNeighbors++;
        }
      }
    }
    return liveNeighbors;
  };

  // Compute next generation of the grid
  const computeNextGeneration = (currentGrid) => {
    return currentGrid.map((row, rowIndex) => 
      row.map((cell, colIndex) => {
        const liveNeighbors = countLiveNeighbors(currentGrid, rowIndex, colIndex);
        
        // Apply Game of Life rules
        if (cell) {
          // Live cell survival rules
          return liveNeighbors === 2 || liveNeighbors === 3;
        } else {
          // Dead cell revival rule
          return liveNeighbors === 3;
        }
      })
    );
  };

  // Start the game
  const startGame = () => {
    const newGrid = createRandomGrid();
    setGrid(newGrid);
    setIsRunning(true);
    
    // Set up timer to update grid every 0.5 seconds
    timerRef.current = setInterval(() => {
      setGrid(prevGrid => computeNextGeneration(prevGrid));
    }, 500);
  };

  // Stop the game
  const stopGame = () => {
    if (timerRef.current) {
      clearInterval(timerRef.current);
      timerRef.current = null;
    }
    setIsRunning(false);
  };

  // Clean up timer on component unmount
  useEffect(() => {
    return () => {
      if (timerRef.current) {
        clearInterval(timerRef.current);
      }
    };
  }, []);

  return (
    <div className="flex flex-col items-center space-y-4 p-4">
      <div className="flex space-x-4 mb-4">
        <button 
          onClick={isRunning ? stopGame : startGame}
        >
          {isRunning ? 'Stop' : 'New Life'}
        </button>
      </div>
      
      <div 
        className="grid"
        style={{
          display: 'grid',
          gridTemplateColumns: `repeat(${COLS}, ${CELL_SIZE}px)`,
          gridTemplateRows: `repeat(${ROWS}, ${CELL_SIZE}px)`,
          gap: '1px',
          border: '1px solid #ccc'
        }}
      >
        {grid.map((row, rowIndex) => 
          row.map((isAlive, colIndex) => (
            <div
              key={`${rowIndex}-${colIndex}`}
              style={{
                width: `${CELL_SIZE}px`,
                height: `${CELL_SIZE}px`,
                backgroundColor: isAlive ? 'black' : 'white',
                border: '1px solid #e0e0e0'
              }}
            />
          ))
        )}
      </div>
    </div>
  );
};

export default GameOfLife;
