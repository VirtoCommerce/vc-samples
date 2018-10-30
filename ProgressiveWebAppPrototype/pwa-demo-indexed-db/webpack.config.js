let path = require('path');
let CopyWebpackPlugin = require('copy-webpack-plugin');

module.exports = {
    entry: './sources/scripts/main.js',
    mode: 'development',
    output: {
        filename: 'bundle.js',
        path: path.resolve(__dirname, 'dist')
    },
    plugins: [
        new CopyWebpackPlugin(
            [
                { from: 'sources/images/*', to: 'images/[name].[ext]', toType: 'template' },
                { from: 'sources/styles/vendor/*', to: 'styles/vendor/[name].[ext]', toType: 'template' },
                { from: 'sources/styles/*', to: 'styles/[name].[ext]', toType: 'template' },
                { from: 'sources/index.html', to: 'index.html', toType: 'template' },
                { from: 'sources/service-worker.js', to: 'service-worker.js', toType: 'template' },
            ],
            {
                flatten: true
        }   )
    ]
};