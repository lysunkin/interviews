package com.acme;

import static org.junit.Assert.assertEquals;

public class TracerTest {

    @org.junit.Test
    public void getLatency() {
        Tracer tracer = new Tracer();
        tracer.parseData("AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7");
        assertEquals("9", tracer.getLatency(new String[]{"A", "B", "C"}));
        assertEquals("5", tracer.getLatency(new String[]{"A", "D"}));
        assertEquals("13", tracer.getLatency(new String[]{"A", "D", "C"}));
        assertEquals("22", tracer.getLatency(new String[]{"A", "E", "B", "C", "D"}));
        assertEquals("NO SUCH TRACE", tracer.getLatency(new String[]{"A", "E", "D"}));
    }

    @org.junit.Test
    public void getNumTracesMaxHops() {
        Tracer tracer = new Tracer();
        tracer.parseData("AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7");
        assertEquals(2, tracer.getNumTracesMaxHops("C", "C", 3));
    }

    @org.junit.Test
    public void getNumTracesExactHops() {
        Tracer tracer = new Tracer();
        tracer.parseData("AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7");
        assertEquals(3, tracer.getNumTracesExactHops("A", "C", 4));
    }

    @org.junit.Test
    public void getShortestTrace() {
        Tracer tracer = new Tracer();
        tracer.parseData("AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7");
        assertEquals(9, tracer.getShortestTrace("A", "C"));
        assertEquals(9, tracer.getShortestTrace("B", "B"));
    }

    @org.junit.Test
    public void getNumTracesMaxLatency() {
        Tracer tracer = new Tracer();
        tracer.parseData("AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7");
        assertEquals(7, tracer.getNumTracesMaxLatency("C", "C", 30));
    }

    @org.junit.Test
    public void parseData() {
        Tracer tracer = new Tracer();
        tracer.parseData("AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7");
        assertEquals("[A, B, C, D, E]", tracer.getRootNodes());
    }
}
