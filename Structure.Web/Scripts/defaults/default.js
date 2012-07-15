﻿// default namespace
Structure = Structure || {};

// shared startup code
$(function () {

    // disable ajax caching, wire up callbacks
    $.ajaxSetup({
        cache: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            return false;
        },
        success: function (data, textStatus, XMLHttpRequest) {
            return false;
        }
    });

    // modal dialog links
    Structure.EnableDialogs();

});

// modal dialogs
Structure.EnableDialogs = function () {
    $(document).on('click', '.dialog', function (e) {
        e.preventDefault();
        var href = $(this).attr('href');
        Structure.Shared.Dialog(href);
    });
};