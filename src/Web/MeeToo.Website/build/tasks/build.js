var gulp = require('gulp');
var runSequence = require('run-sequence');
var changed = require('gulp-changed');
var plumber = require('gulp-plumber');
var paths = require('../paths');
var assign = Object.assign || require('object.assign');
var notify = require('gulp-notify');
var typescript = require('gulp-tsb');
var exec = require('child_process').exec;


var typescriptCompiler = typescriptCompiler || null;
gulp.task('build-system', function() {
  if(!typescriptCompiler) {
    typescriptCompiler = typescript.create(require('../../tsconfig.json').compilerOptions);
  }

  return gulp.src(paths.dtsSrc.concat(paths.source))
    .pipe(plumber())
   
    .pipe(typescriptCompiler())
    .pipe(gulp.dest(paths.output));
});

// copies changed html files to the output directory
gulp.task('build-html', function() {
  return gulp.src(paths.html)
    .pipe(changed(paths.output, {extension: '.html'}))
    .pipe(gulp.dest(paths.output));
});

// copies changed css files to the output directory
gulp.task('build-css', function() {
  return gulp.src(paths.css)
    .pipe(changed(paths.output, {extension: '.css'}))
    .pipe(gulp.dest(paths.output));
});

// runs jspm install from within Gulp
gulp.task('build-jspm', function () {
    exec('jspm install', function (err, stout, stderr) {
    });
});

gulp.task('build', function(callback) {
  return runSequence(
    'clean',
    ['build-system', 'build-html', 'build-css'],
    callback
  );
});
