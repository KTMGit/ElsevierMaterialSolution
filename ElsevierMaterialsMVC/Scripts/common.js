//function createErrorLayer(input, msg) {
//    var $layer = $('<div class="error-layer"></div>');
//    var position = input.position();
//    $layer.html(msg);
//    $layer.attr("id", input.attr("id") + "_layer")
//    $("errorContainer").empty();
//    $("errorContainer").append($layer);
//    var layerHeight = $("#" + input.attr("id") + "_layer").height();
//    $layer.css("position", "absolute");
//    $layer.css("top", (position.top - layerHeight - 20).toString() + "px");
//    $layer.css("left", position.left.toString() + "px");
//    input.parent().append($layer);
//}
/*global classes/enums/functions*/
//
var ini = {};
ini.ajax = new Object();
ini.ajax.responseStatus = {
    Success: 1,
    Error: 2,
    SessionExpired: 3
}


var overlayType = {
    Transparent: 1,
    SemitransparentBlack: 2,
    SemitransparentWhite: 3
}
var displayProgressIndicator = function ($element, text) {
    var $overlay = createOverlay($element, overlayType.SemitransparentWhite);
    var $inner = $('<div class="loader"></div>');
    $overlay.append($inner);
    if (text) {
        var $text = $('<div class="text">' + text + '</div>');
        $inner.append($text);
    }
}

var removeProgressIndicator = function ($element) {
    destroyOverlay($element);
}




ini.ajax.post = function (url, contentType, data, callbackSuccess, callbackError, callbackSession) {
    if (contentType == ini.ajax.contentType.Json) {
        data = JSON.stringify(data);
    }
    _ajax(AjaxType.Post, url, contentType, data, callbackSuccess, callbackError, callbackSession);
}
ini.ajax.get = function (url, data, callbackSuccess, callbackError, callbackSession) {
    _ajax(AjaxType.Get, url, ini.ajax.contentType.UrlEncoded, data, callbackSuccess, callbackError, callbackSession);
}

var AjaxType = {
    Post: 'POST',
    Get: 'GET'
};

ini.ajax.contentType = {
    Json: 'application/json; charset=utf-8',
    UrlEncoded: 'application/x-www-form-urlencoded; charset=UTF-8'
};

ini.ajax.translations = {
    error: 'Error',
    ok: 'Ok',
    message: 'Message',
    sessionExpired: 'Your session has expired, you will be redirected to the login page.'
}

function _ajax(type, url, contentType, data, callbackSuccess, callbackError, callbackSession) {
    $.ajax({
        type: type,
        contentType: contentType,
        data: data,
        url: url,
        success: function (result) {
            switch (result.status) {
                case ini.ajax.responseStatus.Error:
                    if (callbackError) {
                        callbackError(result.data);
                    }
                    else {
                        showAlert(ini.ajax.translations.error, result.data.message, ini.ajax.translations.ok);
                    }
                    break;
                case ini.ajax.responseStatus.Success:
                    if (callbackSuccess) {
                        callbackSuccess(result.data);
                    }
                    break;
                case ini.ajax.responseStatus.SessionExpired:
                    if (callbackSession) {
                        callbackSession(result.data);
                    }
                    else { //redirect to the specified url
                        showAlert(ini.ajax.translations.message, ini.ajax.translations.sessionExpired, ini.ajax.translations.ok, function () {
                            window.location = result.data.redirectUrl;
                        });
                    }
                default:
                    break;
            }
        }
    }).error(function (jqXHR, textStatus, errorThrown) {
        onAjaxError(jqXHR, textStatus, errorThrown);
    });
}
$.ajaxSetup({ //general ajax settings
    cache: false
});
function onAjaxError(jqXHR, textStatus, errorThrown) {
    if (jqXHR.status === 0) {
            // Not connected. Please verify network is connected.
    } else if (jqXHR.status == 404) {
            // Requested page not found. [404]
    } else if (jqXHR.status == 500) {
            // Internal Server Error [500].
    }
            //other status code
}
ini.ajax.autocomplete = function (elemId, action) {
    var $txt = $('#' + elemId);
    $txt.autocomplete({
        source: function (request, response) {
            ini.ajax.get(action, { text: request.term },
                function (result) {
                    response($.map(result, function (item) {
                        return {
                            label: item.Name,
                            value: item.Name
                        };
                    }));
                },
                function (error) {
                    showAlert('Error', error);
                });
        },
        minLength: 1,
        position: {
            my: 'left top',
            at: 'left bottom+5'
        },
        delay: 300,
        messages: {
            noResults: '',
            results: function () { }
        }
    });
}

ini.eventHandlers = {};
ini.eventHandlers.registerEnterClick = function (defaultAction) {
    $('body').on('keypress', function (event) {
        if (event.which == 13 && !$(event.target).is('textarea')) {
            defaultAction();
            return false;
        }
        return true;
    });
}
ini.html = {};
ini.html.formatString = function (text) {
    return text.replace(/(\r\n|\n|\r)/gm, "<br />");
}
ini.text = {};
ini.text.endsWith = function (str, suffix) {
    return str.indexOf(suffix, str.length - suffix.length) !== -1;
}
function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}
ini.style = {};
ini.style.styleTableRows = function (tableId) {
    $('#' + tableId + ' tbody tr:odd').addClass('alt');
}

//manipulation of html elements
ini.html = {};
ini.html.toggleExpander = function(sender, targetId) {
    var $sender = $(sender);
    if ($sender.hasClass('closed')) {
        $('#' + targetId).show();
        $sender.removeClass('closed');
        $sender.find('.nav-content-toggle').removeClass('closed');
    }
    else {
        $('#' + targetId).hide();
        $sender.addClass('closed');
        $sender.find('.nav-content-toggle').addClass('closed');
    }
}

/*
* Constants
*/
/* Fade time for alert, confirm, tooltips and errors. */
var FADE_TIME = 300;
/* Overlay transparency values */
var OVERLAY_TYPE_TRANSPARENT = 0;
var OVERLAY_TYPE_SEMITRANSPARENT_WHITE = 1;
var OVERLAY_TYPE_SEMITRANSPARENT_BLACK = 2;

var ActionType = {
    Sort : 'Sort',
    Page : 'Page',
    Search : 'Search'
}

var SortOrder = {
    Ascending: 0,
    Descending: 1
}

/*
* Methods that needs to run after the document is ready.
*/
$(document).ready(function () {
    renderDocument();
    createTooltips();
   
   
    //    $('.input-validation-error').each(function () {
    //        initializeError($(this));
    //    });
});
/* 
* Methods that needs to run after ajax call is completed.
*/
$(document).ajaxComplete(function () {
    renderDocument();
    createTooltips();
   
  
    $.validator.unobtrusive.parse("form");
});
/*
* Resizing window event. 
* It should redraw alert, confirm, tooltip message and error when window is resized.
*/
$(window).resize(function () {
    $('#alertMsg').center();
    redrawConfirm();
    redrawTooltip();
    redrawOverlays();
});
/*
* Centers the element on the screen.
*/
jQuery.fn.center = function () {
    this.css("position", "absolute");
    this.css("top", (($(window).height() - this.outerHeight()) / 2) + $(window).scrollTop() + "px");
    this.css("left", (($(window).width() - this.outerWidth()) / 2) + $(window).scrollLeft() + "px");
    return this;
}
/*
* Renders the document with design rules for tables and tabs:
* - applies rounded corners to tables that have class 'SKFTable' 
* - creates tabs, apply rounded corners to them and and tabs that
*/

function formatElement() {
    if ($(this).hasClass('jqFormatted')) return;
    $(this).addClass('jqFormatted');
//    var cls = $(this).attr("class");
    //if ($(this).hasClass('SKFTable')) {
    //    _finalizeTable($(this));
    //    _applyCorners($(this));
    //    return;
    //}
    if ($(this).hasClass('SKFRounded')) {
        _applyCorners($(this));
    }
    if ($(this).hasClass('SKFTabs')) {
        $(this).tabs();
        _applyCorners($(this));
        _correctTabWidths($(this));
        return;
    }
    if ($(this).hasClass('field-validation-error')) {
        var inputElem = '#' + $(this).attr('data-valmsg-for').replace('.', '_').replace('[', '_').replace(']', '_');
        var $input = $(inputElem);
        if ($input.is('input:text')) {
            initializeError($input, $(this).html());
        }
        return;
    }
    //if ($(this).is('label')) {
    //    var $elInput = $('#' + $(this).attr('for'));
    //    if ($elInput.is(':checkbox')) {
    //        _setProperCheckStyleInitial($(this), $elInput);
    //    }
    //    if ($elInput.is(':radio')) {
    //        _setProperRadioStyleInitial($(this), $elInput);
    //    }
    //}
    //if ($(this).is('select.SKFSelect')) {
    //    $(this).jqTransSelect();
    //}
}

function renderDocument(container) {
    if (container == undefined || container == null) {
        $('div.SKFTabs, .SKFRounded, select:not(.common), span.field-validation-error, label').each(formatElement);
    }
    else {
        $(container).find('div.SKFTabs, select:not(.common), span.field-validation-error, label').each(formatElement);
    }
}

/*
* Applies corners on the element with the given ID.
*/
function applyCorners(id) {
    _applyCorners($("#" + id));
}

/*
* Applies corners on the given element.
*/
function _applyCorners($elem) {
    $elem.wrap('<div class="corner-wrapper" style="width: ' + ($elem.width() + 2) + 'px"></div>');
    $elem.parent().append('<div class="corner-tl"></div><div class="corner-tr"></div><div class="corner-bl"></div><div class="corner-br"></div>');
}

function _applyCornersTable($elem) {
    $elem.wrap('<div class="corner-wrapper" style="width: ' + $elem.outerWidth() + 'px"></div>');
    $elem.parent().append('<div class="corner-tl"></div><div class="corner-tr"></div><div class="corner-bl"></div><div class="corner-br"></div>');
}

function _finalizeTable($elem) {
    _applyCornersTable($elem);
    $elem.find('tbody tr:odd').addClass('alt');
    //var $row = $elem.find('thead > tr');
    //var headerHeight = $row.height();
    //if (headerHeight > 30) {
    //    $row.find('th').each(function () {
    //        $(this).addClass('header-big');
    //    });
    //}
//    var index = 0;
//    $elem.find('tbody > tr').each(function () {
//        if (index % 2 == 1) {
//            $(this).addClass('alt');
//        }
//        index++;
//    });
}

/*
* Aligns tab widths for tab element with the given id.
*/
function correctTabWidths(id) {
    _correctTabWidths($("#" + id));
}

/*
* Aligns tab widths for given element.
*/
function _correctTabWidths($elem) {
    var num = $elem.find('ul.ui-tabs-nav > li').size();
    var totalWidth = $elem.find('ul.ui-tabs-nav').width();
    var elemWidth = Math.floor(totalWidth / num);
    var sumOfWidths = 0;
    var cnt = 0;
    $elem.find('ul.ui-tabs-nav > li').each(function () {
        cnt += 1;
        if (cnt != num) {
            $(this).width(elemWidth);
        }
    });
    sumOfWidths = (num - 1) * elemWidth;
    $elem.find('ul.ui-tabs-nav > li:last').each(function () {
        //        $(this).width(totalWidth - sumOfWidths - (num - (num % 2)));
        $(this).width(totalWidth - sumOfWidths - (num - 1));
        $(this).css("float", "right");
        $(this).css("border-right", "0px");
    });
}
/*
* Creates alert message with given title, message and OK button title.
*/


showPopup = function (options, $content, $originalContentContainer) {
    if (!options) {
        options = {
            width: 400,
            height: 300,
            title: '',
            close: null
        };
    }
    if (options.automaticallyClose != undefined && options.automaticallyClose == true) {
        if (options.closeInterval == undefined) {
            options.closeInterval = 1000;
        }
        setTimeout(function () {
            $('#popupClose').click();
        }, options.closeInterval);

    }
    $('#popupTitle').html('<b style="font-size: 1.2em">' + options.title + '</b>');
    $('#popupContainer').css('overflow-x', 'hidden');
    $('#popupContainer').css('overflow-y', 'auto');
    $('#popupContainer').css('width', options.width + 'px');
    $('#popupContainer').css('height', options.height + 'px');
    $('#popupClose').unbind('click');
    $('#popupContainer').html("");
    $('#popupContainer').append($content);
    $('#popupClose').click(function () {
        $('#overlay').hide();
        $('#popup').hide();
        customPrintContainerSelector = null;
        if ($originalContentContainer) {
            $originalContentContainer.append($content);
        } else {
            $content.remove();
        }
        if (options.close) {
            options.close();
        }
    });
    $('.buttonContinue').click(function () {
        $('#overlay').hide();
        $('#popup').hide();
        customPrintContainerSelector = null;
        if ($originalContentContainer) {
            $originalContentContainer.append($content);
        } else {
            $content.remove();
        }
        if (options.close) {
            options.close();
        }
    });
    $('#overlay').show();
    $("#overlay").css("height", $(document).height());
    $('#popup').show();
    $('#popup').center();
}

hidePopup = function () {
    $('#popupClose').click();
}

showAlert = function (options) {
    if (!options) {
        options = {
            width: 300,
            height: 200,
            title: '',
            message: '',
            close: null
        };
    } else {
        if (!options.width) {
            options.width = 300;
        }
        if (!options.height) {
            options.height = 200;
        }
    }
    showPopup(options, $('<div>' + options.message + '</div>'), null);
}

showStructure = function (image) {
    var $content = $('#structure');
    var $originalContentContainer = $('#structureContainer');
    
        options = {
            width: 'auto',
            height: 'auto',
            title: '',
            close: null
        };
    
    if (options.automaticallyClose != undefined && options.automaticallyClose == true) {
        if (options.closeInterval == undefined) {
            options.closeInterval = 1000;
        }
        setTimeout(function () {
            $('#popupClose').click();
        }, options.closeInterval);

    }
    $('#popupTitle').html('<b style="font-size: 1.2em">' + options.title + '</b>');
    $('#popupContainer').css('overflow-x', 'hidden');
    $('#popupContainer').css('overflow-y', 'auto');
    $('#popupContainer').css('width', options.width + 'px');
    $('#popupContainer').css('height', options.height + 'px');
    $('#popupClose').unbind('click');
    $('#popupContainer').html("");
    var imgSrc = $("img[src$='"+image+"']");    

    $content.html('<img src="' + imgSrc.attr("src") + '"/>');

    $('#popupContainer').append($content);
    $('#popupClose').click(function () {
        $('#overlay').hide();
        $('#popup').hide();
        customPrintContainerSelector = null;
        if ($originalContentContainer) {
            $originalContentContainer.append($content);
        } else {
            $content.remove();
        }
        if (options.close) {
            options.close();
        }
    });
    $('.buttonContinue').click(function () {
        $('#overlay').hide();
        $('#popup').hide();
        customPrintContainerSelector = null;
        if ($originalContentContainer) {
            $originalContentContainer.append($content);
        } else {
            $content.remove();
        }
        if (options.close) {
            options.close();
        }
    });
    $('#overlay').show();
    $("#overlay").css("height", $(document).height());
    $('#popup').show();
    $('#popup').center();
}

/*
Session expired window
*/
function sessionExpiredWindow(alertTitle, message, okButtonTitle, gotoUrl) {
    if (!okButtonTitle) {
        okButtonTitle = 'OK';
    }
    $('#alertMsg').remove();
    //$('#tooltipMsg').fadeOut(FADE_TIME);
    $('#tooltipMsg').hide();
    permanentTooltip = false;
    //$('#confirmMsg').fadeOut(FADE_TIME);
    $('#confirmMsg').hide();
    destroyOverlay('container');
    var $alert = $('<div id="alertMsg" class="alert" style="display:none"></div>');
    var $wrapper = $('<div class="alert-wrapper"></div>')
    var $header = $('<div class="alert-header"><span class="alert-title">' + alertTitle + '</span></div>');
    var $content = $('<div class="alert-content">' + message + '</div>');
    var $action = $('<div class="alert-action"></div>);');
    var $closeButton = $('<span><input id type="button" value="' + okButtonTitle + '" class="btn" onmousedown="$(this).addClass(\'btn-active\');" onmouseup="$(this).removeClass(\'btn-active\');" onmouseout="$(this).removeClass(\'btn-active\');" /></span>');
    $closeButton.click(function () {
        window.location = gotoUrl;
    });
    $alert.append($wrapper.append($header).append($content).append($action.append($closeButton)));
    $('body').append($alert);
    createOverlay('container', OVERLAY_TYPE_TRANSPARENT);
    $alert.center();
    $alert.show();
}


/*
* Creates confirm message for given senderId, title, message, ok button title, ok callback, cancel button title and cancel callback.
*/
showConfirm = function (message, confirmSettings, okCallback, cancelCallBack) {
    if (!confirmSettings) {
        confirmSettings = {
            width: 300,
            height: 200,
            title: '',
            close: null
        };
    }
   showPopup(confirmSettings, $('#confirm'), $('#confirmContainer'));
    $('#confirmMessage').html(message);
    $('#confirmOk').unbind('click');
    $('#confirmCancel').unbind('click');
    $('#confirmOk').click(function () {
        if (okCallback) {
            okCallback();
        }
        $('#popupClose').click();
    });
    $('#confirmCancel').click(function () {
        if (cancelCallBack) {
            cancelCallBack();
        }
        $('#popupClose').click();
    });
}


/*
* Redraws cofirm message to the correct position when window is resized.
*/
function redrawConfirm() {
    var $confirm = $('#confirmMsg');
    var senderID = $confirm.attr('sender');
    if (!senderID) {
        return;
    }
    var $sender = $('#' + senderID.replace(/\[/g, '\\[').replace(/\]/g, '\\]'));
    if (!$confirm || !$sender) {
        return;
    }
    var confirmWidth = $confirm.width();
    var confirmHeight = $confirm.height();
    var senderOffset = $sender.offset();
    var senderWidth = $sender.width();
    if (!confirmWidth || !confirmHeight || !senderOffset || !senderWidth) {
        return;
    }
    var left = senderOffset.left + senderWidth / 2 - confirmWidth / 2;
    var top = senderOffset.top - confirmHeight - 13;
    $confirm.css('left', left + 'px');
    $confirm.css('top', top + 'px');
}


/* Tooltips */
var permanentTooltip = false;
var tipID = 0;

/* 
* Creates tooltips. 
* Only one tooltip is visible at a time.
* Each element that needs tooltip needs to have class 'SKFToltip', and following attributes:
* - tipHeader: header of the tooltip
* - tipMessage: message of the tooltip
* - tipPersist (optional): set this attribute to '1' if you want this tooltip to persist (it will have a close icon).
*/
function createTooltips() {
    $('.SKFTooltip').unbind('hover').hover(function () {
        var $self = $(this);
        if (!($self.attr('tipClick') == '1')) {
            _createToolTip($self);
        }
    }, function () {
        if (!permanentTooltip) {
            $('#tooltipMsg').fadeOut(FADE_TIME);
        }
    }).click(function () {
        var $self = $(this);
        if ($self.attr('tipClick') == '1') {
            _createToolTip($self);
        }
    });
}


function escapeSquareBrackets(text) {
    return text.replace(/\[/g,'\[').replace(/\]/g, '\]');
}

function _createToolTip($elem) {
    var $previousTip = $('#tooltipMsg');
    var tooltipID = 0;
    if (!$elem.attr('tipID')) {
        tooltipID = tipID;
        tipID++;
        $elem.attr('tipID', tooltipID);
    } else {
        tooltipID = $elem.attr('tipID');
        if ($elem.attr('tipID') == $previousTip.attr('tipID') && permanentTooltip) {
            return;
        }
    }
    var tooltipTitle = $elem.attr('tipHeader');
    var message = $elem.attr('tipMessage');
    var tipPersist = $elem.attr('tipPersist');
    $previousTip.remove();
    permanentTooltip = false;
    if (tipPersist == '1') {
        permanentTooltip = true;
    }
    var $tooltip = $('<div id="tooltipMsg" tipID="' + tooltipID + '" class="tooltip" style="display:none;"></div>');

    var $wrapper = $('<div class="tooltip-wrapper"></div>')
    var $header = $('<div class="tooltip-header"><span class="tooltip-title">' + tooltipTitle + '</span></div>');
    if (permanentTooltip) {
        var $headerClose = $('<span class="tooltip-close"><a class="closeLink" onclick="$(\'#tooltipMsg\').fadeOut(' + FADE_TIME + '); permanentTooltip=false;"></a></span>');
        $header.append($headerClose);
    }
    var $content = $('<div class="tooltip-content">' + message + '</div>');
    var $action = $('<div class="tooltip-action"></div>);');
    var $arrow = $('<div class="tooltip-arrow"></div>');
    $tooltip.append($wrapper.append($header).append($content)).append($arrow);
    $('body').append($tooltip);
    var tooltipWidth = $tooltip.width();
    var tooltipHeight = $tooltip.height();
    var senderOffset = $elem.offset();
    var senderWidth = $elem.width();
    var left = senderOffset.left + senderWidth / 2 - tooltipWidth / 2 - 6;
    var top = senderOffset.top - tooltipHeight - 13;
    $tooltip.css('left', left + 'px');
    $tooltip.css('top', top + 'px');
    $tooltip.fadeIn(FADE_TIME);
}

function createTooltip($elem, title, content, persist) {
    var $previousTip = $('#tooltipMsg');
    var tooltipID = 0;
    if (!$elem.attr('tipID')) {
        tooltipID = tipID;
        tipID++;
        $elem.attr('tipID', tooltipID);
    } else {
        tooltipID = $elem.attr('tipID');
        if ($elem.attr('tipID') == $previousTip.attr('tipID') && permanentTooltip) {
            return;
        }
    }
    var tooltipTitle = title;
    var message = content;
    var tipPersist = persist;
    $previousTip.remove();
    permanentTooltip = false;
    if (tipPersist == '1') {
        permanentTooltip = true;
    }
    var $tooltip = $('<div id="tooltipMsg" tipID="' + tooltipID + '" class="tooltip" style="display:none;"></div>');

    var $wrapper = $('<div class="tooltip-wrapper"></div>')
    var $header = $('<div class="tooltip-header"><span class="tooltip-title">' + tooltipTitle + '</span></div>');
    if (permanentTooltip) {
        var $headerClose = $('<span class="tooltip-close"><a class="closeLink" onclick="$(\'#tooltipMsg\').fadeOut(' + FADE_TIME + '); permanentTooltip=false;"></a></span>');
        $header.append($headerClose);
    }
    var $content = $('<div class="tooltip-content">' + message + '</div>');
    var $action = $('<div class="tooltip-action"></div>);');
    var $arrow = $('<div class="tooltip-arrow"></div>');
    $tooltip.append($wrapper.append($header).append($content)).append($arrow);
    $('body').append($tooltip);
    var tooltipWidth = $tooltip.width();
    var tooltipHeight = $tooltip.height();
    var senderOffset = $elem.offset();
    var senderWidth = $elem.width();
    var left = senderOffset.left + senderWidth / 2 - tooltipWidth / 2 - 6;
    var top = senderOffset.top - tooltipHeight - 13;
    $tooltip.css('left', left + 'px');
    $tooltip.css('top', top + 'px');
    $tooltip.fadeIn(FADE_TIME);
}

function removeTooltip() {
    var tooltipToRemove = $('#tooltipMsg');
    tooltipToRemove.remove();
}


/*
* Redraws tooltip to the correct position when window is resized.
*/
function redrawTooltip() {
    var $tooltip = $('#tooltipMsg');
    var $elem = null;
    $('.SKFTooltip').each(function () {
        if ($(this).attr('tipID') == $tooltip.attr('tipID')) {
            $elem = $(this);
        }
    });
    if (!$tooltip || !$elem) {
        return;
    }
    var tooltipWidth = $tooltip.width();
    var tooltipHeight = $tooltip.height();
    var elemOffset = $elem.offset();
    var elemWidth = $elem.width();
    if (!tooltipWidth || !tooltipHeight || !elemOffset || !elemWidth) {
        return;
    }
    var left = elemOffset.left + elemWidth / 2 - tooltipWidth / 2;
    var top = elemOffset.top - tooltipHeight - 13;
    $tooltip.css('left', left + 'px');
    $tooltip.css('top', top + 'px');
}

var $lastFocusErrorElement;
var $currentErrorMessage;
/*
* Creates error tooltip for the element.
*/
function initializeError($elem, message) {
    removeError($elem);
    if (message == '') {
        return;
    }
    $elem.unbind('focus').unbind('blur').unbind('hover');
    $elem.focus(function () {
        if ($currentErrorMessage) {
            $currentErrorMessage.remove();
        }
        $currentErrorMessage = _createErrorMessage($elem, message);
    }).blur(function () {
        if ($currentErrorMessage) {
            $currentErrorMessage.remove();
        }
        $currentErrorMessage = null;
    }).hover(function () {
        if ($currentErrorMessage) {
            $currentErrorMessage.remove();
        }
        $currentErrorMessage = _createErrorMessage($elem, message);
    }, function () {
        if ($currentErrorMessage) {
            $currentErrorMessage.remove();
        }
        $currentErrorMessage = null;
    });
    if ($elem.is('input:text.textfield, input:password, select.SKFSelect')) {
        $elem.parent().removeClass('textfield-stroke-error');
        $elem.parent().addClass('textfield-stroke-error').removeClass('textfield-stroke');
    } else {
        $elem.removeClass('textfield-stroke-error');
        $elem.addClass('textfield-stroke-error').removeClass('textfield-stroke');
    }
}

function showErrorTooltip(form) {
    var $firstInvalidElement = $(form).find('.input-validation-error').first();
    $firstInvalidElement.focus();
}

/*
* Removes error from the given element
*/
function removeError($elem) {
    $elem.unbind('focus').unbind('blur').unbind('hover');
    if ($elem && ($elem.is('input:text.textfield, input:password.textfield') /* && $elem.hasClass('textfield valid')*/ ) || ($elem.is('select.SKFSelect'))) {
        $elem.parent().removeClass('textfield-stroke-error').addClass('textfield-stroke');
    } else {
        $elem.removeClass('textfield-stroke-error');
    }
}

function _createErrorMessage($elem, message) {
    var $error = $('<div id="errorMsg" class="error"></div>');
    var $wrapper = $('<div class="error-wrapper"></div>')
    var $content = $('<div class="error-content">' + message + '</div>');
    var $arrow = $('<div class="error-arrow"></div>');
    var elemOffset = $elem.offset();
    var elemWidth = $elem.width();
    $error.css('width', elemWidth);
    $error.append($wrapper.append($content).append($arrow));
    $('body').append($error);
    var errorHeight = $error.height();
    var left = elemOffset.left;
    var minWidth = $error.css('min-width').replace('px', '');
    if (elemWidth < minWidth) {
        left -= (minWidth - elemWidth) / 2;
    }
    var top = elemOffset.top - errorHeight - 14;
    $error.css('left', (left + 2) + 'px');
    $error.css('top', top + 'px');
    $error.fadeIn(FADE_TIME);
    return $error;
}

/*
* Creates overlay over an element with given id and overlay type
*/
function createOverlay(elementId, overlayType) {
    var overlay = _createOverlay($('#' + elementId), overlayType);
    return overlay;
}   

/*
* Creates overlay over given element and overlay type
*/
function _createOverlay($element, overlayType) {
    var $overlay = $('<div id="' + $element.attr('id') + '_overlay" class="overlay"></div>');
    var elementOffset = $element.offset();
    $overlay.css('left', (elementOffset.left) + 'px');
    $overlay.css('top', elementOffset.top + 'px');
    $overlay.width($element.width() + 3);
    $overlay.height($element.height() + 3);
    var textColor = '#FFFFFF';
    switch (overlayType) {
        case OVERLAY_TYPE_TRANSPARENT:
            $overlay.addClass('overlay-transparent');
            textColor = '#5E5E5E';
            break;

        case OVERLAY_TYPE_SEMITRANSPARENT_BLACK:
            $overlay.addClass('overlay-semitransparent-black');
            textColor = '#FFFFFF';
            break;

        case OVERLAY_TYPE_SEMITRANSPARENT_WHITE:
            $overlay.addClass('overlay-semitransparent-white');
            textColor = '#5E5E5E';
            break;

        default:
            break;
    }
    $('body').append($overlay);
    return $overlay;
}

var progressIndicatorElementID;
function showProgressIndicator(elementId, text) {
    progressIndicatorElementID = elementId;
    var overlay = createOverlay(elementId, OVERLAY_TYPE_SEMITRANSPARENT_WHITE);
    var $inner = $('<div class="loader"></div>');
    overlay.append($inner);
    textColor = '#5E5E5E';
    if (text) {
        var $text = $('<div class="text">' + text + '</div>');
        $inner.append($text);
        var textHeight = $text.height();
        $text.css('padding-top', (overlay.height() / 2 - textHeight - 20) + 'px');
        $text.css('color', textColor);
    }
}

function hideProgressIndicator() {
    destroyOverlay(progressIndicatorElementID);
}

/*
* Destroys overlay over element with given ID
*/
function destroyOverlay(elementId) {
    _destroyOverlay($('#' + elementId));
}

/*
* Destroys overlay over given element
*/
function _destroyOverlay($element) {
    var $overlay = $('#' + $element.attr('id') + '_overlay');
    $overlay.fadeOut(FADE_TIME, function () {
        $overlay.remove();
    });
}

/*
* Redraws overlays in case of widow resizing.
*/
function redrawOverlays() {
    $('.overlay').each(function () {
        var $overlay = $(this);
        if (!$overlay) {
            return;
        }
        var $element = $('#' + $overlay.attr('id').replace('_overlay', ''));
        if (!$element) {
            return;
        }
        var elementOffset = $element.offset();
        if (!elementOffset) {
            return;
        }
        $overlay.css('left', elementOffset.left + 'px');
        $overlay.css('top', elementOffset.top + 'px');
    });
}

function focusTextbox($el) {
    if (!$el.hasClass('field-validation-error')) {
        $el.parent().addClass('textfield-stroke-active');
    } else {
        $el.parent().addClass('textfield-stroke-error');
    }
}

function unfocusTextbox($el) {
    if (!$el.hasClass('field-validation-error')) {
        $el.parent().removeClass('textfield-stroke-active');
    } else {
        $el.parent().addClass('textfield-stroke-error');
    }
}

//function styleCheck($el) {
//    styleCheckRadio($el, $el, false);
//}

//function styleRadio($el) {
//    $('input[name="' + $('#' + $el.attr('for')).attr('name') + '"]').each(function () {
//        styleCheckRadio($('label[for="' + $(this).attr('id') + '"]'), $el, true);
//    });
//}

//function styleCheckRadio($el, $origin, isRadio) {
//    var $elInput = $('#' + $el.attr('for'));
//    var $originInput = $('#' + $origin.attr('for'));
//    if (isRadio && $elInput.attr('id') == $originInput.attr('id') && $elInput.attr('checked') == 'checked') {
//        return;
//    }
//    if (isRadio && $elInput.attr('id') != $originInput.attr('id')) {
//        _setProperRadioStyle($el, $elInput);
//        return;
//    }
//    _setProperCheckStyle($el, $elInput);
//}

//function _setProperRadioStyle($el, $elInput) {
//    if ($elInput.attr('disabled') == 'disabled') {
//        $el.css("background-position", "0px -32px")
//    } else if (($elInput.hasClass('input-validation-error'))) {
//        $el.css("background-position", "0px -64px")
//    } else {
//        $el.css("background-position", "0px 0px")
//    }
//}

//function _setProperCheckStyle($el, $elInput) {
//    if ($elInput.attr('checked') == 'checked') {
//        if ($elInput.attr('disabled') == 'disabled') {
//            $el.css("background-position", "0px -32px")
//        } else if (($elInput.hasClass('input-validation-error'))) {
//            $el.css("background-position", "0px -64px")
//        } else {
//            $el.css("background-position", "0px 0px")
//        }
//    } else {
//        if ($elInput.attr('disabled') == 'disabled') {
//            $el.css("background-position", "0px -48px")
//        } else if (($elInput.hasClass('input-validation-error'))) {
//            $el.css("background-position", "0px -80px")
//        } else {
//            $el.css("background-position", "0px -16px")
//        }
//    }
//}

//function _setProperRadioStyleInitial($el, $elInput) {
//    if ($elInput.attr('checked') == 'checked') {
//        if ($elInput.attr('disabled') == 'disabled') {
//            $el.css("background-position", "0px -48px")
//        } else if (($elInput.hasClass('input-validation-error'))) {
//            $el.css("background-position", "0px -80px")
//        } else {
//            $el.css("background-position", "0px -16px")
//        }
//    } else {
//        if ($elInput.attr('disabled') == 'disabled') {
//            $el.css("background-position", "0px -32px")
//        } else if (($elInput.hasClass('input-validation-error'))) {
//            $el.css("background-position", "0px -64px")
//        } else {
//            $el.css("background-position", "0px 0px")
//        }
//    }
//}

//function _setProperCheckStyleInitial($el, $elInput) {
//    if ($elInput.attr('checked') == 'checked') {
//        if ($elInput.attr('disabled') == 'disabled') {
//            $el.css("background-position", "0px -48px")
//        } else if (($elInput.hasClass('input-validation-error'))) {
//            $el.css("background-position", "0px -80px")
//        } else {
//            $el.css("background-position", "0px -16px")
//        }
//    } else {
//        if ($elInput.attr('disabled') == 'disabled') {
//            $el.css("background-position", "0px -32px")
//        } else if (($elInput.hasClass('input-validation-error'))) {
//            $el.css("background-position", "0px -64px")
//        } else {
//            $el.css("background-position", "0px 0px")
//        }
//    }
//}

/*
* jQuery extenders
*/

/*
* Centers the element on the screen.
*/
jQuery.fn.center = function () {
    this.css("position", "absolute");
    this.css("top", (($(window).height() - this.outerHeight()) / 2) + $(window).scrollTop() + "px");
    this.css("left", (($(window).width() - this.outerWidth()) / 2) + $(window).scrollLeft() + "px");
    return this;
}

/*grid fns modified*/
var PageElements = new Object();

function GridSettings() {
    this.Filters = null;
    this.Descriptor = null;
    this.ContainerId = null;
    this.Url = null;
}

function GridDescriptor() {
    this.Pager = null;
    this.Sort = null;
    this.RequestType = ActionType.Page;
}

function PagerDescriptor() {
    this.SelectedPage = 0;
    this.SelectedPageSize = 0;
    this.PageSizeDescription = null;
    this.AllowedPageSizes = new Array();
    this.VisiblePageCount = 0;
}

function SortDescriptor() {
    this.PropertyName = null;
    this.Order = SortOrder.Ascending;
}

//function GridRequestDescriptor() {
//    this.Filters = null;
//    this.Pager = new Object();
//    this.Pager.PageSize = 20;
//    this.Pager.Page = 1;
//    this.RequestType = ActionType.Page;
//}

//function GridDescriptor() {
//    this.ContainerId = '';
//    this.GridRequestDescriptor = new GridRequestDescriptor();
//    this.Url = '';
//}

function _RegisterGrid(containerId, url, desc, filters) {
    if (containerId in PageElements) return;
    var settings = new GridSettings();
    var descriptor = new GridDescriptor();
    var sorter = new SortDescriptor();
    var pager = new PagerDescriptor();
    settings.ContainerId = containerId;
    settings.Url = url;
    if (filters) {
        settings.Filters = filters;
    }
    sorter.PropertyName = desc.Sort.PropertyName;
    sorter.Order = desc.Sort.Order;
    descriptor.Sort = sorter;
    var $gridContainer = $('#' + containerId);
    //var $pagerContainer = $gridContainer.find('.pager').first();
    //pager.PageSizeDescription = $pagerContainer.find('.page-size .text').html();
    //$pagerContainer.find('.page-size').children(':not(span[class="text"])').each(function () {
    //    pager.AllowedPageSizes.push(parseInt(this.innerHTML));
    //    if (this.nodeName.toLowerCase() == 'span') pager.SelectedPageSize = parseInt(this.innerHTML);
    //});
    //pager.VisiblePageCount = $pagerContainer.data('pager-vis-page-count');
    //pager.SelectedPage = parseInt($pagerContainer.find('.pages span').html());
    pager.PageSizeDescription = desc.Pager.PageSizeDescription;
    pager.AllowedPageSizes = desc.Pager.AllowedPageSizes;
    pager.SelectedPageSize = desc.Pager.SelectedPageSize;
    pager.VisiblePageCount = desc.Pager.VisiblePageCount;
    pager.SelectedPage = desc.Pager.SelectedPage;
    descriptor.Pager = pager;
    settings.Descriptor = descriptor;
    PageElements[containerId] = settings;
    $gridContainer.on('click', '.pager .page-size a', function (event) {
        _gotoPageSize(containerId, event.target.innerHTML);
    });
    $gridContainer.on('click', '.pager .pages a', function (event) {
        var $elem = $(event.target);
        //var page = $elem.data('pager-page');
        var page = $elem.attr('page');
        if (page === undefined || page === null) {
            page = parseInt(event.target.innerHTML);
        }
        _gotoPage(containerId, page);
    });
    $gridContainer.on('click', '.sort', function (event) {
        var $elem = $(event.currentTarget);
        var prop = $elem.data('sort-prop');
        var ord = SortOrder.Ascending;
        if ($elem.hasClass('sort-asc')) ord = SortOrder.Descending;
        _sortGrid(containerId, prop, ord);
    });
    _finalizeTable($gridContainer.find('table'));
}

//function _RegisterGrid(containerId, url, filters) {
//    PageElements[containerId] = new GridDescriptor();
//    PageElements[containerId].ContainerId = containerId;
//    PageElements[containerId].Url = url;
//    if (filters) {
//        PageElements[containerId].GridRequestDescriptor.Filters = filters;
//    }
//    $('#' + containerId).find('.pager').find('a').each(function () {
//        if ($(this).parent().hasClass('page-size')) {
//            $(this).click(function () {
//                _gotoPageSize(containerId, $(this).html());
//            });
//        } else {
//            if ($(this).parent().hasClass('pages')) {
//                if ($(this).hasClass('previous') || $(this).hasClass('next') || $(this).hasClass('pageset')) {
//                    $(this).click(function () {
//                        _gotoPage(containerId, $(this).attr('page'));
//                    });
//                } else {
//                    $(this).click(function () {
//                        _gotoPage(containerId, $(this).html());
//                    });
//                }
//            }
//        }
//    });
//}

//function _FinalizeGrid(containerId) {
//    var $table = $$('#' + containerId).find('table');
//    _beautifyTable($table);
//    var previousGridRequestDescriptor = PageElements[containerId].GridRequestDescriptor;
//    _RegisterGrid(containerId, PageElements[containerId].Url);
//    if (previousGridRequestDescriptor) {
//        PageElements[containerId].GridRequestDescriptor = previousGridRequestDescriptor;
//    }
//    destroyOverlay(containerId);
//}


//function _UpdateGrid(containerId, filters, pageSize, page, actionType, callback) {
//    createOverlay(containerId, OVERLAY_TYPE_SEMITRANSPARENT_WHITE);
//    if (filters) {
//        PageElements[containerId].GridRequestDescriptor.Filters = filters;
//    }
//    if (pageSize) {
//        PageElements[containerId].GridRequestDescriptor.Pager.PageSize = pageSize;
//    }
//    if (page) {
//        PageElements[containerId].GridRequestDescriptor.Pager.Page = page;
//    }
//    $.ajax({
//        type: 'POST',
//        contentType: 'application/json; charset=utf-8',
//        data: JSON.stringify({ request: { pager: PageElements[containerId].GridRequestDescriptor.Pager, RequestType: actionType }, filters: PageElements[containerId].GridRequestDescriptor.Filters }),
//        url: PageElements[containerId].Url,
//        success: function (data) {
//            if (callback !== undefined && callback !== null) {
//                callback(data);
//            }
//            else $('#' + containerId).html(data);
//            _FinalizeGrid(containerId);
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//            destroyOverlay(containerId);
//        }
//    });
//}

function _UpdateGrid(containerId, callback) {
    createOverlay(containerId, OVERLAY_TYPE_SEMITRANSPARENT_WHITE);
    var settings = PageElements[containerId];
    //displayProgressIndicator('main');
    //$('.progress-indicator').css('display', 'block');
    ini.ajax.post(settings.Url, ini.ajax.contentType.Json, { request: settings.Descriptor, filters: settings.Filters },
        function (result) {
            if (callback !== undefined && callback !== null) {
                callback(result);
            }
            else $('#' + containerId).html(result);
            destroyOverlay(containerId);
            _finalizeTable($('#' + containerId).find('table'));
            //removeProgressIndicator('main');
            //$('.progress-indicator').css('display', 'none');
            $('#materialList').sorttable({
            }).disableSelection();
         
        },
        function (error) {
            destroyOverlay(containerId);
            removeProgressIndicator('main');
            $('.progress-indicator').css('display', 'none');
            showAlert(error);
        }
    );
}

function _sortGrid(containerId, prop, ord) {
    var descriptor = PageElements[containerId].Descriptor;
    descriptor.Sort.PropertyName = prop;
    descriptor.Sort.Order = ord;
    descriptor.RequestType = ActionType.Sort;
    _UpdateGrid(containerId);
}

function _gotoPage(containerId, page) {
    var descriptor = PageElements[containerId].Descriptor;
    descriptor.Pager.SelectedPage = page;
    descriptor.RequestType = ActionType.Page;
    _UpdateGrid(containerId);
}

function _gotoPageSize(containerId, pageSize) {
    var descriptor = PageElements[containerId].Descriptor;
    descriptor.Pager.SelectedPageSize = pageSize;
    descriptor.RequestType = ActionType.Page;
    _UpdateGrid(containerId);
}

function _applyFilters(containerId, filters, callback) {
    var settings = PageElements[containerId];
    settings.Filters = filters;
    settings.Descriptor.RequestType = ActionType.Search;
    _UpdateGrid(containerId, callback);
}

//function _gotoPage(containerId, page) {
//    _UpdateGrid(containerId, null, null, page, ActionType.Page);
//}

//function _gotoPageSize(containerId, pageSize) {
//    _UpdateGrid(containerId, null, pageSize, null, ActionType.Page);
//}

//function _applyFilters(containerId, filters, callback) {
//    _UpdateGrid(containerId, filters, null, null, ActionType.Search, callback);
//}
////var PageElements = new Object();

////function GridSettings() {
////    this.Filters = null;
////    this.Descriptor = null;
////    this.ContainerId = null;
////    this.Url = null;
////}

////function GridDescriptor() {
////    this.Pager = null;
////    this.Sort = null;
////    this.RequestType = ActionType.Page;
////}

////function PagerDescriptor() {
////    this.SelectedPage = 0;
////    this.SelectedPageSize = 0;
////    this.PageSizeDescription = null;
////    this.AllowedPageSizes = new Array();
////    this.VisiblePageCount = 0;
////}

////function SortDescriptor() {
////    this.PropertyName = null;
////    this.Order = SortOrder.Ascending;
////}


////function _RegisterGrid(containerId, url, defaultSortProp, defaultSortOrd, filters) {
////    var settings = new GridSettings();
////    var descriptor = new GridDescriptor();
////    var sorter = new SortDescriptor();
////    var pager = new PagerDescriptor();
////    settings.ContainerId = containerId;
////    settings.Url = url;
////    if (filters) {
////        settings.Filters = filters;
////    }
////    sorter.PropertyName = defaultSortProp;
////    sorter.Order = defaultSortOrd;
////    descriptor.Sort = sorter;
////    var $gridContainer = $('#' + containerId);
////    var $pagerContainer = $gridContainer.find('.pager').first();
////    pager.PageSizeDescription = $pagerContainer.find('.page-size .text').html();
////    $pagerContainer.find('.page-size').children(':not(span[class="text"])').each(function () {
////        pager.AllowedPageSizes.push(parseInt(this.innerHTML));
////        if (this.nodeName.toLowerCase() == 'span') pager.SelectedPageSize = parseInt(this.innerHTML);
////    });
////    pager.VisiblePageCount = $pagerContainer.data('pager-vis-page-count');
////    pager.SelectedPage = parseInt($pagerContainer.find('.pages span').html());
////    descriptor.Pager = pager;
////    settings.Descriptor = descriptor;
////    PageElements[containerId] = settings;
////    $gridContainer.on('click', '.pager .page-size a', function (event) {
////        _gotoPageSize(containerId, event.target.innerHTML);
////    });
////    $gridContainer.on('click', '.pager .pages a', function (event) {
////        var $elem = $(event.target);
////        var page = $elem.data('pager-page');
////        if (page === undefined || page === null) {
////            page = parseInt(event.target.innerHTML);
////        }
////        _gotoPage(containerId, page);
////    });
////    $gridContainer.on('click', '.sort', function (event) {
////        var $elem = $(event.currentTarget);
////        var prop = $elem.data('sort-prop');
////        var ord = SortOrder.Ascending;
////        if ($elem.hasClass('sort-asc')) ord = SortOrder.Descending;
////        _sortGrid(containerId, prop, ord);
////    });
////    _finalizeTable($gridContainer.find('table'));
////}


////function _UpdateGrid(containerId, callback) {
////    createOverlay(containerId, OVERLAY_TYPE_SEMITRANSPARENT_WHITE);
////    var settings = PageElements[containerId];
////    ini.ajax.post(settings.Url, ini.ajax.contentType.Json, { request: settings.Descriptor, filters: settings.Filters },
////        function (result) {
////            if (callback !== undefined && callback !== null) {
////                callback(result);
////            }
////            else $('#' + containerId).html(result);
////            destroyOverlay(containerId);
////            _finalizeTable($('#' + containerId).find('table'));
////        },
////        function (error) {
////            destroyOverlay(containerId);
////            showAlert(error);
////        }
////    );
////}

////function _sortGrid(containerId, prop, ord) {
////    var descriptor = PageElements[containerId].Descriptor;
////    descriptor.Sort.PropertyName = prop;
////    descriptor.Sort.Order = ord;
////    descriptor.RequestType = ActionType.Sort;
////    _UpdateGrid(containerId);
////}

////function _gotoPage(containerId, page) {
////    var descriptor = PageElements[containerId].Descriptor;
////    descriptor.Pager.SelectedPage = page;
////    descriptor.RequestType = ActionType.Page;
////    _UpdateGrid(containerId);
////}

////function _gotoPageSize(containerId, pageSize) {
////    var descriptor = PageElements[containerId].Descriptor;
////    descriptor.Pager.SelectedPageSize = pageSize;
////    descriptor.RequestType = ActionType.Page;
////    _UpdateGrid(containerId);
////}

////function _applyFilters(containerId, filters, callback) {
////    var settings = PageElements[containerId];
////    settings.Filters = filters;
////    settings.Descriptor.RequestType = ActionType.Search;
////    _UpdateGrid(containerId, callback);
////}

//function _gotoPage(containerId, page) {
//    _UpdateGrid(containerId, null, null, page, ActionType.Page);
//}

//function _gotoPageSize(containerId, pageSize) {
//    _UpdateGrid(containerId, null, pageSize, null, ActionType.Page);
//}

//function _applyFilters(containerId, filters, callback) {
//    _UpdateGrid(containerId, filters, null, null, ActionType.Search, callback);
//}
function showDatepicker(element) {
    $(element).datepicker({
        dateFormat: 'dd/mm/yy',
        beforeShow: function (input, inst) { $(inst.dpDiv).addClass('uiOverrride'); },
        onClose: function (dateText, inst) { $(inst.dpDiv).removeClass('uiOverrride'); }
    }).datepicker("show");
}

function displayDate(date) {
    var dd = date.getDate();
    var mm = date.getMonth() + 1;
    var yyyy = date.getFullYear();
    if (dd < 10) { dd = '0' + dd }
    if (mm < 10) { mm = '0' + mm }
    return dd + '/' + mm + '/' + yyyy;
}






/* Numeric values only extender */
/*
*
* Copyright (c) 2006-2011 Sam Collett (http://www.texotela.co.uk)
* Dual licensed under the MIT (http://www.opensource.org/licenses/mit-license.php)
* and GPL (http://www.opensource.org/licenses/gpl-license.php) licenses.
* 
* Version 1.3
* Demo: http://www.texotela.co.uk/code/jquery/numeric/
*
*/
(function ($) {
    // Based on code from http://javascript.nwbox.com/cursor_position/ (Diego Perini <dperini@nwbox.com>)
    $.fn.getSelectionStart = function (o) {
        if (o.createTextRange) {
            var r = document.selection.createRange().duplicate();
            r.moveEnd('character', o.value.length);
            if (r.text == '') return o.value.length;
            return o.value.lastIndexOf(r.text);
        } else return o.selectionStart;
    }

    // set the selection, o is the object (input), p is the position ([start, end] or just start)
    $.fn.setSelection = function (o, p) {
        // if p is number, start and end are the same
        if (typeof p == "number") p = [p, p];
        // only set if p is an array of length 2
        if (p && p.constructor == Array && p.length == 2) {
            if (o.createTextRange) {
                var r = o.createTextRange();
                r.collapse(true);
                r.moveStart('character', p[0]);
                r.moveEnd('character', p[1]);
                r.select();
            }
            else if (o.setSelectionRange) {
                o.focus();
                o.setSelectionRange(p[0], p[1]);
            }
        }
    }
    $.fn.revalidateUnobtrusive = function () {
        var form = $(this);
        form.removeData("validator");
        form.removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse(form);
    }
    $.fn.serializeObject = function() {
    var o = {};
//    var a = this.serializeArray();
    $(this).find('input[type="hidden"], input[type="text"], input[type="password"], input[type="checkbox"]:checked, input[type="radio"]:checked, select').each(function() {
        if ($(this).attr('type') == 'hidden') { //if checkbox is checked do not take the hidden field
            var $parent = $(this).parent();
            var $chb = $parent.find('input[type="checkbox"][name="' + this.name.replace(/\[/g, '\[').replace(/\]/g, '\]') + '"]');
            if ($chb != null) {
                if ($chb.prop('checked')) return;
            }
        }
        if (this.name === null || this.name === undefined || this.name === '') return;
        var elemValue = null;
        if ($(this).is('select')) elemValue = $(this).find('option:selected').val();
        else elemValue = this.value;
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(elemValue || '');
        } else {
            o[this.name] = elemValue || '';
        }
    });
    return o;
    }
    $.fn.hideAndDisable = function() { 
        $(this).hide().prop('disabled', true).find('input, select, textarea').each(function() {
            $(this).prop('disabled', true);
        });
    }
    $.fn.showAndEnable = function() {
        $(this).show().prop('disabled', false).find('input, select, textarea').each(function() {
            $(this).prop('disabled', false);
        });
    }
})(jQuery);




