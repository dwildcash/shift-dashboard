var gulp = require('gulp');
var del = require('del');

// Node_modules deps
var deps = {
    "@aspnet": {
        "signalr/dist/browser/signalr.min.js": "js"
    },
    "admin-lte": {
        "/dist/css/adminlte.min.css": "css",
        "/dist/js/adminlte.min.js": "js"
    },
    "bootstrap": {
        "/dist/css/bootstrap.min.css": "css",
        "/dist/js/bootstrap.bundle.min.js": "js"
    },
    "chart.js": {
        "/dist/Chart.min.js": "js"
    },      
    "@fortawesome/": {
        "/fontawesome-free/webfonts/*": "webfonts",
        "/fontawesome-free/css/all.css": "css",
        "/fontawesome-free/js/all.js": "js"
    },
    "pe7-icon": {
        "/dist/fonts/*": "fonts",
        "/dist/dist/pe-icon-7-stroke.min.css": "css"
    },
    "ionicons": {
        "/dist/fonts/*": "fonts",
        "/dist/css/ionicons.min.css": "css"
    },
    "bootstrap-table": {
        "/dist/bootstrap-table.min.js": "js",
        "/dist/extensions/mobile/bootstrap-table-mobile.js": "js",
        "/dist/bootstrap-table.css": "css"
    },
    "pace-js": {
        "/pace.min.js": "js",
        "/themes/blue/pace-theme-minimal.css": "css"
    },
    "jquery": {
        "/dist/jquery.min.js": "js"
    },
    "jquery-knob": {
        "/dist/jquery.knob.min.js": "js"
    },
    "jquery-sparkline": {
        "/jquery.sparkline.min.js": "js"
    },
    "jquery-ui-touch-punch": {
        "jquery.ui.touch-punch.min.js": "js"
    },
    "jquery-ui-dist": {
        "/jquery-ui.min.js": "js",
        "/jquery-ui.min.css": "css"
    }
};

gulp.task('clean', function (done) {
    return del([
        'wwwroot/js/**/*',
        'wwwroot/fonts/**/*',
        'wwwroot/css/**/*',
        'wwwroot/json/**/*',
        '!wwwroot/css/jquery-ui-autocomplete.css',
        '!wwwroot/js/jquery-jvectormap-world-mill.js',
        '!wwwroot/js/NotificationHub.js',
        '!wwwwroot/js/NotificationUpdateTitle.js',
        '!wwwroot/css/skin-midnight.min.css',
        '!wwwroot/doc/**/*'
    ], done);
});

// Copy required scripts
gulp.task("scripts", function (cb) {
    var streams = [];

    for (var prop in deps) {
        console.log("Prepping wwwroot for: " + prop);
        for (var itemProp in deps[prop]) {
            streams.push(gulp.src("node_modules/" + prop + "/" + itemProp)
                .pipe(gulp.dest("wwwroot/" + deps[prop][itemProp])));
        }
    }

    cb();
});

gulp.task("default", gulp.series('clean', 'scripts'));