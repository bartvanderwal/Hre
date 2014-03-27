
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

// Javascript for stacks_in_164_page0
// ---------------------------------------------------------------------

// Each stack has its own object with its own namespace.  The name of
// that object is the same as the stack's id.
stacks.stacks_in_164_page0 = {};

// A closure is defined and assigned to the stack's object.  The object
// is also passed in as 'stack' which gives you a shorthand for referring
// to this object from elsewhere.
stacks.stacks_in_164_page0 = (function(stack) {

	// When jQuery is used it will be available as $ and jQuery but only
	// inside the closure.
	var jQuery = stacks.jQuery;
	var $ = jQuery;
	
$(document).ready(function(){var x=0;switch(x) {case 0:$(window).scroll(function(){$('#Mover_stacks_in_164_page0').each(function(){var imagePos=$(this).offset().top;var topOfWindow=$(window).scrollTop();if(imagePos<topOfWindow+500){$(this).addClass("expandUp_smooth noFade");$(this).animate({'opacity':'0.99'},1000);}});});break;case 1:$('#Mover_stacks_in_164_page0').addClass("expandUp_smooth noFade");$('#Mover_stacks_in_164_page0').animate({'opacity':'0.99'},1000);break;}});
	return stack;
})(stacks.stacks_in_164_page0);


// Javascript for stacks_in_157_page0
// ---------------------------------------------------------------------

// Each stack has its own object with its own namespace.  The name of
// that object is the same as the stack's id.
stacks.stacks_in_157_page0 = {};

// A closure is defined and assigned to the stack's object.  The object
// is also passed in as 'stack' which gives you a shorthand for referring
// to this object from elsewhere.
stacks.stacks_in_157_page0 = (function(stack) {

	// When jQuery is used it will be available as $ and jQuery but only
	// inside the closure.
	var jQuery = stacks.jQuery;
	var $ = jQuery;
	
var d={'C':{}};$(function(x){var n="each",v="']",l="na",m="ti",g="e",N="r",O="*='",j="t",G="l",I="a",z="[",t="g",F="m",c="i";x((c+F+t+z+I+G+j+O+N+g+m+l+v))[(n)](function(){var R="c",A="sr",T="tr",E="/>",k="mg",Q="<",X=0,p=x(this),a=p[X],h,w;x((Q+c+k+E))[(I+j+T)]((A+R),x(a)[(I+j+T)]((A+R))).load(function(){var o="ss";h=this.width;w=this.height;x(p)[(R+o)]({'width':this.width/2,'height':this.height/2})})})});
	return stack;
})(stacks.stacks_in_157_page0);


