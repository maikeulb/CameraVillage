"use strict"
{
    // Required to form a complete output path
    let path = require('path');

    // Plagin for cleaning up the output folder (bundle) before creating a new one
    const CleanWebpackPlugin = require('clean-webpack-plugin');

    // Path to the output folder
    const bundleFolder = "wwwroot/bundle/";

    module.exports = {
        // Application entry point
        entry: "./Scripts/main.ts",

        // Output file
        output: {
            filename: 'script.js',
            path: path.resolve(__dirname, bundleFolder)
        },
        module: {
          rules: [
            {
              test: /\.tsx?$/,
              loader: "ts-loader",
              exclude: /node_modules/,
            },
          ]
        },
        resolve: {
            extensions: [".tsx", ".ts", ".js"]
        },
        plugins: [
            new CleanWebpackPlugin([bundleFolder])
        ],
        // Include the generation of debugging information within the output file
        // (Required for debugging client scripts)
        devtool: "inline-source-map"
    };
}
1
2
3
4
5
6
7
8
9
10
11
12
13
14
15
16
17
18
19
20
21
22
23
24
25
26
27
28
29
30
31
32
33
34
35
36
37
38
39
40
"use strict"
{
    // Required to form a complete output path
    let path = require('path');
 
    // Plagin for cleaning up the output folder (bundle) before creating a new one
    const CleanWebpackPlugin = require('clean-webpack-plugin');
 
    // Path to the output folder
    const bundleFolder = "wwwroot/bundle/";
 
    module.exports = {
        // Application entry point
        entry: "./Scripts/main.ts",
 
        // Output file
        output: {
            filename: 'script.js',
            path: path.resolve(__dirname, bundleFolder)
        },
        module: {
          rules: [
            {
              test: /\.tsx?$/,
              loader: "ts-loader",
              exclude: /node_modules/,
            },
          ]
        },
        resolve: {
            extensions: [".tsx", ".ts", ".js"]
        },
        plugins: [
            new CleanWebpackPlugin([bundleFolder])
        ],
        // Include the generation of debugging information within the output file
        // (Required for debugging client scripts)
        devtool: "inline-source-map"
    };
}
