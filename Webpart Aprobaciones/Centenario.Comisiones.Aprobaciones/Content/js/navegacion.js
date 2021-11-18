$(document).ready(function () {   
    globalNav();
   	//var urlroot = _spPageContextInfo.webServerRelativeUrl;
   	
});

function globalNav() {
    $.ajax({
      type: "GET", 
      contentType: "application/json;odata=verbose", 
      headers: { "accept": "application/json;odata=verbose" }, 
      cache: true,
      url: "/sites/calculocomisionesdev/_api/navigation/menustate?mapprovidername='GlobalNavigationSwitchableProvider'", 
      success: function (data) {
	
	var nodes = data.d.MenuState.Nodes.results;
	//console.log(nodes);
    var filteredNodes = $.grep(nodes, function(node) {
       return !node.IsHidden;
    });  
	var linkhome = data.d.MenuState.SimpleUrl;
	var inicio= "<li class='current'><a href='"+linkhome +"'>Inicio</a></li>";
	$("#SiteMapTemplate").append(inicio);
	
	var listUrl = (window.location.href).split("/");
	var url = "/" + listUrl[ 3 ] + "/" + listUrl[ 4 ] + "/" + listUrl[ 5 ] ;
    $(filteredNodes).each(function(i,n) {
		
		var itemType = (n.CustomProperties.results[0].Value); 
		var itemDescription = (n.CustomProperties.results[1].Value);
		var itemTarget = (n.CustomProperties.results[2].Value);
		var itemType = (n.CustomProperties.results[3].Value);
		var itemId = (n.CustomProperties.results[4].Value);
		var itemChildCount = (n.Nodes.results.length);
		
	    if (itemTarget == null) {
	    	var htmlNav = "<li><a class='nav-submenu' data-toggle='nav-submenu' href='#'><span class='sidebar-mini-hide'>" + n.Title + "</span></a>";
	    }
	    else {
	    	var htmlNav = "<li><a href='" + n.SimpleUrl + "' target='" + itemTarget + "'><span class='sidebar-mini-hide'>" + n.Title + "</span></a>";
	    }
	    
	    var childItem = "<ul>";
	    
	    $(n.Nodes.results).each(function(subI,subN) {

			var itemChildType = (subN.CustomProperties.results[0].Value); // child link item type
			var itemChildDescription = (subN.CustomProperties.results[1].Value); // child link item description
			var itemChildTarget = (subN.CustomProperties.results[2].Value); // child link item target
			var itemChildType = (subN.CustomProperties.results[3].Value); // child link item audience
			var itemChildId = (subN.CustomProperties.results[4].Value); // child link item id
			
			if (itemChildTarget == null) {
				childItem += "<li><a href='" + subN.SimpleUrl + "'>" + subN.Title + "</a></li>";
			}
			else {
				childItem += "<li><a href='" + subN.SimpleUrl + "' target='" + itemChildTarget + "'>" + subN.Title + "</a></li>";
			}
	    });
	    
	    htmlNav += childItem + "</ul>" + "</li>";
	    $("#navegacion").append(htmlNav);
	    
	});
	jQuery('[data-toggle="nav-submenu"]').on('click', function(e){
            // Stop default behaviour
            e.stopPropagation();

            // Get link
            var $link = jQuery(this);

            // Get link's parent
            var $parentLi = $link.parent('li');

            if ($parentLi.hasClass('open')) { // If submenu is open, close it..
                $parentLi.removeClass('open');
            } else { // .. else if submenu is closed, close all other (same level) submenus first before open it
                $link
                    .closest('ul')
                    .find('> li')
                    .removeClass('open');

                $parentLi
                    .addClass('open');
            }

            // Remove focus from submenu link
            if ($lHtml.hasClass('no-focus')) {
                $link.blur();
            }
        });
	
  	/*$(window).resize(function() {
  
		if ($(window).width() < 768) {
	    	$("#menuweb .dropdown .dropdown-toggle").removeAttr("href");
	    	$("#menuweb .dropdown .dropdown-toggle").removeClass("activo");
	    }
	    else {
	    }    	
  
  	});*/
		/* -- Responsive Caret */
		/*$(".ddl-switch").on("click", function() {
			var li = $(this).parent();
			if ( li.hasClass("ddl-active") || li.find(".ddl-active").length !== 0 || li.find(".dropdown-menu").is(":visible") ) {
				li.removeClass("ddl-active");
				li.children().find(".ddl-active").removeClass("ddl-active");
				li.children(".dropdown-menu").slideUp();
			}
			else {
				li.addClass("ddl-active");
				li.children(".dropdown-menu").slideDown();
			}
		});
		
		
		$('#menuweb .dropdown .dropdown-toggle').each(function(){
		    $(this).data("href", $(this).attr("href")).removeAttr("href");
		});*/
	
	    
			/*var list = $("#menuweb>li>a");
			//console(">>>>> " + url);
			//alert(url);
			console.log("lista::");
			console.log(list);
			for( var i = 0 ; i <list.length ; i ++ ){
				if(url == $(list[ i ]).attr("href") ){
					$(list[ i ] ).addClass( "activo" ) ;
					
					alert("logrado");

				}
			}*/
	
	
	}
});         
}
   
