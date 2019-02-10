package com.acme;

public class Main {

    //
    // task:
    // transform string like "please sit one. sit, please one, one sit please."
    // into "please, sit, one. sit, please, one, one, two please."
    // recursively find words with comma after and apply commas on all other words
    // then find all words with comma in front and replicate commas to other occurrences.
    //

    public static void main(String[] args) {

        String example = "please sit one. sit, please one, one two please.";

        System.out.println("initial:  " + example);
        String result = process(example);
        System.out.println("result:   " + result);
        System.out.println("Equals to expected: " + "please, sit, one. sit, please, one, one, two please.".equals(result));
    }

    private static String process(String input) {
        String[] tokens = input.split(" ");

        boolean changed = true;

        while (changed) {
            changed = false;
            for (int i = 0; i < tokens.length; i++) {
                String token = tokens[i];
                if (token.endsWith(",")) {
                    String toSearch = token.replace(",", "");
                    for (int j = 0; j < tokens.length; j++) {
                        String inner = tokens[j];
                        if (inner.equals(toSearch)) {
                            tokens[j] = token;
                            changed = true;
                        }
                    }

                    if (i != 0 && !tokens[i-1].endsWith(",") && !tokens[i-1].endsWith(".")) {
                        tokens[i-1] = tokens[i-1] + ",";
                        changed = true;
                    }
                }
            }
        }

        return String.join(" ", tokens);
    }
}
