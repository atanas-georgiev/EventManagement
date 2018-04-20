var COOKIE_SETTINGS_NAME = ".EventManagement.Settings";
var HOST_ADDRESS = "http://" + window.location.host + "/EventManagement/Portal";
var UM_HOST_ADDRESS = "http://" + window.location.host + "/EventManagement/UserManagement";
var TAXRAY_PORTAL_LOADED = true;

var header = '<header id="header" class="container-fluid"> \
    <nav class="navbar-taxray navbar navbar-default row"> \
        <div class=""> \
            <div class="navbar-logo col-lg-2 col-md-3 col-xs-10"> \
                <a href="/"> \
                    <img src="" alt=""> \
                </a> \
                <h1 class="fnt-huge fnt-light pull-right">Event Management</h1> \
            </div> \
            <div class="navbar-header col-xs-2 hidden-lg hidden-md pull-right"> \
                <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar"> \
                    <span class="icon-bar"></span> \
                    <span class="icon-bar"></span> \
                    <span class="icon-bar"></span> \
                </button> \
            </div> \
            <div class="col-xs-12 col-md-9 col-lg-10"> \
                <div id="navbar" class="navbar-collapse collapse"> \
                    <div class="col-xs-12 col-md-6 col-lg-7 pull-left"> \
                        <div class="timer clearfix" style="visibility: hidden;"> \
                            <div class="stopwatch pull-left">00:00:00</div> \
                            <div class="info pull-left"> \
                                <p class="fnt-small">small text</p> \
                                <p>Name Surname(GUID 876876)</p> \
                            </div> \
                            <div class="controls pull-left"> \
                                <ul> \
                                    <li> \
                                        <span class="fa fa-play"></span> \
                                    </li> \
                                    <li> \
                                        <span class="fa fa-circle"></span> \
                                    </li> \
                                    <li> \
                                        <span class="fa fa-pause"></span> \
                                    </li> \
                                    <li> \
                                        <span class="fa fa-stop"></span> \
                                    </li> \
                                    <li> \
                                        <span class="fa fa-eye"></span> \
                                    </li> \
                                </ul> \
                            </div> \
                        </div> \
                    </div> \
                    <div class="navbar-profile-details col-xs-12 col-md-6 col-lg-5 text-right pull-right"> \
                        <p id="navbar-username"> \
                            <div class="dropdown show"> \
                                <div class="dropdown-toggle" href="#" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"> \
                                    <span class="blue-lightest"><span id="localization-welcome">Welcome</span>, <span id="welcome-name"></span><b class="caret"></b></span> \
                                </div> \
                                <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenuLink"> \
                                    <li><a class="dropdown-item localization-settings-dialog" onClick="UpdateAvailableSettings();$(\'#mySettingsModal\').modal();">Preferences</a></li> \
                                    <li><a class="dropdown-item" id="localization-logout" onClick="logout()">Logout</a></li> \
                                </ul> \
                            </div> \
                        </p> \
                        <div class="navbar-links"> \
                            <a href="" onClick="logout(event)"><span class="glyphicon glyphicon-off" style="font-size: 12px;"></span> Logout</a> \
                        </div> \
                        <p id="navbar-lastlogin" class="fnt-small"> \
                        </p> \
                    </div> \
                    <div class="navbar-register col-xs-12 col-md-6 col-lg-5 text-right pull-right"> \
                       <div class="navbar-links"> \
                            <br /> \
                            <a href="" onClick="login(event)">Login</a> | <a href="" onClick="register(event)">Register</a> \
                        </div> \
                    </div> \
                    <div class="col-xs-12 col-md-6 col-lg-5 pull-right"> \
                        <ul class="nav navbar-nav navbar-right" id="menu-items"></ul> \
                    </div> \
                </div> \
            </div> \
        </div> \
    </nav> \
</header> \
  <div class="modal fade" id="mySettingsModal" tabindex="-1" role="dialog"> \
    <div class="modal-dialog" role="document"> \
        <div class="modal-content"> \
            <div class="modal-header"> \
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"> \
                    <span aria-hidden="true">&times;</span> \
                </button> \
                <h4 class="modal-title localization-settings-dialog">User preferences</h4> \
            </div> \
            <div class="modal-body"> \
                <form> \
                    <div class="form-group"> \
                        <label for="language-selector" class="form-control-label" id="localization-language">Language:</label> \
                        <select class="form-control" id="language-selector"></select> \
                    </div> \
                    <div class="form-group"> \
                        <label for="date-format-selector" class="form-control-label" id="localization-date-format">Date format:</label> \
                        <select class="form-control" id="date-format-selector"></select> \
                    </div> \
                </form> \
            </div> \
            <div class="modal-footer"> \
                <button id="modal-save" type="button" class="btn btn-primary">Save</button> \
            </div> \
        </div> \
    </div> \
</div> \
<div class="modal fade" id="mySettingsConfirmModal" role="dialog"> \
    <div class="modal-dialog" role="document"> \
        <div class="modal-content"> \
            <div class="modal-header"> \
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"> \
                    <span aria-hidden="true">&times;</span> \
                </button> \
                <h4 id="modal-refresh-header">Information</h4> \
            </div> \
            <div class="modal-body"> \
                <p id="modal-refresh-info">To change the language to English the browser needs to be refreshed. Unsaved data will be lost. Are you sure you want to refresh?</p> \
            </div> \
            <div class="modal-footer"> \
                <button id="modal-refresh-confirm" type="button" class="btn btn-primary">Yes</button> \
                <button id="modal-refresh-abort" type="button" class="btn btn-primary">No</button> \
            </div> \
        </div> \
    </div> \
</div>';
  
var footer = '<footer id="footer" class="container-fluid"> \
    <div class="footer-wrap row"> \
        © ' + new Date().getFullYear() + ' QA Challenge Conference \
    </div> \
</footer>';

function UpdateAvailableSettings() {

    $.getJSON(HOST_ADDRESS + "/api/AvailableSettings",
        function (availableSettings) {
           
            $("#language-selector").html("");
            $.each(availableSettings["languages"], function (index, value) {
                $("#language-selector").append('<option value="' + value["id"] + '">' + value["description"] + '</option>');
            });

            $("#date-format-selector").html("");
            $.each(availableSettings["dateFormats"], function (index, value) {
                $("#date-format-selector").append('<option value="' + value["id"] + '">' + value["description"] + '</option>');
            });

            $.getJSON(HOST_ADDRESS + "/api/Settings",
                function (settings) {
                    $("#language-selector").val(settings["language"]);
                    $("#date-format-selector").val(settings["dateFormat"]);
                });
            
        });
}

function createCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toGMTString();
    }
    document.cookie = name + "=" + value + expires + "; path=/";
}

function deleteAllCookies() {
    var cookies = document.cookie.split(";");
    for (var i = 0; i < cookies.length; i++)
        createCookie(cookies[i].split("=")[0], "", -1);
    document.execCommand("ClearAuthenticationCache");
}

function logout(e) {
    e.preventDefault();
    // Use fetch instead of $.post because it is a redirect
    fetch(UM_HOST_ADDRESS + "/Account/Logout", {
        method: 'POST',
        redirect: 'follow',
        // Won't send cookies without credentials
        credentials: 'same-origin'
    }).then(res => window.location = res.url);
}

function login(e) {
    e.preventDefault();
    window.location = UM_HOST_ADDRESS + "/Account/Login";
}

function register(e) {
    e.preventDefault();
    window.location = UM_HOST_ADDRESS + "/Account/Register";
}

var observer = new MutationObserver(function (mutations) {
    mutations.forEach(function (mutation) {

        for (var i = 0; i < mutation.addedNodes.length; i++) {
            var addedNode = mutation.addedNodes[i];
            
            if (addedNode.tagName === "BODY") {
                addHeaderAndFooter();
                observer.disconnect();
            }
        }
    });
});

var config = { attributes: false, childList: true, characterData: false, subtree : true };

observer.observe(document.documentElement, config);

function updateLanguage(language) {
    if (parseInt(language) === 1) {
        // German
        $("#localization-welcome").html("Willkommen");
        $(".localization-settings-dialog").html("Benutzereinstellungen");
        $("#localization-language").html("Sprache");
        $("#localization-date-format").html("Datumsformat");
        $("#localization-logout").html("Ausloggen");
        $("#modal-save").html("Speichern");

        $("#modal-refresh-header").html("Information");
        $("#modal-refresh-info").html("Um die Sprache auf Deutsch zu wechseln muss die Seite neu geladen werden, möchten Sie die Seite jetzt neu Laden?");
        $("#modal-refresh-confirm").html("Ja");
        $("#modal-refresh-abort").html("Nein");

    } else {
        // "English"
        $("#localization-welcome").html("Welcome");
        $(".localization-settings-dialog").html("Preferences");
        $("#localization-language").html("Language");
        $("#localization-date-format").html("Date format");
        $("#localization-logout").html("Logout");
        $("#modal-save").html("Save");

        $("#modal-refresh-header").html("Information");
        $("#modal-refresh-info").html("To change the language to English the browser needs to be refreshed. Unsaved data will be lost. Are you sure you want to refresh?");
        $("#modal-refresh-confirm").html("Yes");
        $("#modal-refresh-abort").html("No");
    }

    $("body").attr("language", language);
}

function updateDateFormat(dateFormat) {
    $("body").attr("dateFormat", dateFormat);
}

function updateMenu(language) {
    $.getJSON(HOST_ADDRESS + "/api/links", { languageId: language },
        function (data) {
            $.each(data, function (index, value) {
                $("#menu-items").append('<li><a href="'+value['link']+'">' + value['title'] + '</a></li>');
            });
        });
}

$(document).ready(function () {
    addHeaderAndFooter();
    
});

function addHeaderAndFooter() {
    if (!$("<meta />").attr("http-equiv", "X-UA-Compatible")) {
        $("<meta />").attr({ "http-equiv": "X-UA-Compatible", content: "IE=Edge" }).prependTo("head");
    }

    $("body").prepend(header);
    $("body").append(footer);
    $('.navbar-logo a img').attr("src", HOST_ADDRESS + "/kpmg/images/KPMG-logo-white.png");
    
    // Read langulage from cookie or from api
    if ($.cookie(COOKIE_SETTINGS_NAME)) {
        var cookie = $.cookie(COOKIE_SETTINGS_NAME);
        if (cookie) {
            cookie = JSON.parse(cookie);
        }

        if (cookie && cookie["Language"]) {
            updateLanguage(cookie["Language"]);
            updateMenu(cookie["Language"]);
            updateDateFormat(cookie["DateFormat"]);
        }
    } else {
        $.getJSON(HOST_ADDRESS + "/api/settings",
            function (data) {
                updateLanguage(data["language"]);
                updateMenu(data["language"]);
                updateDateFormat(data["dateFormat"]);
            });
    }

    // Read username from api
    $.getJSON(HOST_ADDRESS + "/api/user",
        function (data) {
            $('#welcome-name').html(data);
            $('.navbar-profile-details').show();
            $('.navbar-register').hide();            
        }).fail(function () {
            $('.navbar-profile-details').hide();
            $('.navbar-register').show();
        });

    $("#modal-save").click(function () {
        var data = {
            'Language': $("#language-selector").val(),
            'DateFormat': $("#date-format-selector").val()
        };
        $.ajax
        ({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            type: "POST",
            url: HOST_ADDRESS + '/api/settings',
            dataType: 'json',
            async: true,
            data: JSON.stringify(data),
            success: function () {
                $('#mySettingsModal').modal('hide');
                $('#mySettingsConfirmModal').modal();
            }
        });
    });

    $("#modal-refresh-confirm").click(function () {
        location.reload();
    });

    $("#modal-refresh-abort").click(function () {
        $('#mySettingsConfirmModal').modal('hide');
    });
}