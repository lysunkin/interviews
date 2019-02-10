package com.acme;

import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Paths;

public class Main {

    public static void main(String[] args) {
        // parse command line
        if (args.length == 0) {
            System.out.println("Please specify input file");
            System.exit(-1);
        }

        String text = "";
        try {
            text = new String(Files.readAllBytes(Paths.get(args[0])), StandardCharsets.UTF_8);
        } catch (IOException e) {
            System.out.println("Error reading file " + args[0]);
            System.exit(-1);
        }

        // load input file
        Tracer tracer = new Tracer();
        tracer.parseData(text);

        // testing methods
        System.out.println("#1: " + tracer.getLatency(new String[]{"A", "B", "C"}));
        System.out.println("#2: " + tracer.getLatency(new String[]{"A", "D"}));
        System.out.println("#3: " + tracer.getLatency(new String[]{"A", "D", "C"}));
        System.out.println("#4: " + tracer.getLatency(new String[]{"A", "E", "B", "C", "D"}));
        System.out.println("#5: " + tracer.getLatency(new String[]{"A", "E", "D"}));

        System.out.println("#6: " + tracer.getNumTracesMaxHops("C", "C", 3));
        System.out.println("#7: " + tracer.getNumTracesExactHops("A", "C", 4));
        System.out.println("#8: " + tracer.getShortestTrace("A", "C"));
        System.out.println("#9: " + tracer.getShortestTrace("B", "B"));

        System.out.println("#10: " + tracer.getNumTracesMaxLatency("C", "C", 30));
    }
}
