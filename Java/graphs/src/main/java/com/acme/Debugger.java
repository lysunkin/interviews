package com.acme;

public class Debugger {
    public static boolean isEnabled() {
        // set to true to enable debug output
        return false;
    }

    public static void log(Object o) {
        if (Debugger.isEnabled()) {
            System.out.println(o.toString());
        }
    }
}
