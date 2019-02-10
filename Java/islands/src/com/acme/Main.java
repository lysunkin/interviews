package com.acme;

//
// find number of islands (ones) in the ocean of zeros
//
public class Main {

    int[][] visited;
    int numIslands;

    public int getNumIslands(int[][] islands) {

        numIslands = 0;
        visited = new int[islands.length][islands[0].length];

        for (int i = 0; i < islands.length; i++) {
            for (int j = 0; j < islands[i].length; j++) {
                if (islands[i][j] == 1 && visited[i][j] != 1) {
                    numIslands++;
                    move(islands, i, j);
                }
            }
        }

        return numIslands;
    }

    public void move(int[][] islands, int x, int y) {
        if (x >= islands.length)
            return;
        if (y >= islands[x].length)
            return;
        if (islands[x][y] == 1 && visited[x][y] != 1) {
            visited[x][y] = 1;
            move(islands,x+1, y);
            move(islands,x, y+1);
        }
    }

    public static void main(String[] args) {

        int[][] islands1 = {
                {1,1,0,1},
                {1,1,0,0},
                {0,0,0,0}};

        int[][] islands2 = {
                {1,1,0,1,0,0},
                {1,1,0,0,0,1},
                {0,0,0,0,0,1}};

        System.out.println(new Main().getNumIslands(islands1));
        System.out.println(new Main().getNumIslands(islands2));
    }
}
