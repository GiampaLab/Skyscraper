/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {

    // Project configuration.
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),

        clean: ["./wwwroot/dist/", '.tmp'],

        copy: {
            main: {
                expand: true,
                cwd: './wwwroot/',
                src: ['**', '!**/*.css'],
                dest: './wwwroot/dist/'
            }
        },

        rev: {
            files: {
                src: ['./wwwroot/dist/css/*.css', './wwwroot/dist/js/*.js']
            }
        },
                
        useminPrepare: {
            html: './wwwroot/index.html',
            flow: {
                html: {
                    steps: {
                        js: ['concat', 'uglify', 'rev'],
                        css: ['concat', 'cssmin', 'rev']
                    }
                }
            },
            options: {
                dest: './wwwroot/dist/'
            }
        },

        usemin: {
            html: ['./wwwroot/dist/index.html']
        },
        
        ngconstant: {
            options: {
                name: 'config',
                dest: './wwwroot/config.js'
            },
            local: {
                constants: {
                    constants: grunt.file.readJSON('./configuration.local.json')
                }
            },
            dev: {
                constants: {
                    constants: grunt.file.readJSON('./configuration.dev.json')
                }
            },
            prod: {
                constants: {
                    constants: grunt.file.readJSON('./configuration.production.json')
                }
            },
            build: {
            }
        }
    });

    grunt.loadNpmTasks('grunt-ng-constant');
    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-rev');
    grunt.loadNpmTasks('grunt-usemin');

    // Tell Grunt what to do when we type "grunt" into the terminal
    grunt.registerTask('deployDev', ['clean', 'ngconstant:dev', 'copy', 'useminPrepare', 'concat', 'uglify', 'cssmin', 'rev', 'usemin']);
    grunt.registerTask('deployProd', ['clean', 'ngconstant:prod', 'copy', 'useminPrepare', 'concat', 'uglify', 'cssmin', 'rev', 'usemin']);
};
