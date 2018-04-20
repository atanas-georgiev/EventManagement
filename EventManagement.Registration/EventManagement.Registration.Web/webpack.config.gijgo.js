const path = require('path');
const webpack = require('webpack');
const merge = require('webpack-merge');
const CheckerPlugin = require('awesome-typescript-loader').CheckerPlugin;
const treeShakableModules = [
];
const nonTreeShakableModules = ['gijgo-angular-wrappers'];
const allModules = treeShakableModules.concat(nonTreeShakableModules);

module.exports = (env) => {
    const isDevBuild = !(env && env.prod);
    const sharedConfig = {
        stats: { modules: false },
        resolve: { extensions: [ '.js', '.ts' ] },
        module: {
            rules: [
                { test: /\.ts$/, use: 'awesome-typescript-loader?silent=true' },
            ]
        },
        output: {
            publicPath: 'dist/',
            filename: '[name].js',
            library: '[name]_[hash]'
        },
        plugins: [new CheckerPlugin()]
    };

    const clientBundleConfig = merge(sharedConfig, {
        entry: {
            // To keep development builds fast, include all vendor dependencies in the vendor bundle.
            // But for production builds, leave the tree-shakable ones out so the AOT compiler can produce a smaller bundle.
            gijgo: isDevBuild ? allModules : nonTreeShakableModules
        },
        output: { path: path.join(__dirname, 'wwwroot', 'dist') },
        plugins: [
            new webpack.DllPlugin({
                path: path.join(__dirname, 'wwwroot', 'dist', '[name]-manifest.json'),
                name: '[name]_[hash]'
            })
        ].concat(isDevBuild ? [] : [
            new webpack.optimize.UglifyJsPlugin()
        ])
    });

    return [clientBundleConfig];
}
