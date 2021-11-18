"use strict";

// define a namespace
window.Communica = window.Communica || {};

$(document).ready(function () {
    // initialise
    Communica.Part.init();
    $('#accordion .collapse').on('hidden.bs.collapse', function () {
        Communica.Part.adjustSize();
    });
    $('#accordion .collapse').on('shown.bs.collapse', function () {
        Communica.Part.adjustSize();
    });
    $(".table").on("column-switch.bs.table", function () {
        Communica.Part.adjustSize();
    });
    $('#accordion a.nav-item').bind('click', function (e) {
        setTimeout(function () { Communica.Part.adjustSize() }, 250);
    });
});

function ResizeApp() {
    $(document).on('shown.bs.modal', function () {
        Communica.Part.adjustSize();
    });
    $(document).on('hidden.bs.modal', function () {
        Communica.Part.adjustSize();
    });
    $('#accordion .collapse').on('hidden.bs.collapse', function () {
        Communica.Part.adjustSize();
    })
    Communica.Part.adjustSize();

}
Communica.Part = {
    senderId: '',      // the App Part provides a Sender Id in the URL parameters,
    // every time the App Part is loaded, a new Id is generated.
    // The Sender Id identifies the rendered App Part.
    previousHeight: 0, // the height
    minHeight: 0,      // the minimal allowed height
    minWidth: 0,      // the minimal allowed Width
    firstResize: true, // On the first call of the resize the App Part might be
    // already too small for the content, so force to resize.

    init: function () {
        // parse the URL parameters and get the Sender Id
        var params = document.URL.split("?")[1].split("&");
        for (var i = 0; i < params.length; i = i + 1) {
            var param = params[i].split("=");
            if (param[0].toLowerCase() == "senderid")
                this.senderId = decodeURIComponent(param[1]);
        }

        // find the height of the app part, uses it as the minimal allowed height
        this.previousHeight = this.minHeight = $('body').height();

        // display the Sender Id
        $('#senderId').text(this.senderId);

        // make an initial resize (good if the content is already bigger than the
        // App Part)
        this.adjustSize();
    },

    adjustSize: function () {
        // Post the request to resize the App Part, but just if has to make a resize
        //alert($('.modal.fade.in div:first-child').height());
        var maxHeightPopup = 0;
        $('div.modal').each(function () {
            if ($(this).is(':visible')) {
                var thisH = $(this).height();
                if (thisH > maxHeightPopup) { maxHeightPopup = thisH + 130; }
            }
        });
        //alert(maxHeight);
        var step = 30, // the recommended increment step is of 30px. Source:

            width = $('body').width(),        // the App Part width
            height = $('body').height(),  // the App Part height
            // (now it's 7px more than the body)
            newHeight,                        // the new App Part height
            contentHeight = $('#content-AppPart').height(),
            contentWidth = $('#content-AppPart').width(),
            resizeMessage =
                '<message senderId={Sender_ID}>resize({Width}, {Height})</message>';

        if (contentHeight >= this.minHeight)
            height = contentHeight;

        // if the content height is smaller than the App Part's height,
        // shrink the app part, but just until the minimal allowed height
        if (contentHeight < height - step && contentHeight >= this.minHeight) {
            height = contentHeight;
        }
        
        //if (contentWidth > width - step && contentWidth >= this.minWidth) {
        //    width = contentWidth;
        //}

        // if the content is bigger or smaller then the App Part
        // (or is the first resize)
        if (this.previousHeight !== height || this.firstResize === true) {
            // perform the resizing

            // define the new height within the given increment
            newHeight = Math.floor(height / step) * step +
                step * Math.ceil((height / step) - Math.floor(height / step));
            if (maxHeightPopup > newHeight)
                newHeight = maxHeightPopup;
            // set the parameters
            if (newHeight < 507) {
                newHeight = 507;
            }
            newHeight = newHeight + 20;
            resizeMessage = resizeMessage.replace("{Sender_ID}", this.senderId);
            resizeMessage = resizeMessage.replace("{Height}", newHeight);
            resizeMessage = resizeMessage.replace("{Width}", "100%");
            // we are not changing the width here, but we could

            // post the message
            window.parent.postMessage(resizeMessage, "*");

            // memorize the height
            this.previousHeight = newHeight;

            // further resizes are not the first ones
            this.firstResize = false;
            
        }
    },

    addItem: function () {
        // add an item to the list
        $('#itemsList').append('<li>Item</li>');
        Communica.Part.adjustSize();
    },

    removeItem: function () {
        // remove an item from the list
        $('#itemsList li:last').remove();
        Communica.Part.adjustSize();
    }
};