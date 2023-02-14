module.exports = function (grunt) {
    grunt.initConfig({
        clean: ['wwwroot/lib/*']
    });

    grunt.loadNpmTasks("grunt-contrib-clean");
};