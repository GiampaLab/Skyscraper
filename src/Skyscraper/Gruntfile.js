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
                src: ['./wwwroot/dist/**/*.{js,css}']
            }
        },
        
        concat: {
            options: {
                separator: ';'
            },
            dist: {
                src: ['./wwwroot/**/*.js'],
                dest: './wwwroot/dist/<%= pkg.name %>.js'
            }
        },

        cssmin: {
            target: {
                files: [{
                    expand: true,
                    cwd: './wwwroot\content',
                    src: ['*.css', '!*.min.css'],
                    dest: './wwwroot\dist\content',
                    ext: '.min.css'
                }]
            }
        },
        
        useminPrepare: {
            html: './wwwroot/index.html'
        },

        usemin: {
            html: ['./wwwroot/dist/index.html']
        },

        uglify: {
            options: {
                report: 'min',
                mangle: false
            }
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
