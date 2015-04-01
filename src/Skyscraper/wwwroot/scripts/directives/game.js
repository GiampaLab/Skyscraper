'use strict';

var app = angular.module('skyscraper');

app.directive('game',['Hub', 'constants',
	function(Hub, constants){
		var ddo = {
			restrict: 'A',
			link: function(scope, element, attr){

				scope.gameStarted = false;

				scope.players = [];

				var hubInited = false;

				var needLogin = false;

				var loginInfo;

				scope.hub = new Hub('skyscraperHub', {

					listeners:{
			            'start': function(symbols){
			            	scope.gameStarted = true;
			            	scope.currentCard = initSymbols(symbols);
			            	scope.$apply();
			            },
			            'setExtractedCard': function(symbols, players){
			            	if(!angular.isUndefined(scope.extractedCard) && scope.extractedCard!== null){
			            		scope.currentCard = scope.extractedCard;	
			            	}
			            	scope.extractedCard = initSymbols(symbols);
			            	angular.forEach(players, function(p){
			            		var player = _.find(scope.players, function(pl){
			            			return pl.id === p.id;
			            		})
			            		player.points = p.points;
			            	})
			            	scope.$apply();
			            },
			            'gameOver': function(gameStats){
			            	scope.gameStarted = false;
		            		scope.$apply();
			            },
			            'setPlayers': function(players){
			            	scope.players = players;
			            	scope.$apply();
			            },
			            'joinGame': function(currentlyExtractedCard, playerCurrentCard){
			            	scope.extractedCard = initSymbols(currentlyExtractedCard);
			            	scope.currentCard = initSymbols(playerCurrentCard);
			            	scope.gameStarted = true;
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
                	scope.gameStarted = true;
                	scope.hub.startGame(8);
                }

                scope.select = function(symbol){
                	if(_.contains(_.pluck(scope.currentCard,'value'), symbol.value) && !scope.gameOver){
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

                function execLogin(info){
                	scope.currentPlayer = {displayName: info.displayName, imageUrl: info.image.url, id: info.id};
            		scope.login = true;
            		scope.hub.addPlayer({displayName: info.displayName, imageUrl: info.image.url, id: info.id});
                }

                scope.$on('logout', function(){
            		scope.login = false;
            		scope.players = _.reject(scope.players, function(p){
            			return p.displayName = scope.currentPlayer.displayName;
            		});
                });

				var initSymbols = function(symbols){
					var symbolsArray = [];
					angular.forEach(symbols, function(symbol){
						if(symbol === 0){
							symbolsArray.push({value:'/content/icons/flaticons/30-pin charger.svg', id:symbol});
						}
						else if(symbol === 1){
							symbolsArray.push({value:'/content/icons/flaticons/adidas-superstar.svg', id:symbol});
						}
						else if(symbol === 2){
							symbolsArray.push({value:'/content/icons/flaticons/airport-drive.svg', id:symbol});
						}
						else if(symbol === 3){
							symbolsArray.push({value:'/content/icons/flaticons/apple-watch green.svg', id:symbol});
						}
						else if(symbol === 4){
							symbolsArray.push({value:'/content/icons/flaticons/balloons.svg', id:symbol});
						}
						else if(symbol === 5){
							symbolsArray.push({value:'/content/icons/flaticons/bedge.svg', id:symbol});
						}
						else if(symbol === 6){
							symbolsArray.push({value:'/content/icons/flaticons/bra.svg', id:symbol});
						}
						else if(symbol === 7){
							symbolsArray.push({value:'/content/icons/flaticons/boxers.svg', id:symbol});
						}
						else if(symbol === 8){
							symbolsArray.push({value:'/content/icons/flaticons/butterfly.svg', id:symbol});
						}
						else if(symbol === 9){
							symbolsArray.push({value:'/content/icons/flaticons/calendar.svg', id:symbol});
						}
						else if(symbol === 10){
							symbolsArray.push({value:'/content/icons/tomato-organicfood-v1-codrops-wojciechzasina.svg', id:symbol});
						}
						else if(symbol === 11){
							symbolsArray.push({value:'/content/icons/flaticons/cloud-download.svg', id:symbol});
						}
						else if(symbol === 12){
							symbolsArray.push({value:'/content/icons/flaticons/cpu-64.svg', id:symbol});
						}
						else if(symbol === 13){
							symbolsArray.push({value:'/content/icons/flaticons/cupcake.svg', id:symbol});
						}
						else if(symbol === 14){
							symbolsArray.push({value:'/content/icons/flaticons/cutter.svg', id:symbol});
						}
						else if(symbol === 15){
							symbolsArray.push({value:'/content/icons/flaticons/eye.svg', id:symbol});
						}
						else if(symbol === 16){
							symbolsArray.push({value:'/content/icons/flaticons/hand-watch.svg', id:symbol});
						}
						else if(symbol === 17){
							symbolsArray.push({value:'/content/icons/flaticons/hdd.svg', id:symbol});
						}
						else if(symbol === 18){
							symbolsArray.push({value:'/content/icons/flaticons/heart-lock.svg', id:symbol});
						}
						else if(symbol === 19){
							symbolsArray.push({value:'/content/icons/flaticons/high-heel sandals 2.svg', id:symbol});
						}
						else if(symbol === 20){
							symbolsArray.push({value:'/content/icons/flaticons/ipad-grey.svg', id:symbol});
						}
						else if(symbol === 21){
							symbolsArray.push({value:'/content/icons/flaticons/keyboard.svg', id:symbol});
						}
						else if(symbol === 22){
							symbolsArray.push({value:'/content/icons/watermelon-organicfood-v1-codrops-wojciechzasina.svg', id:symbol});
						}
						else if(symbol === 23){
							symbolsArray.push({value:'/content/icons/flaticons/man-suit 2.svg', id:symbol});
						}
						else if(symbol === 24){
							symbolsArray.push({value:'/content/icons/flaticons/men-hoodie.svg', id:symbol});
						}
						else if(symbol === 25){
							symbolsArray.push({value:'/content/icons/flaticons/mug.svg', id:symbol});
						}
						else if(symbol === 26){
							symbolsArray.push({value:'/content/icons/flaticons/pen.svg', id:symbol});
						}
						else if(symbol === 27){
							symbolsArray.push({value:'/content/icons/flaticons/purse.svg', id:symbol});
						}
						else if(symbol === 28){
							symbolsArray.push({value:'/content/icons/flaticons/ribbon-bow.svg', id:symbol});
						}
						else if(symbol === 29){
							symbolsArray.push({value:'/content/icons/flaticons/scream.svg', id:symbol});
						}
						else if(symbol === 30){
							symbolsArray.push({value:'/content/icons/flaticons/signal-medium.svg', id:symbol});
						}
						else if(symbol === 31){
							symbolsArray.push({value:'/content/icons/flaticons/tape.svg', id:symbol});
						}
						else if(symbol === 32){
							symbolsArray.push({value:'/content/icons/flaticons/thunderbolt-display.svg', id:symbol});
						}
						else if(symbol === 33){
							symbolsArray.push({value:'/content/icons/flaticons/usa-flag.svg', id:symbol});
						}
						else if(symbol === 34){
							symbolsArray.push({value: '/content/icons/flaticons/usb-stick.svg', id:symbol});
						}
						else if(symbol === 35){
							symbolsArray.push( {value:'/content/icons/flaticons/usb-symbol.svg', id:symbol});
						}
						else if(symbol === 36){
							symbolsArray.push( {value:'/content/icons/flaticons/vpn.svg', id:symbol});
						}
						else if(symbol === 37){
							symbolsArray.push( {value:'/content/icons/flaticons/wash-cold.svg', id:symbol});
						}
						else if(symbol === 38){
							symbolsArray.push( {value:'/content/icons/flaticons/webcam.svg', id:symbol});
						}
						else if(symbol === 39){
							symbolsArray.push( {value:'/content/icons/flaticons/wi-fi-low.svg', id:symbol});
						}
						else if(symbol === 40){
							symbolsArray.push( {value:'/content/icons/flaticons/zombie.svg', id:symbol});
						}
						else if(symbol === 41){
							symbolsArray.push( {value:'/content/icons/apple-organicfood-v1-codrops-wojciechzasina.svg', id:symbol});
						}
						else if(symbol === 42){
							symbolsArray.push( {value:'/content/icons/banana-organicfood-v1-codrops-wojciechzasina.svg', id:symbol});
						}
						else if(symbol === 43){
							symbolsArray.push( {value:'/content/icons/basket-organicfood-v1-codrops-wojciechzasina.svg', id:symbol});
						}
						else if(symbol === 44){
							symbolsArray.push( {value:'/content/icons/blender-organicfood-v1-codrops-wojciechzasina.svg', id:symbol});
						}
						else if(symbol === 45){
							symbolsArray.push( {value:'/content/icons/bread-organicfood-v1-codrops-wojciechzasina.svg', id:symbol});
						}
						else if(symbol === 46){
							symbolsArray.push({value: '/content/icons/broccoli-organicfood-v1-codrops-wojciechzasina.svg', id:symbol});
						}
						else if(symbol === 47){
							symbolsArray.push( {value:'/content/icons/carrot-organicfood-v1-codrops-wojciechzasina.svg', id:symbol});
						}
						else if(symbol === 48){
							symbolsArray.push({value: '/content/icons/coconut-organicfood-v1-codrops-wojciechzasina.svg', id:symbol});
						}
						else if(symbol === 49){
							symbolsArray.push( {value:'/content/icons/corn-organicfood-v1-codrops-wojciechzasina.svg', id:symbol});
						}
						else if(symbol === 50){
							symbolsArray.push({value:'/content/icons/cucumber-organicfood-v1-codrops-wojciechzasina.svg', id:symbol});
						}
						else if(symbol === 51){
							symbolsArray.push( {value:'/content/icons/egg-organicfood-v1-codrops-wojciechzasina.svg', id:symbol});
						}
						else if(symbol === 52){
							symbolsArray.push( {value:'/content/icons/eggplant-organicfood-v1-codrops-wojciechzasina.svg', id:symbol});
						}
						else if(symbol === 53){
							symbolsArray.push( {value:'/content/icons/ekobag-organicfood-v1-codrops-wojciechzasina.svg', id:symbol});
						}
						else if(symbol === 54){
							symbolsArray.push( {value:'/content/icons/grain-organicfood-v1-codrops-wojciechzasina.svg', id:symbol});
						}
						else if(symbol === 55){
							symbolsArray.push( {value:'/content/icons/grape-organicfood-v1-codrops-wojciechzasina.svg', id:symbol});
						}
						else if(symbol === 56){
							symbolsArray.push({value:'/content/icons/honey-organicfood-v1-codrops-wojciechzasina.svg', id:symbol});
						}
						else if(symbol === 57){
							symbolsArray.push({value:'/content/icons/jam-organicfood-v1-codrops-wojciechzasina.svg', id:symbol});
						}
						else{
							symbolsArray.push(''+symbol);
						}
					});
					return symbolsArray;
				}	
				
			}
		}
		return ddo;
}]);