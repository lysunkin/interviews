package com.acme;

//
// find the way into the maze and return true if path between two points exists
//
public class Main {

    int[][] visited;
    int[][] field;
    int fieldSize;

    int startX;
    int startY;
    int endX;
    int endY;

    public Main() {
        init();
    }

    public void init() {
        fieldSize = 6;

        startX = 0;
        startY = 0;
        endX = 5;
        endY = 5;

        field = new int[][] {
                {1, 0, 0, 0, 1, 0},
                {1, 1, 1, 1, 1, 0},
                {0, 1, 0, 0, 1, 1},
                {1, 1, 1, 0, 0, 1},
                {1, 0, 1, 1, 0, 1},
                {0, 0, 0, 0, 0, 1}
        };

        visited = new int[][] {
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0}
        };
    }

    private boolean step(int x, int y) {
        if (x < 0 || y < 0 || x >= fieldSize || y >= fieldSize)
            return false;

        if (visited[x][y] == 1)
            return false;

        if (field[x][y] != 1)
            return false;

        visited[x][y] = 1;

        System.out.println((x+1) + ":" + (y+1));

        if (x == endX && y == endY)
            return true;

        return step(x + 1, y) || step(x, y+1) || step(x-1, y) || step(x, y-1);
    }

    public boolean run() {
        return step(startX, startY);
    }

    public static void main(String[] args) {
        Main runner = new Main();
	    System.out.println("Path exists: " + runner.run());
    }
}
