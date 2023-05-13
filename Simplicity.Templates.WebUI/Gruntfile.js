/// <binding ProjectOpened='compile-tailwindcss' />
module.exports = function (grunt) {
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        clean: ['wwwroot/lib/*', 'wwwroot/bundles/*'],
        postcss: {
            options: {
                map: true,
                processors: [
                    require('tailwindcss')(),
                    require('autoprefixer')({ overrideBrowserslist: 'last 2 versions' }), // add vendor prefixes      
                    require('cssnano')()
                ]
            },
            dist: {
                expand: true,
                cwd: 'wwwroot/css/',
                src: ['**/*.css'],
                dest: 'wwwroot/bundles/', 
                ext: '.css'
            }
        },
        watch: {
            postcss: {
                files: 'wwwroot/css/**/*.css',
                tasks: ['compile-tailwindcss'],
                options: {
                    interupt: true
                }
            }
        }
    });

    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks("grunt-contrib-clean");
    grunt.loadNpmTasks('@lodder/grunt-postcss');

    grunt.registerTask('compile-tailwindcss', ['clean', 'postcss'])
    grunt.registerTask('watch-tailwindcss', ['watch:postcss'])
};