
/// <binding />
/// <vs BeforeBuild='default' SolutionOpened='default:watch, watch' />
var gulp = require('gulp');
var gutil = require('gulp-util');
var del = require('del');
var concat = require('gulp-concat');
var watch = require('gulp-watch');
var uglify = require('gulp-uglify');
var cleanCSS = require('gulp-clean-css');
var changed = require('gulp-changed');
var autoprefixer = require('gulp-autoprefixer');
//var minify = require('gulp-minify');
//var browserSync = require('browser-sync');
//var imagemin = require('gulp-imagemin');
//var pngquant = require('imagemin-pngquant');


// local variables

var outputLocation = 'build';
var autoprefixerOptions = 'last 2 versions';
var vendorSources = {
    scripts: [
        'bower_components/jquery/dist/jquery.js',
	    'bower_components/jquery-validation/dist/jquery.validate.js',
	    'bower_components/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js',
        //'Scripts/site/materialize.js',
        'bower_components/izimodal/js/izimodal.js',
        //'bower_components/ladda/dist/spin.min.js',
        //'bower_components/ladda/dist/ladda.min.js',
        //'scripts/universalCssGenerator.js',
        //'bower_components/persian-rex/dist/persian-rex.js',
        'Content/admin/js/common.js',
        'Content/admin/js/uikit_custom.min.js',
        'Content/admin/js/altair_admin_common.js',
        'Content/admin/js/custom/dropify/dist/js/dropify.js',
        'scripts/admin/parsleyjs/dist/parsley.min.js',
        'scripts/admin/jquery.form.min.js',
        'Scripts/admin/jquery-ui.min.js',
        'Scripts/admin/jquery.jtable.min.js',
        'Scripts/admin/plugins_crud_table.js',
        'Scripts/site/material-scrolltop.js',
        'Scripts/site/persianjs.min.js',
        'bower_components/persian-datepicker/lib/persian-date.js',
        'bower_components/persian-datepicker/dist/js/persian-datepicker-0.4.5.js',
        'bower_components/matchHeight/jquery.matchHeight.js',
        'Scripts/admin/countUp.js/dist/countUp.js',
        'Content/admin/js/pages/dashboard.js',
        //'Scripts/jquery.yeke.js',
        //'Scripts/site/path.js',
        //'Scripts/jquery.InfiniteScroll.js',
        'scripts/admin/custom.js',
        'scripts/site.js'
    ],
    styles: [
        //'Content/site/materialize.rtl.css',
        //'Content/comments.css',
        //'Content/utils.css',
        //'Content/rtl.css',
        //'Content/responsive.css',
        //'Content/style.css',
        'content/admin/fonts.css',
        'Content/admin/skins/jquery-ui/material/jquery-ui.min.css',
        'Content/admin/skins/jtable/jtable.min.css',
        'content/admin/css/uikit.rtl.css',
        'content/admin/css/main.css',
        'content/admin/css/themes/themes_combined.min.css',
        'bower_components/font-awesome/css/font-awesome.css',
        'bower_components/persian-datepicker/dist/css/persian-datepicker-0.4.5.css',
        'content/flags.css',
        'content/site/material-scrolltop.css',
        'Content/admin/skins/dropify/css/dropify.css',
        'content/admin/css/colors.css',
        'content/admin/iziModal.css',
        'Content/site/material-scrolltop.css',
        'Content/link.css',
        'Content/uikit-addon-grid.css',
        'content/admin/custom.css',
        'Content/site.css',
    ],
    loginScripts: [
        'bower_components/jquery/dist/jquery.js',
        'bower_components/izimodal/js/izimodal.js',
        //'bower_components/ladda/dist/spin.min.js',
        //'bower_components/ladda/dist/ladda.min.js',
	    'bower_components/jquery-validation/dist/jquery.validate.js',
        'scripts/login/login.js',
    ],
    loginStyles: [
        'bower_components/izimodal/css/izimodal.css',
        //'bower_components/ladda/dist/ladda-themeless.min.css',
        'Content/login/normalize.css',
        'Content/login/login.css'
    ],
    chartScripts: [
        'bower_components/waypoints/lib/jquery.waypoints.js',
        'bower_components/highcharts/js/highcharts.js',
        //'bower_components/highcharts/modules/data.js',
        //'bower_components/highcharts/modules/exporting.js',
        //'bower_components/highcharts/modules/offline-exporting.js',
    ],
    chartStyles: [
        'bower_components/highcharts/css/highcharts.css',
    ],
    images: [
        'Content/images/*'
    ],
    fonts: [
        'fonts/*',
        'bower_components/font-awesome/fonts/*',
        'bower_components/material-design-icons/iconfont/*',
        'bower_components/vazir-font/dist/*'
    ]
};

//function bs(config) {
//    var defaultOpt = {
//        proxy: 'localhost:10209',
//        //tunnel: "kiarashpblog",
//        port: 10208, // same as local dev machine port
//        logPrefix: 'cms',
//        logLevel: 'debug',
//        logConnections: true,
//        online: true,
//        ui: {
//            port: 8080
//        }
//    };
//    var opt = config || defaultOpt;

//    browserSync(opt);
//}

// tasks

gulp.task('clean', function () {
    return del.sync([outputLocation + '/**/*.css', outputLocation + '/**/*.js']);
});

gulp.task('vendor-scripts', function () {
    return gulp.src(vendorSources.scripts)
            .pipe(changed(outputLocation + '/scripts/'))
		    .pipe(concat('scripts.min.js'))
            .pipe(uglify())
		    .pipe(gulp.dest(outputLocation + '/scripts/'));
            //.pipe(browserSync.reload({ stream: true }));
});

gulp.task('vendor-styles', function () {
    return gulp.src(vendorSources.styles)
            .pipe(changed(outputLocation + '/styles/'))
            //.pipe(autoprefixer(autoprefixerOptions))
            .pipe(cleanCSS({keepSpecialComments : 0, target : outputLocation}))
		    .pipe(concat('styles.min.css'))
		    .pipe(gulp.dest(outputLocation + '/styles/'));
            //.pipe(browserSync.reload({ stream: true }));
});

//gulp.task('login-vendor-scripts', function () {
//    return gulp.src(vendorSources.loginScripts)
//            .pipe(changed(outputLocation + '/loginscripts/'))
//		    .pipe(concat('scripts.min.js'))
//            .pipe(uglify())
//		    .pipe(gulp.dest(outputLocation + '/loginscripts/'));
//            //.pipe(browserSync.reload({ stream: true }));
//});

//gulp.task('login-vendor-styles', function () {
//    return gulp.src(vendorSources.loginStyles)
//            .pipe(changed(outputLocation + '/loginstyles/'))
//            //.pipe(autoprefixer(autoprefixerOptions))
//            .pipe(cleanCSS({keepSpecialComments : 0, target : outputLocation}))
//		    .pipe(concat('styles.min.css'))
//		    .pipe(gulp.dest(outputLocation + '/loginstyles/'));
//            //.pipe(browserSync.reload({ stream: true }));
//});

gulp.task('chart-vendor-scripts', function () {
    return gulp.src(vendorSources.chartScripts)
            .pipe(changed(outputLocation + '/chartscripts/'))
		    .pipe(concat('chart.min.js'))
            .pipe(uglify())
		    .pipe(gulp.dest(outputLocation + '/chartscripts/'));
    //.pipe(browserSync.reload({ stream: true }));
});

gulp.task('chart-vendor-styles', function () {
    return gulp.src(vendorSources.chartStyles)
            .pipe(changed(outputLocation + '/chartstyles/'))
            //.pipe(autoprefixer(autoprefixerOptions))
            .pipe(cleanCSS({ keepSpecialComments: 0, target: outputLocation }))
		    .pipe(concat('chart.min.css'))
		    .pipe(gulp.dest(outputLocation + '/chartstyles/'));
    //.pipe(browserSync.reload({ stream: true }));
});

gulp.task('images', function () {
    return gulp.src(vendorSources.images)
            //.pipe(imagemin({
            //    progressive: true,
            //    svgoPlugins: [{ removeViewBox: false }],
            //    use: [pngquant()]
            //}))
		    .pipe(gulp.dest(outputLocation + '/images/'));
});

gulp.task('fonts', function () {
    return gulp.src(vendorSources.fonts)
		    .pipe(gulp.dest(outputLocation + '/fonts/'));
});

gulp.task('watch', function () {
    //bs();
    gulp.watch(vendorSources.styles, ['vendor-styles']);
    //gulp.watch('Content/*.css', ['vendor-styles', 'login-vendor-styles']);
    //gulp.watch('Scripts/*.js', ['vendor-scripts', 'login-vendor-scripts']);
    gulp.watch('Content/*.css', ['vendor-styles']);
    gulp.watch('Scripts/*.js', ['vendor-scripts']);
    gulp.watch([vendorSources.scripts], ['vendor-scripts']);
    //gulp.watch([vendorSources.loginScripts], ['login-vendor-scripts']);
    //gulp.watch([vendorSources.loginStyles], ['login-vendor-styles']);
    gulp.watch([vendorSources.chartScripts], ['chart-vendor-scripts']);
    gulp.watch([vendorSources.chartStyles], ['chart-vendor-styles']);
});


gulp.task('default', ['clean', 'vendor-scripts', 'vendor-styles', 'chart-vendor-scripts', 'chart-vendor-styles', 'watch', 'fonts', 'images'], function () { });
