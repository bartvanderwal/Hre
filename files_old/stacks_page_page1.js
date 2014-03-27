
// 'stacks' is the Stacks global object.
// All of the other Stacks related Javascript will 
// be attatched to it.
var stacks = {};


// this call to jQuery gives us access to the globaal
// jQuery object. 
// 'noConflict' removes the '$' variable.
// 'true' removes the 'jQuery' variable.
// removing these globals reduces conflicts with other 
// jQuery versions that might be running on this page.
stacks.jQuery = jQuery.noConflict(true);

// Javascript for stacks_in_153_page1
// ---------------------------------------------------------------------

// Each stack has its own object with its own namespace.  The name of
// that object is the same as the stack's id.
stacks.stacks_in_153_page1 = {};

// A closure is defined and assigned to the stack's object.  The object
// is also passed in as 'stack' which gives you a shorthand for referring
// to this object from elsewhere.
stacks.stacks_in_153_page1 = (function(stack) {

	// When jQuery is used it will be available as $ and jQuery but only
	// inside the closure.
	var jQuery = stacks.jQuery;
	var $ = jQuery;
	
/*!
 * jQuery Migrate - v1.0.0 - 2013-01-14
 * https://github.com/jquery/jquery-migrate
 * Copyright 2005, 2013 jQuery Foundation, Inc. and other contributors; Licensed MIT
 */
(function( jQuery, window, undefined ) {
"use strict";


var warnedAbout = {};

// List of warnings already given; public read only
jQuery.migrateWarnings = [];

// Set to true to prevent console output; migrateWarnings still maintained
// jQuery.migrateMute = false;

// Forget any warnings we've already given; public
jQuery.migrateReset = function() {
	warnedAbout = {};
	jQuery.migrateWarnings.length = 0;
};

function migrateWarn( msg) {
	if ( !warnedAbout[ msg ] ) {
		warnedAbout[ msg ] = true;
		jQuery.migrateWarnings.push( msg );
		if ( window.console && console.warn && !jQuery.migrateMute ) {
			console.warn( "JQMIGRATE: " + msg );
		}
	}
}

function migrateWarnProp( obj, prop, value, msg ) {
	if ( Object.defineProperty ) {
		// On ES5 browsers (non-oldIE), warn if the code tries to get prop;
		// allow property to be overwritten in case some other plugin wants it
		try {
			Object.defineProperty( obj, prop, {
				configurable: true,
				enumerable: true,
				get: function() {
					migrateWarn( msg );
					return value;
				},
				set: function( newValue ) {
					migrateWarn( msg );
					value = newValue;
				}
			});
			return;
		} catch( err ) {
			// IE8 is a dope about Object.defineProperty, can't warn there
		}
	}

	// Non-ES5 (or broken) browser; just set the property
	jQuery._definePropertyBroken = true;
	obj[ prop ] = value;
}

if ( document.compatMode === "BackCompat" ) {
	// jQuery has never supported or tested Quirks Mode
	migrateWarn( "jQuery is not compatible with Quirks Mode" );
}


var attrFn = {},
	attr = jQuery.attr,
	valueAttrGet = jQuery.attrHooks.value && jQuery.attrHooks.value.get ||
		function() { return null; },
	valueAttrSet = jQuery.attrHooks.value && jQuery.attrHooks.value.set ||
		function() { return undefined; },
	rnoType = /^(?:input|button)$/i,
	rnoAttrNodeType = /^[238]$/,
	rboolean = /^(?:autofocus|autoplay|async|checked|controls|defer|disabled|hidden|loop|multiple|open|readonly|required|scoped|selected)$/i,
	ruseDefault = /^(?:checked|selected)$/i;

// jQuery.attrFn
migrateWarnProp( jQuery, "attrFn", attrFn, "jQuery.attrFn is deprecated" );

jQuery.attr = function( elem, name, value, pass ) {
	var lowerName = name.toLowerCase(),
		nType = elem && elem.nodeType;

	if ( pass ) {
		migrateWarn("jQuery.fn.attr( props, pass ) is deprecated");
		if ( elem && !rnoAttrNodeType.test( nType ) && jQuery.isFunction( jQuery.fn[ name ] ) ) {
			return jQuery( elem )[ name ]( value );
		}
	}

	// Warn if user tries to set `type` since it breaks on IE 6/7/8
	if ( name === "type" && value !== undefined && rnoType.test( elem.nodeName ) ) {
		migrateWarn("Can't change the 'type' of an input or button in IE 6/7/8");
	}

	// Restore boolHook for boolean property/attribute synchronization
	if ( !jQuery.attrHooks[ lowerName ] && rboolean.test( lowerName ) ) {
		jQuery.attrHooks[ lowerName ] = {
			get: function( elem, name ) {
				// Align boolean attributes with corresponding properties
				// Fall back to attribute presence where some booleans are not supported
				var attrNode,
					property = jQuery.prop( elem, name );
				return property === true || typeof property !== "boolean" &&
					( attrNode = elem.getAttributeNode(name) ) && attrNode.nodeValue !== false ?

					name.toLowerCase() :
					undefined;
			},
			set: function( elem, value, name ) {
				var propName;
				if ( value === false ) {
					// Remove boolean attributes when set to false
					jQuery.removeAttr( elem, name );
				} else {
					// value is true since we know at this point it's type boolean and not false
					// Set boolean attributes to the same name and set the DOM property
					propName = jQuery.propFix[ name ] || name;
					if ( propName in elem ) {
						// Only set the IDL specifically if it already exists on the element
						elem[ propName ] = true;
					}

					elem.setAttribute( name, name.toLowerCase() );
				}
				return name;
			}
		};

		// Warn only for attributes that can remain distinct from their properties post-1.9
		if ( ruseDefault.test( lowerName ) ) {
			migrateWarn( "jQuery.fn.attr(" + lowerName + ") may use property instead of attribute" );
		}
	}

	return attr.call( jQuery, elem, name, value );
};

// attrHooks: value
jQuery.attrHooks.value = {
	get: function( elem, name ) {
		var nodeName = ( elem.nodeName || "" ).toLowerCase();
		if ( nodeName === "button" ) {
			return valueAttrGet.apply( this, arguments );
		}
		if ( nodeName !== "input" && nodeName !== "option" ) {
			migrateWarn("property-based jQuery.fn.attr('value') is deprecated");
		}
		return name in elem ?
			elem.value :
			null;
	},
	set: function( elem, value ) {
		var nodeName = ( elem.nodeName || "" ).toLowerCase();
		if ( nodeName === "button" ) {
			return valueAttrSet.apply( this, arguments );
		}
		if ( nodeName !== "input" && nodeName !== "option" ) {
			migrateWarn("property-based jQuery.fn.attr('value', val) is deprecated");
		}
		// Does not return so that setAttribute is also used
		elem.value = value;
	}
};


var matched, browser,
	oldInit = jQuery.fn.init,
	// Note this does NOT include the # XSS fix from 1.7!
	rquickExpr = /^(?:.*(<[\w\W]+>)[^>]*|#([\w\-]*))$/;

// $(html) "looks like html" rule change
jQuery.fn.init = function( selector, context, rootjQuery ) {
	var match;

	if ( selector && typeof selector === "string" && !jQuery.isPlainObject( context ) &&
			(match = rquickExpr.exec( selector )) && match[1] ) {
		// This is an HTML string according to the "old" rules; is it still?
		if ( selector.charAt( 0 ) !== "<" ) {
			migrateWarn("$(html) HTML strings must start with '<' character");
		}
		// Now process using loose rules; let pre-1.8 play too
		if ( context && context.context ) {
			// jQuery object as context; parseHTML expects a DOM object
			context = context.context;
		}
		if ( jQuery.parseHTML ) {
			return oldInit.call( this, jQuery.parseHTML( jQuery.trim(selector), context, true ),
					context, rootjQuery );
		}
	}
	return oldInit.apply( this, arguments );
};
jQuery.fn.init.prototype = jQuery.fn;

jQuery.uaMatch = function( ua ) {
	ua = ua.toLowerCase();

	var match = /(chrome)[ \/]([\w.]+)/.exec( ua ) ||
		/(webkit)[ \/]([\w.]+)/.exec( ua ) ||
		/(opera)(?:.*version|)[ \/]([\w.]+)/.exec( ua ) ||
		/(msie) ([\w.]+)/.exec( ua ) ||
		ua.indexOf("compatible") < 0 && /(mozilla)(?:.*? rv:([\w.]+)|)/.exec( ua ) ||
		[];

	return {
		browser: match[ 1 ] || "",
		version: match[ 2 ] || "0"
	};
};

matched = jQuery.uaMatch( navigator.userAgent );
browser = {};

if ( matched.browser ) {
	browser[ matched.browser ] = true;
	browser.version = matched.version;
}

// Chrome is Webkit, but Webkit is also Safari.
if ( browser.chrome ) {
	browser.webkit = true;
} else if ( browser.webkit ) {
	browser.safari = true;
}

jQuery.browser = browser;

// Warn if the code tries to get jQuery.browser
migrateWarnProp( jQuery, "browser", browser, "jQuery.browser is deprecated" );

jQuery.sub = function() {
	function jQuerySub( selector, context ) {
		return new jQuerySub.fn.init( selector, context );
	}
	jQuery.extend( true, jQuerySub, this );
	jQuerySub.superclass = this;
	jQuerySub.fn = jQuerySub.prototype = this();
	jQuerySub.fn.constructor = jQuerySub;
	jQuerySub.sub = this.sub;
	jQuerySub.fn.init = function init( selector, context ) {
		if ( context && context instanceof jQuery && !(context instanceof jQuerySub) ) {
			context = jQuerySub( context );
		}

		return jQuery.fn.init.call( this, selector, context, rootjQuerySub );
	};
	jQuerySub.fn.init.prototype = jQuerySub.fn;
	var rootjQuerySub = jQuerySub(document);
	migrateWarn( "jQuery.sub() is deprecated" );
	return jQuerySub;
};


var oldFnData = jQuery.fn.data;

jQuery.fn.data = function( name ) {
	var ret, evt,
		elem = this[0];

	// Handles 1.7 which has this behavior and 1.8 which doesn't
	if ( elem && name === "events" && arguments.length === 1 ) {
		ret = jQuery.data( elem, name );
		evt = jQuery._data( elem, name );
		if ( ( ret === undefined || ret === evt ) && evt !== undefined ) {
			migrateWarn("Use of jQuery.fn.data('events') is deprecated");
			return evt;
		}
	}
	return oldFnData.apply( this, arguments );
};


var rscriptType = /\/(java|ecma)script/i,
	oldSelf = jQuery.fn.andSelf || jQuery.fn.addBack,
	oldFragment = jQuery.buildFragment;

jQuery.fn.andSelf = function() {
	migrateWarn("jQuery.fn.andSelf() replaced by jQuery.fn.addBack()");
	return oldSelf.apply( this, arguments );
};

// Since jQuery.clean is used internally on older versions, we only shim if it's missing
if ( !jQuery.clean ) {
	jQuery.clean = function( elems, context, fragment, scripts ) {
		// Set context per 1.8 logic
		context = context || document;
		context = !context.nodeType && context[0] || context;
		context = context.ownerDocument || context;

		migrateWarn("jQuery.clean() is deprecated");

		var i, elem, handleScript, jsTags,
			ret = [];

		jQuery.merge( ret, jQuery.buildFragment( elems, context ).childNodes );

		// Complex logic lifted directly from jQuery 1.8
		if ( fragment ) {
			// Special handling of each script element
			handleScript = function( elem ) {
				// Check if we consider it executable
				if ( !elem.type || rscriptType.test( elem.type ) ) {
					// Detach the script and store it in the scripts array (if provided) or the fragment
					// Return truthy to indicate that it has been handled
					return scripts ?
						scripts.push( elem.parentNode ? elem.parentNode.removeChild( elem ) : elem ) :
						fragment.appendChild( elem );
				}
			};

			for ( i = 0; (elem = ret[i]) != null; i++ ) {
				// Check if we're done after handling an executable script
				if ( !( jQuery.nodeName( elem, "script" ) && handleScript( elem ) ) ) {
					// Append to fragment and handle embedded scripts
					fragment.appendChild( elem );
					if ( typeof elem.getElementsByTagName !== "undefined" ) {
						// handleScript alters the DOM, so use jQuery.merge to ensure snapshot iteration
						jsTags = jQuery.grep( jQuery.merge( [], elem.getElementsByTagName("script") ), handleScript );

						// Splice the scripts into ret after their former ancestor and advance our index beyond them
						ret.splice.apply( ret, [i + 1, 0].concat( jsTags ) );
						i += jsTags.length;
					}
				}
			}
		}

		return ret;
	};
}

jQuery.buildFragment = function( elems, context, scripts, selection ) {
	var ret,
		warning = "jQuery.buildFragment() is deprecated";

	// Set context per 1.8 logic
	context = context || document;
	context = !context.nodeType && context[0] || context;
	context = context.ownerDocument || context;

	try {
		ret = oldFragment.call( jQuery, elems, context, scripts, selection );

	// jQuery < 1.8 required arrayish context; jQuery 1.9 fails on it
	} catch( x ) {
		ret = oldFragment.call( jQuery, elems, context.nodeType ? [ context ] : context[ 0 ], scripts, selection );

		// Success from tweaking context means buildFragment was called by the user
		migrateWarn( warning );
	}

	// jQuery < 1.9 returned an object instead of the fragment itself
	if ( !ret.fragment ) {
		migrateWarnProp( ret, "fragment", ret, warning );
		migrateWarnProp( ret, "cacheable", false, warning );
	}

	return ret;
};

var eventAdd = jQuery.event.add,
	eventRemove = jQuery.event.remove,
	eventTrigger = jQuery.event.trigger,
	oldToggle = jQuery.fn.toggle,
	oldLive = jQuery.fn.live,
	oldDie = jQuery.fn.die,
	ajaxEvents = "ajaxStart|ajaxStop|ajaxSend|ajaxComplete|ajaxError|ajaxSuccess",
	rajaxEvent = new RegExp( "\\b(?:" + ajaxEvents + ")\\b" ),
	rhoverHack = /(?:^|\s)hover(\.\S+|)\b/,
	hoverHack = function( events ) {
		if ( typeof( events ) != "string" || jQuery.event.special.hover ) {
			return events;
		}
		if ( rhoverHack.test( events ) ) {
			migrateWarn("'hover' pseudo-event is deprecated, use 'mouseenter mouseleave'");
		}
		return events && events.replace( rhoverHack, "mouseenter$1 mouseleave$1" );
	};

// Event props removed in 1.9, put them back if needed; no practical way to warn them
if ( jQuery.event.props && jQuery.event.props[ 0 ] !== "attrChange" ) {
	jQuery.event.props.unshift( "attrChange", "attrName", "relatedNode", "srcElement" );
}

// Undocumented jQuery.event.handle was "deprecated" in jQuery 1.7
migrateWarnProp( jQuery.event, "handle", jQuery.event.dispatch, "jQuery.event.handle is undocumented and deprecated" );

// Support for 'hover' pseudo-event and ajax event warnings
jQuery.event.add = function( elem, types, handler, data, selector ){
	if ( elem !== document && rajaxEvent.test( types ) ) {
		migrateWarn( "AJAX events should be attached to document: " + types );
	}
	eventAdd.call( this, elem, hoverHack( types || "" ), handler, data, selector );
};
jQuery.event.remove = function( elem, types, handler, selector, mappedTypes ){
	eventRemove.call( this, elem, hoverHack( types ) || "", handler, selector, mappedTypes );
};

jQuery.fn.error = function() {
	var args = Array.prototype.slice.call( arguments, 0);
	migrateWarn("jQuery.fn.error() is deprecated");
	args.splice( 0, 0, "error" );
	if ( arguments.length ) {
		return this.bind.apply( this, args );
	}
	// error event should not bubble to window, although it does pre-1.7
	this.triggerHandler.apply( this, args );
	return this;
};

jQuery.fn.toggle = function( fn, fn2 ) {

	// Don't mess with animation or css toggles
	if ( !jQuery.isFunction( fn ) || !jQuery.isFunction( fn2 ) ) {
		return oldToggle.apply( this, arguments );
	}
	migrateWarn("jQuery.fn.toggle(handler, handler...) is deprecated");

	// Save reference to arguments for access in closure
	var args = arguments,
		guid = fn.guid || jQuery.guid++,
		i = 0,
		toggler = function( event ) {
			// Figure out which function to execute
			var lastToggle = ( jQuery._data( this, "lastToggle" + fn.guid ) || 0 ) % i;
			jQuery._data( this, "lastToggle" + fn.guid, lastToggle + 1 );

			// Make sure that clicks stop
			event.preventDefault();

			// and execute the function
			return args[ lastToggle ].apply( this, arguments ) || false;
		};

	// link all the functions, so any of them can unbind this click handler
	toggler.guid = guid;
	while ( i < args.length ) {
		args[ i++ ].guid = guid;
	}

	return this.click( toggler );
};

jQuery.fn.live = function( types, data, fn ) {
	migrateWarn("jQuery.fn.live() is deprecated");
	if ( oldLive ) {
		return oldLive.apply( this, arguments );
	}
	jQuery( this.context ).on( types, this.selector, data, fn );
	return this;
};

jQuery.fn.die = function( types, fn ) {
	migrateWarn("jQuery.fn.die() is deprecated");
	if ( oldDie ) {
		return oldDie.apply( this, arguments );
	}
	jQuery( this.context ).off( types, this.selector || "**", fn );
	return this;
};

// Turn global events into document-triggered events
jQuery.event.trigger = function( event, data, elem, onlyHandlers  ){
	if ( !elem & !rajaxEvent.test( event ) ) {
		migrateWarn( "Global events are undocumented and deprecated" );
	}
	return eventTrigger.call( this,  event, data, elem || document, onlyHandlers  );
};
jQuery.each( ajaxEvents.split("|"),
	function( _, name ) {
		jQuery.event.special[ name ] = {
			setup: function() {
				var elem = this;

				// The document needs no shimming; must be !== for oldIE
				if ( elem !== document ) {
					jQuery.event.add( document, name + "." + jQuery.guid, function() {
						jQuery.event.trigger( name, null, elem, true );
					});
					jQuery._data( this, name, jQuery.guid++ );
				}
				return false;
			},
			teardown: function() {
				if ( this !== document ) {
					jQuery.event.remove( document, name + "." + jQuery._data( this, name ) );
				}
				return false;
			}
		};
	}
);


})( jQuery, window );


/**
 * jQuery Masonry v2.1.06
 * A dynamic layout plugin for jQuery
 * The flip-side of CSS Floats
 * http://masonry.desandro.com
 *
 * Licensed under the MIT license.
 * Copyright 2012 David DeSandro
 */
(function(a,b,c){"use strict";var d=b.event,e;d.special.smartresize={setup:function(){b(this).bind("resize",d.special.smartresize.handler)},teardown:function(){b(this).unbind("resize",d.special.smartresize.handler)},handler:function(a,c){var d=this,f=arguments;a.type="smartresize",e&&clearTimeout(e),e=setTimeout(function(){b.event.handle.apply(d,f)},c==="execAsap"?0:100)}},b.fn.smartresize=function(a){return a?this.bind("smartresize",a):this.trigger("smartresize",["execAsap"])},b.Mason=function(a,c){this.element=b(c),this._create(a),this._init()},b.Mason.settings={isResizable:!0,isAnimated:!1,animationOptions:{queue:!1,duration:500},gutterWidth:0,isRTL:!1,isFitWidth:!1,containerStyle:{position:"relative"}},b.Mason.prototype={_filterFindBricks:function(a){var b=this.options.itemSelector;return b?a.filter(b).add(a.find(b)):a},_getBricks:function(a){var b=this._filterFindBricks(a).css({position:"absolute"}).addClass("masonry-brick");return b},_create:function(c){this.options=b.extend(!0,{},b.Mason.settings,c),this.styleQueue=[];var d=this.element[0].style;this.originalStyle={height:d.height||""};var e=this.options.containerStyle;for(var f in e)this.originalStyle[f]=d[f]||"";this.element.css(e),this.horizontalDirection=this.options.isRTL?"right":"left";var g=this.element.css("padding-"+this.horizontalDirection),h=this.element.css("padding-top");this.offset={x:g?parseInt(g,10):0,y:h?parseInt(h,10):0},this.isFluid=this.options.columnWidth&&typeof this.options.columnWidth=="function";var i=this;setTimeout(function(){i.element.addClass("masonry")},0),this.options.isResizable&&b(a).bind("smartresize.masonry",function(){i.resize()}),this.reloadItems()},_init:function(a){this._getColumns(),this._reLayout(a)},option:function(a,c){b.isPlainObject(a)&&(this.options=b.extend(!0,this.options,a))},layout:function(a,b){for(var c=0,d=a.length;c<d;c++)this._placeBrick(a[c]);var e={};e.height=Math.max.apply(Math,this.colYs);if(this.options.isFitWidth){var f=0;c=this.cols;while(--c){if(this.colYs[c]!==0)break;f++}e.width=(this.cols-f)*this.columnWidth-this.options.gutterWidth}this.styleQueue.push({$el:this.element,style:e});var g=this.isLaidOut?this.options.isAnimated?"animate":"css":"css",h=this.options.animationOptions,i;for(c=0,d=this.styleQueue.length;c<d;c++)i=this.styleQueue[c],i.$el[g](i.style,h);this.styleQueue=[],b&&b.call(a),this.isLaidOut=!0},_getColumns:function(){var a=this.options.isFitWidth?this.element.parent():this.element,b=a.width();this.columnWidth=this.isFluid?this.options.columnWidth(b):this.options.columnWidth||this.$bricks.outerWidth(!0)||b,this.columnWidth+=this.options.gutterWidth,this.cols=Math.floor((b+this.options.gutterWidth)/this.columnWidth),this.cols=Math.max(this.cols,1)},_placeBrick:function(a){var c=b(a),d,e,f,g,h;d=Math.ceil(c.outerWidth(!0)/this.columnWidth),d=Math.min(d,this.cols);if(d===1)f=this.colYs;else{e=this.cols+1-d,f=[];for(h=0;h<e;h++)g=this.colYs.slice(h,h+d),f[h]=Math.max.apply(Math,g)}var i=Math.min.apply(Math,f),j=0;for(var k=0,l=f.length;k<l;k++)if(f[k]===i){j=k;break}var m={top:i+this.offset.y};m[this.horizontalDirection]=this.columnWidth*j+this.offset.x,this.styleQueue.push({$el:c,style:m});var n=i+c.outerHeight(!0),o=this.cols+1-l;for(k=0;k<o;k++)this.colYs[j+k]=n},resize:function(){var a=this.cols;this._getColumns(),(this.isFluid||this.cols!==a)&&this._reLayout()},_reLayout:function(a){var b=this.cols;this.colYs=[];while(b--)this.colYs.push(0);this.layout(this.$bricks,a)},reloadItems:function(){this.$bricks=this._getBricks(this.element.children())},reload:function(a){this.reloadItems(),this._init(a)},appended:function(a,b,c){if(b){this._filterFindBricks(a).css({top:this.element.height()});var d=this;setTimeout(function(){d._appended(a,c)},1)}else this._appended(a,c)},_appended:function(a,b){var c=this._getBricks(a);this.$bricks=this.$bricks.add(c),this.layout(c,b)},remove:function(a){this.$bricks=this.$bricks.not(a),a.remove()},destroy:function(){this.$bricks.removeClass("masonry-brick").each(function(){this.style.position="",this.style.top="",this.style.left=""});var c=this.element[0].style;for(var d in this.originalStyle)c[d]=this.originalStyle[d];this.element.unbind(".masonry").removeClass("masonry").removeData("masonry"),b(a).unbind(".masonry")}},b.fn.imagesLoaded=function(a){function h(){a.call(c,d)}function i(a){var c=a.target;c.src!==f&&b.inArray(c,g)===-1&&(g.push(c),--e<=0&&(setTimeout(h),d.unbind(".imagesLoaded",i)))}var c=this,d=c.find("img").add(c.filter("img")),e=d.length,f="data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///ywAAAAAAQABAAACAUwAOw==",g=[];return e||h(),d.bind("load.imagesLoaded error.imagesLoaded",i).each(function(){var a=this.src;this.src=f,this.src=a}),c};var f=function(b){a.console&&a.console.error(b)};b.fn.masonry=function(a){if(typeof a=="string"){var c=Array.prototype.slice.call(arguments,1);this.each(function(){var d=b.data(this,"masonry");if(!d){f("cannot call methods on masonry prior to initialization; attempted to call method '"+a+"'");return}if(!b.isFunction(d[a])||a.charAt(0)==="_"){f("no such method '"+a+"' for masonry instance");return}d[a].apply(d,c)})}else this.each(function(){var c=b.data(this,"masonry");c?(c.option(a||{}),c._init()):b.data(this,"masonry",new b.Mason(a,this))});return this}})(window,jQuery);



    jQuery(document).ready(function($){
		
		/* See if visitor is using an iOS device or not. Used to make sure items are not faded out on iOS. */
		function isiPhone(){
		    return (
		        (navigator.platform.indexOf("iPhone") != -1) ||
		        (navigator.platform.indexOf("iPod") != -1) || 
		        (navigator.platform.indexOf("iPad") != -1)
		    );
		}

		var $container = $('#stacks_in_153_page1');
		$container.imagesLoaded(function(){
		  $container.masonry({
		  itemSelector: '#stacks_in_153_page1 .layer',
		  columnWidth: 150,
		  gutterWidth: 10,
		  isAnimated: true,
		  isFitWidth: true
		  });
		});


		fadeOutOpacity = 70 / 100;

		$('#stacks_in_153_page1 .layer').hide().each(function(count) {
			$(this).delay(60*count).fadeIn(300);
		});
		
		if(isiPhone()){

			$('#stacks_in_153_page1 .layer').css({'opacity':'1'});

		} else {
		
			
		} 	
		

	});
	return stack;
})(stacks.stacks_in_153_page1);


// Javascript for stacks_in_40_page1
// ---------------------------------------------------------------------

// Each stack has its own object with its own namespace.  The name of
// that object is the same as the stack's id.
stacks.stacks_in_40_page1 = {};

// A closure is defined and assigned to the stack's object.  The object
// is also passed in as 'stack' which gives you a shorthand for referring
// to this object from elsewhere.
stacks.stacks_in_40_page1 = (function(stack) {

	// When jQuery is used it will be available as $ and jQuery but only
	// inside the closure.
	var jQuery = stacks.jQuery;
	var $ = jQuery;
	
// Start Montage stack Javascript code$(document).ready(function() {;(function($) {    // Namespace all events.    var eventNamespace = 'waitForImages';    // CSS properties which contain references to images.     $.waitForImages = {        hasImageProperties: [        'backgroundImage',        'listStyleImage',        'borderImage',        'borderCornerImage'        ]    };        // Custom selector to find `img` elements that have a valid `src` attribute and have not already loaded.    $.expr[':'].uncached = function(obj) {        // Ensure we are dealing with an `img` element with a valid `src` attribute.        if ( ! $(obj).is('img[src!=""]')) {            return false;        }        // Firefox's `complete` property will always be`true` even if the image has not been downloaded.        // Doing it this way works in Firefox.        var img = document.createElement('img');        img.src = obj.src;        return ! img.complete;    };    $.fn.waitForImages = function(finishedCallback, eachCallback, waitForAll) {        // Handle options object.        if ($.isPlainObject(arguments[0])) {            eachCallback = finishedCallback.each;            waitForAll = finishedCallback.waitForAll;            finishedCallback = finishedCallback.finished;        }        // Handle missing callbacks.        finishedCallback = finishedCallback || $.noop;        eachCallback = eachCallback || $.noop;        // Convert waitForAll to Boolean        waitForAll = !! waitForAll;        // Ensure callbacks are functions.        if (!$.isFunction(finishedCallback) || !$.isFunction(eachCallback)) {            throw new TypeError('An invalid callback was supplied.');        };        return this.each(function() {            // Build a list of all imgs, dependent on what images will be considered.            var obj = $(this),                allImgs = [];            if (waitForAll) {                // CSS properties which may contain an image.                var hasImgProperties = $.waitForImages.hasImageProperties || [],                    matchUrl = /url\((['"]?)(.*?)\1\)/g;                                // Get all elements, as any one of them could have a background image.                obj.find('*').each(function() {                    var element = $(this);                    // If an `img` element, add it. But keep iterating in case it has a background image too.                    if (element.is('img:uncached')) {                        allImgs.push({                            src: element.attr('src'),                            element: element[0]                        });                    }                    $.each(hasImgProperties, function(i, property) {                        var propertyValue = element.css(property);                        // If it doesn't contain this property, skip.                        if ( ! propertyValue) {                            return true;                        }                        // Get all url() of this element.                        var match;                        while (match = matchUrl.exec(propertyValue)) {                            allImgs.push({                                src: match[2],                                element: element[0]                            });                        };                    });                });            } else {                // For images only, the task is simpler.                obj                 .find('img:uncached')                 .each(function() {                    allImgs.push({                        src: this.src,                        element: this                    });                });            };            var allImgsLength = allImgs.length,                allImgsLoaded = 0;            // If no images found, don't bother.            if (allImgsLength == 0) {                finishedCallback.call(obj[0]);            };            $.each(allImgs, function(i, img) {                                var image = new Image;                                // Handle the image loading and error with the same callback.                $(image).bind('load.' + eventNamespace + ' error.' + eventNamespace, function(event) {                    allImgsLoaded++;                                        // If an error occurred with loading the image, set the third argument accordingly.                    eachCallback.call(img.element, allImgsLoaded, allImgsLength, event.type == 'load');                                        if (allImgsLoaded == allImgsLength) {                        finishedCallback.call(obj[0]);                        return false;                    };                                    });                image.src = img.src;            });        });    };})(jQuery);if ("linktopage" == "lightbox"){(function($) {    $.fn.stacks_in_40_page1lightbox_me = function(options) {        return this.each(function() {            var                opts = $.extend({}, $.fn.stacks_in_40_page1lightbox_me.defaults, options),                $overlay = $(),                $self = $(this),                $iframe = $('<iframe id="foo" style="z-index: ' + (opts.zIndex + 1) + ';border: none; margin: 0; padding: 0; position: absolute; width: 100%; height: 100%; top: 0; left: 0; filter: mask();"/>'),                ie6 = false;            if (opts.showOverlay) {                //check if there's an existing overlay, if so, make subequent ones clear               var $currentOverlays = $(".js_lb_overlay:visible");                if ($currentOverlays.length > 0){                    $overlay = $('<div class="lb_overlay_clear js_lb_overlay"/>');                } else {                    $overlay = $('<div class="' + opts.classPrefix + '_overlay js_lb_overlay"/>');                }            }            /*----------------------------------------------------               DOM Building            ---------------------------------------------------- */            if (ie6) {                var src = /^https/i.test(window.location.href || '') ? 'javascript:false' : 'about:blank';                $iframe.attr('src', src);               // $('body').append($iframe);            } // iframe shim for ie6, to hide select elements            $('body').append($overlay);            $overlay.append($self.hide())            $overlay.append("<div class='stacks_in_40_page1popupcontrolbar' style='z-index:" + (opts.zIndex + 10) + ";'><div class='stacks_in_40_page1xbutton'></div></div>");;            /*----------------------------------------------------               Overlay CSS stuffs            ---------------------------------------------------- */            // set css of the overlay            if (opts.showOverlay) {                setOverlayHeight(); // pulled this into a function because it is called on window resize.                $overlay.css({ position: 'absolute', width: '100%', top: 0, left: 0, right: 0, bottom: 0, zIndex: (opts.zIndex + 2), display: 'none' });				if (!$overlay.hasClass('lb_overlay_clear')){                	$overlay.css(opts.overlayCSS);                }            }            /*----------------------------------------------------               Animate it in.            ---------------------------------------------------- */               //            if (opts.showOverlay) {                $overlay.fadeIn(opts.overlaySpeed, function() {                    setSelfPosition();                    $self[opts.appearEffect](opts.lightboxSpeed, function() { setOverlayHeight(); setSelfPosition(); opts.onLoad()});                });            } else {                setSelfPosition();                $self[opts.appearEffect](opts.lightboxSpeed, function() { opts.onLoad()});            }            /*----------------------------------------------------               Hide parent if parent specified (parentLightbox should be jquery reference to any parent lightbox)            ---------------------------------------------------- */            if (opts.parentLightbox) {                opts.parentLightbox.fadeOut(200);            }            /*----------------------------------------------------               Bind Events            ---------------------------------------------------- */            $(window).resize(setOverlayHeight)                     .resize(setSelfPosition)                     .scroll(setSelfPosition);                                 $(window).bind('keyup.stacks_in_40_page1lightbox_me', observeKeyPress);                                 if (opts.closeClick) {                $overlay.click(function(e) { closeLightbox(); e.preventDefault; });            }            $self.delegate(opts.closeSelector, "click", function(e) {                closeLightbox(); e.preventDefault();            });            $self.bind('stacks_in_40_page1xbutton', closeLightbox);            $self.bind('reposition', setSelfPosition);                        /*--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------              -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- */            /*----------------------------------------------------               Private Functions            ---------------------------------------------------- */                   /* Remove or hide all elements */            function closeLightbox() {                var s = $self[0].style;                if (opts.destroyOnClose) {                    $self.add($overlay).remove();                } else {                    $self.add($overlay).hide();                }                //show the hidden parent lightbox                if (opts.parentLightbox) {                    opts.parentLightbox.fadeIn(200);                }                $iframe.remove();                				// clean up events.                $self.undelegate(opts.closeSelector, "click");                $(window).unbind('reposition', setOverlayHeight);                $(window).unbind('reposition', setSelfPosition);                $(window).unbind('scroll', setSelfPosition);                $(window).unbind('keyup.stacks_in_40_page1lightbox_me');                if (ie6)                    s.removeExpression('top');                opts.onClose();            }            /* Function to bind to the window to observe the escape/enter key press */            function observeKeyPress(e) {                if((e.keyCode == 27 || (e.DOM_VK_ESCAPE == 27 && e.which==0)) && opts.closeEsc) closeLightbox();            }            /* Set the height of the overlay                    : if the document height is taller than the window, then set the overlay height to the document height.                    : otherwise, just set overlay height: 100%            */            function setOverlayHeight() {                if ($(window).height() < $(document).height()) {                    $overlay.css({height: $(document).height() + 'px'});                     $iframe.css({height: $(document).height() + 'px'});                 } else {                    $overlay.css({height: '100%'});                    if (ie6) {                        $('html,body').css('height','100%');                        $iframe.css('height', '100%');                    } // ie6 hack for height: 100%; TODO: handle this in IE7                }            }            /* Set the position of the modal'd window ($self)                    : if $self is taller than the window, then make it absolutely positioned                    : otherwise fixed            */            function setSelfPosition() {                var s = $self[0].style;                // reset CSS so width is re-calculated for margin-left CSS                                /* we have to get a little fancy when dealing with height, because stacks_in_40_page1lightbox_me                    is just so fancy.                 */                // if the height of $self is bigger than the window and self isn't already position absolute                    var topOffset = $(document).scrollTop();                    $self.css({"height":$(window).height(), "width": "auto"});                        var maxWidth = $(window).width(); // Max width for the image    var maxHeight = 500;    // Max height for the image    var ratio = 0;  // Used for aspect ratio    var width = $(".stacks_in_40_page1clone").width();    // Current image width    var height = $(".stacks_in_40_page1clone").height();  // Current image height    // Check if the current width is larger than the max    if(width > maxWidth){        ratio = maxWidth / width;   // get ratio for scaling image        $(".stacks_in_40_page1clone").css("width", maxWidth); // Set new width        $(".stacks_in_40_page1clone").css("height", height * ratio);  // Scale height based on ratio        height = height * ratio;    // Reset height to match scaled image    }    var width = $(".stacks_in_40_page1clone").width();    // Current image width    var height = $(".stacks_in_40_page1clone").height();  // Current image height    // Check if current height is larger than max    if(height > maxHeight){        ratio = maxHeight / height; // get ratio for scaling image        $(".stacks_in_40_page1clone").css("height", maxHeight);   // Set new height        $(".stacks_in_40_page1clone").css("width", width * ratio);    // Scale width based on ratio        width = width * ratio;    // Reset width to match scaled image    } // end        var verticaloffset = ($(window).height() / 2) + (topOffset) - ($(".stacks_in_40_page1clone").height() / 2);    $self.css({left: '50%', marginLeft: ($self.outerWidth() / 2) * -1,  zIndex: (opts.zIndex + 3) });    $(".stacks_in_40_page1clone").css({position: 'absolute', top: verticaloffset + 'px'});    var botoffset = 0 + topOffset;    $(".stacks_in_40_page1popupcontrolbar").css("bottom", botoffset + "px");    $(".stacks_in_40_page1popupcontrolbar").slideDown();            }        });    };    $.fn.stacks_in_40_page1lightbox_me.defaults = {        // animation        appearEffect: "fadeIn",        appearEase: "",        overlaySpeed: 250,        lightboxSpeed: 300,        // close        closeSelector: ".stacks_in_40_page1xbutton",        closeClick: true,        closeEsc: true,        // behavior        destroyOnClose: false,        showOverlay: true,        parentLightbox: false,        // callbacks        onLoad: function() {},        onClose: function() {},        // style        classPrefix: 'lb',        zIndex: 999,        centered: false,        modalCSS: {top: '40px'},        overlayCSS: {background: 'black', opacity: .3}    }})(jQuery);}// end lightbox script	/**	 * jQuery stacks_in_40_page1Montage plugin	 * http://www.codrops.com/	 *	 * Copyright 2011, Pedro Botelho	 * Licensed under the MIT license.	 * http://www.opensource.org/licenses/mit-license.php	 *	 * Date: Tue Aug 30 2011	 */	(function(window, $, undefined) { /*	* Array.max, Array.min 	* @John Resig	* http://ejohn.org/blog/fast-javascript-maxmin/	*/		// function to get the Max value in array		Array.max = function(array) {			return Math.max.apply(Math, array);		}; // function to get the Min value in array		Array.min = function(array) {			return Math.min.apply(Math, array);		}; /*	* smartresize: debounced resize event for jQuery	*	* latest version and complete README available on Github:	* https://github.com/louisremi/jquery.smartresize.js	*	* Copyright 2011 @louis_remi	* Licensed under the MIT license.	*/		var $event = $.event,			resizeTimeout;		$event.special.smartresize = {			setup: function() {				$(this).bind("resize", $event.special.smartresize.handler);			},			teardown: function() {				$(this).unbind("resize", $event.special.smartresize.handler);			},			handler: function(event, execAsap) { // Save the context				var context = this,					args = arguments; // set correct event type				event.type = "smartresize";				if (resizeTimeout) {					clearTimeout(resizeTimeout);				}				resizeTimeout = setTimeout(function() {					jQuery.event.handle.apply(context, args);				}, execAsap === "execAsap" ? 0 : 50);			}		};		$.fn.smartresize = function(fn) {			return fn ? this.bind("smartresize", fn) : this.trigger("smartresize", ["execAsap"]);		}; // ======================= imagesLoaded Plugin ===============================		// https://github.com/desandro/imagesloaded		// $('#my-container').imagesLoaded(myFunction)		// execute a callback when all images have loaded.		// needed because .load() doesn't work on cached images		// callback function gets image collection as argument		//  `this` is the container		// original: mit license. paul irish. 2010.		// contributors: Oren Solomianik, David DeSandro, Yiannis Chatzikonstantinou		$.fn.imagesLoaded = function(callback) {			var $images = this.find('img'),				len = $images.length,				_this = this,				blank = 'data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///ywAAAAAAQABAAACAUwAOw==';			function triggerCallback() {				callback.call(_this, $images);			}			function imgLoaded() {				if (--len <= 0 && this.src !== blank) {					setTimeout(triggerCallback);					$images.unbind('load error', imgLoaded);				}			}			if (!len) {				triggerCallback();			}			$images.bind('load error', imgLoaded).each(function() { // cached images don't fire load sometimes, so we reset src.				if (this.complete || this.complete === undefined) {					var src = this.src; // webkit hack from http://groups.google.com/group/jquery-dev/browse_thread/thread/eee6ab7b2da50e1f					// data uri bypasses webkit log warning (thx doug jones)					this.src = blank;					this.src = src;				}			});			return this;		};		$.stacks_in_40_page1Montage = function(options, element) {			this.element = $(element).show();			this.cache = {};			this.heights = new Array();			this._create(options);		};		$.stacks_in_40_page1Montage.defaults = {			liquid: false,			// if you use percentages (or no width at all) for the container's width, then set this to true			// this will set the body's overflow-y to scroll ( fix for the scrollbar's width problem ) 			margin: 1,			// space between images.			minw: 70,			// the minimum width that a picture should have.			minh: 20,			// the minimum height that a picture should have.			maxh: 250,			// the maximum height that a picture should have.			alternateHeight: false,			// alternate the height value for every row. If true this has priority over defaults.fixedHeight.			alternateHeightRange: { // the height will be a random value between min and max.				min: 100,				max: 300			},			fixedHeight: null,			// if the value is set this has priority over defaults.minsize. All images will have this height.			minsize: false,			// minw,minh are irrelevant. Chosen height is the minimum one available.			fillLastRow: true // if true, there will be no gaps in the container. The last image will fill any white space available		};		$.stacks_in_40_page1Montage.prototype = {			_getImageWidth: function($img, h) {				var i_w = $img.width(),					i_h = $img.height(),					r_i = i_h / i_w;				return Math.ceil(h / r_i);			},			_getImageHeight: function($img, w) {				var i_w = $img.width(),					i_h = $img.height(),					r_i = i_h / i_w;				return Math.ceil(r_i * w);			},			_chooseHeight: function() { // get the minimum height				if (this.options.minsize) {					return Array.min(this.heights);				} // otherwise get the most repeated heights. From those choose the minimum.				var result = {},					max = 0,					res, val, min;				for (var i = 0, total = this.heights.length; i < total; ++i) {					var val = this.heights[i],						inc = (result[val] || 0) + 1;					if (val < this.options.minh || val > this.options.maxh) continue;					result[val] = inc;					if (inc >= max) {						max = inc;						res = val;					}				}				for (var i in result) {					if (result[i] === max) {						val = i;						min = min || val;						if (min < this.options.minh) min = null;						else if (min > val) min = val;						if (min === null) min = val;					}				}				if (min === undefined) min = this.heights[0];				res = min;				return res;			},			_stretchImage: function($img) {				var prevWrapper_w = $img.parent().width(),					new_w = prevWrapper_w + this.cache.space_w_left,					crop = {						x: new_w + this.options.margin * 2, // my ammendment added double margin size to new width						y: this.theHeight					};				var new_image_w = $img.width() + this.cache.space_w_left,					new_image_h = this._getImageHeight($img, new_image_w);				this._cropImage($img, new_image_w, new_image_h, crop);				this.cache.space_w_left = this.cache.container_w; // if this.options.alternateHeight is true, change row / change height				if (this.options.alternateHeight) this.theHeight = Math.floor(Math.random() * (this.options.alternateHeightRange.max - this.options.alternateHeightRange.min + 1) + this.options.alternateHeightRange.min);			},			_updatePrevImage: function($nextimg) {				var $prevImage = this.element.find('img.stacks_in_40_page1Montage:last');				this._stretchImage($prevImage);				this._insertImage($nextimg);			},			_insertImage: function($img) { // width the image should have with height = this.theHeight.				var new_w = this._getImageWidth($img, this.theHeight); // use the minimum height available if this.options.minsize = true.				if (this.options.minsize && !this.options.alternateHeight) {				if (this.cache.space_w_left <= this.options.margin * 2) {						this._updatePrevImage($img);					} else {						if (new_w > this.cache.space_w_left) {							var crop = {								x: this.cache.space_w_left,								y: this.theHeight							};							this._cropImage($img, new_w, this.theHeight, crop);							this.cache.space_w_left = this.cache.container_w;							$img.addClass('stacks_in_40_page1Montage');						} else {							var crop = {								x: new_w,								y: this.theHeight							};							this._cropImage($img, new_w, this.theHeight, crop);							this.cache.space_w_left -= new_w;							$img.addClass('stacks_in_40_page1Montage');						}					}				} else { // the width is lower than the minimum width allowed.					if (new_w < this.options.minw) { // the minimum width allowed is higher than the space left to fill the row.						// need to resize the previous (last) item in that row.						if (this.options.minw > this.cache.space_w_left) {							this._updatePrevImage($img);						} else {							var new_w = this.options.minw,								new_h = this._getImageHeight($img, new_w),								crop = {									x: new_w,									y: this.theHeight								};							this._cropImage($img, new_w, new_h, crop);							this.cache.space_w_left -= new_w;							$img.addClass('stacks_in_40_page1Montage');						}					} else { // the new width is higher than the space left but the space left is lower than the minimum width allowed.						// need to resize the previous (last) item in that row.						if (new_w > this.cache.space_w_left && this.cache.space_w_left < this.options.minw) {							this._updatePrevImage($img);						} else if (new_w > this.cache.space_w_left && this.cache.space_w_left >= this.options.minw) {							var crop = {								x: this.cache.space_w_left,								y: this.theHeight							};							this._cropImage($img, new_w, this.theHeight, crop);							this.cache.space_w_left = this.cache.container_w; // if this.options.alternateHeight is true, change row / change height							if (this.options.alternateHeight) this.theHeight = Math.floor(Math.random() * (this.options.alternateHeightRange.max - this.options.alternateHeightRange.min + 1) + this.options.alternateHeightRange.min);							$img.addClass('stacks_in_40_page1Montage');						} else {							var crop = {								x: new_w,								y: this.theHeight							};							this._cropImage($img, new_w, this.theHeight, crop);							this.cache.space_w_left -= new_w;							$img.addClass('stacks_in_40_page1Montage');						}					}				}			},			_cropImage: function($img, w, h, cropParam) { // margin value				var dec = this.options.margin * 2;				var $wrapper = $img.parent('span'); // resize the image				this._resizeImage($img, w, h); // adjust the top / left values to slice the image without loosing the its ratio				$img.css({					left: -(w - cropParam.x) / 2 + 'px',					top: -(h - cropParam.y) / 2 + 'px'				}); // wrap the image in a <a> element				$wrapper.addClass('stacks_in_40_page1am-wrapper').css({					width: cropParam.x - dec + 'px',					height: cropParam.y + 'px',					margin: this.options.margin				});			},			_resizeImage: function($img, w, h) {				$img.css({					width: w + 'px',					height: h + 'px'				});			},			_reload: function() { // container's width				var new_el_w = this.element.width(); // if different, something changed...				if (new_el_w !== this.cache.container_w) {					this.element.hide();					this.cache.container_w = new_el_w;					this.cache.space_w_left = new_el_w;					var instance = this;					instance.$imgs.removeClass('stacks_in_40_page1Montage').each(function(i) {						instance._insertImage($(this));					});					if (instance.options.fillLastRow && instance.cache.space_w_left !== instance.cache.container_w) {						instance._stretchImage(instance.$imgs.eq(instance.totalImages - 1));					}					instance.element.show();				}			},			_create: function(options) {				this.options = $.extend(true, {}, $.stacks_in_40_page1Montage.defaults, options);				var instance = this,					el_w = instance.element.width();				instance.$imgs = instance.element.find('img');				instance.totalImages = instance.$imgs.length; // solve the scrollbar width problem.				if (instance.options.liquid) $('html').css('overflow-y', 'scroll'); // save the heights of all images.				if (!instance.options.fixedHeight) {					instance.$imgs.each(function(i) {						var $img = $(this),							img_w = $img.width(); // if images have width > instance.options.minw then "resize" image.						if (img_w < instance.options.minw && !instance.options.minsize) {							var new_h = instance._getImageHeight($img, instance.options.minw);							instance.heights.push(new_h);						} else {							instance.heights.push($img.height());						}					});				} // calculate which height to use for each image.				instance.theHeight = (!instance.options.fixedHeight && !instance.options.alternateHeight) ? instance._chooseHeight() : instance.options.fixedHeight;				if (instance.options.alternateHeight) instance.theHeight = Math.floor(Math.random() * (instance.options.alternateHeightRange.max - instance.options.alternateHeightRange.min + 1) + instance.options.alternateHeightRange.min); // save some values.				instance.cache.container_w = el_w; // space left to fill the row.				instance.cache.space_w_left = el_w; // wrap the images with the right sizes.				instance.$imgs.each(function(i) {					instance._insertImage($(this));				});				if (instance.options.fillLastRow && instance.cache.space_w_left !== instance.cache.container_w) {					instance._stretchImage(instance.$imgs.eq(instance.totalImages - 1));				} // window resize event : reload the container.				$(window).bind('smartresize.stacks_in_40_page1Montage', function() {					instance._reload();				});			},			add: function($images, callback) { // adds one or more images to the container				var $images_stripped = $images.find('img');				this.$imgs = this.$imgs.add($images_stripped);				this.totalImages = this.$imgs.length;				this._add($images, callback);			},			_add: function($images, callback) {				var instance = this;				$images.find('img').each(function(i) {					instance._insertImage($(this));				});				if (instance.options.fillLastRow && instance.cache.space_w_left !== instance.cache.container_w) instance._stretchImage(instance.$imgs.eq(instance.totalImages - 1));				if (callback) callback.call($images);			},			destroy: function(callback) {				this._destroy(callback);			},			_destroy: function(callback) {				this.$imgs.removeClass('stacks_in_40_page1Montage').css({					position: '',					width: '',					height: '',					left: '',					top: ''				}).unwrap();				if (this.options.liquid) $('html').css('overflow', '');				this.element.unbind('.stacks_in_40_page1Montage').removeData('stacks_in_40_page1Montage');				$(window).unbind('.stacks_in_40_page1Montage');				if (callback) callback.call();			},			option: function(key, value) { // set options AFTER initialization:				if ($.isPlainObject(key)) {					this.options = $.extend(true, this.options, key);				}			}		}; // taken from jquery.masonry		// 	 https://github.com/desandro/masonry		// helper function for logging errors		// $.error breaks jQuery chaining		var logError = function(message) {			if (this.console) {				console.error(message);			}		}; // Structure taken from jquery.masonry		// 	 https://github.com/desandro/masonry		// =======================  Plugin bridge  ===============================		// leverages data method to either create or return $.stacks_in_40_page1Montage constructor		// A bit from jQuery UI		//   https://github.com/jquery/jquery-ui/blob/master/ui/jquery.ui.widget.js		// A bit from jcarousel 		//   https://github.com/jsor/jcarousel/blob/master/lib/jquery.jcarousel.js		$.fn.stacks_in_40_page1Montage = function(options) {			if (typeof options === 'string') { // call method				var args = Array.prototype.slice.call(arguments, 1);				this.each(function() {					var instance = $.data(this, 'stacks_in_40_page1Montage');					if (!instance) {						logError("cannot call methods on stacks_in_40_page1Montage prior to initialization; " + "attempted to call method '" + options + "'");						return;					}					if (!$.isFunction(instance[options]) || options.charAt(0) === "_") {						logError("no such method '" + options + "' for stacks_in_40_page1Montage instance");						return;					} // apply method					instance[options].apply(instance, args);				});			} else {				this.each(function() {					var instance = $.data(this, 'stacks_in_40_page1Montage');					if (instance) { // apply options & reload						instance.option(options || {});						instance._reload();					} else { // initialize new instance						$.data(this, 'stacks_in_40_page1Montage', new $.stacks_in_40_page1Montage(options, this));					}				});			}			return this;		};	})(window, jQuery);						$(function() {		var $stacks_in_40_page1container = $('#stacks_in_40_page1am-container'),			$imgs = $stacks_in_40_page1container.find('img').hide(),			totalImgs = $imgs.length,			cnt = 0;					$imgs.each(function(i) {			var $img = $(this);			$img.removeClass("imageStyle");			if ($img.parent("a").length){			var $thelink = $img.parent("a");			$img.unwrap();			$img.unwrap();			$img.parent().wrap($thelink);			}else{			$img.unwrap();			$img.parent().wrap("<a href='#' class='stacks_in_40_page1dummylink'>");			}						$('#stacks_in_40_page1am-container').waitForImages(function() {			//$('<img />').load(function() {				++cnt;				if (cnt === totalImgs) {					$imgs.show();					$stacks_in_40_page1container.stacks_in_40_page1Montage({						liquid                  : true,						minsize                 : false,						fillLastRow             : true,						minw                    : 150, 						minh                    : 200, 						maxh                    : 300,						alternateHeight: true,						alternateHeightRange: {							min: 150,							max: 250						},						margin: 2					});				}			}).attr('src', $img.attr('src'));		});	});					if ("none" == "titlesfade"){	    $(".stacks_in_40_page1am-outerwrapper").mouseenter(function(){	    $(".stacks_in_40_page1completeimageoverlay").css({"opacity": 0.5}).fadeIn();    }).mouseleave(function(){	    $(".stacks_in_40_page1completeimageoverlay").stop().fadeOut();     });            		$("#stacks_in_40_page1 span").hover(function() {		if ($(this).parent("a").length){		$(this).parent().attr("title", "");		$(".stacks_in_40_page1completeimageoverlay", this).show().stop().animate({"opacity": 0});		if ("linktopage" == "lightbox"){		var magpos = ($(this).height() / 2) - 46;		$(".stacks_in_40_page1zoomoverlay", this).css({"background-position":"center " + magpos + "px" });		$(".stacks_in_40_page1zoomoverlay", this).show().stop().animate({"opacity": 1});		}		$(".stacks_in_40_page1imageoverlay", this).stop().slideDown(200);		}			}, function() {		$(".stacks_in_40_page1completeimageoverlay", this).stop().animate({"opacity": 0.5});		if ("linktopage" == "lightbox"){		$(".stacks_in_40_page1zoomoverlay", this).show().stop().animate({"opacity": 0}); 		}		$(".stacks_in_40_page1imageoverlay", this).slideUp(200);		});	   }	   	   	   	   	   if ("none" == "titles"){		$("#stacks_in_40_page1 span").hover(function() {		if ($(this).parent("a").length){		$(this).parent().attr("title", "");		if ("linktopage" == "lightbox"){		var magpos = ($(this).height() / 2) - 46;		$(".stacks_in_40_page1zoomoverlay", this).css({"background-position":"center " + magpos + "px" });		$(".stacks_in_40_page1zoomoverlay", this).show().stop().animate({"opacity": 1});		}		$(".stacks_in_40_page1imageoverlay", this).stop().slideDown(200);		}			}, function() {		if ("linktopage" == "lightbox"){		$(".stacks_in_40_page1zoomoverlay", this).show().stop().animate({"opacity": 0}); 		}		$(".stacks_in_40_page1imageoverlay", this).slideUp(200);		});	   }	   	   	   	   	if ("none" == "none"){		$("#stacks_in_40_page1 span").hover(function() {		if ($(this).parent("a").length){		$(this).parent().attr("title", "");		}			}, function() {				// mouseout stuff not needed		});	    }	   	   	   	  $(".stacks_in_40_page1dummylink").click(function(e) {		   	  e.preventDefault();		  });	  	   	   	   	  if ("linktopage" == "lightbox"){	  $("#stacks_in_40_page1 span").click(function(e) {		var $tempimage = $("<img src='" + $("img", this).attr("src") + "' />"); 		$tempimage.addClass("stacks_in_40_page1clone").stacks_in_40_page1lightbox_me({        centered: true,        overlayCSS: {background : 'Black', opacity : 1},        closeSelector: '.stacks_in_40_page1xbutton',        closeClick: true,        destroyOnClose: true,        zIndex: 1000100,        onLoad: function() { 	                    },        onClose: function() {        	    $(".stacks_in_40_page1popupcontrolbar").fadeOut();            //$(".stacks_in_40_page1clone").remove();            }        });    e.preventDefault();    });	}		}); // end doc ready// End Montage stack Javascript code
	return stack;
})(stacks.stacks_in_40_page1);


// Javascript for stacks_in_38_page1
// ---------------------------------------------------------------------

// Each stack has its own object with its own namespace.  The name of
// that object is the same as the stack's id.
stacks.stacks_in_38_page1 = {};

// A closure is defined and assigned to the stack's object.  The object
// is also passed in as 'stack' which gives you a shorthand for referring
// to this object from elsewhere.
stacks.stacks_in_38_page1 = (function(stack) {

	// When jQuery is used it will be available as $ and jQuery but only
	// inside the closure.
	var jQuery = stacks.jQuery;
	var $ = jQuery;
	
//-- Fluid Columns v1.0.3 Copyright @2010-2012 Joe Workman --//

(function(a,b,c){"use strict";var d=function(a){return a.charAt(0).toUpperCase()+a.slice(1)},e="Moz Webkit Khtml O Ms".split(" "),f=function(a){var b=document.documentElement.style,c;if(typeof b[a]=="string")return a;a=d(a);for(var f=0,g=e.length;f<g;f++){c=e[f]+a;if(typeof b[c]=="string")return c}},g=f("transform"),h=f("transitionProperty"),i={csstransforms:function(){return!!g},csstransforms3d:function(){var a=!!f("perspective");if(a){var c=" -o- -moz- -ms- -webkit- -khtml- ".split(" "),d="@media ("+c.join("transform-3d),(")+"modernizr)",e=b("<style>"+d+"{#modernizr{height:3px}}"+"</style>").appendTo("head"),g=b('<div id="modernizr" />').appendTo("html");a=g.height()===3,g.remove(),e.remove()}return a},csstransitions:function(){return!!h}};if(a.Modernizr)for(var j in i)Modernizr.hasOwnProperty(j)||Modernizr.addTest(j,i[j]);else a.Modernizr=function(){var a={_version:"1.6ish: miniModernizr for Isotope"},c=" ",d,e;for(e in i)d=i[e](),a[e]=d,c+=" "+(d?"":"no-")+e;b("html").addClass(c);return a}();if(Modernizr.csstransforms){var k=Modernizr.csstransforms3d?{translate:function(a){return"translate3d("+a[0]+"px, "+a[1]+"px, 0) "},scale:function(a){return"scale3d("+a+", "+a+", 1) "}}:{translate:function(a){return"translate("+a[0]+"px, "+a[1]+"px) "},scale:function(a){return"scale("+a+") "}},l=function(a,c,d){var e=b.data(a,"isoTransform")||{},f={},h,i={},j;f[c]=d,b.extend(e,f);for(h in e)j=e[h],i[h]=k[h](j);var l=i.translate||"",m=i.scale||"",n=l+m;b.data(a,"isoTransform",e),a.style[g]=n};b.cssNumber.scale=!0,b.cssHooks.scale={set:function(a,b){l(a,"scale",b)},get:function(a,c){var d=b.data(a,"isoTransform");return d&&d.scale?d.scale:1}},b.fx.step.scale=function(a){b.cssHooks.scale.set(a.elem,a.now+a.unit)},b.cssNumber.translate=!0,b.cssHooks.translate={set:function(a,b){l(a,"translate",b)},get:function(a,c){var d=b.data(a,"isoTransform");return d&&d.translate?d.translate:[0,0]}}}var m,n;Modernizr.csstransitions&&(m={WebkitTransitionProperty:"webkitTransitionEnd",MozTransitionProperty:"transitionend",OTransitionProperty:"oTransitionEnd",transitionProperty:"transitionEnd"}[h],n=f("transitionDuration"));var o=b.event,p;o.special.smartresize={setup:function(){b(this).bind("resize",o.special.smartresize.handler)},teardown:function(){b(this).unbind("resize",o.special.smartresize.handler)},handler:function(a,b){var c=this,d=arguments;a.type="smartresize",p&&clearTimeout(p),p=setTimeout(function(){jQuery.event.handle.apply(c,d)},b==="execAsap"?0:100)}},b.fn.smartresize=function(a){return a?this.bind("smartresize",a):this.trigger("smartresize",["execAsap"])},b.Isotope=function(a,c,d){this.element=b(c),this._create(a),this._init(d)};var q=["overflow","position","width","height"],r=b(a);b.Isotope.settings={resizable:!0,layoutMode:"masonry",containerClass:"isotope",itemClass:"isotope-item",hiddenClass:"isotope-hidden",hiddenStyle:{opacity:0,scale:.001},visibleStyle:{opacity:1,scale:1},animationEngine:"best-available",animationOptions:{queue:!1,duration:800},sortBy:"original-order",sortAscending:!0,resizesContainer:!0,transformsEnabled:!b.browser.opera,itemPositionDataEnabled:!1},b.Isotope.prototype={_create:function(a){this.options=b.extend({},b.Isotope.settings,a),this.styleQueue=[],this.elemCount=0;var c=this.element[0].style;this.originalStyle={};for(var d=0,e=q.length;d<e;d++){var f=q[d];this.originalStyle[f]=c[f]||""}this.element.css({overflow:"hidden",position:"relative"}),this._updateAnimationEngine(),this._updateUsingTransforms();var g={"original-order":function(a,b){b.elemCount++;return b.elemCount},random:function(){return Math.random()}};this.options.getSortData=b.extend(this.options.getSortData,g),this.reloadItems();var h=b(document.createElement("div")).prependTo(this.element);this.offset=h.position(),h.remove();var i=this;setTimeout(function(){i.element.addClass(i.options.containerClass)},0),this.options.resizable&&r.bind("smartresize.isotope",function(){i.resize()}),this.element.delegate("."+this.options.hiddenClass,"click",function(){return!1})},_getAtoms:function(a){var b=this.options.itemSelector,c=b?a.filter(b).add(a.find(b)):a,d={position:"absolute"};this.usingTransforms&&(d.left=0,d.top=0),c.css(d).addClass(this.options.itemClass),this.updateSortData(c,!0);return c},_init:function(a){this.$filteredAtoms=this._filter(this.$allAtoms),this._sort(),this.reLayout(a)},option:function(a){if(b.isPlainObject(a)){this.options=b.extend(!0,this.options,a);var c;for(var e in a)c="_update"+d(e),this[c]&&this[c]()}},_updateAnimationEngine:function(){var a=this.options.animationEngine.toLowerCase().replace(/[ _\-]/g,""),b;switch(a){case"css":case"none":b=!1;break;case"jquery":b=!0;break;default:b=!Modernizr.csstransitions}this.isUsingJQueryAnimation=b,this._updateUsingTransforms()},_updateTransformsEnabled:function(){this._updateUsingTransforms()},_updateUsingTransforms:function(){var a=this.usingTransforms=this.options.transformsEnabled&&Modernizr.csstransforms&&Modernizr.csstransitions&&!this.isUsingJQueryAnimation;a||(delete this.options.hiddenStyle.scale,delete this.options.visibleStyle.scale),this.getPositionStyles=a?this._translate:this._positionAbs},_filter:function(a){var b=this.options.filter===""?"*":this.options.filter;if(!b)return a;var c=this.options.hiddenClass,d="."+c,e=a.filter(d),f=e;if(b!=="*"){f=e.filter(b);var g=a.not(d).not(b).addClass(c);this.styleQueue.push({$el:g,style:this.options.hiddenStyle})}this.styleQueue.push({$el:f,style:this.options.visibleStyle}),f.removeClass(c);return a.filter(b)},updateSortData:function(a,c){var d=this,e=this.options.getSortData,f,g;a.each(function(){f=b(this),g={};for(var a in e)!c&&a==="original-order"?g[a]=b.data(this,"isotope-sort-data")[a]:g[a]=e[a](f,d);b.data(this,"isotope-sort-data",g)})},_sort:function(){var a=this.options.sortBy,b=this._getSorter,c=this.options.sortAscending?1:-1,d=function(d,e){var f=b(d,a),g=b(e,a);f===g&&a!=="original-order"&&(f=b(d,"original-order"),g=b(e,"original-order"));return(f>g?1:f<g?-1:0)*c};this.$filteredAtoms.sort(d)},_getSorter:function(a,c){return b.data(a,"isotope-sort-data")[c]},_translate:function(a,b){return{translate:[a,b]}},_positionAbs:function(a,b){return{left:a,top:b}},_pushPosition:function(a,b,c){b+=this.offset.left,c+=this.offset.top;var d=this.getPositionStyles(b,c);this.styleQueue.push({$el:a,style:d}),this.options.itemPositionDataEnabled&&a.data("isotope-item-position",{x:b,y:c})},layout:function(a,b){var c=this.options.layoutMode;this["_"+c+"Layout"](a);if(this.options.resizesContainer){var d=this["_"+c+"GetContainerSize"]();this.styleQueue.push({$el:this.element,style:d})}this._processStyleQueue(a,b),this.isLaidOut=!0},_processStyleQueue:function(a,c){var d=this.isLaidOut?this.isUsingJQueryAnimation?"animate":"css":"css",e=this.options.animationOptions,f,g,h,i;g=function(a,b){b.$el[d](b.style,e)};if(this._isInserting&&this.isUsingJQueryAnimation)g=function(a,b){f=b.$el.hasClass("no-transition")?"css":d,b.$el[f](b.style,e)};else if(c){var j=!1,k=this;h=!0,i=function(){j||(c.call(k.element,a),j=!0)};if(this.isUsingJQueryAnimation&&d==="animate")e.complete=i,h=!1;else if(Modernizr.csstransitions){var l=0,o=this.styleQueue[0].$el,p;while(!o.length){p=this.styleQueue[l++];if(!p)return;o=p.$el}var q=parseFloat(getComputedStyle(o[0])[n]);q>0&&(g=function(a,b){b.$el[d](b.style,e).one(m,i)},h=!1)}}b.each(this.styleQueue,g),h&&i(),this.styleQueue=[]},resize:function(){this["_"+this.options.layoutMode+"ResizeChanged"]()&&this.reLayout()},reLayout:function(a){this["_"+this.options.layoutMode+"Reset"](),this.layout(this.$filteredAtoms,a)},addItems:function(a,b){var c=this._getAtoms(a);this.$allAtoms=this.$allAtoms.add(c),b&&b(c)},insert:function(a,b){this.element.append(a);var c=this;this.addItems(a,function(a){var d=c._filter(a);c._addHideAppended(d),c._sort(),c.reLayout(),c._revealAppended(d,b)})},appended:function(a,b){var c=this;this.addItems(a,function(a){c._addHideAppended(a),c.layout(a),c._revealAppended(a,b)})},_addHideAppended:function(a){this.$filteredAtoms=this.$filteredAtoms.add(a),a.addClass("no-transition"),this._isInserting=!0,this.styleQueue.push({$el:a,style:this.options.hiddenStyle})},_revealAppended:function(a,b){var c=this;setTimeout(function(){a.removeClass("no-transition"),c.styleQueue.push({$el:a,style:c.options.visibleStyle}),c._isInserting=!1,c._processStyleQueue(a,b)},10)},reloadItems:function(){this.$allAtoms=this._getAtoms(this.element.children())},remove:function(a){var b=this,c=function(){b.$allAtoms=b.$allAtoms.not(a),a.remove()};a.filter(":not(."+this.options.hiddenClass+")").length?(this.styleQueue.push({$el:a,style:this.options.hiddenStyle}),this.$filteredAtoms=this.$filteredAtoms.not(a),this._sort(),this.reLayout(c)):c()},shuffle:function(a){this.updateSortData(this.$allAtoms),this.options.sortBy="random",this._sort(),this.reLayout(a)},destroy:function(){var a=this.usingTransforms,b=this.options;this.$allAtoms.removeClass(b.hiddenClass+" "+b.itemClass).each(function(){var b=this.style;b.position="",b.top="",b.left="",b.opacity="",a&&(b[g]="")});var c=this.element[0].style;for(var d=0,e=q.length;d<e;d++){var f=q[d];c[f]=this.originalStyle[f]}this.element.unbind(".isotope").undelegate("."+b.hiddenClass,"click").removeClass(b.containerClass).removeData("isotope"),r.unbind(".isotope")},_getSegments:function(a){var b=this.options.layoutMode,c=a?"rowHeight":"columnWidth",e=a?"height":"width",f=a?"rows":"cols",g=this.element[e](),h,i=this.options[b]&&this.options[b][c]||this.$filteredAtoms["outer"+d(e)](!0)||g;h=Math.floor(g/i),h=Math.max(h,1),this[b][f]=h,this[b][c]=i},_checkIfSegmentsChanged:function(a){var b=this.options.layoutMode,c=a?"rows":"cols",d=this[b][c];this._getSegments(a);return this[b][c]!==d},_masonryReset:function(){this.masonry={},this._getSegments();var a=this.masonry.cols;this.masonry.colYs=[];while(a--)this.masonry.colYs.push(0)},_masonryLayout:function(a){var c=this,d=c.masonry;a.each(function(){var a=b(this),e=Math.ceil(a.outerWidth(!0)/d.columnWidth);e=Math.min(e,d.cols);if(e===1)c._masonryPlaceBrick(a,d.colYs);else{var f=d.cols+1-e,g=[],h,i;for(i=0;i<f;i++)h=d.colYs.slice(i,i+e),g[i]=Math.max.apply(Math,h);c._masonryPlaceBrick(a,g)}})},_masonryPlaceBrick:function(a,b){var c=Math.min.apply(Math,b),d=0;for(var e=0,f=b.length;e<f;e++)if(b[e]===c){d=e;break}var g=this.masonry.columnWidth*d,h=c;this._pushPosition(a,g,h);var i=c+a.outerHeight(!0),j=this.masonry.cols+1-f;for(e=0;e<j;e++)this.masonry.colYs[d+e]=i},_masonryGetContainerSize:function(){var a=Math.max.apply(Math,this.masonry.colYs);return{height:a}},_masonryResizeChanged:function(){return this._checkIfSegmentsChanged()},_fitRowsReset:function(){this.fitRows={x:0,y:0,height:0}},_fitRowsLayout:function(a){var c=this,d=this.element.width(),e=this.fitRows;a.each(function(){var a=b(this),f=a.outerWidth(!0),g=a.outerHeight(!0);e.x!==0&&f+e.x>d&&(e.x=0,e.y=e.height),c._pushPosition(a,e.x,e.y),e.height=Math.max(e.y+g,e.height),e.x+=f})},_fitRowsGetContainerSize:function(){return{height:this.fitRows.height}},_fitRowsResizeChanged:function(){return!0},_cellsByRowReset:function(){this.cellsByRow={index:0},this._getSegments(),this._getSegments(!0)},_cellsByRowLayout:function(a){var c=this,d=this.cellsByRow;a.each(function(){var a=b(this),e=d.index%d.cols,f=Math.floor(d.index/d.cols),g=Math.round((e+.5)*d.columnWidth-a.outerWidth(!0)/2),h=Math.round((f+.5)*d.rowHeight-a.outerHeight(!0)/2);c._pushPosition(a,g,h),d.index++})},_cellsByRowGetContainerSize:function(){return{height:Math.ceil(this.$filteredAtoms.length/this.cellsByRow.cols)*this.cellsByRow.rowHeight+this.offset.top}},_cellsByRowResizeChanged:function(){return this._checkIfSegmentsChanged()},_straightDownReset:function(){this.straightDown={y:0}},_straightDownLayout:function(a){var c=this;a.each(function(a){var d=b(this);c._pushPosition(d,0,c.straightDown.y),c.straightDown.y+=d.outerHeight(!0)})},_straightDownGetContainerSize:function(){return{height:this.straightDown.y}},_straightDownResizeChanged:function(){return!0},_masonryHorizontalReset:function(){this.masonryHorizontal={},this._getSegments(!0);var a=this.masonryHorizontal.rows;this.masonryHorizontal.rowXs=[];while(a--)this.masonryHorizontal.rowXs.push(0)},_masonryHorizontalLayout:function(a){var c=this,d=c.masonryHorizontal;a.each(function(){var a=b(this),e=Math.ceil(a.outerHeight(!0)/d.rowHeight);e=Math.min(e,d.rows);if(e===1)c._masonryHorizontalPlaceBrick(a,d.rowXs);else{var f=d.rows+1-e,g=[],h,i;for(i=0;i<f;i++)h=d.rowXs.slice(i,i+e),g[i]=Math.max.apply(Math,h);c._masonryHorizontalPlaceBrick(a,g)}})},_masonryHorizontalPlaceBrick:function(a,b){var c=Math.min.apply(Math,b),d=0;for(var e=0,f=b.length;e<f;e++)if(b[e]===c){d=e;break}var g=c,h=this.masonryHorizontal.rowHeight*d;this._pushPosition(a,g,h);var i=c+a.outerWidth(!0),j=this.masonryHorizontal.rows+1-f;for(e=0;e<j;e++)this.masonryHorizontal.rowXs[d+e]=i},_masonryHorizontalGetContainerSize:function(){var a=Math.max.apply(Math,this.masonryHorizontal.rowXs);return{width:a}},_masonryHorizontalResizeChanged:function(){return this._checkIfSegmentsChanged(!0)},_fitColumnsReset:function(){this.fitColumns={x:0,y:0,width:0}},_fitColumnsLayout:function(a){var c=this,d=this.element.height(),e=this.fitColumns;a.each(function(){var a=b(this),f=a.outerWidth(!0),g=a.outerHeight(!0);e.y!==0&&g+e.y>d&&(e.x=e.width,e.y=0),c._pushPosition(a,e.x,e.y),e.width=Math.max(e.x+f,e.width),e.y+=g})},_fitColumnsGetContainerSize:function(){return{width:this.fitColumns.width}},_fitColumnsResizeChanged:function(){return!0},_cellsByColumnReset:function(){this.cellsByColumn={index:0},this._getSegments(),this._getSegments(!0)},_cellsByColumnLayout:function(a){var c=this,d=this.cellsByColumn;a.each(function(){var a=b(this),e=Math.floor(d.index/d.rows),f=d.index%d.rows,g=Math.round((e+.5)*d.columnWidth-a.outerWidth(!0)/2),h=Math.round((f+.5)*d.rowHeight-a.outerHeight(!0)/2);c._pushPosition(a,g,h),d.index++})},_cellsByColumnGetContainerSize:function(){return{width:Math.ceil(this.$filteredAtoms.length/this.cellsByColumn.rows)*this.cellsByColumn.columnWidth}},_cellsByColumnResizeChanged:function(){return this._checkIfSegmentsChanged(!0)},_straightAcrossReset:function(){this.straightAcross={x:0}},_straightAcrossLayout:function(a){var c=this;a.each(function(a){var d=b(this);c._pushPosition(d,c.straightAcross.x,0),c.straightAcross.x+=d.outerWidth(!0)})},_straightAcrossGetContainerSize:function(){return{width:this.straightAcross.x}},_straightAcrossResizeChanged:function(){return!0}},b.fn.imagesLoaded=function(a){function i(a){a.target.src!==f&&b.inArray(this,g)===-1&&(g.push(this),--e<=0&&(setTimeout(h),d.unbind(".imagesLoaded",i)))}function h(){a.call(c,d)}var c=this,d=c.find("img").add(c.filter("img")),e=d.length,f="data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///ywAAAAAAQABAAACAUwAOw==",g=[];e||h(),d.bind("load.imagesLoaded error.imagesLoaded",i).each(function(){var a=this.src;this.src=f,this.src=a});return c};var s=function(b){a.console&&a.console.error(b)};b.fn.isotope=function(a,c){if(typeof a=="string"){var d=Array.prototype.slice.call(arguments,1);this.each(function(){var c=b.data(this,"isotope");if(!c)s("cannot call methods on isotope prior to initialization; attempted to call method '"+a+"'");else{if(!b.isFunction(c[a])||a.charAt(0)==="_"){s("no such method '"+a+"' for isotope instance");return}c[a].apply(c,d)}})}else this.each(function(){var d=b.data(this,"isotope");d?(d.option(a),d._init(c)):b.data(this,"isotope",new b.Isotope(a,this,c))});return this}})(window,jQuery);$(document).ready(function(){$('#stacks_in_38_page1').isotope({itemSelector:'.fluid_cell',layoutMode:'fitRows',animationEngine:'best-available'});});

	return stack;
})(stacks.stacks_in_38_page1);


// Javascript for stacks_in_5_page1
// ---------------------------------------------------------------------

// Each stack has its own object with its own namespace.  The name of
// that object is the same as the stack's id.
stacks.stacks_in_5_page1 = {};

// A closure is defined and assigned to the stack's object.  The object
// is also passed in as 'stack' which gives you a shorthand for referring
// to this object from elsewhere.
stacks.stacks_in_5_page1 = (function(stack) {

	// When jQuery is used it will be available as $ and jQuery but only
	// inside the closure.
	var jQuery = stacks.jQuery;
	var $ = jQuery;
	
// Start Contact Form stack Javascript code


$(document).ready(function(){


	$(".stacks_in_5_page1sendmail").click(function(){
		var comment = $("#stacks_in_5_page1comment").val();
		if(comment == "comment"){
		
		$(".stacks_in_5_page1response").css({"background":"#FEF4FA", "border":"1px solid #D6392B"});
		if("none" == "block"){
		var autoreplySubject = "Thank you for contacting us";
		autoreplySubject = escape(autoreplySubject);
		var autoreply = $(".stacks_in_5_page1autoreplayInner").text();
		autoreply = escape(autoreply);
		var autoreplysig = $(".stacks_in_5_page1autoreplaySig").html();
		autoreplysig = escape(autoreplysig);
		}else{
		var autoreply = "noreply";
		var autoreplySubject = "No Subject";
		}
		var namelabel = $(".stacks_in_5_page1namelabel").text();
		var emaillabel = $(".stacks_in_5_page1emaillabel").text();
		var subjectlabel = $(".stacks_in_5_page1subjectlabel").text();
		var messagelabel = $(".stacks_in_5_page1messagelabel").text();
		
		var owneremail = "obscureinfo@hetrondjeeilanden.nl";
		
		var valid = '';
		var isr = ' is required.';
		var name = $("#stacks_in_5_page1name").val();
		var mail = $("#stacks_in_5_page1mail").val();
		var subject = $("#stacks_in_5_page1subject").val();
		var text = $("#stacks_in_5_page1text").val();
		text = encodeURIComponent(text);

		
		if (name.length<1) {
			valid += '<br />' + namelabel +isr;
		}
		if (!mail.match(/^([a-z0-9._-]+@[a-z0-9._-]+\.[a-z]{2,4}$)/i)) {
			valid += '<br />' + emaillabel +isr;
		}
		if (subject.length<1) {
			valid += '<br />' + subjectlabel +isr;
		}
		if (text.length<1) {
			valid += '<br />' + messagelabel +isr;
		}
		if (valid!='') {
			$(".stacks_in_5_page1response").html("Error:"+valid);
			$(".stacks_in_5_page1response").slideDown(300);
		}
		else {
			var datastr ='owneremail=' + owneremail + '&namelabel=' + namelabel + '&emaillabel=' + emaillabel + '&subjectlabel=' + subjectlabel + '&messagelabel=' + messagelabel + '&autoreply=' + autoreply + '&autoreplysig=' + autoreplysig + '&name=' + name + '&mail=' + mail + '&subject=' + subject + '&text=' + text + '&autosubject=' + autoreplySubject;
			$(".stacks_in_5_page1response").css({"background":"#ffffff", "border":"1px solid #60D667"});
			$(".stacks_in_5_page1response").html("<div class='stacks_in_5_page1loader'><img src='files/contactAssets/loader.gif' /></div>Bericht wordt verstuurd .... ");
			$(".stacks_in_5_page1response").slideDown(300);
			setTimeout(function(){
			stacks_in_5_page1send(datastr);
			}, 2000);
		}
		} // end if comment
		return false;
	});
	
	
	
	function stacks_in_5_page1clearform(){
		$("#stacks_in_5_page1name").val("");
		$("#stacks_in_5_page1mail").val("");
		$("#stacks_in_5_page1subject").val("");
		$("#stacks_in_5_page1text").val("");
	}
	
	
	
	function stacks_in_5_page1send(datastr){
	$.ajax({	
		type: "POST",
		url: "files/contactAssets/contactform.php",
		data: datastr,
		cache: false,
		success: function(data){
		 if(data.indexOf("success") >= 0){
		     $(".stacks_in_5_page1response").fadeIn("slow");
		     $(".stacks_in_5_page1response").css({"background":"#ffffff", "border":"1px solid #60D667"});
			 $(".stacks_in_5_page1response").html("Thank you! Je bericht is verstuurd.");
			 stacks_in_5_page1clearform();
			 setTimeout(function(){
			 $(".stacks_in_5_page1response").fadeOut("slow")
			 },2000);
		 }else{
		 	 $(".stacks_in_5_page1response").fadeIn("slow");
    		 $(".stacks_in_5_page1response").css({"background":"#FEF4FA", "border":"1px solid #D6392B"});
			 $(".stacks_in_5_page1response").html("Oeps… Er ging iets mis! Probeer het nog eens...");
			 setTimeout(function(){
			 $(".stacks_in_5_page1response").fadeOut("slow")
			 },2000);
		 }
		},
		error: function(){
   			 $(".stacks_in_5_page1response").fadeIn("slow");
    		 $(".stacks_in_5_page1response").css({"background":"#FEF4FA", "border":"1px solid #D6392B"});
			 $(".stacks_in_5_page1response").html(data);
			 setTimeout(function(){
			 $(".stacks_in_5_page1response").fadeOut("slow")
			 },2000);
  		}

	});
	}

	
	
});



// End Contact Form stack Javascript code
	return stack;
})(stacks.stacks_in_5_page1);


// Javascript for stacks_in_12_page1
// ---------------------------------------------------------------------

// Each stack has its own object with its own namespace.  The name of
// that object is the same as the stack's id.
stacks.stacks_in_12_page1 = {};

// A closure is defined and assigned to the stack's object.  The object
// is also passed in as 'stack' which gives you a shorthand for referring
// to this object from elsewhere.
stacks.stacks_in_12_page1 = (function(stack) {

	// When jQuery is used it will be available as $ and jQuery but only
	// inside the closure.
	var jQuery = stacks.jQuery;
	var $ = jQuery;
	

// xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
// TWEETS STACK BY http://www.doobox.co.uk XXXXXXX
// COPYRIGHT@2010 MR JG SIMPSON, TRADING AS DOOBOX
// ALL RIGHTS RESERVED XXXXXXXXXXXXXXXXXXXXXXXXXXX
// xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx




// DOCUMENT READY FUNCTIONS
$(document).ready(function() {

(function($){
	
	$.fn.stacks_in_12_page1twitterfeed = function(username, options) {	
	
		// Set pluign defaults
		var defaults = {
			limit: 3,
			tweeticon: true,
			retweets: true,
			replies: true,
			ssl: false
		};  
		var options = $.extend(defaults, options); 
		
		// Functions
		return this.each(function(i, e) {
			var $e = $(e);
			var s = '';
			
			// Add feed class to user div
			if (!$e.hasClass('stacks_in_12_page1twitterFeed')) $e.addClass('stacks_in_12_page1twitterFeed');
			
			// Check for valid user name
			if (username == null) return false;

			// Check limit does not exceed max
			if (options.limit > 200) options.limit = 200;

			// Check for SSL protocol
			if (options.ssl) s = 's';

			// Reverse replies option
			if (options.replies == true) { options.replies = false; } else { options.replies = true; }

			// Define Twitter feed request
			var url = 'http'+ s +'://api.twitter.com/1/statuses/user_timeline.json?include_rts='+ options.retweets +'&exclude_replies='+ options.replies +'&screen_name='+ username +'&count='+ options.limit;
			var params = {};

			params.count = options.limit;

			// Send request
			jQuery.ajax({
				url: url,
				data: params,
				dataType: 'jsonp',
				success: function (o) {
					_callback(e, o, options);
				}
			});
				
		});
	};
	
	// Callback function to create HTML result
	var _callback = function(e, feeds, options) {
		if (!feeds) {
			return false;
		}
		var html = '';	
		

		// Add body
		html += '<ul>';
		
		// Add feeds
		for (var i=0; i<feeds.length; i++) {
			
			// Get individual feed
			if (feeds[i].retweeted_status) {
				var tweet= feeds[i].retweeted_status;
			} else {
				var tweet= feeds[i];
			}
			var link = '<a href="http://twitter.com/' + tweet.user.screen_name + '/" title="Visit '+ tweet.user.name +' on Twitter">';

			// Add feed row
			html += '<li class="stacks_in_12_page1twitterRow"><div class="stacks_in_12_page1rowinner">';

			// Add user icon if required
			if ("0" == "show") {
				var icon = tweet.user.profile_image_url;

				html += link + '<img src="'+ icon +'" alt="'+ name +'" /></a>';
			}
			
			if ("0" == "custom") {
				html += link + '<img src="files/testimagecontrol_12.png" alt="'+ name +'" /></a>';
			}
		
			
			// Add lapsed time if required
			if ("none" == "top") {
				var lapsedTime = getLapsedTime(tweet.created_at);
				html += '<div class="stacks_in_12_page1tweetTime">'+ lapsedTime +'</div>'
			}
			
			
			html += '<div class="stacks_in_12_page1bodycontainer">';
			
			

			// Add user if required
				var name = tweet.user.name;
				if ("none" == "username") {
				html += '<div class="stacks_in_12_page1tweetName">'+ link + name +'</a></div>'
				}else if("none" == "custom"){
				html += '<div class="stacks_in_12_page1tweetName">Recently Tweeted</div>'
				}else{
				html += ""
				}
			
			
			// Get tweet text and add links (by Yusuke Horie)
			var text = tweet.text
				.replace(/(https?:\/\/[-_.!~*\'()a-zA-Z0-9;\/?:\@&=+\$,%#]+)/, function (u) {
					var shortUrl = (u.length > 30) ? u.substr(0, 30) + '…': u;
					return '<a href="' + u + '" title="Click to view this link">' + shortUrl + '</a>';
				})
				.replace(/@([a-zA-Z0-9_]+)/g, '@<a href="http://twitter.com/$1" title="Click to view $1 on Twitter">$1</a>')
				.replace(/(?:^|\s)#([^\s\.\+:!]+)/g, function (a, u) {
					return ' <a href="http://twitter.com/search?q=' + encodeURIComponent(u) + '" title="Click to view this on Twitter">#' + u + '</a>';
			});
			html += '<div class="stacks_in_12_page1tweetmessage">'+ text+'</div>'
			html += '</div>';
			
			// Add lapsed time if required
			if ("none" == "bottom") {
				var lapsedTime = getLapsedTime(tweet.created_at);
				html += '<div class="stacks_in_12_page1tweetTime">'+ lapsedTime +'</div>'
			}
			
			
			html += '<div style="clear:both;"></div></div></li>';
						
		}
		
		html += '</ul>' +
			'</div>'
		
		$(e).html(html);
	};

	function getLapsedTime(strDate) {
		
		// Reformat Twitter date so that IE can convert
		strDate = Date.parse(strDate.replace(/^([a-z]{3})( [a-z]{3} \d\d?)(.*)( \d{4})$/i, '$1,$2$4$3'));

		// Define current time and format tweet date
		var todayDate = new Date();	
		var tweetDate = new Date(strDate)

		// Get lasped time in seconds
		var lapsedTime = Math.round((todayDate.getTime() - tweetDate.getTime())/1000)

		// Return lasped time in seconds, minutes, hours, days and weeks
		if (lapsedTime < 60) {
			return 'JP';
		} else if (lapsedTime < (60*60)) {
			return (Math.round(lapsedTime / 60)) + 'm';
		} else if (lapsedTime < (24*60*60)) {
			return (Math.round(lapsedTime / 3600)) + 'h';  //removed -1 from hour not so sure yet??
		} else if (lapsedTime < (7*24*60*60)) {
			return (Math.round(lapsedTime / 86400)) + 'd';  //removed -1 from day not so sure yet??
		} else {
			return (Math.round(lapsedTime / 604800)) + 'w'; //removed -1 from week not so sure yet??
		}
	};
})(jQuery);



$(document).ready(function () {

	$('.stacks_in_12_page1tweets').stacks_in_12_page1twitterfeed('trondjeeilanden', {
		limit: 3
	});
	
	var itsIEnine = navigator.userAgent.match(/MSIE 9/i) != null;

	if(itsIEnine){
	$(".stacks_in_12_page1rowinner").css({
    "background" : "#F2F2F2"
    });
	}
	
});


});





// xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
// END DOOBOX TWEETS STACK XXXXXXXXXXXXXXXXXXXX
// xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
	return stack;
})(stacks.stacks_in_12_page1);


