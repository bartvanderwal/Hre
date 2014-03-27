/*
	# ExtraContent (sans-jQuery) #
	
	AUTHOR:	Adam Merrifield <http://adam.merrifield.ca>
			Jeroen Ransijn <http://twitter.com/Jeroen_Ransijn>
	VERSION: r0.5
	DATE: 12-16-10 09:28
	
	USAGE:
	- call this script in the <head>
	- change the value of ecValue to match the number of ExtraContent
		areas in your theme
*/

/* DomReady: <http://code.google.com/p/domready/> */
(function() { var DomReady = window.DomReady = {}; var userAgent = navigator.userAgent.toLowerCase(); var browser = { version: (userAgent.match(/.+(?:rv|it|ra|ie)[\/: ]([\d.]+)/) || [])[1], safari: /webkit/.test(userAgent), opera: /opera/.test(userAgent), msie: (/msie/.test(userAgent)) && (!/opera/.test(userAgent)), mozilla: (/mozilla/.test(userAgent)) && (!/(compatible|webkit)/.test(userAgent)) }; var readyBound = false; var isReady = false; var readyList = []; function domReady() { if (!isReady) { isReady = true; if (readyList) { for (var fn = 0; fn < readyList.length; fn++) { readyList[fn].call(window, []); } readyList = []; } } }; function addLoadEvent(func) { var oldonload = window.onload; if (typeof window.onload != 'function') { window.onload = func; } else { window.onload = function() { if (oldonload) { oldonload(); } func(); } } }; function bindReady() { if (readyBound) { return; } readyBound = true; if (document.addEventListener && !browser.opera) { document.addEventListener("DOMContentLoaded", domReady, false); } if (browser.msie && window == top)(function() { if (isReady) return; try { document.documentElement.doScroll("left"); } catch(error) { setTimeout(arguments.callee, 0); return; } domReady(); })(); if (browser.opera) { document.addEventListener("DOMContentLoaded", function() { if (isReady) return; for (var i = 0; i < document.styleSheets.length; i++) if (document.styleSheets[i].disabled) { setTimeout(arguments.callee, 0); return; } domReady(); }, false); } if (browser.safari) { var numStyles; (function() { if (isReady) return; if (document.readyState != "loaded" && document.readyState != "complete") { setTimeout(arguments.callee, 0); return; } if (numStyles === undefined) { var links = document.getElementsByTagName("link"); for (var i = 0; i < links.length; i++) { if (links[i].getAttribute('rel') == 'stylesheet') { numStyles++; } } var styles = document.getElementsByTagName("style"); numStyles += styles.length; } if (document.styleSheets.length != numStyles) { setTimeout(arguments.callee, 0); return; } domReady(); })(); } addLoadEvent(domReady); }; DomReady.ready = function(fn, args) { bindReady(); if (isReady) { fn.call(window, []); } else { readyList.push(function() { return fn.call(window, []); }); } }; bindReady(); })();

/* ExtraContent */
DomReady.ready(function() {
	var extraContent = (function() {
		// change ecValue to suit your theme
		var ecValue = 4;
		for (var i=1;i<=ecValue;i++)
		{
			if(document.getElementById("myExtraContent" +i))
			{
				var ecContent = document.getElementById("myExtraContent" +i);
				var ecContainer = document.getElementById("extraContainer" +i);
				var scripts = document.getElementsByTagName("script");
				var thisScript = scripts[scripts.length - 1];
				if (thisScript.parentNode == ecContent) {
					thisScript.parentNode.removeChild(thisScript);
				};
				ecContainer.appendChild(ecContent);
			}
		}
		return ecValue;
	})();
});

var pre = [
  'code',
  'front-end',
  'codepen',
  'design',
  'ux',
  'ui',
  'web',
  'mobile',
  'css',
  'html5',
  'javascript'
];

var title = [
  'ninja',
  'rockstar',
  'dude',
  'addict',
  'samurai',
  'wizard',
  'pro',
  'chef',
  'king',
  'architect',
  'tinkerer',
  'hacker',
  'barbarian',
  'guru',
  'evangelist',
  'lover',
  'jedi',
  'expert',
  'assassin',
  'pirate',
  'warrior',
  'strategist'
];

$(document).ready(function() {

  $t1 = $('#t1');
  $t2 = $('#t2');
  $gen = $('#gen');
  
  var preInitRand;
  var titleInitRand;
  
  function setTitle() {
    $t1.html('');
    $t2.html('');
    
    preInitRand = Math.floor((Math.random()*pre.length));
    titleInitRand = Math.floor((Math.random()*title.length));
    
    for (var i = 0; i < 5; i++) {
      $t1.append('<div class="pre-item item">' + pre[preInitRand] + '</div>');
      preInitRand++;
    
      if (preInitRand === pre.length) {
        preInitRand = 0;  
      }
    
      $t2.append('<div class="title-item item">' + title[titleInitRand] + '</div>');
      titleInitRand++;
    
      if (titleInitRand === title.length) {
        titleInitRand = 0;  
      }
    }
  }
  
  setTitle();
  var randRollPre;
  var randRollTitle;
  
  $gen.click(function() {
    
    randRollPre = Math.floor((Math.random()*pre.length)+5);
    randRollTitle =
Math.floor((Math.random()*title.length)+5);
    
    /**
     * Roll through first column
     **/
    for (var x = 0; x < randRollPre; x++) {
      
      if (preInitRand === 0) {
        preInitRand = pre.length-1;
      }

      $t1.prepend('<div class="pre-item item">' + pre[preInitRand--] + '</div>');
      $('.pre-item').css('top', '0');
    }
    
    var preMoves = randRollPre * 35;
    
    $('.pre-item').animate({
      top: preMoves + 'px'
    }, 500).promise().done(function() {
      for (var y = 0; y < randRollPre; y++) {
        $('.pre-item:last-child').remove();
        $('.pre-item').css('top', '0');
      }
    });
    
    /**
     * Roll through second column
     **/
    
    console.log(randRollTitle);
    
    for (var j = 0; j < randRollTitle; j++) {
      
      if (titleInitRand === title.length) {
        titleInitRand = 0;
      }

      $t2.append('<div class="title-item item">' + title[titleInitRand++] + '</div>');
      $('.title-item').css('bottom', '0');
    }
    
    var titleMoves = randRollTitle * 35;
    
    $('.title-item').animate({
      bottom: titleMoves + 'px'
    }, 500).promise().done(function() {
      for (var k = 0; k < randRollTitle; k++) {
        $('.title-item:first-child').remove();
        $('.title-item').css('bottom', '0');
      }
    }); 
  })
})