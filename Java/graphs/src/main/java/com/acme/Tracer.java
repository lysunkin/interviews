package com.acme;

import javafx.util.Pair;

import java.util.*;

public class Tracer {

    private final int MAX_HOPS = 10;

    Comparator<Pair<List<String>, Integer>> latencyComparator = Comparator.comparing(Pair::getValue);

    // it can be a useful comparator, but not at this moment
    Comparator<Pair<List<String>, Integer>> numHopsComparator = (o1, o2) -> {
        if (o1.getKey().size() == o2.getKey().size())
            return 0;
        else if (o1.getKey().size() > o2.getKey().size())
            return 1;
        else
            return -1;
    };

    // node names are strings due to the possibility of using descriptive (not single letter) names
    private Map<String, List<Pair<String, Integer>>> graph;

    public String getLatency(String[] nodes) {
        int latency = getLatencyRecursive(nodes, 0);

        if (latency != -1)
            return Integer.toString(latency);
        else
            return "NO SUCH TRACE";
    }

    private int getLatencyRecursive(String[] nodes, int current) {

        // if we reached the end of list
        if (current == nodes.length - 1)
            return 0;

        if (graph.containsKey(nodes[current])) {
            for (Pair<String, Integer> p : graph.get(nodes[current])) {
                if (nodes[current + 1].equals(p.getKey())) {
                    int lat = getLatencyRecursive(nodes, current + 1);
                    if (lat == -1)
                        return -1;
                    return p.getValue() + lat;
                }
            }
        }

        // no connected nodes found
        return -1;
    }

    public int getNumTracesMaxHops(String start, String end, int maxHops) {

        List<Pair<List<String>, Integer>> pathsFound = new ArrayList<>();
        findPathsRecursive(start, end, 0, 0, maxHops, Integer.MAX_VALUE, new Stack<>(), pathsFound);

        Debugger.log(pathsFound);

        return pathsFound.size();
    }

    public int getNumTracesExactHops(String start, String end, int hops) {
        List<Pair<List<String>, Integer>> pathsFound = new ArrayList<>();
        findPathsRecursiveExactHops(start, end, 0, 0, hops, Integer.MAX_VALUE, new Stack<>(), pathsFound);

        Debugger.log(pathsFound);
        return pathsFound.size();
    }

    public int getShortestTrace(String start, String end) {
        List<Pair<List<String>, Integer>> pathsFound = new ArrayList<>();

        findShortestPathsRecursive(start, end, 0, 0, MAX_HOPS, new Stack<>(), pathsFound);

        Collections.sort(pathsFound, latencyComparator);

        Debugger.log(pathsFound);

        if (pathsFound.size() > 0)
            return pathsFound.get(0).getValue();
        else
            return 0;
    }

    public int getNumTracesMaxLatency(String start, String end, int maxLatency) {
        List<Pair<List<String>, Integer>> pathsFound = new ArrayList<>();
        findPathsRecursive(start, end, 0, 0, Integer.MAX_VALUE, maxLatency, new Stack<>(), pathsFound);

        Collections.sort(pathsFound, latencyComparator);

        Debugger.log(pathsFound);
        return pathsFound.size();
    }

    public void parseData(String data) {

        graph = new HashMap<>();

        for (String part : data.split("\\,")) {
            String p = part.trim();
            if (p.length() > 2) {
                String start = p.substring(0, 1);
                String end = p.substring(1, 2);
                String lat = p.substring(2);
                int latency;
                try {
                    latency = Integer.parseInt(lat);
                } catch (NumberFormatException ex) {
                    // TODO: it is still a disputable question if incorrect data should be ignored or the whole process of loading should be aborted
                    // SL: now just ignore incorrect item
                    continue;
                }

                if (graph.containsKey(start)) {
                    graph.get(start).add(new Pair<>(end, latency));
                } else {
                    List<Pair<String, Integer>> l = new ArrayList<>();
                    l.add(new Pair<>(end, latency));
                    graph.put(start, l);
                }
            }
        }
    }

    public String getRootNodes() {
        return graph.keySet().toString();
    }

    public void findShortestPathsRecursive(String start, String end, int currentHops, int currentLatency, int maxHops, Stack<String> currentPath, List<Pair<List<String>, Integer>> pathsFound) {
        if (!graph.containsKey(start))
            return;

        currentPath.add(start);

        // need to limit recursion
        if (currentHops > maxHops)
            return;

        if (start.equals(end) && currentHops != 0) {
            pathsFound.add(new Pair<>((Stack<String>) currentPath.clone(), currentLatency));
            currentPath.pop();
            return;
        } else {
            for (Pair<String, Integer> successor : graph.get(start)) {
                findShortestPathsRecursive(successor.getKey(), end, currentHops+1, currentLatency + successor.getValue(), maxHops, currentPath, pathsFound);
            }
        }

        currentPath.pop();
    }

    public void findPathsRecursive(String start, String end, int currentHops, int currentLatency, int maxHops, int maxLatency, Stack<String> currentPath, List<Pair<List<String>, Integer>> pathsFound) {
        if (!graph.containsKey(start))
            return;

        currentPath.add(start);

        if (currentHops > maxHops) {
            currentPath.pop();
            return;
        }

        if (currentLatency >= maxLatency) {
            currentPath.pop();
            return;
        }

        if (start.equals(end) && currentHops != 0) {
            pathsFound.add(new Pair<>((Stack<String>) currentPath.clone(), currentLatency));
        }

        for (Pair<String, Integer> successor : graph.get(start)) {
            findPathsRecursive(successor.getKey(), end, currentHops + 1, currentLatency + successor.getValue(), maxHops, maxLatency, currentPath, pathsFound);
        }
        currentPath.pop();
    }

    public void findPathsRecursiveExactHops(String start, String end, int currentHops, int currentLatency, int exactHops, int maxLatency, Stack<String> currentPath, List<Pair<List<String>, Integer>> pathsFound) {
        if (!graph.containsKey(start))
            return;

        currentPath.add(start);

        if (currentHops > exactHops) {
            currentPath.pop();
            return;
        }

        if (currentLatency >= maxLatency) {
            currentPath.pop();
            return;
        }

        if (start.equals(end) && currentHops == exactHops) {
            pathsFound.add(new Pair<>((Stack<String>) currentPath.clone(), currentLatency));
        }

        for (Pair<String, Integer> successor : graph.get(start)) {
            findPathsRecursiveExactHops(successor.getKey(), end, currentHops + 1, currentLatency + successor.getValue(), exactHops, maxLatency, currentPath, pathsFound);
        }

        currentPath.pop();
    }

    // debugging method for playing with functionality
    public void findPaths(String start, String end, int maxHops, int maxLatency) {
        List<Pair<List<String>, Integer>> pathsFound = new ArrayList<>();
        findPathsRecursive(start, end, 0, 0, maxHops, maxLatency, new Stack<>(), pathsFound);
        //Collections.sort(pathsFound, latencyComparator);
        //Collections.sort(pathsFound, numHopsComparator);
        System.out.println(pathsFound);
    }
}
