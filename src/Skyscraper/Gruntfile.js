/// <binding AfterBuild='deployLocal' />
/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {

    // Project configuration.
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),

        clean: ["./wwwroot/", '.tmp', "!./wwwroot/bin/"],

        copy: {
            main: {
                expand: true,
                cwd: './client/',
                src: ['**'],
                dest: './wwwroot/'
            }
        },

        rev: {
            files: {
                src: ['./wwwroot/css/*.css', './wwwroot/js/*.js']
            }
        },
                
        useminPrepare: {
            html: './client/index.html',
            flow: {
                html: {
                    steps: {
                        js: ['concat', 'uglify', 'rev'],
                        css: ['concat', 'cssmin', 'rev']
                    }
                }
            },
            options: {
                dest: './wwwroot/'
            }
        },

        usemin: {
            html: ['./wwwroot/index.html']
        },
        
        ngconstant: {
            options: {
                name: 'config',
                dest: './client/config.js'
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
    grunt.registerTask('deployLocal', ['clean', 'ngconstant:local', 'copy']);
    grunt.registerTask('deployDev', ['clean', 'ngconstant:dev', 'copy', 'useminPrepare', 'concat', 'uglify', 'cssmin', 'rev', 'usemin']);
    grunt.registerTask('deployProd', ['clean', 'ngconstant:prod', 'copy', 'useminPrepare', 'concat', 'uglify', 'cssmin', 'rev', 'usemin']);
};
