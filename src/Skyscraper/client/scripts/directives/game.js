'use strict';

var app = angular.module('skyscraper');

app.directive('game',['Hub', 'constants',
	function(Hub, constants){
		var ddo = {
			restrict: 'A',
			link: function(scope, element, attr){

			    scope.gameStarted = false;

			    scope.gameOver = false;

				scope.players = [];

				var hubInited = false;

				var needLogin = false;

				var loginInfo;

				scope.hub = new Hub('skyscraperHub', {

					listeners:{
					    'start': function (symbols, extractedCardSymbols, players) {
					        scope.gameOver = false;
			            	scope.gameStarted = true;
			            	scope.currentCard = symbols;
			            	scope.extractedCard = extractedCardSymbols;
			            	setPlayerPoints(players);
			            	scope.$apply();
			            },
			            'setExtractedCard': function(symbols, players){
			            	scope.extractedCard = symbols;
			            	setPlayerPoints(players);
			            	scope.$apply();
			            },
			            'gameOver': function (gameStats) {
			                scope.gameStats = gameStats;
			                scope.gameStarted = false;
			                scope.gameOver = true;
		            		scope.$apply();
			            },
			            'setPlayers': function(players){
			            	scope.players = players;
			            	scope.$apply();
			            },
			            'joinGame': function(currentlyExtractedCard, playerCurrentCard){
			            	scope.extractedCard = currentlyExtractedCard;
			            	scope.currentCard = playerCurrentCard;
			            	scope.gameStarted = true;
			            	scope.gameOver = false;
			            	scope.$apply();
			            }
					},
					//server side methods
		        	methods: ['extractCard', 'cardMatched',  'startGame', 'addPlayer'],
		        	rootPath: constants.signalREndpoint
				});

				scope.hub.promise.done(function(result){
					hubInited = true;
					if(needLogin){
						execLogin(loginInfo);
					}
				});

                scope.startGame = function(){
                	scope.hub.startGame(8);
                }

                scope.select = function(symbol){
                    if (_.contains(_.pluck(scope.currentCard, 'id'), symbol.id) && !scope.gameOver) {
                        if (!angular.isUndefined(scope.extractedCard) && scope.extractedCard !== null) {
                            scope.currentCard = scope.extractedCard;
                        }
                		scope.hub.cardMatched(_.pluck(scope.extractedCard,'id'), scope.currentPlayer.id);
                	}
                }

                scope.$on('login', function(event, info){
                	if(hubInited){
	                	scope.$apply(function(){
	                		execLogin(info);
	                	});
                	}
                	else{
                		loginInfo = info;
                		needLogin = true;
                	}
                });

                scope.$on('logout', function () {
                    scope.login = false;
                    scope.players = _.reject(scope.players, function (p) {
                        return p.displayName = scope.currentPlayer.displayName;
                    });
                });

                function execLogin(info){
                	scope.currentPlayer = {displayName: info.displayName, imageUrl: info.image.url, id: info.id};
            		scope.login = true;
            		scope.hub.addPlayer({displayName: info.displayName, imageUrl: info.image.url, id: info.id});
                }

                function setPlayerPoints(players) {
                    angular.forEach(players, function (p) {
                        var player = _.find(scope.players, function (pl) {
                            return pl.id === p.id;
                        })
                        player.points = p.points;
                    });
                }
			}
		}
		return ddo;
}]);