var appRoot = 'src/';
var outputRoot = 'wwwroot/dist/';
var exportSourceRoot = 'wwwroot/';
var exporSrvtRoot = 'export/';

module.exports = {
    root: appRoot,
    source: appRoot + '**/*.ts',
    html: appRoot + '**/*.html',
    css: appRoot + '**/*.css',
    style: 'styles/**/*.css',
    output: outputRoot,
    exportSourceRoot: exportSourceRoot,
    exportSrv: exporSrvtRoot,
    doc: './doc',
    dtsSrc: [
      'typings/globals/**/*.d.ts',
      'typings/custom/**/*.d.ts',
      './wwwroot/jspm_packages/**/*.d.ts'
    ]
}
