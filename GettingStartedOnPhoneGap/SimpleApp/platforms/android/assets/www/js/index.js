/// <reference path="jquery-2.1.0.js" />

var app = {
    // Application Constructor
    initialize: function () {
        this.bindEvents();
    },
    // Bind Event Listeners
    //
    // Bind any events that are required on startup. Common events are:
    // 'load', 'deviceready', 'offline', and 'online'.
    bindEvents: function () {
        document.addEventListener('deviceready', this.onDeviceReady, false);
    },
    // deviceready Event Handler
    //
    // The scope of 'this' is the event. In order to call the 'receivedEvent'
    // function, we must explicity call 'app.receivedEvent(...);'
    onDeviceReady: function () {
        app.receivedEvent('deviceready');
    },
    // Update DOM on a Received Event
    receivedEvent: function (id) {
        var parentElement = document.getElementById(id);
        var listeningElement = parentElement.querySelector('.listening');
        var receivedElement = parentElement.querySelector('.received');

        listeningElement.setAttribute('style', 'display:none;');
        receivedElement.setAttribute('style', 'display:block;');
        px500.showMenu();
        px500.loadPhotos(px500.pageUrl);
        console.log('Received Event: ' + id);
    }
};

String.prototype.format = function () {
    var formatted = this;
    for (var i = 0; i < arguments.length; i++) {
        var regexp = new RegExp('\\{' + i + '\\}', 'gi');
        formatted = formatted.replace(regexp, arguments[i]);
    }
    return formatted;
};



function menuSelected() {
    $(".photos").html('');
    $(".app").show();
    
    px500.loadPhotos(px500.pageUrl.format($(".menu").val()));
    
}

var px500 = {
    loadPhotos: function (photoUrl) {
        console.log("Loading Photos " + photoUrl);
        $.getJSON(photoUrl, null, function (data, status, jqXHR) {
            $(".app").hide();
            $.each(data.photos, function (key, value) {
                $("<img />", { "src": value.image_url, "class": "photo" }).appendTo(".photos");
            });
        })
    },

    photoStreams: ['popular', 'editors', 'upcoming'],

    currentPage: 1,

    pageUrl: "https://api.500px.com/v1/photos?feature={0}&rpp=10&page=1&exclude=4&consumer_key=<CONSUMER KEY>",

    showMenu: function () {
        $.each(px500.photoStreams, function (key, value) {
            $('<option />', { html: value, "value": value }).appendTo(".menu");
        });
    }
};
