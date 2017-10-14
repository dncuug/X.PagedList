var gulp = require("gulp"),
    uglify = require("gulp-uglify"),
    rename = require('gulp-rename');

gulp.task("minifyJS", function () {
    gulp.src(["jquery-ajax-unobtrusive.js"], { base: "." })
        .pipe(uglify())
        .pipe(rename({suffix: '.min'}))
        .pipe(gulp.dest("."));
});

gulp.task("default", ["minifyJS"]);