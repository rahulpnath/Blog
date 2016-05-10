'use strict';

var gulp = require('gulp'),
    gutil = require('gulp-util'),
    babelify = require('babelify'),
    browserify = require('browserify'),
    source = require('vinyl-source-stream'),
    connect = require('gulp-connect'),
    del = require('del'),
    lint = require('gulp-eslint'),
    open = require('gulp-open');


var path = {
    HTML: './src/*.html',
    DEST: './dist/',
    JS: './src/**/*.js',
    MAINJS: './src/main.js'
};

var appConfig = {
    localBaseUrl: 'http://localhost',
    port: 8090,
    paths : path
}

gulp.task('copyHtmlFiles', function () {
    gulp.src(path.HTML)
        .pipe(gulp.dest(path.DEST))
        .pipe(connect.reload());

});

gulp.task('open', function(){
        gulp.src(path.DEST + 'Index.html')
        .pipe(open({uri : appConfig.localBaseUrl + ':'+appConfig.port + '/'}));
});

gulp.task('lint', function () {
    gulp.src(path.JS)
        .pipe(lint({ config: 'eslint.config.json' }))
        .pipe(lint.format())
});

gulp.task('js', function () {
    browserify(path.MAINJS, { debug: true })
        .transform(babelify, { presets: ['react', 'es2015'] })
        .bundle()
        .on('error', console.error.bind(console))
        .pipe(source('main.js'))
        .pipe(gulp.dest(path.DEST))
        .pipe(connect.reload());;

});

gulp.task('connect', function () {
    connect.server({
        root: 'dist',
        livereload: true,
        port: appConfig.port
    });
});

gulp.task('reload', function () {
    gulp.src('dist/**/*').pipe(connect.reload());
});

gulp.task('build', ['copyHtmlFiles', 'js']);

gulp.task('watch', function () {
    gulp.watch(path.HTML, ['copyHtmlFiles']);
    gulp.watch(path.JS, ['js']);
});

gulp.task('clean', function () {
    del([path.DEST]);
});
gulp.task('default', ['build', 'connect', 'lint', 'open', 'watch']);